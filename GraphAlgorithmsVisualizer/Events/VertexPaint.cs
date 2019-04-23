using GraphAlgorithmsVisualizer.UI;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphAlgorithmsVisualizer.Events
{
    public class VertexPaint : IEvent
    {
        private VertexUI _vertex;
        private SolidColorBrush _startColor;
        private SolidColorBrush _endColor;
        public Dictionary<string, string> Locals { get; }
        public CodeLine Line { get; }
        public bool IsVisualizable { get; set; }
        public VertexPaint(VertexUI vertex, SolidColorBrush startColor, SolidColorBrush endColor, CodeLine line, Dictionary<string, string> locals)
        {
            _vertex = vertex;
            _startColor = startColor;
            _endColor = endColor;
            Line = line;
            Locals = locals;
            IsVisualizable = true;
        }
        public void Run()
        {
            _vertex.Fill = _endColor;
        }
        public void PlayBack()
        {
            _vertex.Fill = _startColor;
        }
    }
}
