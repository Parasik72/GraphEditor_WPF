using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;
using GraphApp.Model;

namespace GraphApp.ViewModel {
    [Serializable]
    class MainWindowViewModel : INotifyPropertyChanged {
        private Model.Workspace _workspace;
        private Model.ModeButton _selectedMode;
        private Model.Node _selectedNode;
        private Model.Edge _selectedEdge;
        private Pair<string, string> _selectedAlgorithm;
        private ICommand _canvasClickCommand;
        private Model.ObjectT<bool> _backgroundCanvas;
        private Model.ObjectT<bool> _dfsResShow;
        private Model.GraphFile _graphFile;
        private int _counterName;
        private bool _isOriented;
        private double _panelX;
        private double _panelY;
        private Controls.URControl _URControl;
        private bool _bCanMove;
        private List<ThreeElement<Model.Node, double, double>> arrMoveNode;
        private WindowState _windowStateVar;
        private ResizeMode _resizeModeVar;
        public ResizeMode ResizeModeVar {
            get => _resizeModeVar;
            set {
                _resizeModeVar = value;
                OnPropertyChanged();
            }
        }
        public Model.ObjectT<bool> windowStateIcon {
            get => WindowStateVar == WindowState.Normal ? new ObjectT<bool> { Value = true } : new ObjectT<bool> { Value = false };
            set {
                OnPropertyChanged();
            }
        }
        public WindowState WindowStateVar {
            get => _windowStateVar;
            set {
                _windowStateVar = value;
                OnPropertyChanged();
                OnPropertyChanged("windowStateIcon");
            }
        }
        public Pair<string, string> SelectedAlgorithm {
            get => _selectedAlgorithm;
            set {
                _selectedAlgorithm = value;
                OnPropertyChanged();
            }
        }
        public bool BCanMove {
            get => _bCanMove;
            set {
                _bCanMove = value;
                OnPropertyChanged();
            }
        }
        public Controls.URControl URControl {
            get => _URControl;
            set {
                _URControl = value;
                OnPropertyChanged();
            }
        }
        public ICommand changeGraphOrientedCommand { get; set; }
        public ICommand changeEdgeDirectionCommand { get; set; }
        public ICommand canvasClickCommand {
            get => _canvasClickCommand;
            set {
                _canvasClickCommand = value;
                OnPropertyChanged();
            }
        }
        public ICommand newGraphCommand { get; set; }
        public ICommand loadGraphCommand { get; set; }
        public ICommand saveGraphCommand { get; set; }
        public ICommand saveAsGraphCommand { get; set; }
        public ICommand undoCommand { get; set; }
        public ICommand redoCommand { get; set; }
        public ICommand nodeChangeNameCommand { get; set; }

        public ICommand dfsCommand { get; set; }
        public ICommand closeAlgorithmWindowCommand { get; set; }
        public ICommand helpCommand { get; set; }
        public ICommand selectModeHotKeyCommand { get; set; }
        public ICommand appShutdownCommand { get; set; }
        public ICommand minimizeWindowCommand { get; set; }
        public ICommand normalMaxWindowCommand { get; set; }
        public ICommand clickWindowAlgorithmInfoCommand { get; set; }
        public int CounterName {
            get => _counterName;
            set {
                _counterName = value;
                OnPropertyChanged();
            }
        }
        public Model.GraphFile GraphFile {
            get => _graphFile;
            set {
                _graphFile = value;
                OnPropertyChanged();
            }
        }
        public Model.ObjectT<bool> graphOriented {
            get => new Model.ObjectT<bool> { Value = IsOriented };
            set {
                OnPropertyChanged();
            }
        }
        public bool IsOriented {
            get => _isOriented;
            set {
                _isOriented = value;
                OnPropertyChanged();
                OnPropertyChanged("graphOriented");
            }
        }
        public Model.ObjectT<bool> DfsResShow {
            get => _dfsResShow;
            set {
                _dfsResShow = value;
                _bCanUseModeFunc = !value.Value;
                OnPropertyChanged();
            }
        }
        public Model.ObjectT<bool> BackgroundCanvas {
            get => _backgroundCanvas;
            set {
                _backgroundCanvas = value;
                OnPropertyChanged();
            }
        }
        public Model.ModeButton SelectedMode {
            get => _selectedMode;
            set {
                _selectedMode = value;
                OnPropertyChanged();
            }
        }
        public Model.Workspace Workspace {
            get => _workspace;
            set {
                _workspace = value;
                OnPropertyChanged();
            }
        }
        public Model.Node SelectedNode {
            get => _selectedNode;
            set {
                _selectedNode = value;
                OnPropertyChanged();
            }
        }
        public Model.Edge SelectedEdge {
            get => _selectedEdge;
            set {
                _selectedEdge = value;
                OnPropertyChanged();
            }
        }
        public double PanelX {
            get => _panelX;
            set {
                if (value.Equals(_panelX)) return;
                _panelX = value;
                OnPropertyChanged();
            }
        }
        public double PanelY {
            get => _panelY;
            set {
                if (value.Equals(_panelY)) return;
                _panelY = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Model.Node> selectedNodesForConnect { get; private set; }
        public ObservableCollection<Model.ModeButton> ModeBtns { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void newFileClear() {
            SelectedNode = null;
            SelectedEdge = null;
            selectedNodesForConnect = new ObservableCollection<Model.Node>(new Model.Node[] { });
            SelectedMode = ModeBtns[0];
            ObservableCollection<Model.Node> emptyNodesArr = new ObservableCollection<Model.Node>(new Model.Node[] { });
            ObservableCollection<Model.Edge> emptyEdgesArr = new ObservableCollection<Model.Edge>(new Model.Edge[] { });
            arrMoveNode = new List<ThreeElement<Node, double, double>>();
            Workspace = new Model.Workspace { Nodes = emptyNodesArr, Edges = emptyEdgesArr };
            BackgroundCanvas = new Model.ObjectT<bool> { Value = true };
            DfsResShow = new ObjectT<bool> { Value = false };
            GraphFile = new Model.GraphFile { BSaved = true, FileName = "New file", FilePath = "", BNew = true };
            SelectedAlgorithm = new Pair<string, string>("", "");
            CounterName = 0;
            IsOriented = true;
            _URControl = new Controls.URControl();
            changeModeFuncs(new object());
        }
        public MainWindowViewModel() {
            ModeBtns = new ObservableCollection<Model.ModeButton>(Model.ModeButton.getDefaultModes(
                changeMode,
                createNode,
                deleteNode,
                setMovable,
                setUnMovable,
                selectNode,
                unselectNode,
                addNodeForConnect,
                deleteEdge,
                selectEdge,
                changeModeFuncs,
                mouseUp));
            canvasClickCommand = new DelegateCommand(createNode);
            changeGraphOrientedCommand = new DelegateCommand(changeGraphOriented);
            changeEdgeDirectionCommand = new DelegateCommand(changeEdgeDirection);
            newGraphCommand = new DelegateCommand(newGraph);
            loadGraphCommand = new DelegateCommand(loadGraph);
            saveGraphCommand = new DelegateCommand(saveGraph);
            saveAsGraphCommand = new DelegateCommand(saveAsGraph);
            undoCommand = new DelegateCommand(undo);
            redoCommand = new DelegateCommand(redo);
            nodeChangeNameCommand = new DelegateCommand(nodeNameChange);
            dfsCommand = new DelegateCommand(dfsHandler);
            closeAlgorithmWindowCommand = new DelegateCommand(closeAlgorithmWindow);
            helpCommand = new DelegateCommand(help);
            selectModeHotKeyCommand = new DelegateCommand(selectModeHotKey);
            appShutdownCommand = new DelegateCommand(appShutdown);
            minimizeWindowCommand = new DelegateCommand(minimizeWindow);
            normalMaxWindowCommand = new DelegateCommand(normalMaxWindow);
            clickWindowAlgorithmInfoCommand = new DelegateCommand(clickWindowAlgorithmInfo);
            newFileClear();
            WindowStateVar = WindowState.Normal;
            ResizeModeVar = ResizeMode.CanResizeWithGrip;
        }
        private bool _bCanUseModeFunc = true;
        private void clickWindowAlgorithmInfo(object sender) {
            _bCanUseModeFunc = false;
        }
        private void normalMaxWindow(object sender) {
            ResizeModeVar = WindowStateVar == WindowState.Normal ? ResizeMode.NoResize : ResizeMode.CanResizeWithGrip;
            WindowStateVar = WindowStateVar == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
        private void minimizeWindow(object sender) => WindowStateVar = WindowState.Minimized;
        private void appShutdown(object sender) {
            if (!GraphFile.BSaved) {
                MessageBoxResult mbRes = MessageBox.Show("Do you want to save the file before exiting the program?", "File save", MessageBoxButton.YesNoCancel);
                if (mbRes == MessageBoxResult.Yes) {
                    if (GraphFile.BNew)
                        saveAsGraph(sender);
                    else
                        saveGraph(sender);
                } else if (mbRes == MessageBoxResult.Cancel)
                    return;
            }
            App.Current.Shutdown();
        }
        private void selectModeHotKey(object sender) {
            int obj = int.Parse((string)sender);
            SelectedMode = ModeBtns[obj];
            changeModeFuncs(new object());
        }
        private void help(object sender) {
            const string addNodeInfo = "Node add: to select click on the button on the left side of the bar or do the combination of the 'Ctrl+1' keys.\n" +
                                       "To add a node, left-click on the workspace.\n";
            const string deleteNodeInfo = "\nNode remove: click on the button on the left side of the bar or do the combination of the 'Ctrl+2' keys.\n" +
                                          "To remove a node, left-click on the node you want to remove by left-click.\n";
            const string addEdgeInfo = "\nEdge add: to select click on the button on the left side of the bar or do the combination of the 'Ctrl+3' keys.\n" +
                                       "To add an edge, you need to select two nodes by left-clicking.\n";
            const string deleteEdgeInfo = "\nEdge remove: to select click on the button on the left side of the bar or do the combination of the 'Ctrl+4' keys.\n" +
                                          "To remove an edge, you need to select the edge you want to remove by left-click.\n";
            const string moveSelectInfo = "\nMove/select: to select click on the button on the left side of the bar or do the combination of the 'Ctrl+5' keys.\n" +
                                          "To move a node, hold down the left button on it and drag the mouse.\n" +
                                          "To select an element, left-click on it.\n";
            const string newfileInfo = "\nNew file: to create new file click on the menu 'File' and click on 'New' or do the combination of the 'Ctrl+N' keys.\n";
            const string openfileInfo = "\nOpen file: to open file click on the menu 'File' and click on 'Open' or do the combination of the 'Ctrl+O' keys.\n";
            const string savefileInfo = "\nSave file: to save file click on the menu 'File' and click on 'Save' or do the combination of the 'Ctrl+S' keys.\n";
            const string saveAsfileInfo = "\nSave as file: to save as file click on the menu 'File' and click on 'Save as' or click 'F12' key.\n";
            const string algorithmInfo = "\nAlgorithms: to perform any algorithm click on the menu 'Algorithms' and select it.\n";
            const string graphOrientationInfo = "\nGraph orientation: to change graph oriantation click on the button on the bottom side of the bar or do the combination of the 'Ctrl+B' keys.\n";
            const string resInfo = addNodeInfo + deleteNodeInfo + addEdgeInfo + deleteEdgeInfo + moveSelectInfo + newfileInfo + openfileInfo + savefileInfo + saveAsfileInfo + algorithmInfo + graphOrientationInfo;
            MessageBox.Show(resInfo, "Help menu",MessageBoxButton.OK ,MessageBoxImage.Question);
        }
        private List<Pair<Model.Node, bool>> unVisitedNode = new List<Pair<Model.Node, bool>>();
        private void dfs(Model.Node node) {
            foreach (var item in node.ConnectedNodes)
                foreach (var itemUnSelected in unVisitedNode)
                    if(item.First.Id == itemUnSelected.First.Id && !itemUnSelected.Second && (item.Second == "To" || item.Second == "T/Fr" || !graphOriented.Value)) {
                        SelectedAlgorithm.Second += $" {itemUnSelected.First.Name} >";
                        itemUnSelected.Second = true;
                        dfs(item.First);
                        break;
                    }
        }
        private void dfsHandler(object sender) {
            if(SelectedNode == null) {
                MessageBox.Show("Need to select a node to perform this operation");
                return;
            }
            SelectedAlgorithm.First = "DFS";
            SelectedAlgorithm.Second = $"{SelectedNode.Name} >";
            DfsResShow = new ObjectT<bool> { Value = true };
            foreach (var item in Workspace.Nodes)
                unVisitedNode.Add(new Pair<Node, bool>(item, item.Id == SelectedNode.Id));
            dfs(SelectedNode);
            SelectedAlgorithm.Second = SelectedAlgorithm.Second.Substring(0, SelectedAlgorithm.Second.Length - 1);
            unVisitedNode.Clear();
        }
        private void closeAlgorithmWindow(object sender) {
            DfsResShow = new ObjectT<bool> { Value = false };
        }
        private void mouseUp(object sender) {
            if (arrMoveNode.Count > 0) {
                Model.Node node = arrMoveNode.First().First;
                double prevPosX, prevPosY, PosX, PosY;
                prevPosX = arrMoveNode.First().Second;
                prevPosY = arrMoveNode.First().Third;
                PosX = arrMoveNode.Last().First.PositionX;
                PosY = arrMoveNode.Last().First.PositionY;
                URCommands.NodeMoveCommand NAC = new URCommands.NodeMoveCommand(
                    node,
                    prevPosX,
                    prevPosY,
                    PosX,
                    PosY,
                    Workspace.Nodes);
                URControl.Execute(NAC);
                arrMoveNode.Clear();
            }
        }
        private void undo(object sender) {
            URControl.Undo();
            changeModeFuncs(sender);
            bool checkSelected = false;
            foreach (var item in Workspace.Edges) {
                item.updatePos();
                item.BHaveDirection = graphOriented.Value;
                item.visibilityTriangleFrom = Visibility.Hidden;
                item.visibilityTriangleTo = Visibility.Hidden;
                if (SelectedEdge == item)
                    checkSelected = true;
            }
            if (!checkSelected) {
                SelectedEdge = null;
                foreach (var item in Workspace.Edges)
                    if (item.BSelected.Value != "Transparent")
                        item.BSelected = new ObjectT<string> { Value = "Transparent" };
            }
            checkSelected = false;
            foreach (var item in Workspace.Nodes)
                if (SelectedNode == item) {
                    checkSelected = true;
                    break;
                }
            if (!checkSelected) {
                SelectedNode = null;
                foreach (var item in Workspace.Nodes) 
                    if (item.BSelected.Value != "Transparent")
                        item.BSelected = new ObjectT<string> { Value = "Transparent" };
            }
        }
        private void redo(object sender) {
            URControl.Redo();
            changeModeFuncs(sender);
            bool checkSelected = false;
            foreach (var item in Workspace.Edges) {
                item.updatePos();
                item.BHaveDirection = graphOriented.Value;
                item.visibilityTriangleFrom = Visibility.Hidden;
                item.visibilityTriangleTo = Visibility.Hidden;
                if (SelectedEdge == item)
                    checkSelected = true;
            }
            if (!checkSelected) {
                SelectedEdge = null;
                foreach (var item in Workspace.Edges)
                    if (item.BSelected.Value != "Transparent")
                        item.BSelected = new ObjectT<string> { Value = "Transparent" };
            }
            checkSelected = false;
            foreach (var item in Workspace.Nodes)
                if (SelectedNode == item) {
                    checkSelected = true;
                    break;
                }
            if (!checkSelected) {
                SelectedNode = null;
                foreach (var item in Workspace.Nodes)
                    if (item.BSelected.Value != "Transparent")
                        item.BSelected = new ObjectT<string> { Value = "Transparent" };
            }
        }
        private void updateEdgePosition(object sender) {
            ThreeElement<Model.Node, double, double> obj = (ThreeElement<Model.Node, double, double>)sender;
            foreach (var item in Workspace.Edges)
                if(item.FromNode.Id == obj.First.Id || item.ToNode.Id == obj.First.Id)
                    item.updatePos();
            if(BCanMove)
                arrMoveNode.Add(obj);
            GraphFile.BSaved = false;
        }
        private void OnPropertyChanged([CallerMemberName]string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        private void changeMode(object obj) {
            SelectedMode = ((Button)obj).DataContext as Model.ModeButton;
            changeModeFuncs(obj);
        }
        private void selectNode(object sender) {
            if (SelectedNode != null) {
                SelectedNode.BSelected = new ObjectT<string> { Value = "Transparent" };
                SelectedNode.blurRadius = 10;
            }
            SelectedNode = ((Grid)sender).DataContext as Model.Node;
            SelectedNode.BSelected = new ObjectT<string> { Value = "#0000FF" };
            SelectedNode.blurRadius = 10;
            if(SelectedEdge != null) {
                SelectedEdge.BSelected = new ObjectT<string> { Value = "Transparent" };
                SelectedEdge.blurRadius = 10;
            }
            SelectedEdge = null;
        }
        private bool _bUselectEdge = true;
        private void selectEdge(object sender) {
            if (SelectedEdge != null) {
                SelectedEdge.BSelected = new ObjectT<string> { Value = "Transparent" };
                SelectedEdge.blurRadius = 10;
            }
            SelectedEdge = ((Grid)sender).DataContext as Model.Edge;
            if (SelectedEdge != null) {
                SelectedEdge.BSelected = new ObjectT<string> { Value = "#0000FF" };
                SelectedEdge.blurRadius = 10;
            }
            if (SelectedNode != null) {
                SelectedNode.BSelected = new ObjectT<string> { Value = "Transparent" };
                SelectedNode.blurRadius = 10;
            }
            SelectedNode = null;
            _bUselectEdge = false;
        }
        private void unselectNode(object sender) {
            if (SelectedNode != null) {
                SelectedNode.BSelected = new ObjectT<string> { Value = "Transparent" };
                SelectedNode.blurRadius = 10;
            }
            SelectedNode = null;
            if (_bUselectEdge) {
                if (SelectedEdge != null) {
                    SelectedEdge.BSelected = new ObjectT<string> { Value = "Transparent" };
                    SelectedEdge.blurRadius = 10;
                }
                SelectedEdge = null;
            } else
                _bUselectEdge = true;
        }
        private void addNodeForConnect(object sender) {
            if (!_bCanUseModeFunc) return;
            var obj = ((Grid)sender).DataContext as Model.Node;
            if (selectedNodesForConnect.Count == 1 && selectedNodesForConnect[0] != obj) {
                selectedNodesForConnect.Add(obj);
                URCommands.EdgeAddCommand NAC = new URCommands.EdgeAddCommand(
                    selectedNodesForConnect,
                    Workspace.Edges,
                    Workspace.Nodes,
                    new DelegateCommand(edgeWeightChange));
                URControl.Execute(NAC);
                foreach (var item in Workspace.Edges)
                    if(item.FromNode.Id == obj.Id || item.ToNode.Id == obj.Id) {
                        item.BHaveDirection = graphOriented.Value;
                        item.visibilityTriangleFrom = Visibility.Hidden;
                        item.visibilityTriangleTo = Visibility.Hidden;
                        break;
                    }
                selectedNodesForConnect.First().BSelected = new ObjectT<string> { Value = "Transparent" };
                selectedNodesForConnect.First().blurRadius = 0;
                if (SelectedNode != null && SelectedNode == selectedNodesForConnect[0]) {
                    SelectedNode.BSelected = new ObjectT<string> { Value = "#0000FF" };
                    SelectedNode.blurRadius = 10;
                }
                selectedNodesForConnect.Clear();
                GraphFile.BSaved = false;
            } else if (selectedNodesForConnect.Count == 0) {
                selectedNodesForConnect.Add(obj);
                selectedNodesForConnect.First().BSelected = new ObjectT<string> { Value = "#00FF00" };
                selectedNodesForConnect.First().blurRadius = 10;
            } 
        }
        private void deleteEdge(object sender) {
            if (!_bCanUseModeFunc) return;
            var obj = ((Grid)sender).DataContext as Model.Edge;
            URCommands.EdgeDeleteCommand NAC = new URCommands.EdgeDeleteCommand(
                    obj,
                    Workspace.Nodes,
                    Workspace.Edges);
            URControl.Execute(NAC);
            if (obj == SelectedEdge)
                SelectedEdge = null;
            GraphFile.BSaved = false;
        }
        private void changeModeFuncs(object sender) {
            if (sender is Border)
                SelectedMode = ((Border)sender).DataContext as Model.ModeButton;
            canvasClickCommand = SelectedMode.Funcs[0];
            foreach (var item in Workspace.Nodes) {
                item.deleteNode = SelectedMode.Funcs[1];
                item.MouseUp = SelectedMode.Funcs[4];
            }
            if (SelectedMode.Funcs[2] != null)
                SelectedMode.Funcs[2].Execute(new object());
            foreach (var item in Workspace.Edges)
                item.deleteEdge = SelectedMode.Funcs[3];
            BackgroundCanvas = new Model.ObjectT<bool> { Value = SelectedMode.Funcs[3] == null };
            if(selectedNodesForConnect.Count > 0) {
                selectedNodesForConnect.First().BSelected = new ObjectT<string> { Value = "Transparent" };
                selectedNodesForConnect.First().blurRadius = 0;
                selectedNodesForConnect.Clear();
            }
        }
        private void setMovable(object sender) {
            foreach (var item in Workspace.Nodes)
                item.bCanMove = true;
            BCanMove = true;
        }
        private void setUnMovable(object sender) {
            foreach (var item in Workspace.Nodes)
                item.bCanMove = false;
            BCanMove = false;
        }
        private void createNode(object sender) {
            if (!_bCanUseModeFunc) return;
            URCommands.NodeAddCommand NAC = new URCommands.NodeAddCommand(
                Workspace.Nodes, 
                PanelX, 
                PanelY, 
                new DelegateCommand(updateEdgePosition), 
                CounterName,
                BCanMove,
                new DelegateCommand(nodeNameChange));
            URControl.Execute(NAC);
            ++CounterName;
            GraphFile.BSaved = false;
        }
        private void deleteNode(object sender) {
            if (!_bCanUseModeFunc) return;
            var obj = ((Grid)sender).DataContext as Model.Node;
            URCommands.NodeDeleteCommand NAC = new URCommands.NodeDeleteCommand(
               obj,
               Workspace.Edges,
               Workspace.Nodes);
            URControl.Execute(NAC);
            if (SelectedEdge != null) {
                bool bEdgeExists = false;
                foreach (var item in Workspace.Edges)
                    if (item.FromNode.Id == SelectedEdge.FromNode.Id && item.ToNode.Id == SelectedEdge.ToNode.Id) {
                        bEdgeExists = true;
                        break;
                    }
                if (!bEdgeExists)
                    SelectedEdge = null;
            }
            if (obj == SelectedNode)
                SelectedNode = null;
            GraphFile.BSaved = false;
        }
        private void updateGraphOriented() {
            foreach (var item in Workspace.Edges) {
                item.BHaveDirection = IsOriented;
                item.visibilityTriangleFrom = Visibility.Hidden;
                item.visibilityTriangleTo = Visibility.Hidden;
            }
        }
        private void changeGraphOriented(object sender) {
            IsOriented = !IsOriented;
            graphOriented = new Model.ObjectT<bool>();
            updateGraphOriented();
            GraphFile.BSaved = false;
        }
        private void nodeNameChange(object sender) {
            var obj = (Pair<Model.Node, string>)sender;
            URCommands.NodeChangeNameCommand NAC = new URCommands.NodeChangeNameCommand(
               obj.First,
               obj.Second,
               Workspace.Nodes);
            URControl.Execute(NAC);
            GraphFile.BSaved = false;
        }
        private void edgeWeightChange(object sender) {
            var obj = (Pair<Model.Edge, int>)sender;
            URCommands.EdgeWeightChangeCommand NAC = new URCommands.EdgeWeightChangeCommand(
               obj.First,
               obj.Second,
               Workspace.Edges);
            URControl.Execute(NAC);
            GraphFile.BSaved = false;
        }
        private void changeEdgeDirection(object sender) {
            URCommands.EdgeChangeDirectionCommand NAC = new URCommands.EdgeChangeDirectionCommand(
               SelectedEdge,
               Workspace.Edges);
            URControl.Execute(NAC);
            GraphFile.BSaved = false;
        }
        private void newGraph(object sender) {
            try {
                if (GraphFile.BSaved) {
                    newFileClear();
                    return;
                }
                MessageBoxResult mbRes = MessageBox.Show("Do you want save file?", "File save", MessageBoxButton.YesNoCancel);
                if (mbRes == MessageBoxResult.Yes) {
                    if (GraphFile.BNew)
                        saveAsGraph(sender);
                } else if (mbRes == MessageBoxResult.Cancel)
                    return;
                else if (mbRes == MessageBoxResult.No) {
                    newFileClear();
                    return;
                }
                string filePath = GraphFile.FilePath;
                GraphFile.nodes = Workspace.Nodes.ToList();
                GraphFile.edges = Workspace.Edges.ToList();
                GraphFile.ConnectedNodes = new List<Pair<int, List<ThreeElement<Node, string, int>>>>();
                foreach (var item in Workspace.Nodes)
                    GraphFile.ConnectedNodes.Add(new Pair<int, List<ThreeElement<Model.Node, string, int>>>(item.Id, item.ConnectedNodes.ToList()));
                GraphFile.FileName = filePath.Split('\\').Last().Split('.').First();
                GraphFile.FilePath = filePath;
                GraphFile.BSaved = true;
                GraphFile.BNew = false;
                GraphFile.LastId = CounterName;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(GraphFile));
                using (Stream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    xmlSerializer.Serialize(fs, GraphFile);
                newFileClear();
            } catch (Exception err) {
                MessageBox.Show(err.Message);
            }
        }
        private void saveAsGraph(object sender) {
            try {
                SaveFileDialog saveFD = new SaveFileDialog() { Filter = "Graph App Data|*.xml" };
                if (saveFD.ShowDialog() == false)
                    return;
                string filePath = saveFD.FileName;
                GraphFile.nodes = Workspace.Nodes.ToList();
                GraphFile.edges = Workspace.Edges.ToList();
                GraphFile.ConnectedNodes = new List<Pair<int, List<ThreeElement<Node, string, int>>>>();
                foreach (var item in Workspace.Nodes)
                    GraphFile.ConnectedNodes.Add(new Pair<int, List<ThreeElement<Model.Node, string, int>>>(item.Id, item.ConnectedNodes.ToList()));
                GraphFile.FileName = filePath.Split('\\').Last().Split('.').First();
                GraphFile.FilePath = filePath;
                GraphFile.BSaved = true;
                GraphFile.BNew = false;
                GraphFile.LastId = CounterName;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(GraphFile));
                using (Stream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    xmlSerializer.Serialize(fs, GraphFile);
            } catch (Exception err) {
                MessageBox.Show(err.Message);
            }
        }
        private void load(OpenFileDialog openFD) {
            try {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(GraphFile));
                object openGraphFile = new object();
                using (Stream fs = new FileStream(openFD.FileName, FileMode.Open, FileAccess.Read))
                    openGraphFile = xmlSerializer.Deserialize(fs);
                newFileClear();
                Model.GraphFile gf = (Model.GraphFile)openGraphFile;
                GraphFile = gf;
                foreach (var item in gf.nodes) {
                    Workspace.Nodes.Add(new Model.Node {
                        bCanMove = true,
                        PositionX = item.PositionXDes,
                        PositionY = item.PositionYDes,
                        deleteNode = null,
                        updateEdgePosition = new DelegateCommand(updateEdgePosition),
                        Name = item.Name,
                        Id = item.Id
                    });
                    Workspace.Nodes[Workspace.Nodes.Count - 1].bCanMove = false;
                }
                foreach (var item in gf.edges) {
                    Model.Node FromNode = new Model.Node();
                    Model.Node ToNode = new Model.Node();
                    foreach (var item2 in Workspace.Nodes) {
                        if (item2.Id == item.FromNode.Id)
                            FromNode = item2;
                        else if (item2.Id == item.ToNode.Id)
                            ToNode = item2;
                    }
                    Workspace.Edges.Add(new Model.Edge {
                        FromNode = FromNode,
                        ToNode = ToNode,
                        deleteEdge = null,
                        ForItem1 = new ThreeElement<Model.Node, string, int>(ToNode, item.ForItem1.Second, item.ForItem1.Third),
                        ForItem2 = new ThreeElement<Model.Node, string, int>(FromNode, item.ForItem2.Second, item.ForItem2.Third),
                        Weight = item.Weight,
                        BHaveDirection = IsOriented,
                        WeightChangeHandler = new DelegateCommand(edgeWeightChange),
                        BInitialized = true
                    });
                }
                foreach (var item in gf.ConnectedNodes) {
                    Model.Node nodeConNodes = new Model.Node();
                    foreach (var itemFind in Workspace.Nodes.ToArray()) {
                        if (item.First == itemFind.Id) {
                            nodeConNodes = itemFind;
                            break;
                        }
                    }
                    foreach (var item2 in item.Second) {
                        Model.Node findToNode = new Model.Node();
                        foreach (var itemForAddFind in Workspace.Nodes.ToArray()) {
                            if (item2.First.Id == itemForAddFind.Id) {
                                findToNode = itemForAddFind;
                                break;
                            }
                        }
                        foreach (var itemEdge in Workspace.Edges) {
                            if (itemEdge.FromNode.Id == findToNode.Id && itemEdge.ToNode.Id == item.First) {
                                nodeConNodes.ConnectedNodes.Add(itemEdge.ForItem2);
                                findToNode.ConnectedNodes.Add(itemEdge.ForItem1);
                            }
                        }
                    }
                }
                GraphFile = gf;
                CounterName = gf.LastId;
            } catch (Exception err) {
                MessageBox.Show(err.Message);
            }
        }
        private void loadGraph(object sender) {
            try {
                OpenFileDialog openFD = new OpenFileDialog() { Filter = "Graph App Data|*.xml" };
                if (openFD.ShowDialog() == false)
                    return;
                if (GraphFile.BSaved) {
                    load(openFD);
                    return;
                }
                MessageBoxResult mbRes = MessageBox.Show("Do you want save file?", "File save", MessageBoxButton.YesNoCancel);
                if (mbRes == MessageBoxResult.Yes) {
                    if (GraphFile.BNew)
                        saveAsGraph(sender);
                    else
                        saveGraph(sender);
                    load(openFD);
                } else if (mbRes == MessageBoxResult.No)
                    load(openFD);
                else
                    return;
            } catch (Exception err) {
                MessageBox.Show(err.Message);
            }
        }
        private void saveGraph(object sender) {
            try {
                if (GraphFile.BNew) {
                    saveAsGraph(sender);
                    return;
                }
                string filePath = GraphFile.FilePath;
                GraphFile.nodes = Workspace.Nodes.ToList();
                GraphFile.edges = Workspace.Edges.ToList();
                GraphFile.ConnectedNodes = new List<Pair<int, List<ThreeElement<Node, string, int>>>>();
                foreach (var item in Workspace.Nodes)
                    GraphFile.ConnectedNodes.Add(new Pair<int, List<ThreeElement<Model.Node, string, int>>>(item.Id, item.ConnectedNodes.ToList()));
                GraphFile.FileName = filePath.Split('\\').Last().Split('.').First();
                GraphFile.FilePath = filePath;
                GraphFile.BSaved = true;
                GraphFile.BNew = false;
                GraphFile.LastId = CounterName;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(GraphFile));
                using (Stream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    xmlSerializer.Serialize(fs, GraphFile);
            } catch (Exception err) {
                MessageBox.Show(err.Message);
            }
        }
    }
}