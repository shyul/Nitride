/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// SampleBuffer
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Nitride.EE
{
    public class WaveFormGroup
    {
        public WaveFormGroup(int numOfCh, int maxLength)
        {
            WaveForms = new WaveForm[numOfCh];

            for (int i = 0; i < numOfCh; i++)
            {
                WaveForms[i] = new WaveForm(maxLength);
            }

            HasUpdatedItem = false;
        }

        public bool HasUpdatedItem
        {
            get => WaveForms.Where(n => n.IsUpdated).Count() > 0;
            set => WaveForms.RunEach(n => n.IsUpdated = value);
        }

        public int NumOfCh => WaveForms.Length;

        public int Length => WaveForms.First().Length;

        public double SampleRate => WaveForms.First().SampleRate;

        public void Configure(int length, double rate = 1, double startTime = 0)
        {
            WaveForms.RunEach(n => n.Configure(length, rate, startTime));
        }

        public WaveForm[] WaveForms { get; }

        public WaveForm this[int i] => WaveForms[i];

        /// <summary>
        /// Copy from DataBuffer
        /// </summary>
        /// <param name="source"></param>
        /// <param name="format"></param>
        /// <param name="index"></param>
        /// <returns>Samples Copied</returns>
        public int CopyData(DataBuffer source, DataBufferFormat format, double[] scale, int index = 0)
        {
            HasUpdatedItem = true;

            int i, j;
            int pt = 0;
            int num = NumOfCh;
            int count = (Length * NumOfCh) + index; // R16 = 1, C32 = 2, C32 Dual = 4

            switch (format)
            {
                default:
                case DataBufferFormat.S16:
                    for (i = index; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = (new Complex(source.S16[i + j], 0)) * scale[j];
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.DS16:
                    for (i = index; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = (new Complex((source.DS16[i + j].D1), (source.DS16[i + j].D2))) * scale[j];
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.S32:
                    for (i = index; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = (new Complex(source.S32[i + j], 0)) * scale[j];
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.DS32:
                    for (i = index; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = (new Complex(source.DS32[i + j].D1, source.DS32[i + j].D2 * scale[j])) * scale[j];
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.S64:
                    for (i = index; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = (new Complex(source.S64[i + j], 0)) * scale[j];
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.DS64:
                    for (i = index; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = (new Complex(source.DS64[i + j].D1, source.DS64[i + j].D2)) * scale[j];
                        }
                        pt++;
                    }
                    break;
            }
            /*
            for (j = 0; j < num; j++)
            {
                WaveForms[j].IsUpdated = true;
            }*/

            return pt;
        }
    }
}
