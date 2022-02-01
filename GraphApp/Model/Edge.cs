using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace GraphApp.Model {
    [Serializable]
    public class Edge : INotifyPropertyChanged {
        private string _elementType = "Edge";
        private Model.Node _fromNode;
        private Model.Node _toNode;
        private int _weight;
        private ThreeElement<Model.Node, string, int> _forItem1;
        private ThreeElement<Model.Node, string, int> _forItem2;
        [NonSerialized]
        private ObjectT<string> _bSelected;
        [NonSerialized]
        private ICommand _deleteEdge;
        [NonSerialized]
        private ICommand _weightChangeHandler;
        private bool _bHaveDirection;
        [NonSerialized]
        private bool _bInitialized;
        public Edge() {
            BInitialized = false;
            BSelected = new ObjectT<string> { Value = "Transparent" };
        }
        public bool BInitialized {
            get => _bInitialized;
            set {
                _bInitialized = value;
                OnPropertyChanged();
            }
        }
        public bool BHaveDirection {
            get => _bHaveDirection;
            set {
                _bHaveDirection = value;
                OnPropertyChanged();
            }
        }
        public Visibility visibilityTriangleTo {
            get => (_forItem1.Second == "To" || _forItem1.Second == "T/Fr") && BHaveDirection ? Visibility.Visible : Visibility.Hidden;
            set {
                OnPropertyChanged();
            }
        }
        public Visibility visibilityTriangleFrom {
            get => (_forItem2.Second == "To" || _forItem2.Second == "T/Fr") && BHaveDirection ? Visibility.Visible : Visibility.Hidden;
            set {
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        public ICommand WeightChangeHandler {
            get => _weightChangeHandler;
            set {
                _weightChangeHandler = value;
                OnPropertyChanged();
            }
        }
        private bool bCanChangeWeight = true;
        public int WeightUR {
            get => _weight;
            set {
                _weight = value;
                bCanChangeWeight = false;
                OnPropertyChanged("Weight");
            }
        }
        public int Weight {
            get => _weight;
            set {
                if (value > -1000) {
                    int prevWeight = _weight;
                    _weight = value;
                    if (_forItem1 != null) {
                        _forItem1.Third = value;
                        _forItem2.Third = value;
                        OnPropertyChanged();
                        if (BInitialized && bCanChangeWeight)
                            WeightChangeHandler.Execute(new Pair<Model.Edge, int>(this, prevWeight));
                    }
                } else {
                    int prevWeight = _weight;
                    _weight = -999;
                    if (_forItem1 != null) {
                        _forItem1.Third = -999;
                        _forItem2.Third = -999;
                        OnPropertyChanged();
                        if (BInitialized && bCanChangeWeight)
                            WeightChangeHandler.Execute(new Pair<Model.Edge, int>(this, prevWeight));
                    }
                }
                bCanChangeWeight = true;
            }
        }
        public string ElementType {
            get => _elementType;
            set {
                _elementType = value;
                OnPropertyChanged();
            }
        }
        public ThreeElement<Model.Node, string, int> ForItem1 {
            get => _forItem1;
            set {
                _forItem1 = value;
                OnPropertyChanged();
            }
        }
        public ThreeElement<Model.Node, string, int> ForItem2 {
            get => _forItem2;
            set {
                _forItem2 = value;
                OnPropertyChanged();
            }
        }
        public Model.Node FromNode {
            get => _fromNode;
            set {
                _fromNode = value;
                OnPropertyChanged();
            }
        }
        public Model.Node ToNode {
            get => _toNode;
            set {
                _toNode = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public string lineFromTrianglePosTo {
            get {
                double X1 = _fromNode.PositionX, Y1 = _fromNode.PositionY;
                double X2 = _toNode.PositionX, Y2 = _toNode.PositionY;
                return getTrianglePos(X1, Y1, X2, Y2);
            }
            set {
                OnPropertyChanged();
            }
        }
        public string lineFromTrianglePosFrom{
            get {
                double X1 = _toNode.PositionX, Y1 = _toNode.PositionY;
                double X2 = _fromNode.PositionX, Y2 = _fromNode.PositionY;
                return getTrianglePos(X1, Y1, X2, Y2);
            }
            set {
                OnPropertyChanged();
            }
        }
        private string getTrianglePos(double X1, double Y1, double X2, double Y2) {
            string first = rotateElement(new Pair<double, double>(X2, Y2 + 17), new Pair<double, double>(X2 + 17, Y2 + 17), Math.Atan2(Y1 - Y2, X1 - X2) - Math.PI);
            string second = rotateElement(new Pair<double, double>(X2 - 14, Y2 + 28), new Pair<double, double>(X2 + 17, Y2 + 17), Math.Atan2(Y1 - Y2, X1 - X2) - Math.PI);
            string third = rotateElement(new Pair<double, double>(X2 - 14, Y2 + 6), new Pair<double, double>(X2 + 17, Y2 + 17), Math.Atan2(Y1 - Y2, X1 - X2) - Math.PI);
            return first + ' ' + second + ' ' + third;
        }
        private string rotateElement(Pair<double, double> elemCoord, Pair<double, double> elemCenter, double angle) {
            double x = elemCenter.First + ((elemCoord.First - elemCenter.First) * Math.Cos(angle) - (elemCoord.Second - elemCenter.Second) * Math.Sin(angle));
            double y = elemCenter.Second + ((elemCoord.First - elemCenter.First) * Math.Sin(angle) + (elemCoord.Second - elemCenter.Second) * Math.Cos(angle));
            return ((int)x).ToString() + ',' + ((int)y).ToString();
        }
        public string lineFrom {
            get => (_fromNode.PositionX + 17).ToString() + ',' + (_fromNode.PositionY + 17).ToString();
            set {
                OnPropertyChanged();
            }
        }
        public string lineTo {
            get => (_toNode.PositionX + 17).ToString() + ',' + (_toNode.PositionY + 17).ToString();
            set {
                OnPropertyChanged();
            }
        }
        public string weightPosition {
            get {
                double posX = (_fromNode.PositionX + 17 + _toNode.PositionX + 17) / 2;
                double posY = (_fromNode.PositionY + 17 + _toNode.PositionY + 17) / 2;
                return ((int)posX - 12).ToString() + ',' + ((int)posY - 12).ToString();
            }
            set {
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        public ICommand deleteEdge {
            get => _deleteEdge;
            set {
                _deleteEdge = value;
                OnPropertyChanged();
            }
        }
        public void updatePos() {
            OnPropertyChanged("lineFrom");
            OnPropertyChanged("lineTo");
            OnPropertyChanged("weightPosition");
            OnPropertyChanged("lineFromTrianglePosTo");
            OnPropertyChanged("lineFromTrianglePosFrom");
        }
        public int blurRadius {
            get => BSelected.Value == "#0000FF" ? 10 : 0;
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
