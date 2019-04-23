using System;
using System.Collections.Generic;
using GraphAlgorithmsVisualizer.UI;
using GraphAlgorithmsVisualizer.Events;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public class Tarjan : IAlgorithm
    {
        private const string path = "GraphAlgorithmsVisualizer.AlgorithmCodes.tarjan.txt";
        private Graph _graph;
        private Dictionary<VertexUI, int> _index;
        private Dictionary<VertexUI, int> _lowlink;
        private int index;
        private Stack<VertexUI> s;
        private SolidColorBrush _vertexStartColor;
        private SolidColorBrush _edgeStartColor;
        private SolidColorBrush _red = new SolidColorBrush(Colors.Red);
        private SolidColorBrush _blue = new SolidColorBrush(Colors.Blue);
        private List<List<VertexUI>> _components;
        public EventStack EventStack { get; }
        public List<CodeLine> Lines { get; }
        public bool HasOutput { get; }

        public Tarjan(Graph graph)
        {
            _graph = graph;
            EventStack = new EventStack()
            {
                Events = new List<IEvent>()
            };
            _index = new Dictionary<VertexUI, int>();
            _lowlink = new Dictionary<VertexUI, int>();
            foreach (var pair in graph.AdjacencyList)
            {
                _vertexStartColor = pair.Key.Fill as SolidColorBrush;
                _index[pair.Key] = int.MinValue;
                _lowlink[pair.Key] = int.MaxValue;
                pair.Key.descriptions.Add("index", "undefined");
                pair.Key.descriptions.Add("lowlink", "undefined");
            }
            _edgeStartColor = new SolidColorBrush(Colors.Black);
            HasOutput = true;
            var code = FileUtils.ReadCode(path);
            Lines = new List<CodeLine>();
            foreach (var line in code)
            {
                var codeLine = new CodeLine();
                codeLine.SetText(line);
                Lines.Add(codeLine);
            }
            index = -1;
        }
        public void Run(VertexUI startVertex)
        {
            index = 0;
            AddEmptyEvent(20, CreateVariables());
            s = new Stack<VertexUI>();
            AddEmptyEvent(21, CreateVariables());
            _components = new List<List<VertexUI>>();
            AddEmptyEvent(22, CreateVariables());
            foreach (var v in _graph.AdjacencyList.Keys)
            {
                var color = _components.SelectMany(comp => comp).Contains(v) ? _blue : _vertexStartColor;
                var vertexPaint = new VertexPaint(v, color, _red, Lines[23], CreateVariables(v));
                EventStack.Events.Add(vertexPaint);
                AddEmptyEvent(24, CreateVariables(v));
                if (_index[v] < 0)
                {
                    AddEmptyEvent(25, CreateVariables(v));
                    StrongConnect(v);
                }
                color = _components.SelectMany(comp => comp).Contains(v) ? _blue : _vertexStartColor;
                vertexPaint = new VertexPaint(v, _red, color, Lines[23], CreateVariables(v));
                vertexPaint.IsVisualizable = true;
                EventStack.Events.Add(vertexPaint);
            }
            AddEmptyEvent(26, CreateVariables());
        }
        public List<string> GetOutput()
        {
            List<string> result = new List<string>();
            result.Add("vertex;index;lowlink");
            foreach (var v in _graph.AdjacencyList.Keys)
            {
                result.Add(v.Text + ";" + _index[v] + ";" + _lowlink[v]);
            }
            return result;
        }

        private void StrongConnect(VertexUI v)
        {
            AddEmptyEvent(0, CreateVariables(v));
            ChangeIndex(v, index, 1, CreateVariables(v));
            ChangeLowlink(v, index, 2, CreateVariables(v));
            index++;
            AddEmptyEvent(3, CreateVariables(v));
            s.Push(v);
            AddEmptyEvent(4, CreateVariables(v));
            foreach (var edge in _graph.AdjacencyList[v])
            {
                var w = edge.EndVertex;
                var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, _red, Lines[5], CreateVariables(v, w));
                EventStack.Events.Add(edgePaint);
                AddEmptyEvent(6, CreateVariables(v, w));
                if (_index[w] < 0)
                {
                    AddEmptyEvent(7, CreateVariables(v, w));
                    StrongConnect(w);
                    ChangeLowlink(v, Math.Min(_lowlink[v], _lowlink[w]), 8, CreateVariables(v, w));
                }
                AddEmptyEvent(9, CreateVariables(v, w));
                if (_index[w] >= 0 && s.Contains(w))
                {
                    ChangeLowlink(v, Math.Min(_lowlink[v], _index[w]), 10, CreateVariables(v, w));
                }
                edgePaint = new EdgePaintAndThicken(edge, _red, _edgeStartColor, Lines[5], CreateVariables(v, w));
                edgePaint.IsReverse = true;
                edgePaint.IsVisualizable = false;
                EventStack.Events.Add(edgePaint);
            }
            AddEmptyEvent(11, CreateVariables(v));
            if (_lowlink[v] == _index[v])
            {
                var result = new List<VertexUI>();
                AddEmptyEvent(12, CreateVariables(v, null, result));
                VertexUI w = null;
                do
                {
                    AddEmptyEvent(13, CreateVariables(v, null, result));
                    w = s.Pop();
                    AddEmptyEvent(14, CreateVariables(v, w, result));
                    AddVertexToComponent(result, w, v);
                    AddEmptyEvent(15, CreateVariables(v, w, result));
                    AddEmptyEvent(16, CreateVariables(v, w, result));
                }
                while (w.Text != v.Text);
                _components.Add(result);
                AddEmptyEvent(17, CreateVariables(v, w, result));
            }
        }

        private void AddVertexToComponent(List<VertexUI> component, VertexUI vertex, VertexUI start)
        {
            if (vertex != start)
            {
                var vertexPaint = new VertexPaint(vertex, _vertexStartColor, _blue, null, null);
                vertexPaint.IsVisualizable = false;
                EventStack.Events.Add(vertexPaint);
            }
            else
            {
                var vertexPaint = new VertexPaint(vertex, _red, _blue, null, null);
                vertexPaint.IsVisualizable = false;
                EventStack.Events.Add(vertexPaint);
            }
            foreach (var v in component)
            {
                foreach (var edge in _graph.AdjacencyList[v])
                {
                    if (edge.EndVertex == vertex)
                    {
                        var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, _blue, null, CreateVariables(null, null, null));
                        edgePaint.IsVisualizable = false;
                        EventStack.Events.Add(edgePaint);
                    }
                }
            }
            foreach (var edge in _graph.AdjacencyList[vertex])
            {
                if (component.Contains(edge.EndVertex))
                {
                    var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, _blue, null, CreateVariables(null, null, null));
                    edgePaint.IsVisualizable = false;
                    EventStack.Events.Add(edgePaint);
                }
            }
            component.Add(vertex);
        }

        private void ChangeIndex(VertexUI vertex, int index, int lineNumber, Dictionary<string, string> locals)
        {
            var oldDistance = _index[vertex] == int.MinValue ? "undefined" : _index[vertex].ToString();
            var changeVertexDescription = new ChangeVertexDescription(vertex, "index", oldDistance, index.ToString(), Lines[lineNumber], locals);
            _index[vertex] = index;
            EventStack.Events.Add(changeVertexDescription);
        }

        private void ChangeLowlink(VertexUI vertex, int lowlink, int lineNumber, Dictionary<string, string> locals)
        {
            var oldDistance = _lowlink[vertex] == int.MaxValue ? "undefined" : _lowlink[vertex].ToString();
            var changeVertexDescription = new ChangeVertexDescription(vertex, "lowlink", oldDistance, lowlink.ToString(), Lines[lineNumber], locals);
            _lowlink[vertex] = lowlink;
            EventStack.Events.Add(changeVertexDescription);
        }

        private void AddEmptyEvent(int lineNumber, Dictionary<string, string> locals)
        {
            var emptyEvent = new EmptyEvent(Lines[lineNumber], locals);
            EventStack.Events.Add(emptyEvent);
        }

        private Dictionary<string, string> CreateVariables(VertexUI v = null, VertexUI w = null, List<VertexUI> component = null)
        {
            var result = new Dictionary<string, string>();
            if (index >= 0)
            {
                result.Add("index", index.ToString());
            }
            if (s != null)
            {
                List<string> listStrings = new List<string>();
                foreach (var vertex in s)
                {
                    listStrings.Add(vertex.Text);
                }
                result.Add("s", string.Join(", ", listStrings));
            }
            if (_components != null)
            {
                List<string> listStrings = new List<string>();
                foreach (var comp in _components)
                {
                    List<string> compStrings = new List<string>();
                    foreach (var vertex in comp)
                    {
                        compStrings.Add(vertex.Text);
                    }
                    string textComp = string.Join(", ", compStrings);
                    listStrings.Add(textComp);
                }
                result.Add("components", string.Join("\n", listStrings));
            }
            if (v != null)
            {
                result.Add("v", v.Text);
            }
            if (w != null)
            {
                result.Add("w", w.Text);
            }
            if (component != null)
            {
                List<string> listStrings = new List<string>();
                foreach (var vertex in component)
                {
                    listStrings.Add(vertex.Text);
                }
                result.Add("component", string.Join(", ", listStrings));
            }
            return result;
        }
    }
}
