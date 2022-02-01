using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.URCommands {
    public class NodeAddCommand : ICommandUR {
        private Model.Node node;
        private ObservableCollection<Model.Node> Nodes;
        private double PanelX;
        private double PanelY;
        private DelegateCommand updateEdgePosition;
        private DelegateCommand NameChangeHandler;
        private int CounterName;
        private bool BCanMove;
        public NodeAddCommand(ObservableCollection<Model.Node> Nodes, double PanelX, double PanelY, DelegateCommand updateEdgePosition, int CounterName,  bool BCanMove, DelegateCommand NameChangeHandler) {
            this.node = null;
            this.Nodes = Nodes;
            this.PanelX = PanelX;
            this.PanelY = PanelY;
            this.updateEdgePosition = updateEdgePosition;
            this.NameChangeHandler = NameChangeHandler;
            this.CounterName = CounterName;
            this.BCanMove = BCanMove;
        }
        public string Name { get => "Node add"; }
        public void Execute() {
            double maxX = System.Windows.SystemParameters.PrimaryScreenWidth - 285,
                   maxY = System.Windows.SystemParameters.PrimaryScreenHeight - 97;
            double posX = PanelX - 17;
            double posY = PanelY - 17;
            if (PanelX - 17 <= 0)
                posX = PanelX;
            else if (PanelX - 17 > maxX)
                posX = maxX;
            if (PanelY - 17 <= 0)
                posY = PanelY;
            else if (PanelY - 17 > maxY)
                posY = maxY;
            node = new Model.Node {
                bCanMove = true,
                PositionX = posX,
                PositionY = posY,
                deleteNode = null,
                MouseUp = null,
                updateEdgePosition = updateEdgePosition,
                Name = (CounterName % 1000).ToString(),
                Id = CounterName,
                ConnectedNodes = new ObservableCollection<ThreeElement<Model.Node, string, int>>(),
                NameChangeHandler = NameChangeHandler,
                BInitialized = true
            };
            node.bCanMove = BCanMove;
            Nodes.Add(node);
        }
        public void UnExecute() {
            Nodes.Remove(node);
        }
    }
}
