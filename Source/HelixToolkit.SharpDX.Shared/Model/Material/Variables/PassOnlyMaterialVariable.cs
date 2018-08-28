﻿/*
The MIT License (MIT)
Copyright (c) 2018 Helix Toolkit contributors
*/
#if !NETFX_CORE
namespace HelixToolkit.Wpf.SharpDX.Model
#else
namespace HelixToolkit.UWP.Model
#endif
{
    using Render;
    using Shaders;
    using Utilities;
    /// <summary>
    /// 
    /// </summary>
    public sealed class PassOnlyMaterialVariable : MaterialVariable
    {
        public ShaderPass MaterialPass { get; }

        public ShaderPass ShadowPass { get; }

        public ShaderPass WireframePass { get; }

        private readonly string passName;
        public PassOnlyMaterialVariable(string passName, IRenderTechnique technique)
            : base(technique.EffectsManager, technique, DefaultMeshConstantBufferDesc)
        {
            this.passName = passName;
            MaterialPass = technique[passName];
            ShadowPass = technique[DefaultPassNames.ShadowPass];
            WireframePass = technique[DefaultPassNames.Wireframe];
        }

        public override bool BindMaterialResources(RenderContext context, DeviceContextProxy deviceContext, ShaderPass shaderPass)
        {
            return true;
        }

        public override ShaderPass GetPass(RenderType renderType, RenderContext context)
        {
            return MaterialPass;
        }
        public override ShaderPass GetShadowPass(RenderType renderType, RenderContext context)
        {
            return ShadowPass;
        }

        public override ShaderPass GetWireframePass(RenderType renderType, RenderContext context)
        {
            return WireframePass;
        }
        protected override void UpdateInternalVariables(DeviceContextProxy context)
        {
        }

        protected override void WriteMaterialDataToConstantBuffer(global::SharpDX.DataStream cbStream)
        {
        }

        public override void Draw(DeviceContextProxy deviceContext, IAttachableBufferModel bufferModel, int instanceCount)
        {
            DrawIndexed(deviceContext, bufferModel.IndexBuffer.ElementCount, instanceCount);
        }
    }
}
