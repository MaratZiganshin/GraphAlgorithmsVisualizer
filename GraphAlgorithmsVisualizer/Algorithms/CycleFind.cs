using System.Collections.Generic;
using GraphAlgorithmsVisualizer.UI;
using System.Windows.Media;
using GraphAlgorithmsVisualizer.Events;
using System.IO;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public class CycleFind : IAlgorithm
    {
        private const string path = "GraphAlgorithmsVisualizer.AlgorithmCodes.cyclefind.txt";
        private SolidColorBrush _edgeStartColor;
        private SolidColorBrush _vertexStartColor;
        private Dictionary<VertexUI, SolidColorBrush> _color;
        private Graph _graph;
        private bool stopped;
        public List<CodeLine> Lines { get; }
        public EventStack EventStack { get; private set; }
        public bool HasOutput { get; }
        public CycleFind(Graph graph)
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
            stopped = false;
            var _code = FileUtils.ReadCode(path);
            Lines = new List<CodeLine>();
            foreach (var line in _code)
            {
                var codeLine = new CodeLine();
                codeLine.SetText(line);
                Lines.Add(codeLine);
            }
        }
        public List<string> GetOutput()
        {
            return new List<string>();
        }

        public void Run(VertexUI start)
        {
            foreach (var v in _graph.AdjacencyList.Keys)
            {
                if (!stopped)
                {
                    AddEmptyEvent(10, createVariables(v));
                    AddEmptyEvent(11, createVariables(v));
                    if (ColorEqualsComparer.Instance.Equals(_color[v], _vertexStartColor))
                    {
                        AddEmptyEvent(12, createVariables(v));
                        Dfs(v);
                    }
                }
            }
        }

        private void Dfs(VertexUI v)
        {
            Repaint(v, new SolidColorBrush(Colors.Gray), 1);
            foreach (var edge in _graph.AdjacencyList[v])
            {
                if (!stopped)
                {
                    var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, new SolidColorBrush(Colors.Red), Lines[2], createVariables(v));
                    EventStack.Events.Add(edgePaint);
                    var endVertex = edge.EndVertex;
                    AddEmptyEvent(3, createVariables(v));
                    if (ColorEqualsComparer.Instance.Equals(_color[endVertex], _vertexStartColor))
                    {
                        AddEmptyEvent(4, createVariables(v));
                        Dfs(endVertex);
                    }
                    AddEmptyEvent(5, createVariables(v));
                    if (ColorEqualsComparer.Instance.Equals(_color[endVertex], new SolidColorBrush(Colors.Gray)))
                    {
                        AddEmptyEvent(6, createVariables(v));
                        stopped = true;
                    }
                    edgePaint = new EdgePaintAndThicken(edge, new SolidColorBrush(Colors.Red), _edgeStartColor, Lines[2], createVariables(v));
                    edgePaint.IsVisualizable = false;
                    edgePaint.IsReverse = true;
                    EventStack.Events.Add(edgePaint);
                }
            }
            if (!stopped)
                Repaint(v, new SolidColorBrush(Colors.Black), 7);
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

        private Dictionary<string, string> createVariables(VertexUI v)
        {
            var result = new Dictionary<string, string>();
            result.Add("v", v.Text);
            return result;
        }
    }
}
