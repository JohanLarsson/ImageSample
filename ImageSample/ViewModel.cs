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
        private readonly int[] ints = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private int _count;
        private Stopwatch _stopwatch;
        private double _countPerSecond;

        public ViewModel()
        {
            Task.Run(() => Allocate());
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public double CountPerMilliSecond
        {
            get { return Count / Stopwatch.Elapsed.TotalMilliseconds; }
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
                var items = ints.Select(x => new Item { Address = x.ToString() })
                                .ToArray();
                _count++;
                if (Count % 10000 == 0)
                {
                    OnPropertyChanged("Count");
                    OnPropertyChanged("Stopwatch");
                    OnPropertyChanged("CountPerMilliSecond");
                }
            }
        }
    }
}
