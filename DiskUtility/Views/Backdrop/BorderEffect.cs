﻿using DiskUtility.Helpers.Backdrop;
using DiskUtility.WindowsAPI.ComTypes;
using System;
using System.Runtime.InteropServices;
using Windows.Graphics.Effects;

namespace DiskUtility.Views.Backdrop
{
    [Guid("2A2D49C0-4ACF-43C7-8C6A-7C4A27874D27")]
    public sealed class BorderEffect : IGraphicsEffect, IGraphicsEffectSource, IGraphicsEffectD2D1Interop
    {
        public CanvasEdgeBehavior ExtendX { get; set; } = CanvasEdgeBehavior.Clamp;

        public CanvasEdgeBehavior ExtendY { get; set; } = CanvasEdgeBehavior.Clamp;

        public IGraphicsEffectSource Source { get; set; }

        public string Name { get; set; } = string.Empty;

        public int GetEffectId(out Guid id)
        {
            id = typeof(BorderEffect).GUID;
            return 0;
        }

        public int GetNamedPropertyMapping(string name, out uint index, out GRAPHICS_EFFECT_PROPERTY_MAPPING mapping)
        {
            switch (name)
            {
                case nameof(ExtendX):
                    {
                        index = 0;
                        mapping = GRAPHICS_EFFECT_PROPERTY_MAPPING.GRAPHICS_EFFECT_PROPERTY_MAPPING_DIRECT;
                        break;
                    }
                case nameof(ExtendY):
                    {
                        index = 1;
                        mapping = GRAPHICS_EFFECT_PROPERTY_MAPPING.GRAPHICS_EFFECT_PROPERTY_MAPPING_DIRECT;
                        break;
                    }
                default:
                    {
                        index = 0xFF;
                        mapping = (GRAPHICS_EFFECT_PROPERTY_MAPPING)0xFF;
                        break;
                    }
            }

            return 0;
        }

        public int GetProperty(uint index, out IntPtr source)
        {
            if (index is 0)
            {
                BackdropHelper.PropertyValueStatics.CreateUInt32((uint)ExtendX, out IntPtr ptr);

                if (!ptr.Equals(IntPtr.Zero))
                {
                    source = ptr;
                    return 0;
                }
            }
            else if (index is 1)
            {
                BackdropHelper.PropertyValueStatics.CreateUInt32((uint)ExtendY, out IntPtr ptr);

                if (!ptr.Equals(IntPtr.Zero))
                {
                    source = ptr;
                    return 0;
                }
            }

            source = IntPtr.Zero;
            return -2147483637;
        }

        public int GetPropertyCount(out uint count)
        {
            count = 2;
            return 0;
        }

        public int GetSource(uint index, out IGraphicsEffectSource source)
        {
            if (index is 0)
            {
                source = Source;
                return 0;
            }
            else
            {
                source = null;
                return 2147483637;
            }
        }

        public int GetSourceCount(out uint count)
        {
            count = 1;
            return 0;
        }
    }
}
