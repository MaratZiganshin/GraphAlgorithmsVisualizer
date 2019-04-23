using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphAlgorithmsVisualizer.UI;

namespace GraphAlgorithmsVisualizer
{
    public class Graph
    {
        Dictionary<VertexUI, List<Edge>> adjacencyList = new Dictionary<VertexUI, List<Edge>>();
        public Dictionary<VertexUI, List<Edge>> AdjacencyList {
            get
            {
                return adjacencyList;
            }
        }
        
        private void RemoveAdjectiveEdges(VertexUI vertexToDelete)
        {
            foreach (var vertex in adjacencyList)
            {
                vertex.Value.RemoveAll(edge => edge.StartVertex == vertexToDelete || edge.EndVertex == vertexToDelete);
            }
        }

        public void AddVertex(VertexUI vertexShape)
        {
            adjacencyList.Add(vertexShape, new List<Edge>());
        }
        public void AddEdge(EdgeUI edgeShape, VertexUI edgeStart, VertexUI edgeEnd)
        {
            var edge = new Edge()
            {
                StartVertex = edgeStart,
                EndVertex = edgeEnd,
                UIShape = edgeShape
            };
            adjacencyList[edgeStart].Add(edge);
        }
        public void RemoveVertex(VertexUI vertexShape)
        {
            RemoveAdjectiveEdges(vertexShape);
            adjacencyList.Remove(vertexShape);
        }
        public List<Edge> FindAllAdjectiveEdges(VertexUI vertexToDelete)
        {
            var list = new List<Edge>();
            foreach (var vertex in adjacencyList)
            {
                list.AddRange(vertex.Value.FindAll(edge => edge.StartVertex == vertexToDelete || edge.EndVertex == vertexToDelete));
            }
            return list;
        }
        public void RemoveEdge(EdgeUI edgeShape)
        {
            foreach (var vertex in adjacencyList)
            {
                vertex.Value.RemoveAll(edge => edge.UIShape == edgeShape);                
            }  
        }
        public Edge GetEdge(VertexUI start, VertexUI end)
        {
            foreach (var vertex in adjacencyList)
            {
                var seekingEdge= from edge in vertex.Value where (edge.StartVertex == start && edge.EndVertex == end) select edge;

                if (seekingEdge.Count() == 1) //if edge found return it
                    return seekingEdge.ElementAt(0);
            }
            return null; //if not founded
        }
        public List<Edge> GetAllEdges()
        {
            var result = new List<Edge>();
            foreach (var pair in adjacencyList)
            {
                foreach (var edge in pair.Value)
                {
                    if (result.Find(e => e.UIShape == edge.UIShape) == null)
                        result.Add(edge);
                }
            }
            return result;
        }
        public override string ToString()
        {
            StringBuilder outputLog = new StringBuilder();
            outputLog.AppendLine("Вершины:");
            foreach (var vertex in adjacencyList)
                outputLog.AppendLine("\t" + vertex.Key);

            outputLog.AppendLine("Ребра:");
            foreach (var vertex in adjacencyList)
            {
                foreach (var edge in adjacencyList[vertex.Key])
                {
                    outputLog.AppendLine("\t" + edge.StartVertex.Name + " ---> " + edge.EndVertex.Name);
                }
            }

            return outputLog.ToString();
        }   
    }
}