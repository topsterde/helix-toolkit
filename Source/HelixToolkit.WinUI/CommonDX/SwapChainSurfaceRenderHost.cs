﻿/*
The MIT License (MIT)
Copyright (c) 2018 Helix Toolkit contributors
*/
using HelixToolkit.Logger;
using SharpDX.Direct3D11;
using System;
using System.Runtime.CompilerServices;
#if DX11_1
using Device = SharpDX.Direct3D11.Device1;
using DeviceContext = SharpDX.Direct3D11.DeviceContext1;
#else
using Device = SharpDX.Direct3D11.Device;
#endif

#if !NET5_0
namespace HelixToolkit.Wpf.SharpDX
#else
#if CORE
namespace HelixToolkit.SharpDX.Core
#else
namespace HelixToolkit.WinUI
#endif
#endif
{
    namespace Render
    {
        /// <summary>
        /// 
        /// </summary>
        public class SwapChainSurfaceRenderHost : DefaultRenderHost
        {
            protected readonly IntPtr surface;
            /// <summary>
            /// Initializes a new instance of the <see cref="SwapChainSurfaceRenderHost"/> class.
            /// </summary>
            /// <param name="surface">The window PTR.</param>
            public SwapChainSurfaceRenderHost(IntPtr surface)
            {
                this.surface = surface;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SwapChainSurfaceRenderHost"/> class.
            /// </summary>
            /// <param name="surface">The surface.</param>
            /// <param name="createRenderer">The create renderer.</param>
            public SwapChainSurfaceRenderHost(IntPtr surface, Func<IDevice3DResources, IRenderer> createRenderer) : base(createRenderer)
            {
                this.surface = surface;
            }
            /// <summary>
            /// Creates the render buffer.
            /// </summary>
            /// <returns></returns>
            protected override DX11RenderBufferProxyBase CreateRenderBuffer()
            {
                Logger.Log(LogLevel.Information, "DX11SwapChainRenderBufferProxy", nameof(SwapChainRenderHost));
                return new DX11SwapChainRenderBufferProxy(surface, EffectsManager);
            }
        }
    }

}
