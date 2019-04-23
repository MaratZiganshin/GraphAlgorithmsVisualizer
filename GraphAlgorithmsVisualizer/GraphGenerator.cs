using GraphAlgorithmsVisualizer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithmsVisualizer
{
    public static class GraphGenerator
    {
        public static Graph GenerateGraph(int number, double probability)
        {
            Random random = new Random();
            Graph graph = new Graph();
            for (int i = 1; i <= number; i++)
            {
                VertexUI vertex = new VertexUI()
                {
                    Radius = 20,
                    Fill = MainWindow.StartVertexColor,
                    Text = i.ToString(),
                };
                graph.AddVertex(vertex);
            }
            for (int i = 1; i <= number; i++)
            {
                for (int j = 1; j <= number; j++)
                {
                    if (i != j)
                    {
                        if (random.NextDouble() < probability)
                        {
                            var startVertex = graph.AdjacencyList.First(vertex => vertex.Key.Text == i.ToString()).Key;
                            var endVertex = graph.AdjacencyList.First(vertex => vertex.Key.Text == j.ToString()).Key;
                            var existingVertex = graph.GetEdge(endVertex, startVertex);
                            if (existingVertex != null)
                            {
                                existingVertex.UIShape.IsDirected = false;
                                graph.AddEdge(existingVertex.UIShape, startVertex, endVertex);
                            }
                            else
                            {
                                var edgeUi = new EdgeUI()
                                {
                                    IsDirected = true,
                                    IsWeighted = true,
                                    StartVertex = startVertex,
                                    EndVertex = endVertex,
                                    Weight = 1
                                };
                                graph.AddEdge(edgeUi, startVertex, endVertex);
                            }
                        }
                    }
                }
            }
            PlaceVerteces(graph);
            return graph;
        }
        private static void PlaceVerteces(Graph graph)
        {
            int index = 1;
            int row = 1;
            var columns = Math.Ceiling(Math.Sqrt(graph.AdjacencyList.Count));
            while (true)
            {
                for (int i = 1; i <= columns; i++)
                {
                    var vertex = graph.AdjacencyList.Keys.FirstOrDefault(x => x.Text == index.ToString());
                    vertex.LeftOffset = row * 100;
                    vertex.TopOffset = i * 100;
                    index++;
                    if (index > graph.AdjacencyList.Count)
                        break;
                }
                row++;
                if (index > graph.AdjacencyList.Count)
                    break;
            }
        }
    }
}
