using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphAlgorithmsVisualizer.UI;
using GraphAlgorithmsVisualizer.Events;
using System.Windows.Media;
using System.IO;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public class Kosaraju : IAlgorithm
    {
        private const string path = "GraphAlgorithmsVisualizer.AlgorithmCodes.kosaraju.txt";
        private Graph _graph;
        private SolidColorBrush _gray = new SolidColorBrush(Colors.Gray);
        private SolidColorBrush _red = new SolidColorBrush(Colors.Red);
        private SolidColorBrush _blue = new SolidColorBrush(Colors.Blue);
        private SolidColorBrush _edgeStartColor;
        private SolidColorBrush _vertexStartColor;
        private Dictionary<VertexUI, SolidColorBrush> _color;
        private Dictionary<VertexUI, int> _component;
        private List<VertexUI> _ord;
        int _comp;
        public EventStack EventStack { get; }
        public List<CodeLine> Lines { get; }
        public bool HasOutput { get; }

        public Kosaraju(Graph graph)
        {
            _graph = graph;
            EventStack = new EventStack()
            {
                Events = new List<IEvent>()
            };
            _color = new Dictionary<VertexUI, SolidColorBrush>(graph.AdjacencyList.Count);
            _edgeStartColor = new SolidColorBrush(Colors.Black);
            _component = new Dictionary<VertexUI, int>();
            foreach (var pair in graph.AdjacencyList)
            {
                _component.Add(pair.Key, -1);
                _color.Add(pair.Key, pair.Key.Fill as SolidColorBrush);
                _vertexStartColor = pair.Key.Fill as SolidColorBrush;
                pair.Key.descriptions.Add("component", "undefined");
            }
            HasOutput = true;
            _ord = new List<VertexUI>();
            _comp = 1;
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
            AddEmptyEvent(13, CreateVariables(null));
            foreach (var v in _graph.AdjacencyList.Keys)
            {
                AddEmptyEvent(14, CreateVariables(v));
                AddEmptyEvent(15, CreateVariables(v));
                if (ColorEqualsComparer.Instance.Equals(_color[v], _vertexStartColor))
                {
                    AddEmptyEvent(16, CreateVariables(v));
                    DFS1(v);
                }
            }
            InvertAllEdges();
            AddEmptyEvent(17, CreateVariables());
            for (int i = _ord.Count - 1; i >= 0; i--)
            {
                var v = _ord[i];
                AddEmptyEvent(18, CreateVariables(v));
                AddEmptyEvent(19, CreateVariables(v));
                if (_component[v] == -1)
                {
                    AddEmptyEvent(20, CreateVariables(v));
                    DFS2(v);
                    BuildComponent(_comp);
                    _comp++;
                    AddEmptyEvent(21, CreateVariables(v));
                }
            }
            AddEmptyEvent(22, CreateVariables());
        }

        public List<string> GetOutput()
        {
            List<string> result = new List<string>();
            result.Add("vertex;component");
            foreach (var v in _graph.AdjacencyList.Keys)
            {
                result.Add(v.Text + ";" + _component[v]);
            }
            return result;
        }

        private void DFS1(VertexUI v)
        {
            AddEmptyEvent(0, CreateVariables(v));
            var vertexColor = new VertexPaint(v, _vertexStartColor, _gray, Lines[1], CreateVariables(v));
            EventStack.Events.Add(vertexColor);
            _color[v] = _gray;
            foreach (var edge in _graph.AdjacencyList[v])
            {
                var u = edge.EndVertex;
                var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, _red, Lines[2], CreateVariables(v, u));
                EventStack.Events.Add(edgePaint);
                AddEmptyEvent(3, CreateVariables(v, u));
                if (ColorEqualsComparer.Instance.Equals(_color[u], _vertexStartColor))
                {
                    AddEmptyEvent(4, CreateVariables(v, u));
                    DFS1(u);
                }
                edgePaint = new EdgePaintAndThicken(edge, _red, _edgeStartColor, Lines[2], CreateVariables(v, u));
                edgePaint.IsVisualizable = false;
                edgePaint.IsReverse = true;
                EventStack.Events.Add(edgePaint);
            }
            _ord.Add(v);
            AddEmptyEvent(5, CreateVariables(v));
        }

        private void DFS2(VertexUI v)
        {
            AddEmptyEvent(7, CreateVariables(v));
            _component[v] = _comp;
            var changeDescription = new ChangeVertexDescription(v, "component", "undefined", _comp.ToString(), Lines[8], CreateVariables(v));
            EventStack.Events.Add(changeDescription);
            var incomeEdges = _graph.AdjacencyList.Values.SelectMany(_ => _).Where(_ => _.EndVertex == v);
            foreach (var edge in incomeEdges)
            {
                var u = edge.StartVertex;
                var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, _red, Lines[9], CreateVariables(v, u));
                EventStack.Events.Add(edgePaint);
                AddEmptyEvent(10, CreateVariables(v, u));
                if (_component[u] == -1)
                {
                    AddEmptyEvent(11, CreateVariables(v, u));
                    DFS2(u);
                }
                edgePaint = new EdgePaintAndThicken(edge, _red, _edgeStartColor, Lines[9], CreateVariables(v, u));
                edgePaint.IsVisualizable = false;
                edgePaint.IsReverse = true;
                EventStack.Events.Add(edgePaint);
            }
        }

        private void AddEmptyEvent(int lineNumber, Dictionary<string, string> locals)
        {
            var emptyEvent = new EmptyEvent(Lines[lineNumber], locals);
            EventStack.Events.Add(emptyEvent);
        }

        private void BuildComponent(int comp)
        {
            var compVertexes = _graph.AdjacencyList.Keys.Where(_ => _component[_] == comp);
            foreach (var v in compVertexes)
            {
                var vertexPaint = new VertexPaint(v, _gray, _blue, Lines[21], CreateVariables());
                vertexPaint.IsVisualizable = false;
                EventStack.Events.Add(vertexPaint);
            }
            var compEdges = _graph.AdjacencyList.Values.SelectMany(_ => _)
                .Where(_ => compVertexes.Contains(_.StartVertex) && compVertexes.Contains(_.EndVertex));
            foreach (var e in compEdges)
            {
                var edgePaint = new EdgePaintAndThicken(e, _edgeStartColor, _blue, Lines[21], CreateVariables());
                edgePaint.IsVisualizable = false;
                EventStack.Events.Add(edgePaint);
            }
        }

        private void InvertAllEdges()
        {
            foreach (var v in _graph.AdjacencyList.Keys)
            {
                foreach (var edge in _graph.AdjacencyList[v])
                {
                    if (edge.UIShape.IsDirected)
                    {
                        var revertEdge = new EdgeOrientationChange(edge);
                        revertEdge.IsVisualizable = false;
                        EventStack.Events.Add(revertEdge);
                    }
                }
            }
        }

        private Dictionary<string, string> CreateVariables(VertexUI v = null, VertexUI u = null)
        {
            var result = new Dictionary<string, string>();
            if (_comp != null)
            {
                result.Add("comp", _comp.ToString());
            }
            if (_ord != null)
            {
                List<string> listStrings = new List<string>();
                foreach (var vertex in _ord)
                {
                    listStrings.Add(vertex.Text);
                }
                result.Add("ord", string.Join(", ", listStrings));
            }
            if (v != null)
            {
                result.Add("v", v.Text);
            }
            if (u != null)
            {
                result.Add("u", u.Text);
            }
            return result;
        }
    }
}
