using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.URCommands {
    public class NodeDeleteCommand : ICommandUR {
        private Model.Node node;
        private ObservableCollection<Model.Edge> edges;
        private ObservableCollection<Model.Node> nodes;
        private ObservableCollection<Pair<Model.Node, ThreeElement<Model.Node, string, int>>> arrNodeBack;
        private ObservableCollection<Model.Edge> arrForDelEdge;
        public NodeDeleteCommand(Model.Node node, ObservableCollection<Model.Edge> edges, ObservableCollection<Model.Node> nodes) {
            this.node = node;
            this.edges = edges;
            this.nodes = nodes;
        }
        public string Name {  get => "Delete node"; }
        public void Execute() {
            foreach (var item in nodes)
                if(item.Id == node.Id) {
                    node = item;
                    break;
                }
            arrNodeBack = new ObservableCollection<Pair<Model.Node, ThreeElement<Model.Node, string, int>>>();
            ObservableCollection<ThreeElement<Model.Node, string, int>> arrForDel = new ObservableCollection<ThreeElement<Model.Node, string, int>>(new ThreeElement<Model.Node, string, int>[] { });
            foreach (var item in node.ConnectedNodes) {
                foreach (var itemForCheck in item.First.ConnectedNodes)
                    if (itemForCheck.First == node)
                        arrForDel.Add(itemForCheck);
                foreach (var itemForDel in arrForDel) {
                    arrNodeBack.Add(new Pair<Model.Node, ThreeElement<Model.Node, string, int>>(item.First, itemForDel));
                    item.First.ConnectedNodes.Remove(itemForDel);
                }
                arrForDel.Clear();
            }
            arrForDelEdge = new ObservableCollection<Model.Edge>(new Model.Edge[] { });
            foreach (var item in edges)
                if (item.FromNode == node || item.ToNode == node)
                    arrForDelEdge.Add(item);
            foreach (var item in arrForDelEdge)
                edges.Remove(item);
            nodes.Remove(node);
        }
        public void UnExecute() {
            nodes.Add(node);
            foreach (var itemDeleted in arrNodeBack)
                itemDeleted.First.ConnectedNodes.Add(itemDeleted.Second);
            foreach (var item in arrForDelEdge)
                edges.Add(item);
        }
    }
}