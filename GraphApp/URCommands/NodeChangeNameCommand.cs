using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.URCommands {
    class NodeChangeNameCommand : ICommandUR {
        private Model.Node node;
        private ObservableCollection<Model.Node> Nodes;
        private string NameNode;
        private string prevNameNode;
        private bool bChange;
        public NodeChangeNameCommand(Model.Node node, string prevName, ObservableCollection<Model.Node> Nodes) {
            this.node = node;
            this.NameNode = node.Name;
            this.prevNameNode = prevName;
            this.Nodes = Nodes;
            bChange = false;
        }
        public string Name { get => "Node change name"; }
        public void Execute() {
            if (bChange) {
                foreach (var item in Nodes)
                    if(item.Id == node.Id) {
                        node = item; 
                        break;
                    }
                node.NameUR = NameNode;
            } else
                bChange = true;
        }
        public void UnExecute() {
            node.NameUR = prevNameNode;
        }
    }
}
