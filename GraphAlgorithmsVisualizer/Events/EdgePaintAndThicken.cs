using GraphAlgorithmsVisualizer.UI;
using System.Windows.Media;
using System;
using System.Collections.Generic;

namespace GraphAlgorithmsVisualizer.Events
{
    public class EdgePaintAndThicken : EdgePaint
    {
        public bool IsReverse { get; set; }

        public EdgePaintAndThicken(Edge edge, SolidColorBrush startColor, SolidColorBrush endColor, CodeLine line, Dictionary<string, string> locals) : base(edge, startColor, endColor, line, locals)
        {
            IsReverse = false;
        }
        public override void Run()
        {
            base.Run();
            if (!IsReverse)
            {
                _edge.UIShape.arrow.line.StrokeThickness = 7;
                _edge.UIShape.arrow.polyLine.StrokeThickness = 7;
                _edge.UIShape.revertArrow.line.StrokeThickness = 7;
                _edge.UIShape.revertArrow.polyLine.StrokeThickness = 7;
            }
            else
            {
                _edge.UIShape.arrow.line.StrokeThickness = 2;
                _edge.UIShape.arrow.polyLine.StrokeThickness = 2;
                _edge.UIShape.revertArrow.line.StrokeThickness = 2;
                _edge.UIShape.revertArrow.polyLine.StrokeThickness = 2;
            }
        }
        public override void PlayBack()
        {
            base.PlayBack();
            if (!IsReverse)
            {
                _edge.UIShape.arrow.line.StrokeThickness = 2;
                _edge.UIShape.arrow.polyLine.StrokeThickness = 2;
                _edge.UIShape.revertArrow.line.StrokeThickness = 2;
                _edge.UIShape.revertArrow.polyLine.StrokeThickness = 2;
            }
            else
            {
                _edge.UIShape.arrow.line.StrokeThickness = 7;
                _edge.UIShape.arrow.polyLine.StrokeThickness = 7;
                _edge.UIShape.revertArrow.line.StrokeThickness = 7;
                _edge.UIShape.revertArrow.polyLine.StrokeThickness = 7;
            }
        }
    }
}
