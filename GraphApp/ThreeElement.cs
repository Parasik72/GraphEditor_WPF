using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp {
    public class ThreeElement<T, U, W> : INotifyPropertyChanged {
        private T _first;
        private U _second;
        private W _third;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public ThreeElement() {
        }
        public ThreeElement(T first, U second, W third) {
            First = first;
            Second = second;
            Third = third;
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
        public W Third {
            get => _third;
            set {
                _third = value;
                OnPropertyChanged();
            }
        }
    }
}
