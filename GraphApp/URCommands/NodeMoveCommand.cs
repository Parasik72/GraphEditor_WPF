using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphApp.URCommands {
    class NodeMoveCommand : ICommandUR {
        private Model.Node node;
        private double prevPosX;
        private double prevPosY;
        private double PosX;
        private double PosY;
        private ObservableCollection<Model.Node> nodes;
        public NodeMoveCommand(Model.Node node, double prevPosX, double prevPosY, double PosX, double PosY, ObservableCollection<Model.Node> nodes) {
            this.node = node;
            this.prevPosX = prevPosX;
            this.prevPosY = prevPosY;
            this.PosX = PosX;
            this.PosY = PosY;
            this.nodes = nodes;
        }
        public string Name { get => "Node move"; }
        public void Execute() {
            foreach (var item in nodes)
                if(item.Id == node.Id) {
                    node = item;
                    break;
                }
            bool bCanMove = node.bCanMove;
            node.bCanMove = true;
            node.PositionX = PosX;
            node.PositionY = PosY;
            node.bCanMove = bCanMove;
        }
        public void UnExecute() {
            bool bCanMove = node.bCanMove;
            node.bCanMove = true;
            node.PositionX = prevPosX;
            node.PositionY = prevPosY;
            node.bCanMove = bCanMove;
        }
    }
}
