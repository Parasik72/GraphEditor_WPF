using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model {
    [Serializable]
    public class ObjectT<T> : INotifyPropertyChanged {
        private T _value;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public T Value {
            get => _value;
            set {
                _value = value;
                OnPropertyChanged();
            }
        }
    }
}
