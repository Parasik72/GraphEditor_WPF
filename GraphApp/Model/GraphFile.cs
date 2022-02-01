using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GraphApp.Model {
    [Serializable]
    public class GraphFile : INotifyPropertyChanged {
        public List<Model.Node> nodes = new List<Model.Node>();
        public List<Model.Edge> edges = new List<Model.Edge>();
        public List<Pair<int, List<ThreeElement<Model.Node, string, int>>>> ConnectedNodes = new List<Pair<int, List<ThreeElement<Model.Node, string, int>>>>();
        private string _fileName;
        private string _filePath;
        private bool _bSaved;
        private int _lastId;
        [NonSerialized]
        private bool _bNew;
        public int LastId {
            get => _lastId;
            set {
                _lastId = value;
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        public bool BNew {
            get => _bNew;
            set {
                _bNew = value;
                OnPropertyChanged();
            }
        }
        public bool BSaved {
            get => _bSaved;
            set {
                _bSaved = value;
                OnPropertyChanged();
                OnPropertyChanged("FileNameApp");
            }
        }
        public string FilePath {
            get => _filePath;
            set {
                _filePath = value;
                OnPropertyChanged();
            }
        }
        public string FileName {
            get => _fileName;
            set {
                _fileName = value;
                OnPropertyChanged();
                OnPropertyChanged("FileNameApp");
            }
        }
        public string FileNameApp {
            get => BSaved ? _fileName : _fileName + '*';
            set {
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
