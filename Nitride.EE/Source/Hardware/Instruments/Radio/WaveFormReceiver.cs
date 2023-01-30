using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nitride;

namespace Nitride.EE
{
    public abstract class WaveFormReceiver : IInstrumentResource, IDisposable
    {
        public IInstrument Parent { get; set; }

        public virtual int NumOfCh => 1;

        public abstract int MaxSampleLength { get; }

        public int PoolSize => WaveFormPool.Count;

        public string ResourceName { get; set; }

        public bool IsReady { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public virtual IClock SampleClock { get; }

        public virtual uint DecimationRate { get; set; }

        public double[] DSP_Gain { get; protected set; }

        public virtual double SampleRate => SampleClock.Frequency / DecimationRate;

        public virtual double SampleTime => SampleLength / SampleRate;

        public virtual double Bandwidth => SampleRate / 2;

        public virtual double MaxBandwidth => SampleClock.Frequency / 2;

        public virtual int SampleLength { get; set; }

        protected List<WaveFormGroup> WaveFormPool { get; set; } = new();

        public Action<WaveFormGroup> WaveFormEnqueue { get; set; }

        protected WaveFormGroup CurrentWaveFormGroup
        {
            get
            {
                return WaveFormPool.Where(n => !n.HasUpdatedItem).FirstOrDefault();
            }
        }

        protected CancellationTokenSource Handle_CancellationTokenSource { get; set; }

        public bool IsRunning => Handle_CancellationTokenSource is CancellationTokenSource cts && (!cts.IsCancellationRequested);

        public bool IsStopped =>

            (Handle_CancellationTokenSource is null || Handle_CancellationTokenSource.IsCancellationRequested) &&
            (HandleFetch_Task is null || HandleFetch_Task.Status != TaskStatus.Running) &&
            (HandleCopy_Task is null || HandleCopy_Task.Status != TaskStatus.Running);



        protected Task HandleFetch_Task { get; set; }

        protected Task HandleCopy_Task { get; set; }

        protected abstract void HandleFetch();

        protected abstract void HandleCopy();

        public abstract void ApplyConfig();

        public virtual bool IsPause { get; set; } = true;

        public abstract bool TrigSingle();

        public abstract bool TrigContinous();

        public abstract void TrigPause();

        public abstract void TrigStop();

        public void Dispose()
        {
            if (Handle_CancellationTokenSource is not null)
            {
                Handle_CancellationTokenSource.Cancel();
            }

            while (HandleFetch_Task.Status == TaskStatus.Running || HandleCopy_Task.Status == TaskStatus.Running) ;

            WaveFormPool.ForEach(n => n.HasUpdatedItem = false);
        }
    }
}
