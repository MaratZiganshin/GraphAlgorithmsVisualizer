using GraphAlgorithmsVisualizer.Events;
using GraphAlgorithmsVisualizer.UI;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.IO;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public class Dijkstra : IAlgorithm
    {
        private const string path = "GraphAlgorithmsVisualizer.AlgorithmCodes.dijkstra.txt";
        private SolidColorBrush _edgeStartColor;
        private SolidColorBrush _vertexStartColor;
        private Dictionary<VertexUI, int> _distances;
        private Dictionary<VertexUI, VertexUI> _parents;
        private List<VertexUI> list;
        private Graph _graph;
        public List<CodeLine> Lines { get; }
        public EventStack EventStack { get; private set; }
        public bool HasOutput { get; }
        public Dijkstra(Graph graph)
        {
            _graph = graph;
            EventStack = new EventStack()
            {
                Events = new List<IEvent>()
            };
            _edgeStartColor = new SolidColorBrush(Colors.Black);
            _distances = new Dictionary<VertexUI, int>(graph.AdjacencyList.Count);
            _parents = new Dictionary<VertexUI, VertexUI>(graph.AdjacencyList.Count);
            list = new List<VertexUI>();
            foreach (var pair in graph.AdjacencyList)
            {
                _distances[pair.Key] = int.MaxValue;
                _parents[pair.Key] = null;
                _vertexStartColor = pair.Key.Fill as SolidColorBrush;
                pair.Key.descriptions.Add("distance", "infinity");
                pair.Key.descriptions.Add("parent", "null");
                list.Add(pair.Key);
            }
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

        public void Run(VertexUI startVertex)
        {
            AddEmptyEvent(0, CreateVariables(null, null, null));
            ChangeDistance(startVertex, 0, 1, CreateVariables( null, null, null));
            while (list.Count != 0)
            {
                AddEmptyEvent(2, CreateVariables(null, null, null));
                var u = list.OrderBy(elem => _distances[elem]).First();
                var vertexPaint = new VertexPaint(u, _vertexStartColor, new SolidColorBrush(Colors.Red), Lines[3], CreateVariables(u, null, null));
                EventStack.Events.Add(vertexPaint);
                list.Remove(u);
                AddEmptyEvent(4, CreateVariables(u, null, null));
                foreach (var edge in _graph.AdjacencyList[u])
                {
                    var v = edge.EndVertex;
                    var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, new SolidColorBrush(Colors.Red), Lines[5], CreateVariables(u, v, null));
                    EventStack.Events.Add(edgePaint);
                    var alt = _distances[u] + edge.UIShape.Weight;
                    AddEmptyEvent(6, CreateVariables(u, v, alt));
                    AddEmptyEvent(7, CreateVariables(u, v, alt));
                    if (alt < _distances[v])
                    {
                        ChangeDistance(v, alt, 8, CreateVariables(u, v, alt));
                        _distances[v] = alt;
                        ChangeParent(v, u, 9, CreateVariables(u, v, alt));
                        _parents[v] = u;
                    }

                    edgePaint = new EdgePaintAndThicken(edge, new SolidColorBrush(Colors.Red), _edgeStartColor, null, null);
                    edgePaint.IsVisualizable = false;
                    edgePaint.IsReverse = true;
                    EventStack.Events.Add(edgePaint);
                }
                vertexPaint = new VertexPaint(u, new SolidColorBrush(Colors.Red), _vertexStartColor, Lines[3], CreateVariables(u, null, null));
                vertexPaint.IsVisualizable = false;
                EventStack.Events.Add(vertexPaint);
            }
            BuildPaths();
        }

        private void BuildPaths()
        {
            foreach (var end in _graph.AdjacencyList.Keys)
            {
                if (_parents[end] != null)
                {
                    var start = _parents[end];
                    var edge = _graph.AdjacencyList[start].FirstOrDefault(_ => _.EndVertex == end);
                    var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, new SolidColorBrush(Colors.Blue), Lines[10], CreateVariables(null, null, null));
                    edgePaint.IsVisualizable = false;
                    EventStack.Events.Add(edgePaint);
                }
            }
        }

        private void ChangeDistance(VertexUI vertex, int distance, int lineNumber, Dictionary<string, string> locals)
        {
            var oldDistance = _distances[vertex] == int.MaxValue ? "infinity" : _distances[vertex].ToString();
            var changeVertexDescription = new ChangeVertexDescription(vertex, "distance", oldDistance, distance.ToString(), Lines[lineNumber], locals);
            _distances[vertex] = distance;
            EventStack.Events.Add(changeVertexDescription);
        }

        private void ChangeParent(VertexUI vertex, VertexUI parent, int lineNumber, Dictionary<string, string> locals)
        {
            var changeVertexDescription = new ChangeVertexDescription(vertex, "parent", _parents[vertex]?.Text, parent?.Text, Lines[lineNumber], locals);
            _parents[vertex] = parent;
            EventStack.Events.Add(changeVertexDescription);
        }

        private void AddEmptyEvent(int lineNumber, Dictionary<string, string> locals)
        {
            var emptyEvent = new EmptyEvent(Lines[lineNumber], locals);
            EventStack.Events.Add(emptyEvent);
        }

        private Dictionary<string, string> CreateVariables(VertexUI u, VertexUI v, int? alt)
        {
            var result = new Dictionary<string, string>();
            if (list != null)
            {
                List<string> listStrings = new List<string>();
                foreach (var vertex in list)
                {
                    listStrings.Add(vertex.Text);
                }
                result.Add("list", string.Join(", ", listStrings));
            }
            if (u != null)
            {
                result.Add("u", u.Text);
            }
            if (v != null)
            {
                result.Add("v", v.Text);
            }
            if (alt != null)
            {
                result.Add("alt", alt.Value.ToString());
            }
            return result;
        }
    }
}
