using System.Collections.Generic;
using GraphAlgorithmsVisualizer.UI;
using GraphAlgorithmsVisualizer.Events;
using System.IO;
using System.Windows.Media;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public class Kahn : IAlgorithm
    {
        private const string path = "GraphAlgorithmsVisualizer.AlgorithmCodes.kahn.txt";
        private Graph _graph;
        private Dictionary<VertexUI, int> InDegree;
        private SolidColorBrush _vertexStartColor;
        private SolidColorBrush _edgeStartColor;
        private List<VertexUI> _order;
        public EventStack EventStack { get; }
        public List<CodeLine> Lines { get; }
        public bool HasOutput { get; }

        public Kahn(Graph graph)
        {
            _graph = graph;
            EventStack = new EventStack()
            {
                Events = new List<IEvent>()
            };
            InDegree = new Dictionary<VertexUI, int>();
            foreach (var v in graph.AdjacencyList.Keys)
            {
                _vertexStartColor = v.Fill as SolidColorBrush;
                InDegree.Add(v, 0);
                v.descriptions.Add("inDegree", "0");
            }
            HasOutput = true;
            _edgeStartColor = new SolidColorBrush(Colors.Black);
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
            string result = "";
            foreach (var v in _order)
            {
                result = result + v.Text + ";";
            }
            return new List<string>() { result };
        }

        public void Run(VertexUI startVertex)
        {
            foreach (var vertex in _graph.AdjacencyList.Keys)
            {
                foreach (var edge in _graph.AdjacencyList[vertex])
                {
                    var end = edge.EndVertex;
                    InDegree[end]++;
                }
            }

            var queue = new Queue<VertexUI>();
            foreach (var vertex in _graph.AdjacencyList.Keys)
            {
                var changeInDegree = new ChangeVertexDescription(vertex, "inDegree", "0", InDegree[vertex].ToString(), Lines[1], CreateVariables(null, null));
                changeInDegree.IsVisualizable = false;
                EventStack.Events.Add(changeInDegree);
                if (InDegree[vertex] == 0)
                {
                    queue.Enqueue(vertex);
                }
            }
            AddEmptyEvent(1, CreateVariables(null, null));
            AddEmptyEvent(2, CreateVariables(queue, null));

            var top_order = new List<VertexUI>();

            AddEmptyEvent(3, CreateVariables(queue, top_order));

            while (queue.Count != 0)
            {
                AddEmptyEvent(4, CreateVariables(queue, top_order));
                var u = queue.Dequeue();
                var vertexPaint = new VertexPaint(u, _vertexStartColor, new SolidColorBrush(Colors.Red), Lines[5], CreateVariables(queue, top_order));
                EventStack.Events.Add(vertexPaint);
                top_order.Add(u);
                AddEmptyEvent(6, CreateVariables(queue, top_order));
                foreach (var edge in _graph.AdjacencyList[u])
                {
                    var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, new SolidColorBrush(Colors.Red), Lines[7], CreateVariables(queue, top_order));
                    EventStack.Events.Add(edgePaint);
                    var end = edge.EndVertex;
                    InDegree[end]--;
                    var changeInDegree = new ChangeVertexDescription(end, "inDegree", (InDegree[end] + 1).ToString(), InDegree[end].ToString(), Lines[8], CreateVariables(queue, top_order));
                    EventStack.Events.Add(changeInDegree);
                    AddEmptyEvent(9, CreateVariables(queue, top_order));
                    if (InDegree[end] == 0)
                    {
                        queue.Enqueue(end);
                        AddEmptyEvent(10, CreateVariables(queue, top_order));
                    }
                    edgePaint = new EdgePaintAndThicken(edge, new SolidColorBrush(Colors.Red), _edgeStartColor, Lines[7], CreateVariables(queue, top_order));
                    edgePaint.IsVisualizable = false;
                    edgePaint.IsReverse = true;
                    EventStack.Events.Add(edgePaint);
                }
                vertexPaint = new VertexPaint(u, new SolidColorBrush(Colors.Red), _vertexStartColor, Lines[5], CreateVariables(queue, top_order));
                vertexPaint.IsVisualizable = false;
                EventStack.Events.Add(vertexPaint);
            }
            AddEmptyEvent(11, CreateVariables(queue, top_order));
            _order = top_order;
        }

        private void AddEmptyEvent(int lineNumber, Dictionary<string, string> locals)
        {
            var emptyEvent = new EmptyEvent(Lines[lineNumber], locals);
            EventStack.Events.Add(emptyEvent);
        }

        private Dictionary<string, string> CreateVariables(Queue<VertexUI> q, List<VertexUI> top_order)
        {
            var result = new Dictionary<string, string>();
            if (q != null)
            {
                List<string> queueStrings = new List<string>();
                foreach (var vertex in q)
                {
                    queueStrings.Add(vertex.Text);
                }
                result.Add("Q", string.Join(", ", queueStrings));
            }
            if (top_order != null)
            {
                List<string> listStrings = new List<string>();
                foreach (var vertex in top_order)
                {
                    listStrings.Add(vertex.Text);
                }
                result.Add("top_order", string.Join(", ", listStrings));
            }
            return result;
        }
    }
}
