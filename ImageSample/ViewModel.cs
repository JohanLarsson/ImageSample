using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ImageSample.Annotations;

namespace ImageSample
{
    public class ViewModel : INotifyPropertyChanged
    {
        private int[] _ints = Enumerable.Range(0, 1000).ToArray();
        private int _count;
        private Stopwatch _stopwatch;
        private Task _task;
        private CancellationTokenSource _cancellationTokenSource;
        private Timer _timer;

        public ViewModel()
        {
            Run();
            _timer = new Timer(Notify, null, TimeSpan.Zero, TimeSpan.FromSeconds(0.1));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Length
        {
            get { return _ints.Length; }
            set
            {
                _cancellationTokenSource.Cancel();
                _task.Wait();
                _ints = Enumerable.Range(0, value).ToArray();
                Run();
                OnPropertyChanged();
            }
        }

        public int Count
        {
            get { return _count; }
            set
            {
                if (value == _count) return;
                _count = value;
                OnPropertyChanged();
            }
        }

        public Stopwatch Stopwatch
        {
            get { return _stopwatch; }
        }

        public double CountPerMillisecond
        {
            get { return Count / Stopwatch.Elapsed.TotalMilliseconds; }
        }

        public double MicrosecondPerCount
        {
            get { return 1000 * Stopwatch.Elapsed.TotalMilliseconds / Count; }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Run()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            _task = Task.Run(() => Allocate(), token);
        }

        private void Allocate()
        {
            _stopwatch = Stopwatch.StartNew();
            Count = 0;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var items = _ints.Select(x => new Item { Address = x.ToString() })
                                 .ToArray();
                _count++;
            }
        }

        private void Notify(object state)
        {
            OnPropertyChanged("Count");
            OnPropertyChanged("Stopwatch");
            OnPropertyChanged("CountPerMillisecond");
            OnPropertyChanged("MicrosecondPerCount");
        }
    }
}
