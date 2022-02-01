using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace GraphApp.Controls {
    public interface IMoveThumb { 
        double PositionX { get; set; }
        double PositionY { get; set; }
        ICommand updateEdgePosition { get; set; }
    }
    public class MoveThumb : Thumb {
        public MoveThumb() {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }
        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e) {
            if(sender is Thumb) {
                Thumb thumb = (Thumb)sender;
                Model.Node node = ((MoveThumb)sender).DataContext as Model.Node;
                double prevPosX = node.PositionX;
                double prevPosY = node.PositionY;
                IMoveThumb myEllipse = (IMoveThumb)thumb.DataContext;
                double maxX = System.Windows.SystemParameters.PrimaryScreenWidth - 285,
                       maxY = System.Windows.SystemParameters.PrimaryScreenHeight - 97;
                if (myEllipse.PositionX + e.HorizontalChange > 0 && myEllipse.PositionX + e.HorizontalChange < maxX)
                    myEllipse.PositionX += e.HorizontalChange;
                if (myEllipse.PositionY + e.VerticalChange > 0 && myEllipse.PositionY + e.VerticalChange < maxY)
                    myEllipse.PositionY += e.VerticalChange;
                if (myEllipse.PositionX < 0)
                    myEllipse.PositionX = 0;
                if (myEllipse.PositionY < 0)
                    myEllipse.PositionY = 0;
                if(myEllipse.PositionX > maxX)
                    myEllipse.PositionX = maxX;
                if (myEllipse.PositionY > maxY)
                    myEllipse.PositionY = maxY;
                myEllipse.updateEdgePosition.Execute(new ThreeElement<Model.Node, double, double>(node, prevPosX, prevPosY));
            }
        }
    }
}
