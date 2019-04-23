using GraphAlgorithmsVisualizer.Events;
using GraphAlgorithmsVisualizer.UI;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public class DFS : IAlgorithm
    {
        private const string path = "GraphAlgorithmsVisualizer.AlgorithmCodes.dfs.txt";
        private SolidColorBrush _edgeStartColor;
        private SolidColorBrush _vertexStartColor;
        private Dictionary<VertexUI, SolidColorBrush> _color;
        private Graph _graph;
        public List<CodeLine> Lines { get; }
        public EventStack EventStack { get; private set; }
        public bool HasOutput { get; }

        public DFS(Graph graph)
        {
            _graph = graph;
            EventStack = new EventStack()
            {
                Events = new List<IEvent>()
            };
            _color = new Dictionary<VertexUI, SolidColorBrush>(graph.AdjacencyList.Count);
            _edgeStartColor = new SolidColorBrush(Colors.Black);
            foreach (var pair in graph.AdjacencyList)
            {
                _color.Add(pair.Key, pair.Key.Fill as SolidColorBrush);
                _vertexStartColor = pair.Key.Fill as SolidColorBrush;
            }
            HasOutput = false;
            var _code = FileUtils.ReadCode(path);
            Lines = new List<CodeLine>();
            foreach (var line in _code)
            {
                var codeLine = new CodeLine();
                codeLine.SetText(line);
                Lines.Add(codeLine);
            }
        }

        public void Run(VertexUI start)
        {
            AddEmptyEvent(0, createVariables(start));
            Repaint(start, new SolidColorBrush(Colors.Gray), 1);
            foreach (var edge in _graph.AdjacencyList[start])
            {
                var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, new SolidColorBrush(Colors.Red), Lines[2], createVariables(start));
                EventStack.Events.Add(edgePaint);
                var endVertex = edge.EndVertex;
                AddEmptyEvent(3, createVariables(start));
                if (ColorEqualsComparer.Instance.Equals(_color[endVertex], _vertexStartColor))
                {
                    AddEmptyEvent(4, createVariables(start));
                    Run(endVertex);
                }
                edgePaint = new EdgePaintAndThicken(edge, new SolidColorBrush(Colors.Red), _edgeStartColor, Lines[2], createVariables(start));
                edgePaint.IsVisualizable = false;
                edgePaint.IsReverse = true;
                EventStack.Events.Add(edgePaint);
            }
            Repaint(start, new SolidColorBrush(Colors.Black), 5);
        }

        public List<string> GetOutput()
        {
            return new List<string>();
        }

        private void AddEmptyEvent(int lineNumber, Dictionary<string, string> locals)
        {
            var emptyEvent = new EmptyEvent(Lines[lineNumber], locals);
            EventStack.Events.Add(emptyEvent);
        }

        private void Repaint(VertexUI vertex, SolidColorBrush color, int lineNumber)
        {
            var vertexPaint = new VertexPaint(vertex, _color[vertex], color, Lines[lineNumber], createVariables(vertex));
            _color[vertex] = color;
            EventStack.Events.Add(vertexPaint);
        }

        private Dictionary<string, string> createVariables(VertexUI vertex)
        {
            var result = new Dictionary<string, string>();
            result.Add("startVertex", vertex.Text);
            return result;
        }
    }
}
