using GraphAlgorithmsVisualizer.UI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GraphAlgorithmsVisualizer
{
    public static class FileUtils
    {
        public static void Save(Graph graph)
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "Graph";
            dialog.DefaultExt = ".graph"; // Default file extension
            dialog.Filter = "Graph files (.graph)|*.graph"; // Filter files by extension

            var result = dialog.ShowDialog();

            if (result == true)
            {
                var fileName = dialog.FileName;
                using (StreamWriter file = new StreamWriter(fileName))
                {
                    file.WriteLine(graph.AdjacencyList.Count);
                    foreach (var pair in graph.AdjacencyList)
                    {
                        file.WriteLine(pair.Key.Text + ";" + pair.Key.TopOffset + ";" + pair.Key.LeftOffset);
                    }
                    var edges = graph.AdjacencyList.SelectMany(pair => pair.Value.Select(edge => edge)).ToArray();
                    file.WriteLine(edges.Length);
                    foreach (var edge in edges)
                    {
                        file.WriteLine(edge.StartVertex.Text + ";" + edge.EndVertex.Text + ";" + edge.UIShape.Weight);
                    }
                }
            }
        }

        public static Graph Open()
        {
            Graph graph = new Graph();
            OpenFileDialog dialog = new OpenFileDialog();
            
            dialog.DefaultExt = ".graph"; // Default file extension
            dialog.Filter = "Graph files (.graph)|*.graph"; // Filter files by extension

            var result = dialog.ShowDialog();

            if (result == true)
            {
                var fileName = dialog.FileName;
                using (StreamReader file = new StreamReader(fileName))
                {
                    var count = int.Parse(file.ReadLine());
                    for (int i = 0; i < count; i++)
                    {
                        var vertexDescription = file.ReadLine().Split(';');
                        var vertex = new VertexUI()
                        {
                            Radius = 20,
                            Fill = MainWindow.StartVertexColor,
                            Text = vertexDescription[0],
                            TopOffset = double.Parse(vertexDescription[1]),
                            LeftOffset = double.Parse(vertexDescription[2]),
                        };
                        graph.AddVertex(vertex);
                    }
                    count = int.Parse(file.ReadLine());
                    for (int i = 0; i < count; i++)
                    {
                        var edgeDescription = file.ReadLine().Split(';');
                        var startVertex = graph.AdjacencyList.First(vertex => vertex.Key.Text == edgeDescription[0]).Key;
                        var endVertex = graph.AdjacencyList.First(vertex => vertex.Key.Text == edgeDescription[1]).Key;
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
                                Weight = int.Parse(edgeDescription[2])
                            };
                            graph.AddEdge(edgeUi, startVertex, endVertex);
                        }
                    }
                }

                return graph;
            }

            return null;
        }

        public static Graph OpenMatrix()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text files (.txt)|*.txt"; // Filter files by extension

            var result = dialog.ShowDialog();

            if (result == true)
            {
                Graph graph = new Graph();
                var fileName = dialog.FileName;
                List<List<int>> matrix = new List<List<int>>();
                string [] lines = File.ReadAllLines(fileName);
                foreach (var line in lines)
                {
                    var weightStrings = line.Split(' ');
                    List<int> list = new List<int>();
                    foreach (var str in weightStrings)
                    {
                        if (!string.IsNullOrWhiteSpace(str))
                        {
                            int weight = int.Parse(str);
                            list.Add(weight);
                        }
                    }
                    matrix.Add(list);
                }
                for (int i = 0; i < matrix.Count; i++)
                {
                    VertexUI vertex = new VertexUI()
                    {
                        Radius = 20,
                        Fill = MainWindow.StartVertexColor,
                        Text = (i + 1).ToString(),
                        LeftOffset = new Random().Next(100),
                        TopOffset = new Random().Next(100)
                    };
                    graph.AddVertex(vertex);
                }
                for (int i = 0; i < matrix.Count; i++)
                {
                    for (int j = 0; j < matrix.Count; j++)
                    {
                        if (matrix[i][j] != 0)
                        {
                            var startVertex = graph.AdjacencyList.First(vertex => vertex.Key.Text == (i + 1).ToString()).Key;
                            var endVertex = graph.AdjacencyList.First(vertex => vertex.Key.Text == (j + 1).ToString()).Key;
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
                                    Weight = matrix[i][j]
                                };
                                graph.AddEdge(edgeUi, startVertex, endVertex);
                            }
                        }
                    }
                }
                PlaceVerteces(graph);
                return graph;
            }

            return null;
        }

        public static void SaveMatrix(Graph graph)
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "Graph";
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text files (.txt)|*.txt";

            var result = dialog.ShowDialog();

            if (result == true)
            {
                var fileName = dialog.FileName;
                using (StreamWriter file = new StreamWriter(fileName))
                {
                    foreach (var first in graph.AdjacencyList.Keys)
                    {
                        foreach (var second in graph.AdjacencyList.Keys)
                        {
                            var edge = graph.AdjacencyList[first].FirstOrDefault(_ => _.EndVertex == second);
                            if (edge != null)
                            {
                                file.Write(edge.UIShape.Weight + " ");
                            }
                            else
                            {
                                file.Write("0 ");
                            }
                        }
                        file.WriteLine();
                    }
                }
            }
        }

        public static Graph OpenAdjList()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text files (.txt)|*.txt"; // Filter files by extension

            var result = dialog.ShowDialog();

            if (result == true)
            {
                Graph graph = new Graph();
                var fileName = dialog.FileName;
                List<List<string>> matrix = new List<List<string>>();
                string[] lines = File.ReadAllLines(fileName);
                foreach (var line in lines)
                {
                    var vertexLine = line.Split(' ')[0];
                    var vertex = new VertexUI()
                    {
                        Radius = 20,
                        Fill = MainWindow.StartVertexColor,
                        Text = vertexLine,
                        LeftOffset = new Random().Next(100),
                        TopOffset = new Random().Next(100)
                    };
                    graph.AddVertex(vertex);
                }
                foreach (var line in lines)
                {
                    var vertexLines = line.Split(' ');
                    var startVertexLine = vertexLines[0];
                    for (int i = 1; i < vertexLines.Length; i++)
                    {
                        var endVertexLine = vertexLines[i];
                        if (!string.IsNullOrWhiteSpace(endVertexLine))
                        {
                            var startVertex = graph.AdjacencyList.First(vertex => vertex.Key.Text == startVertexLine).Key;
                            var endVertex = graph.AdjacencyList.First(vertex => vertex.Key.Text == endVertexLine).Key;
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
                
                PlaceVerteces(graph);
                return graph;
            }

            return null;
        }

        public static void SaveAdjList(Graph graph)
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "Graph";
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text files (.txt)|*.txt";

            var result = dialog.ShowDialog();

            if (result == true)
            {
                var fileName = dialog.FileName;
                using (StreamWriter file = new StreamWriter(fileName))
                {
                    foreach (var first in graph.AdjacencyList.Keys)
                    {
                        file.Write(first.Text + " ");
                        foreach (var edge in graph.AdjacencyList[first])
                        {
                            file.Write(edge.EndVertex.Text + " ");
                        }
                        file.WriteLine();
                    }
                }
            }
        }

        public static string[] ReadCode(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(path))
            using (StreamReader reader = new StreamReader(stream))
            {
                var result = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                return result;
            }
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
