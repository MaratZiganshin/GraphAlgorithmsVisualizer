using System.Collections.Generic;
using GraphAlgorithmsVisualizer.UI;
using System.Windows.Media;
using GraphAlgorithmsVisualizer.Events;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public class BellmanFord : IAlgorithm
    {
        private const string path = "GraphAlgorithmsVisualizer.AlgorithmCodes.bellmanford.txt";
        private SolidColorBrush _edgeStartColor;
        private SolidColorBrush _vertexStartColor;
        private Dictionary<VertexUI, long> _distances;
        private Dictionary<VertexUI, VertexUI> _parents;
        private Graph _graph;
        public bool HasOutput { get; }
        public EventStack EventStack { get; }
        public List<CodeLine> Lines { get; }

        public BellmanFord(Graph graph)
        {
            _graph = graph;
            EventStack = new EventStack()
            {
                Events = new List<IEvent>()
            };
            _edgeStartColor = new SolidColorBrush(Colors.Black);
            _distances = new Dictionary<VertexUI, long>(graph.AdjacencyList.Count);
            _parents = new Dictionary<VertexUI, VertexUI>(graph.AdjacencyList.Count);
            foreach (var pair in graph.AdjacencyList)
            {
                _distances[pair.Key] = int.MaxValue;
                _parents[pair.Key] = null;
                _vertexStartColor = pair.Key.Fill as SolidColorBrush;
                pair.Key.descriptions.Add("distance", "infinity");
                pair.Key.descriptions.Add("parent", "null");
            }

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
            ChangeDistance(startVertex, 0, 1, CreateVariables(null, null, null));
            var edges = _graph.AdjacencyList.SelectMany(v => v.Value);
            for (int i = 0; i < _graph.AdjacencyList.Count; i++)
            {
                AddEmptyEvent(2, CreateVariables(i, null, null));
                foreach (var edge in edges)
                {
                    var u = edge.StartVertex;
                    var v = edge.EndVertex;
                    var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, new SolidColorBrush(Colors.Red), Lines[3], CreateVariables(i, edge, null));
                    EventStack.Events.Add(edgePaint);

                    long alt = _distances[u] + edge.UIShape.Weight;
                    AddEmptyEvent(4, CreateVariables(i, edge, alt));
                    AddEmptyEvent(5, CreateVariables(i, edge, alt));
                    if (alt < _distances[v])
                    {
                        ChangeDistance(v, alt, 6, CreateVariables(i, edge, alt));
                        ChangeParent(v, u, 7, CreateVariables(i, edge, alt));
                    }

                    edgePaint = new EdgePaintAndThicken(edge, new SolidColorBrush(Colors.Red), _edgeStartColor, Lines[3], CreateVariables(i, edge, null));
                    edgePaint.IsVisualizable = false;
                    edgePaint.IsReverse = true;
                    EventStack.Events.Add(edgePaint);
                }
            }
            BuildPaths();
        }
        public List<string> GetOutput()
        {
            List<string> result = new List<string>();
            result.Add("vertex;distance;parent");
            foreach (var v in _graph.AdjacencyList.Keys)
            {
                result.Add(v.Text + ";" + _distances[v] + ";" + (_parents[v] == null ? "null" : _parents[v].ToString()));
            }
            return result;
        }
        private void BuildPaths()
        {
            foreach (var end in _graph.AdjacencyList.Keys)
            {
                if (_parents[end] != null)
                {
                    var start = _parents[end];
                    var edge = _graph.AdjacencyList[start].FirstOrDefault(_ => _.EndVertex == end);
                    var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, new SolidColorBrush(Colors.Blue), Lines[8], CreateVariables(null, null, null));
                    edgePaint.IsVisualizable = false;
                    EventStack.Events.Add(edgePaint);
                }
            }
        }
        private void AddEmptyEvent(int lineNumber, Dictionary<string, string> locals)
        {
            var emptyEvent = new EmptyEvent(Lines[lineNumber], locals);
            EventStack.Events.Add(emptyEvent);
        }
        private void ChangeDistance(VertexUI vertex, long distance, int lineNumber, Dictionary<string, string> locals)
        {
            var oldDistance = _distances[vertex] == int.MaxValue ? "infinity" : _distances[vertex].ToString();
            var changeVertexDescription = new ChangeVertexDescription(vertex, "distance", oldDistance, distance.ToString(), Lines[lineNumber], locals);
            _distances[vertex] = (int)distance;
            EventStack.Events.Add(changeVertexDescription);
        }
        private void ChangeParent(VertexUI vertex, VertexUI parent, int lineNumber, Dictionary<string, string> locals)
        {
            var changeVertexDescription = new ChangeVertexDescription(vertex, "parent", _parents[vertex]?.Text, parent?.Text, Lines[lineNumber], locals);
            _parents[vertex] = parent;
            EventStack.Events.Add(changeVertexDescription);
        }
        private Dictionary<string, string> CreateVariables(int? i, Edge edge, long? alt)
        {
            var result = new Dictionary<string, string>();
            if (i != null)
            {
                result.Add("i", i.Value.ToString());
            }
            if (edge != null)
            {
                result.Add("(u, v)", $"({edge.StartVertex.Text}, {edge.EndVertex.Text})");
            }
            
            if (alt != null)
            {
                result.Add("alt", alt.Value.ToString());
            }
            return result;
        }
    }
}
