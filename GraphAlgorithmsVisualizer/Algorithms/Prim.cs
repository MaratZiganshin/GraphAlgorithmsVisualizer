using System.Collections.Generic;
using GraphAlgorithmsVisualizer.UI;
using System.Windows.Media;
using System.Linq;
using GraphAlgorithmsVisualizer.Events;
using System.IO;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public class Prim : IAlgorithm
    {
        private const string path = "GraphAlgorithmsVisualizer.AlgorithmCodes.prim.txt";
        private Graph _graph;
        private Dictionary<VertexUI, int> _keys;
        private Dictionary<VertexUI, VertexUI> _parents;
        private SolidColorBrush _vertexStartColor;
        private SolidColorBrush _edgeStartColor;
        private List<VertexUI> _vertexes;
        public EventStack EventStack { get; }
        public List<CodeLine> Lines { get; }
        public bool HasOutput { get; }
        public Prim(Graph graph)
        {
            _graph = graph;
            EventStack = new EventStack()
            {
                Events = new List<IEvent>()
            };
            _keys = new Dictionary<VertexUI, int>();
            _parents = new Dictionary<VertexUI, VertexUI>();
            _edgeStartColor = new SolidColorBrush(Colors.Black);
            foreach (var pair in graph.AdjacencyList)
            {
                _vertexStartColor = pair.Key.Fill as SolidColorBrush;
                _keys[pair.Key] = int.MaxValue;
                _parents[pair.Key] = null;
                pair.Key.descriptions.Add("key", "infinity");
                pair.Key.descriptions.Add("parent", "null");
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
            _vertexes = new List<VertexUI>();
        }

        public void Run(VertexUI startVertex)
        {
            ChangeKey(startVertex, 0, 1, null);
            _vertexes.AddRange(_graph.AdjacencyList.Keys);
            AddEmptyEvent(2, null);
            while (_vertexes.Count != 0)
            {
                AddEmptyEvent(3, null);
                var u = _vertexes.OrderBy(v => _keys[v]).First();
                _vertexes.Remove(u);
                var vertexPaint = new VertexPaint(u, _vertexStartColor, new SolidColorBrush(Colors.Red), Lines[4], CreateVariables(u, null));
                EventStack.Events.Add(vertexPaint);
                foreach (var edge in _graph.AdjacencyList[u])
                {
                    var end = edge.EndVertex;
                    var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, new SolidColorBrush(Colors.Red), Lines[5], CreateVariables(u, end));
                    EventStack.Events.Add(edgePaint);
                    AddEmptyEvent(6, CreateVariables(u, end));
                    if (_vertexes.Contains(end) && edge.UIShape.Weight < _keys[end])
                    {
                        ChangeParent(end, u, 7, CreateVariables(u, end));
                        ChangeKey(end, edge.UIShape.Weight, 8, CreateVariables(u, end));
                    }
                    edgePaint = new EdgePaintAndThicken(edge, new SolidColorBrush(Colors.Red), _edgeStartColor, Lines[5], CreateVariables(u, end));
                    edgePaint.IsVisualizable = false;
                    edgePaint.IsReverse = true;
                    EventStack.Events.Add(edgePaint);
                }
                vertexPaint = new VertexPaint(u, new SolidColorBrush(Colors.Red), _vertexStartColor, Lines[4], CreateVariables(u, null));
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
                    var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, new SolidColorBrush(Colors.Blue), Lines[9], CreateVariables(null, null));
                    edgePaint.IsVisualizable = false;
                    EventStack.Events.Add(edgePaint);
                }
            }
        }
        public List<string> GetOutput()
        {
            List<string> result = new List<string>();
            result.Add("vertex;key;parent");
            foreach (var v in _graph.AdjacencyList.Keys)
            {
                result.Add(v.Text + ";" + _keys[v] + ";" + _parents[v]);
            }
            return result;
        }

        private void AddEmptyEvent(int lineNumber, Dictionary<string, string> locals)
        {
            var emptyEvent = new EmptyEvent(Lines[lineNumber], locals);
            EventStack.Events.Add(emptyEvent);
        }

        private void ChangeKey(VertexUI vertex, int key, int lineNumber, Dictionary<string, string> locals)
        {
            var oldDistance = _keys[vertex] == int.MaxValue ? "infinity" : _keys[vertex].ToString();
            var changeVertexDescription = new ChangeVertexDescription(vertex, "key", oldDistance, key.ToString(), Lines[lineNumber], locals);
            _keys[vertex] = key;
            EventStack.Events.Add(changeVertexDescription);
        }

        private void ChangeParent(VertexUI vertex, VertexUI parent, int lineNumber, Dictionary<string, string> locals)
        {
            var changeVertexDescription = new ChangeVertexDescription(vertex, "parent", _parents[vertex]?.Text, parent?.Text, Lines[lineNumber], locals);
            _parents[vertex] = parent;
            EventStack.Events.Add(changeVertexDescription);
        }

        private Dictionary<string, string> CreateVariables(VertexUI u, VertexUI v)
        {
            var result = new Dictionary<string, string>();
            if (_vertexes != null)
            {
                List<string> listStrings = new List<string>();
                foreach (var vertex in _vertexes)
                {
                    listStrings.Add(vertex.Text);
                }
                result.Add("Q", string.Join(", ", listStrings));
            }
            if (u != null)
            {
                result.Add("u", u.Text);
            }
            if (v != null)
            {
                result.Add("v", v.Text);
            }
            return result;
        }
    }
}
