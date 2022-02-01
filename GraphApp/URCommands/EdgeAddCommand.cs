using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphApp.URCommands {
    class EdgeAddCommand : ICommandUR {
        private ObservableCollection<Model.Edge> edges;
        private ObservableCollection<Model.Node> nodes;
        private Model.Edge edge;
        private Model.Node item1;
        private Model.Node item2;
        private ThreeElement<Model.Node, string, int> forItem1;
        private ThreeElement<Model.Node, string, int> forItem2;
        private DelegateCommand edgeWeightChange;
        public EdgeAddCommand(ObservableCollection<Model.Node> selectedNodesForConnect, ObservableCollection<Model.Edge> edges, ObservableCollection<Model.Node> nodes, DelegateCommand edgeWeightChange) {
            this.edges = edges;
            this.nodes = nodes;
            this.edgeWeightChange = edgeWeightChange;
            forItem1 = new ThreeElement<Model.Node, string, int>();
            forItem2 = new ThreeElement<Model.Node, string, int>();
            edge = new Model.Edge();
            item1 = selectedNodesForConnect[0];
            item2 = selectedNodesForConnect[1];
        }
        public string Name { get => "Edge add"; }
        public void Execute() {
            bool checkItem = true, bItem1 = false, bItem2 = false;
            foreach (var item in nodes) {
                if(item.Id == item1.Id) {
                    item1 = item;
                    bItem1 = true;
                } else if (item.Id == item2.Id) {
                    item2 = item;
                    bItem2 = true;
                }
                if (bItem1 && bItem2)
                    break;
            }
            forItem1 = new ThreeElement<Model.Node, string, int>(item2, "To", 1);
            forItem2 = new ThreeElement<Model.Node, string, int>(item1, "From", 1);
            foreach (var item in item1.ConnectedNodes) {
                if (item.First == forItem1.First && item.Second != forItem1.Second) {
                    item.Second = "T/Fr";
                    checkItem = false;
                    break;
                } else if (item.First == forItem1.First) {
                    checkItem = false;
                    break;
                }
            }
            foreach (var item in item2.ConnectedNodes) {
                if (item.First == forItem2.First && item.Second != forItem2.Second) {
                    item.Second = "T/Fr";
                    checkItem = false;
                    break;
                } else if (item.First == forItem2.First) {
                    checkItem = false;
                    break;
                }
            }
            if (checkItem) {
                item1.ConnectedNodes.Add(forItem1);
                item2.ConnectedNodes.Add(forItem2);
                edge = new Model.Edge {
                    FromNode = item1,
                    ToNode = item2,
                    deleteEdge = null,
                    ForItem1 = forItem1,
                    ForItem2 = forItem2,
                    Weight = 1,
                    BHaveDirection = true,
                    WeightChangeHandler = edgeWeightChange,
                    BInitialized = true
                };
                edges.Add(edge);
            }
        }
        public void UnExecute() {
            item1.ConnectedNodes.Remove(forItem1);
            item2.ConnectedNodes.Remove(forItem2);
            edges.Remove(edge);
        }
    }
}
