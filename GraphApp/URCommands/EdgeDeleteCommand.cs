using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.URCommands {
    class EdgeDeleteCommand : ICommandUR {
        private Model.Edge edge;
        private ObservableCollection<Model.Node> nodes;
        private ObservableCollection<Model.Edge> edges;
        private ThreeElement<Model.Node, string, int> ForItem1;
        private ThreeElement<Model.Node, string, int> ForItem2;
        public EdgeDeleteCommand(Model.Edge edge, ObservableCollection<Model.Node> nodes, ObservableCollection<Model.Edge> edges) {
            this.edge = edge;
            this.nodes = nodes;
            this.edges = edges;
        }
        public string Name { get => "Edge delete"; }
        public void Execute() {
            foreach (var item in edges)
                if(item.FromNode.Id == edge.FromNode.Id && item.ToNode.Id == edge.ToNode.Id) {
                    edge = item;
                    break;
                }
            bool bItem1 = false, bItem2 = false;
            foreach (var item in nodes) {
                if(item.Id == edge.ForItem1.First.Id) {
                    edge.ForItem1.First = item;
                    bItem1 = true;
                } else if(item.Id == edge.ForItem2.First.Id) {
                    edge.ForItem2.First = item;
                    bItem1 = true;
                }
                if (bItem1 && bItem2)
                    break;
            }
            ForItem1 = edge.ForItem1;
            ForItem2 = edge.ForItem2;
            edge.FromNode.ConnectedNodes.Remove(edge.ForItem1);
            edge.ToNode.ConnectedNodes.Remove(edge.ForItem2);
            edges.Remove(edge);
        }
        public void UnExecute() {
            edge.FromNode.ConnectedNodes.Add(ForItem1);
            edge.ToNode.ConnectedNodes.Add(ForItem2);
            edges.Add(edge);
        }
    }
}
