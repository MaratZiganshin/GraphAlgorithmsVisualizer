using GraphAlgorithmsVisualizer.UI;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace GraphAlgorithmsVisualizer.Events
{
    public class EdgePaint : IEvent
    {
        protected Edge _edge;
        private SolidColorBrush _startColor;
        private SolidColorBrush _endColor;

        public Dictionary<string, string> Locals { get; }

        public CodeLine Line { get; }
        public bool IsVisualizable { get; set; }

        public EdgePaint(Edge edge, SolidColorBrush startColor, SolidColorBrush endColor, CodeLine line, Dictionary<string, string> locals)
        {
            _edge = edge;
            _startColor = startColor;
            _endColor = endColor;
            Line = line;
            Locals = locals;
            IsVisualizable = true;
        }
        public virtual void Run()
        {
            _edge.UIShape.Fill = _endColor;
        }
        public virtual void PlayBack()
        {
            _edge.UIShape.Fill = _startColor;
        }
    }
}
