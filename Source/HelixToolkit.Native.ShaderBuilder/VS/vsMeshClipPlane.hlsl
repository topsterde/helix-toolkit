#ifndef VSMESHDEFAULT_HLSL
#define VSMESHDEFAULT_HLSL
#define CLIPPLANE
#define MESH
#include"..\Common\Common.hlsl"
#include"..\Common\DataStructs.hlsl"
#pragma pack_matrix( row_major )

PSInputClip main(VSInput input)
{
    PSInputClip output = (PSInputClip) 0;
    float4 inputp = input.p;
    float3 inputn = input.n;
    float3 inputt1 = input.t1;
    float3 inputt2 = input.t2;
    if (bInvertNormal)
    {
        inputn = -inputn;
    }

	// compose instance matrix
    if (bHasInstances)
    {
        matrix mInstance =
        {
            input.mr0,
			input.mr1,
			input.mr2,
			input.mr3
        };
        inputp = mul(input.p, mInstance);
        inputn = mul(inputn, (float3x3) mInstance);
        if (bHasNormalMap)
        {
            inputt1 = mul(inputt1, (float3x3) mInstance);
            inputt2 = mul(inputt2, (float3x3) mInstance);
        }
    }

	//set position into world space	
    output.p = mul(inputp, mWorld);
    float3 vEye = vEyePos - output.p.xyz;
    output.vEye = float4(normalize(vEye), length(vEye)); //Use wp for camera->vertex direction
	//set normal for interpolation	
    output.n = normalize(mul(inputn, (float3x3) mWorld));
    	//set texture coords
    output.t = mul(float2x4(uvTransformR1, uvTransformR2), float4(input.t, 0, 1)).xy;
    if (bHasDisplacementMap)
    {
        const float mipInterval = 20;
        float mipLevel = clamp((distance(output.p.xyz, vEyePos) - mipInterval) / mipInterval, 0, 6);
        float4 h = texDisplacementMap.SampleLevel(samplerDisplace, output.t, mipLevel);
        output.p.xyz += output.n * mul(h, displacementMapScaleMask);
    }
    output.wp = output.p;
	//set position into clip space	
    output.p = mul(output.p, mViewProjection);

	//set position into light-clip space
    if (bHasShadowMap)
    {
        output.sp = mul(output.wp, vLightViewProjection);
    }

	//set color
    output.c = input.c;
    output.cDiffuse = vMaterialDiffuse;
    output.c2 = mad(vMaterialAmbient, vLightAmbient, vMaterialEmissive);


    if (bHasNormalMap)
    {
		// transform the tangents by the world matrix and normalize
        output.t1 = normalize(mul(inputt1, (float3x3) mWorld));
        output.t2 = normalize(mul(inputt2, (float3x3) mWorld));
    }
    else
    {
        output.t1 = 0.0f;
        output.t2 = 0.0f;
    }
    output.clipPlane = float4(0, 0, 0, 0);
    if (EnableCrossPlane.x)
    {
        float3 p = output.wp.xyz - CrossPlaneParams._m00_m01_m02 * CrossPlaneParams._m03;
        output.clipPlane.x = dot(CrossPlaneParams._m00_m01_m02, p);
    }
    if (EnableCrossPlane.y)
    {
        float3 p = output.wp.xyz - CrossPlaneParams._m10_m11_m12 * CrossPlaneParams._m13;
        output.clipPlane.y = dot(CrossPlaneParams._m10_m11_m12, p);
    }
    if (EnableCrossPlane.z)
    {
        float3 p = output.wp.xyz - CrossPlaneParams._m20_m21_m22 * CrossPlaneParams._m23;
        output.clipPlane.z = dot(CrossPlaneParams._m20_m21_m22, p);
    }
    if (EnableCrossPlane.w)
    {
        float3 p = output.wp.xyz - CrossPlaneParams._m30_m31_m32 * CrossPlaneParams._m33;
        output.clipPlane.w = dot(CrossPlaneParams._m20_m21_m22, p);
    }

    return output;
}

#endif