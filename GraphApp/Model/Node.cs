using GalaSoft.MvvmLight.Command;
using GraphApp.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace GraphApp.Model {
    [Serializable]
    public class Node : INotifyPropertyChanged, IMoveThumb {
        private string _elementType = "Node";
        private string _name;
        private double _positionX;
        private double _positionY;
        private int _id;
        private bool _bCanMove = false;
        [NonSerialized]
        private ObjectT<string> _bSelected;
        [NonSerialized]
        private ICommand _deleteNode;
        [NonSerialized]
        private ICommand _updateEdgePosition;
        [NonSerialized]
        private ICommand _mouseUp;
        [NonSerialized]
        private ICommand _nameChangeHandler;
        [NonSerialized]
        private bool _bInitialized;
        public bool BInitialized {
            get => _bInitialized;
            set {
                _bInitialized = value;
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        public ObservableCollection<ThreeElement<Node, string, int>> ConnectedNodes { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public Node() {
            ConnectedNodes = new ObservableCollection<ThreeElement<Node, string, int>>(new ThreeElement<Node, string, int>[] { });
            BInitialized = false;
            BSelected = new ObjectT<string> { Value = "Transparent" };
        }
        [XmlIgnore]
        public ICommand NameChangeHandler {
            get => _nameChangeHandler;
            set {
                _nameChangeHandler = value;
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        public ICommand MouseUp {
            get => _mouseUp;
            set {
                _mouseUp = value;
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        public ICommand updateEdgePosition { 
            get => _updateEdgePosition; 
            set {
                _updateEdgePosition = value;
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        public ICommand deleteNode {
            get => _deleteNode;
            set {
                _deleteNode = value;
                OnPropertyChanged();
            }
        }
        public int Id {
            get => _id;
            set {
                _id = value;
                OnPropertyChanged();
            }
        }
        public double PositionX {
            get => _positionX;
            set {
                if (_bCanMove) {
                    _positionX = value;
                    OnPropertyChanged();
                }
            }
        }
        public double PositionXDes {
            get => _positionX;
            set {
                _positionX = value;
                OnPropertyChanged();
            }
        }
        public double PositionY {
            get => _positionY;
            set {
                if (_bCanMove) {
                    _positionY = value;
                    OnPropertyChanged();
                }
            }
        }
        public double PositionYDes {
            get => _positionY;
            set {
                _positionY = value;
                OnPropertyChanged();
            }
        }
        public string NameUR {
            get => _name;
            set {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Name {
            get => _name;
            set {
                if (value.Trim().Length != 0) {
                    string _prevName = _name;
                    _name = value;
                    OnPropertyChanged();
                    if (BInitialized)
                        NameChangeHandler.Execute(new Pair<Model.Node, string>(this, _prevName));
                }
            }
        }
        public string NameForTable {
            get => "Name: " + _name;
            set {
                OnPropertyChanged();
            }
        }
        public string ElementType {
            get => _elementType;
            set {
                _elementType = value;
                OnPropertyChanged();
            }
        }
        public bool bCanMove {
            get => _bCanMove;
            set {
                _bCanMove = value;
                OnPropertyChanged();
            }
        }
        public int blurRadius {
            get => BSelected.Value == "Transparent" ? 0 : 10;
            set {
                OnPropertyChanged();
            }
        }
        public ObjectT<string> BSelected {
            get => _bSelected;
            set {
                _bSelected = value;
                OnPropertyChanged();
            }
        }
    }
}