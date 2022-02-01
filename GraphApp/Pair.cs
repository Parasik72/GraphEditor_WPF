using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp {
    public class Pair<T, U> : INotifyPropertyChanged {
        private T _first;
        private U _second;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public Pair() {
        }
        public Pair(T first, U second) {
            this.First = first;
            this.Second = second;
        }
        public T First {
            get => _first;
            set {
                _first = value;
                OnPropertyChanged();
            }
        }
        public U Second {
            get => _second;
            set {
                _second = value;
                OnPropertyChanged();
            }
        }
    }
}
