using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.URCommands {
    class EdgeWeightChangeCommand : ICommandUR {
        private Model.Edge edge;
        private ObservableCollection<Model.Edge> edges;
        private int weightEdge;
        private int prevWeightEdge;
        private bool bChange;
        public EdgeWeightChangeCommand(Model.Edge edge, int prevWeightEdge, ObservableCollection<Model.Edge> edges) {
            this.edge = edge;
            this.weightEdge = edge.Weight;
            this.prevWeightEdge = prevWeightEdge;
            this.edges = edges;
            bChange = false;
        }
        public string Name { get => "Edge change weight"; }
        public void Execute() {
            if (bChange) {
                foreach (var item in edges)
                    if (item.FromNode.Id == edge.FromNode.Id && item.ToNode.Id == edge.ToNode.Id) {
                        edge = item;
                        break;
                    }
                edge.WeightUR = weightEdge;
                edge.Weight = weightEdge;
            } else
                bChange = true;
        }
        public void UnExecute() {
            edge.WeightUR = prevWeightEdge;
            edge.Weight = prevWeightEdge;
        }
    }
}
