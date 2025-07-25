﻿using DiskUtility.Helpers.Backdrop;
using DiskUtility.WindowsAPI.ComTypes;
using System;
using System.Runtime.InteropServices;
using Windows.Graphics.Effects;

namespace DiskUtility.Views.Backdrop
{
    [Guid("12F575E8-4DB1-485F-9A84-03A07DD3829F")]
    public sealed class CrossFadeEffect : IGraphicsEffect, IGraphicsEffectSource, IGraphicsEffectD2D1Interop
    {
        public float CrossFade { get; set; } = 0.5f;

        public IGraphicsEffectSource Source1 { get; set; }

        public IGraphicsEffectSource Source2 { get; set; }

        public string Name { get; set; } = string.Empty;

        public int GetEffectId(out Guid id)
        {
            id = typeof(CrossFadeEffect).GUID;
            return 0;
        }

        public int GetNamedPropertyMapping(string name, out uint index, out GRAPHICS_EFFECT_PROPERTY_MAPPING mapping)
        {
            switch (name)
            {
                case nameof(CrossFade):
                    {
                        index = 0;
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
                BackdropHelper.PropertyValueStatics.CreateSingle((float)CrossFade, out IntPtr ptr);
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
            count = 1;
            return 0;
        }

        public int GetSource(uint index, out IGraphicsEffectSource source)
        {
            if (index is 0)
            {
                source = Source1;
                return 0;
            }
            else if (index is 1)
            {
                source = Source2;
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
            count = 2;
            return 0;
        }
    }
}
