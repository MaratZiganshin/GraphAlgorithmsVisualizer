using System.Collections.Generic;
using GraphAlgorithmsVisualizer.UI;
using System.Windows.Media;
using GraphAlgorithmsVisualizer.Events;
using System.IO;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public class TopologicalSort : IAlgorithm
    {
        private const string path = "GraphAlgorithmsVisualizer.AlgorithmCodes.topsort.txt";
        private SolidColorBrush _edgeStartColor;
        private SolidColorBrush _vertexStartColor;
        private Dictionary<VertexUI, SolidColorBrush> _color;
        private Graph _graph;
        private List<VertexUI> sorted_list;
        public EventStack EventStack { get; }
        public List<CodeLine> Lines { get; }
        public bool HasOutput { get; }

        public TopologicalSort(Graph graph)
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
            sorted_list = new List<VertexUI>();
            HasOutput = true;
            var code = FileUtils.ReadCode(path);
            Lines = new List<CodeLine>();
            foreach (var line in code)
            {
                var codeLine = new CodeLine();
                codeLine.SetText(line);
                Lines.Add(codeLine);
            }
        }

        public void Run(VertexUI startVertex)
        {
            AddEmptyEvent(8, createVariables(null));
            foreach (VertexUI vertex in _graph.AdjacencyList.Keys)
            {
                var vertexPaint = new VertexPaint(vertex, _color[vertex], new SolidColorBrush(Colors.Red), Lines[9], createVariables(vertex));
                EventStack.Events.Add(vertexPaint);
                AddEmptyEvent(10, createVariables(vertex));
                if (ColorEqualsComparer.Instance.Equals(_color[vertex], _vertexStartColor))
                {
                    AddEmptyEvent(11, createVariables(vertex));
                    TopSort(vertex);
                }
                else
                {
                    vertexPaint = new VertexPaint(vertex, new SolidColorBrush(Colors.Red), _color[vertex], Lines[9], createVariables(vertex));
                    vertexPaint.IsVisualizable = false;
                    EventStack.Events.Add(vertexPaint);
                }
            }
            AddEmptyEvent(12, createVariables(null));
        }

        public void TopSort(VertexUI vertex)
        {
            var vertexPaint = new VertexPaint(vertex, _color[vertex], new SolidColorBrush(Colors.Gray), Lines[1], createVariables(vertex));
            EventStack.Events.Add(vertexPaint);
            _color[vertex] = new SolidColorBrush(Colors.Gray);
            foreach (var edge in _graph.AdjacencyList[vertex])
            {
                var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, new SolidColorBrush(Colors.Red), Lines[2], createVariables(vertex));
                EventStack.Events.Add(edgePaint);
                var endVertex = edge.EndVertex;
                AddEmptyEvent(3, createVariables(vertex));
                if (ColorEqualsComparer.Instance.Equals(_color[endVertex], _vertexStartColor))
                {
                    AddEmptyEvent(4, createVariables(vertex));
                    TopSort(endVertex);
                }
                edgePaint = new EdgePaintAndThicken(edge, new SolidColorBrush(Colors.Red), _edgeStartColor, Lines[2], createVariables(vertex));
                edgePaint.IsVisualizable = false;
                edgePaint.IsReverse = true;
                EventStack.Events.Add(edgePaint);
            }
            sorted_list.Add(vertex);
            AddEmptyEvent(5, createVariables(vertex));
        }

        public List<string> GetOutput()
        {
            string result = "";
            foreach (var v in sorted_list)
            {
                result = result + v.Text + ";";
            }
            return new List<string>() { result };
        }

        private void AddEmptyEvent(int lineNumber, Dictionary<string, string> locals)
        {
            var emptyEvent = new EmptyEvent(Lines[lineNumber], locals);
            EventStack.Events.Add(emptyEvent);
        }

        private Dictionary<string, string> createVariables(VertexUI vertex)
        {
            var result = new Dictionary<string, string>();
            if (sorted_list != null)
            {
                List<string> listStrings = new List<string>();
                foreach (var v in sorted_list)
                {
                    listStrings.Add(v.Text);
                }
                result.Add("sorted_list", string.Join(", ", listStrings));
            }
            if (vertex != null)
            {
                result.Add("vertex", vertex.Text);
            }
            
            return result;
        }
    }
}
