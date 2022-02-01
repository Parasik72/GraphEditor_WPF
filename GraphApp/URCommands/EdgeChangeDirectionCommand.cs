using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphApp.URCommands {
    class EdgeChangeDirectionCommand : ICommandUR {
        private Model.Edge edge;
        private ObservableCollection<Model.Edge> edges;
        private string forItem1Second;
        private string forItem2Second;
        public EdgeChangeDirectionCommand(Model.Edge edge, ObservableCollection<Model.Edge> edges) {
            this.edge = edge;
            this.edges = edges;
        }
        public string Name { get => "Edge change direction"; }
        private string[] edgeDirections = new string[] { "To", "From", "T/Fr" };
        public void Execute() {
            foreach (var item in edges) {
                if(item.FromNode.Id == edge.FromNode.Id && item.ToNode.Id == edge.ToNode.Id) {
                    edge = item;
                    break;
                }
            }
            int i = 0;
            for (; i < edgeDirections.Length; ++i)
                if (edgeDirections[i] == edge.ForItem1.Second)
                    break;
            forItem1Second = edge.ForItem1.Second;
            forItem2Second = edge.ForItem2.Second;
            edge.ForItem1.Second = edgeDirections[(i + 1) % edgeDirections.Length];
            if (edge.ForItem1.Second == "To")
                edge.ForItem2.Second = "From";
            else if (edge.ForItem1.Second == "From") 
                edge.ForItem2.Second = "To";
            else 
                edge.ForItem2.Second = "T/Fr";
            edge.visibilityTriangleFrom = Visibility.Visible;
            edge.visibilityTriangleTo = Visibility.Visible;
        }
        public void UnExecute() {
            edge.ForItem1.Second = forItem1Second;
            edge.ForItem2.Second = forItem2Second;
            edge.visibilityTriangleFrom = Visibility.Visible;
            edge.visibilityTriangleTo = Visibility.Visible;
        }
    }
}