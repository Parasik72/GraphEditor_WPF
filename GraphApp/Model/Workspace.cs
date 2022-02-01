using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Drawing;

namespace GraphApp.Model {
    [Serializable]
    public class Workspace : INotifyPropertyChanged {
        private ObservableCollection<Model.Node> _nodes;
        private ObservableCollection<Model.Edge> _edges;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public ObservableCollection<Model.Node> Nodes {
            get => _nodes;
            set {
                _nodes = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Model.Edge> Edges {
            get => _edges;
            set {
                _edges = value;
                OnPropertyChanged();
            }
        }
    }
}
