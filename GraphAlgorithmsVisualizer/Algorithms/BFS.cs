using GraphAlgorithmsVisualizer.Events;
using GraphAlgorithmsVisualizer.UI;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public class BFS : IAlgorithm
    {
        private const string path = "GraphAlgorithmsVisualizer.AlgorithmCodes.bfs.txt";
        private SolidColorBrush _black = new SolidColorBrush(Colors.Black);
        private SolidColorBrush _gray = new SolidColorBrush(Colors.Gray);
        private SolidColorBrush _red = new SolidColorBrush(Colors.Red);
        private Dictionary<VertexUI, int> _distances;
        private Dictionary<VertexUI, VertexUI> _parents;
        private SolidColorBrush _vertexStartColor;
        private Dictionary<VertexUI, SolidColorBrush> _color;
        private Graph _graph;
        public List<CodeLine> Lines { get; }
        public EventStack EventStack { get; }
        public bool HasOutput { get; }
        public BFS(Graph graph)
        {
            _graph = graph;
            EventStack = new EventStack()
            {
                Events = new List<IEvent>()
            };
            _color = new Dictionary<VertexUI, SolidColorBrush>(graph.AdjacencyList.Count);
            _distances = new Dictionary<VertexUI, int>(graph.AdjacencyList.Count);
            _parents = new Dictionary<VertexUI, VertexUI>(graph.AdjacencyList.Count);
            foreach (var pair in graph.AdjacencyList)
            {
                _color.Add(pair.Key, pair.Key.Fill as SolidColorBrush);
                _vertexStartColor = pair.Key.Fill as SolidColorBrush;
                _distances[pair.Key] = -1;
                _parents[pair.Key] = null;
                pair.Key.descriptions.Add("distance", "infinity");
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
        }

        private Queue<VertexUI> queue { get; }
        private VertexUI v { get; }

        public void Run(VertexUI startVertex)
        {
            AddEmptyEvent(0, CreateVariables(null, null));
            var queue = new Queue<VertexUI>();
            queue.Enqueue(startVertex);
            AddEmptyEvent(1, CreateVariables(null, queue));
            Repaint(startVertex, _gray, 2, CreateVariables(null, queue));
            ChangeDistance(startVertex, 0, 3, CreateVariables(null, queue));
            while (queue.Count != 0)
            {
                AddEmptyEvent(4, CreateVariables(null, queue));
                var v = queue.Dequeue();
                AddEmptyEvent(5, CreateVariables(v, queue));
                foreach (var edge in _graph.AdjacencyList[v])
                {
                    AddEmptyEvent(6, CreateVariables(v, queue));
                    EventStack.Events.Add(new EdgePaintAndThicken(edge, _black, _red, Lines[7], CreateVariables(v, queue)));
                    var end = edge.EndVertex;
                    AddEmptyEvent(8, CreateVariables(v, queue));
                    if (ColorEqualsComparer.Instance.Equals(_color[end], _vertexStartColor))
                    {
                        Repaint(end, _gray, 9, CreateVariables(v, queue));
                        queue.Enqueue(end);
                        AddEmptyEvent(10, CreateVariables(v, queue));
                        ChangeDistance(end, _distances[v] + 1, 11, CreateVariables(v, queue));
                        ChangeParent(end, v, 12, CreateVariables(v, queue));
                    }
                    var edgePaint = new EdgePaintAndThicken(edge, _red, _black, Lines[6], CreateVariables(v, queue));
                    edgePaint.IsVisualizable = false;
                    edgePaint.IsReverse = true;
                    EventStack.Events.Add(edgePaint);
                }
            }
            AddEmptyEvent(13, CreateVariables(null, queue));
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

        private void Repaint(VertexUI vertex, SolidColorBrush color, int lineNumber, Dictionary<string, string> locals)
        {
            var vertexPaint = new VertexPaint(vertex, _color[vertex], color, Lines[lineNumber], locals);
            _color[vertex] = _gray;
            EventStack.Events.Add(vertexPaint);
        }

        private void ChangeDistance(VertexUI vertex, int distance, int lineNumber, Dictionary<string, string> locals)
        {
            var oldDistance = _distances[vertex] == -1 ? "infinity" : _distances[vertex].ToString();
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

        private Dictionary<string, string> CreateVariables(VertexUI v, Queue<VertexUI> queue)
        {
            var result = new Dictionary<string, string>();
            if (v != null)
            {
                result.Add("v", v.Text);
            }
            if (queue != null)
            {
                List<string> queueStrings = new List<string>();
                foreach (var vertex in queue)
                {
                    queueStrings.Add(vertex.Text);
                }
                result.Add("queue", string.Join(", ", queueStrings));
            }
            return result;
        }
    }
}
