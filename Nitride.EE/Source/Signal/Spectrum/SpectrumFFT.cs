/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// FFTStream
/// WaveFormReceiver -> WaveForm -> (FreqTrace) SpectrumFFT -> (Frame) SpectrumData -> SpectrumChart
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class SpectrumFFT : IDisposable
    {
        public SpectrumFFT(SpectrumData sd, int poolSize, int maxLength)
        {
            SpectrumData = sd;
            PoolSize = poolSize;

            for (int i = 0; i < poolSize; i++)
            {
                FreqTracePool.Add(new FreqTrace(maxLength));
            }

            HandleFFT_CancellationTokenSource = new CancellationTokenSource();
            HandleFFT_Task = new(() => HandleFFT(), HandleFFT_CancellationTokenSource.Token);
            HandleFFT_Task.Start();
        }

        ~SpectrumFFT() => Dispose();

        public void Dispose()
        {
            HandleFFT_CancellationTokenSource.Cancel();
            while (HandleFFT_Task.Status == TaskStatus.Running) ;
        }

        public int PoolSize { get; }

        public bool IsRunning => HandleFFT_CancellationTokenSource is CancellationTokenSource cts && (!cts.IsCancellationRequested);

        public bool IsStopped =>

            (HandleFFT_CancellationTokenSource is null || HandleFFT_CancellationTokenSource.IsCancellationRequested) &&
            (HandleFFT_Task is null || HandleFFT_Task.Status != TaskStatus.Running);

        public void Configure(int length, double startFreq, double stopFreq, WindowsType winType) //, bool flip)
        {
            Length = length;
            StartFreq = startFreq;
            StopFreq = stopFreq;
            FreqStep = (StopFreq - StartFreq) / (Length - 1D);
            FreqTracePool.ForEach(n => n.Configure(length, startFreq, stopFreq, winType, true)); // bool flip = true;
        }

        public bool FlipSpectrum => FreqTracePool.First().FlipSpectrum;

        public double Gain => FreqTracePool.First().FFT.Gain;

        public int Length { get; private set; }

        public double StartFreq { get; private set; }

        public double StopFreq { get; private set; }

        public double FreqStep { get; private set; }

        public void WaveFormEnqueue(WaveForm wf)
        {
            if (IsRunning)
            {
                if (WaveFormQueue.Count >= PoolSize - 1)
                {
                    WaveFormQueue.TryDequeue(out var wfo);
                    wfo.IsUpdated = false;
                }
                WaveFormQueue.Enqueue(wf);
            }
        }

        private ConcurrentQueue<WaveForm> WaveFormQueue { get; } = new();

        private List<FreqTrace> FreqTracePool { get; } = new();

        private SpectrumData SpectrumData { get; }

        private Task HandleFFT_Task { get; set; }

        private CancellationTokenSource HandleFFT_CancellationTokenSource { get; set; }

        private void HandleFFT()
        {
            while (IsRunning)
            {
                if (WaveFormQueue.Count > 0 && FreqTracePool.Where(n => !n.IsUpdated).FirstOrDefault() is FreqTrace trace)
                {
                    WaveFormQueue.TryDequeue(out var wf);

                    if (!SpectrumData.PauseUpdate) 
                    {
                        trace.Transform(wf.Data); // Here: Trace.IsUpdate = true;
                        SpectrumData.AppendTrace(trace);
                    }

                    wf.IsUpdated = false; // End of WaveForm's data lifecycle.
                }
                else
                    Thread.Sleep(5);
            }
        }
    }
}