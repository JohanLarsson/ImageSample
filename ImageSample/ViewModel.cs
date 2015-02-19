using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ImageSample.Annotations;

namespace ImageSample
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly int[] _ints = Enumerable.Range(0, 1000).ToArray();
        private int _count;
        private Stopwatch _stopwatch;

        public ViewModel()
        {
            Task.Run(() => Allocate());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int[] Ints
        {
            get { return _ints; }
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
            private set
            {
                if (Equals(value, _stopwatch)) return;
                _stopwatch = value;
                OnPropertyChanged();
            }
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

        private void Allocate()
        {
            _stopwatch = Stopwatch.StartNew();
            while (true)
            {
                var items = _ints.Select(x => new Item { Address = x.ToString() })
                                 .ToArray();
                _count++;
                if (_stopwatch.ElapsedMilliseconds % 100 == 0)
                {
                    OnPropertyChanged("Count");
                    OnPropertyChanged("Stopwatch");
                    OnPropertyChanged("CountPerMillisecond");
                    OnPropertyChanged("MicrosecondPerCount");
                }
            }
        }
    }
}
