using System.Collections.Generic;
using GraphAlgorithmsVisualizer.UI;
using GraphAlgorithmsVisualizer.Events;
using System.IO;
using System.Windows.Media;
using System.Linq;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public class Kruskal : IAlgorithm
    {
        private const string path = "GraphAlgorithmsVisualizer.AlgorithmCodes.kruskal.txt";
        private SolidColorBrush _vertexStartColor;
        private SolidColorBrush _edgeStartColor;
        private Graph _graph;
        private List<Edge> _tree;
        public EventStack EventStack { get; }
        public List<CodeLine> Lines { get; }
        public bool HasOutput { get; }

        public Kruskal(Graph graph)
        {
            _graph = graph;
            EventStack = new EventStack()
            {
                Events = new List<IEvent>()
            };
            _vertexStartColor = _graph.AdjacencyList.Keys.First().Fill;
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
        }

        public void Run(VertexUI startVertex)
        {
            List<Edge> resultTree = new List<Edge>();
            AddEmptyEvent(1, CreateVariables(resultTree));
            List<List<VertexUI>> trees = new List<List<VertexUI>>();
            AddEmptyEvent(2, CreateVariables(resultTree));
            foreach (var vertex in _graph.AdjacencyList.Keys)
            {
                trees.Add(new List<VertexUI>() { vertex });
                Repaint(vertex, 3);
            }
            AddEmptyEvent(4, CreateVariables(resultTree));
            List<Edge> sortedEdges = _graph.GetAllEdges();
            sortedEdges.Sort(Comparer<Edge>.Create(
                (e1, e2) => e1.UIShape.Weight.CompareTo(e2.UIShape.Weight)));
            foreach (var edge in sortedEdges)
            {
                var edgePaint = new EdgePaintAndThicken(edge, _edgeStartColor, new SolidColorBrush(Colors.Blue), Lines[5], CreateVariables(resultTree));
                EventStack.Events.Add(edgePaint);
                var firstTree = FindTree(trees, edge.StartVertex);
                var secondTree = FindTree(trees, edge.EndVertex);
                AddEmptyEvent(6, CreateVariables(resultTree));
                if (firstTree != secondTree)
                {
                    resultTree.Add(edge);
                    trees.Remove(secondTree);
                    firstTree.AddRange(secondTree);
                    AddEmptyEvent(7, CreateVariables(resultTree));
                    var addEdge = new EdgePaint(edge, new SolidColorBrush(Colors.Blue), new SolidColorBrush(Colors.Red), Lines[8], CreateVariables(resultTree));
                    EventStack.Events.Add(addEdge);
                }
                else
                {
                    edgePaint = new EdgePaintAndThicken(edge, new SolidColorBrush(Colors.Blue), _edgeStartColor, Lines[5], CreateVariables(resultTree));
                    edgePaint.IsVisualizable = false;
                    edgePaint.IsReverse = true;
                    EventStack.Events.Add(edgePaint);
                }
            }
            AddEmptyEvent(9, CreateVariables(resultTree));
            _tree = resultTree;
        }

        public List<string> GetOutput()
        {
            List<string> result = new List<string>();
            foreach (var edge in _tree)
            {
                result.Add($"({edge.StartVertex.Text}, {edge.EndVertex.Text})");
            }
            return result;
        }

        private void Repaint(VertexUI vertex, int lineNumber)
        {
            var vertexPaint = new VertexPaint(vertex, _vertexStartColor, new SolidColorBrush(Colors.Red), Lines[lineNumber], null);
            vertexPaint.IsVisualizable = false;
            EventStack.Events.Add(vertexPaint);
        }

        private void AddEmptyEvent(int lineNumber, Dictionary<string, string> locals)
        {
            var emptyEvent = new EmptyEvent(Lines[lineNumber], locals);
            EventStack.Events.Add(emptyEvent);
        }

        private List<VertexUI> FindTree(List<List<VertexUI>> trees, VertexUI vertex)
        {
            List<VertexUI> result = null;
            foreach (var tree in trees)
            {
                foreach (var v in tree)
                {
                    if (v == vertex)
                        result = tree;
                }
            }
            return result;
        }

        private Dictionary<string, string> CreateVariables(List<Edge> tree)
        {
            var result = new Dictionary<string, string>();
            if (tree != null)
            {
                List<string> queueStrings = new List<string>();
                foreach (var edge in tree)
                {
                    queueStrings.Add($"({edge.StartVertex.Text}, {edge.EndVertex.Text})");
                }
                result.Add("resultTree", string.Join(",\n", queueStrings));
            }
            return result;
        }
    }
}
