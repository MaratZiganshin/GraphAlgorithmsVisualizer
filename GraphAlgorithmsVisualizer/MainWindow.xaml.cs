using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using GraphAlgorithmsVisualizer.Algorithms;
using System.Linq;
using System.Threading.Tasks;
using GraphAlgorithmsVisualizer.UI;
using System.Windows.Controls.Primitives;

namespace GraphAlgorithmsVisualizer
{
    public partial class MainWindow : Window
    {
        VertexUI selectedForAlgorithm = null;
        bool isPlaying;
        static int NumberOfVertex = 1;
        Graph mainGraph = new Graph();
        OperationMode mode = OperationMode.None;
        public static SolidColorBrush StartVertexColor = new SolidColorBrush(Color.FromRgb(184, 242, 177));
        VertexUI selectedVertex = null;
        List<string> algorithms;
        string chosenAlgorithm;
        EventStack eventStack;
        int eventIndex;
        double speed;
        SelectVertexLabel selectVertexLabel = new SelectVertexLabel();
        bool capturedObject = false;
        double XcapturedObject, Xcanvas, YcapturedObject, Ycanvas;
        UIElement source = null;

        public MainWindow()
        {
            InitializeComponent();
            algorithms = new List<string>()
            {
                "BFS",
                "DFS",
                "Topological sort",
                "Dijkstra",
                "Kruskal",
                "Prim",
                "Kahn",
                "BellmanFord",
                "Cycle Search",
                "Tarjan",
                "Kosaraju"
            };
            chosenAlgorithm = algorithms.First();
            chooseAlgorithm.ItemsSource = algorithms;
            chooseAlgorithm.SelectedItem = chosenAlgorithm;
        }
        public void CanvasClick(object sender, MouseButtonEventArgs e)
        {
            if (mode == OperationMode.CreateVertex)
            {
                CreateVertex(e);
            }
            RemoveSelecting();
            return;
        }
        private void CreateVertex(MouseButtonEventArgs e)
        {
            Point mouseСoordinates = e.GetPosition(MainCanvas);

            int radius = 20;
            var vertexShape = new VertexUI()
            {
                Fill = StartVertexColor,
                Radius = radius,
                Text = (NumberOfVertex++).ToString(),
                LeftOffset = mouseСoordinates.X - radius,
                TopOffset = mouseСoordinates.Y - radius
            };

            AddEvents(vertexShape);

            MainCanvas.Children.Add(vertexShape);

            mainGraph.AddVertex(vertexShape);
        }
        public void ChangeMode(object sender, RoutedEventArgs e)
        {
            if (mode != OperationMode.Algorithm && mode != OperationMode.AlgorithmFast)
            {
                var button = sender as ToggleButton;
                SetLeftButtonStyles();
                button.Background = new SolidColorBrush(Colors.LightGray);
                switch (button.Uid)
                {
                    case "CursorSwitch":
                        mode = OperationMode.None;
                        break;
                    case "VertexSwitch":
                        mode = OperationMode.CreateVertex;
                        break;
                    case "OrientedEdgeSwitch":
                        mode = OperationMode.CreateOrientedEdge;
                        break;
                    case "EdgeSwitch":
                        mode = OperationMode.CreateEdge;
                        break;
                    case "DeleteSwitch":
                        mode = OperationMode.Remove;
                        break;
                    default:
                        mode = OperationMode.None;
                        break;
                }
                RemoveSelecting();
            }
        }
        public void VertexClick(object sender, MouseButtonEventArgs e)
        {
            if (mode == OperationMode.None || mode == OperationMode.Algorithm || mode == OperationMode.AlgorithmFast) VertexStartMoving(sender, e);
            if (mode == OperationMode.Remove) RemoveVertex(sender);
            if (mode == OperationMode.CreateEdge) CreateEdge(sender, e, false);
            if (mode == OperationMode.CreateOrientedEdge) CreateEdge(sender, e, true);
            if (mode == OperationMode.SelectVertex)
            {
                selectedForAlgorithm = sender as VertexUI;
                MainCanvas.Children.Remove(selectVertexLabel);
                mode = OperationMode.Algorithm;
                switch (chosenAlgorithm)
                {
                    case "DFS":
                        RunAlgorithm(new DFS(mainGraph));
                        break;
                    case "BFS":
                        RunAlgorithm(new BFS(mainGraph));
                        break;
                    case "Dijkstra":
                        RunAlgorithm(new Dijkstra(mainGraph));
                        break;
                    case "Prim":
                        RunAlgorithm(new Prim(mainGraph));
                        break;
                    case "BellmanFord":
                        RunAlgorithm(new BellmanFord(mainGraph));
                        break;
                    default:
                        break;
                }
            }
            if (mode == OperationMode.SelectVertexForFast)
            {
                selectedForAlgorithm = sender as VertexUI;
                MainCanvas.Children.Remove(selectVertexLabel);
                mode = OperationMode.AlgorithmFast;
                switch (chosenAlgorithm)
                {
                    case "DFS":
                        RunAlgorithmFast(new DFS(mainGraph));
                        break;
                    case "BFS":
                        RunAlgorithmFast(new BFS(mainGraph));
                        break;
                    case "Dijkstra":
                        RunAlgorithmFast(new Dijkstra(mainGraph));
                        break;
                    case "Prim":
                        RunAlgorithmFast(new Prim(mainGraph));
                        break;
                    case "BellmanFord":
                        RunAlgorithmFast(new BellmanFord(mainGraph));
                        break;
                    default:
                        break;
                }
            }
        }

        public void EdgeClick(object sender, MouseButtonEventArgs e)
        {
            if (mode == OperationMode.Remove)
            {
                EdgeUI deletedEdge = sender as EdgeUI;
                MainCanvas.Children.Remove(deletedEdge);
                mainGraph.RemoveEdge(deletedEdge);
            }
        }
        private void RemoveVertex(object sender)
        {
            VertexUI myElement = sender as VertexUI;
            List<Edge> edges = mainGraph.FindAllAdjectiveEdges(myElement);
            foreach (var edge in edges)
            {
                MainCanvas.Children.Remove(edge.UIShape);
            }
            MainCanvas.Children.Remove(myElement);
            mainGraph.RemoveVertex(myElement);
        }
        private void CreateEdge(object sender, MouseButtonEventArgs e, bool oriented = false)
        {
            e.Handled = true;
            VertexUI vertexShape = sender as VertexUI;

            if (selectedVertex == null || selectedVertex == vertexShape) //select vertex and return control
            {
                selectedVertex = vertexShape;
                SetBackground(vertexShape, new SolidColorBrush(Color.FromRgb(180, 0, 0)), new { R = 0, G = 0, B = 0 });
                return;
            }

            if (mainGraph.GetEdge(vertexShape, selectedVertex) != null || mainGraph.GetEdge(selectedVertex, vertexShape) != null)
            {
                RemoveSelecting();
                return;
            }

            if (oriented)
            {
                CreateOriented(vertexShape);
            }
            else
            {
                CreateUnoriented(vertexShape);
            }
        }
        
        private void VertexStartMoving(object sender, MouseButtonEventArgs e)
        {
            (sender as VertexUI).Opacity = 0.5;
            source = (UIElement)sender;
            Mouse.Capture(source);
            capturedObject = true;
            XcapturedObject = Canvas.GetLeft(source);
            Xcanvas = e.GetPosition(MainCanvas).X;
            YcapturedObject = Canvas.GetTop(source);
            Ycanvas = e.GetPosition(MainCanvas).Y;
        }
        private void VertexMoving(object sender, MouseEventArgs e)
        {
            if (capturedObject)
            {
                double x = e.GetPosition(MainCanvas).X;
                double y = e.GetPosition(MainCanvas).Y;
                if (x < 0 || y < 0 || x > MainCanvas.ActualWidth || y > MainCanvas.ActualHeight)
                    return;
                XcapturedObject += x - Xcanvas;
                Canvas.SetLeft(source, XcapturedObject);
                Xcanvas = x;
                YcapturedObject += y - Ycanvas;
                Canvas.SetTop(source, YcapturedObject);
                Ycanvas = y;
            }
        }
        private void VertexEndMoving(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (mode != OperationMode.None && mode != OperationMode.AlgorithmFast && mode != OperationMode.Algorithm) return;
            //reset and drop
            (sender as VertexUI).Opacity = 1;
            Mouse.Capture(null);
            capturedObject = false;
        }

        private void AddEvents(VertexUI vertexShape)
        {
            vertexShape.MouseLeftButtonDown += VertexClick;
            vertexShape.MouseLeftButtonUp += VertexEndMoving;
            vertexShape.MouseMove += VertexMoving;
        }

        private void CreateOriented(VertexUI vertex)
        {
            var edgeShape = new EdgeUI()
            {
                StartVertex = selectedVertex,
                EndVertex = vertex,
                Weight = 1,
                IsWeighted = true,
                IsDirected = true
            };
            edgeShape.MouseLeftButtonUp += EdgeClick;

            mainGraph.AddEdge(edgeShape, selectedVertex, vertex);
            MainCanvas.Children.Add(edgeShape);

            RemoveSelecting();
        }
        private void CreateUnoriented(VertexUI vertex)
        {
            var edgeShape = new EdgeUI()
            {
                StartVertex = selectedVertex,
                EndVertex = vertex,
                Weight = 1,
                IsWeighted = true,
                IsDirected = false
            };
            edgeShape.MouseLeftButtonUp += EdgeClick;

            mainGraph.AddEdge(edgeShape, vertex, selectedVertex);
            mainGraph.AddEdge(edgeShape, selectedVertex, vertex);
            MainCanvas.Children.Add(edgeShape);

            RemoveSelecting();
        }
        private void RemoveSelecting()
        {
            if (selectedVertex != null)
            {
                SetBackground(selectedVertex, StartVertexColor, new { R = 0, G = 0, B = 0 }); //reset background of selected ellipse
                selectedVertex = null;
            }
        }
        private void SetBackground(VertexUI e, SolidColorBrush backgorund, dynamic border)
        {
            e.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)backgorund.Color.R, (byte)backgorund.Color.G, (byte)backgorund.Color.B));
        }        

        private void SetLeftButtonStyles()
        {
            CursorSwitch.Background = new SolidColorBrush(Colors.White);
            VertexSwitch.Background = new SolidColorBrush(Colors.White);
            OrientedEdgeSwitch.Background = new SolidColorBrush(Colors.White);
            EdgeSwitch.Background = new SolidColorBrush(Colors.White);
            DeleteSwitch.Background = new SolidColorBrush(Colors.White);
        }

        public void AlgorithmSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chosenAlgorithm = chooseAlgorithm.SelectedItem as string;
        }

        public void Play(object sender, RoutedEventArgs e)
        {
            if (mode != OperationMode.Algorithm && mode != OperationMode.AlgorithmFast &&
                mode != OperationMode.SelectVertex && mode != OperationMode.SelectVertexForFast)
            {
                RemoveSelecting();
                mode = OperationMode.Algorithm;
                ChangeImage(play, "StopButton");
                switch (chosenAlgorithm)
                {
                    case "Kruskal":
                        RunAlgorithm(new Kruskal(mainGraph));
                        break;
                    case "Kahn":
                        RunAlgorithm(new Kahn(mainGraph));
                        break;
                    case "Topological sort":
                        RunAlgorithm(new TopologicalSort(mainGraph));
                        break;
                    case "Tarjan":
                        RunAlgorithm(new Tarjan(mainGraph));
                        break;
                    case "Cycle Search":
                        RunAlgorithm(new CycleFind(mainGraph));
                        break;
                    case "Kosaraju":
                        RunAlgorithm(new Kosaraju(mainGraph));
                        break;
                    default:
                        AskForVertexChoose(false);
                        break;
                }
            }
            else if (mode == OperationMode.Algorithm || mode == OperationMode.SelectVertex)
            {
                ChangeImage(play, "PlayButton");
                ReturnStartGraph();
                MainCanvas.Children.Remove(selectVertexLabel);
                mode = OperationMode.None;
            }
        }

        public void FastPlay(object sender, RoutedEventArgs e)
        {
            if (mode != OperationMode.Algorithm && mode != OperationMode.AlgorithmFast &&
                mode != OperationMode.SelectVertex && mode != OperationMode.SelectVertexForFast)
            {
                isPlaying = true;
                RemoveSelecting();
                mode = OperationMode.AlgorithmFast;
                ChangeImage(fast, "StopButton");
                switch (chosenAlgorithm)
                {
                    case "Kruskal":
                        RunAlgorithmFast(new Kruskal(mainGraph));
                        break;
                    case "Kahn":
                        RunAlgorithmFast(new Kahn(mainGraph));
                        break;
                    case "Topological sort":
                        RunAlgorithmFast(new TopologicalSort(mainGraph));
                        break;
                    case "Tarjan":
                        RunAlgorithmFast(new Tarjan(mainGraph));
                        break;
                    case "Cycle Search":
                        RunAlgorithmFast(new CycleFind(mainGraph));
                        break;
                    case "Kosaraju":
                        RunAlgorithmFast(new Kosaraju(mainGraph));
                        break;
                    default:
                        AskForVertexChoose(true);
                        break;
                }
            }
            else if (mode == OperationMode.AlgorithmFast || mode == OperationMode.SelectVertexForFast)
            {
                isPlaying = false;
                ChangeImage(fast, "FastButton");
                ReturnStartGraph();
                MainCanvas.Children.Remove(selectVertexLabel);
                mode = OperationMode.None;
            }
        }

        public void ChangeSpeed(object sender, RoutedEventArgs e)
        {
            speed = speedChanger.Value;
        }

        public void StepForward(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mode == OperationMode.Algorithm)
                {
                    if (eventStack != null && eventIndex < eventStack.Events.Count)
                    {
                        ClearDescriptions();
                        SetAllCodeLinesInactive();
                        var ev = eventStack.Events[eventIndex];
                        ev.Run();
                        ev.Line?.SetActive();
                        variableStack.SetVariables(ev.Locals);
                        eventIndex++;
                        if (!ev.IsVisualizable)
                            StepForward(sender, e);
                    }
                }
            }
            catch (Exception) { }
        }

        public void StepBack(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mode == OperationMode.Algorithm)
                {
                    if (eventStack != null && eventIndex > 0)
                    {
                        ClearDescriptions();
                        SetAllCodeLinesInactive();
                        eventIndex--;
                        var ev = eventStack.Events[eventIndex];
                        ev.PlayBack();
                        ev.Line?.SetActive();
                        variableStack.SetVariables(ev.Locals);
                        if (!ev.IsVisualizable)
                            StepBack(sender, e);
                    }
                }
            }
            catch (Exception) { }
        }

        public void PlayToEnd(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mode == OperationMode.Algorithm)
                {
                    if (eventStack != null && eventIndex < eventStack.Events.Count)
                    {
                        ClearDescriptions();
                        SetAllCodeLinesInactive();
                        var ev = eventStack.Events[eventIndex];
                        ev.Run();
                        ev.Line?.SetActive();
                        variableStack.SetVariables(ev.Locals);
                        eventIndex++;
                        PlayToEnd(sender, e);
                    }
                }
            }
            catch (Exception) { }
        }

        private async void RunAlgorithm(IAlgorithm algorithm)
        {
            try
            {
                algorithm.Run(selectedForAlgorithm);
                SetCodeLines(algorithm);
                eventStack = algorithm.EventStack;
                eventIndex = 0;
                if (algorithm.HasOutput)
                    buttonStack.Children.Add(new SaveResultsButton(algorithm));
            }
            catch (Exception) { }
        }

        public void SaveGraph(object sender, RoutedEventArgs e)
        {
            try
            {
                FileUtils.Save(mainGraph);
            }
            catch (Exception) { }
        }

        public void SaveMatrix(object sender, RoutedEventArgs e)
        {
            try
            {
                FileUtils.SaveMatrix(mainGraph);
            }
            catch (Exception) { }
        }

        public void OpenMatrix(object sender, RoutedEventArgs e)
        {
            Graph graph = null;
            try
            {
                graph = FileUtils.OpenMatrix();
            }
            catch (Exception)
            {
                ShowErrorMessage();
            }
            PlaceGraphOnCanvas(graph);
        }

        public void OpenGraph(object sender, RoutedEventArgs e)
        {
            Graph graph = null;
            try
            {
                graph = FileUtils.Open();
            }
            catch (Exception)
            {
                ShowErrorMessage();
            }
            PlaceGraphOnCanvas(graph);
        }

        private async void RunAlgorithmFast(IAlgorithm algorithm)
        {
            algorithm.Run(selectedForAlgorithm);
            SetCodeLines(algorithm);
            eventStack = algorithm.EventStack;
            eventIndex = 0;
            while (isPlaying && eventIndex < eventStack.Events.Count)
            {
                if (speed != 0)
                {
                    if (eventStack != null && eventIndex < eventStack.Events.Count)
                    {
                        ClearDescriptions();
                        SetAllCodeLinesInactive();
                        var ev = eventStack.Events[eventIndex];
                        ev.Run();
                        ev.Line?.SetActive();
                        variableStack.SetVariables(ev.Locals);
                        eventIndex++;
                        if (ev.IsVisualizable)
                            await Task.Delay((int)(50 / speed));
                    }                   
                }
                else
                {
                    await Task.Delay(1000);
                }
            }
        }

        private void ChangeImage(Button button, string name)
        {
            button.Style = this.FindResource(name) as Style;
        }

        private void ReturnStartGraph()
        {
            ClearDescriptions();
            foreach (var vertex in mainGraph.AdjacencyList)
            {
                SetBackground(vertex.Key, StartVertexColor, new { R = 0, G = 0, B = 0 });
                foreach (var edge in mainGraph.AdjacencyList[vertex.Key])
                {
                    if (edge.UIShape.IsReverted)
                        edge.UIShape.IsReverted = false;
                    edge.UIShape.Fill = new SolidColorBrush(Colors.Black);
                    edge.UIShape.arrow.line.StrokeThickness = 2;
                    edge.UIShape.arrow.polyLine.StrokeThickness = 2;
                    edge.UIShape.revertArrow.line.StrokeThickness = 2;
                    edge.UIShape.revertArrow.polyLine.StrokeThickness = 2;
                }
                var descriptions = vertex.Key.descriptions.descriptionList.Children;
                descriptions.RemoveRange(0, descriptions.Count);
            }
            codeStack.Children.Clear();
            variableStack.stack.Children.Clear();
            buttonStack.Children.Clear();
        }

        private void ClearDescriptions()
        {
            foreach (var vertex in mainGraph.AdjacencyList)
            {
                vertex.Key.descriptions.ClearDescriptions();
            }
        }

        public void ClearGraph(object sender, RoutedEventArgs e)
        {
            ClearGraph();
        }

        public void OpenAdjList(object sender, RoutedEventArgs e)
        {
            Graph graph = null;
            try
            {
                graph = FileUtils.OpenAdjList();
            }
            catch (Exception)
            {
                ShowErrorMessage();
            }
            PlaceGraphOnCanvas(graph);
        }

        public void SaveAdjList(object sender, RoutedEventArgs e)
        {
            FileUtils.SaveAdjList(mainGraph);
        }

        public void GenerateGraph(object sender, RoutedEventArgs e)
        {
            Graph graph = null;
            try
            {
                var window = ShowGenerateWindow();
                graph = GraphGenerator.GenerateGraph(window.Count, window.Probability);
            }
            catch (Exception)
            {
                ShowErrorMessage();
            }
            PlaceGraphOnCanvas(graph);
        }

        private void PlaceGraphOnCanvas(Graph graph)
        {
            try
            {
                if (graph != null)
                {
                    ClearGraph();
                    if (graph.AdjacencyList.Keys.Max(_ => _.LeftOffset) > MainCanvas.ActualWidth)
                    {
                        var coef = MainCanvas.ActualWidth / graph.AdjacencyList.Keys.Max(_ => _.LeftOffset);
                        foreach (var v in graph.AdjacencyList.Keys)
                        {
                            v.LeftOffset = v.LeftOffset * coef - 40;
                        }
                    }
                    if (graph.AdjacencyList.Keys.Max(_ => _.TopOffset) > MainCanvas.ActualHeight)
                    {
                        var coef = MainCanvas.ActualHeight / graph.AdjacencyList.Keys.Max(_ => _.TopOffset);
                        foreach (var v in graph.AdjacencyList.Keys)
                        {
                            v.TopOffset = v.TopOffset * coef - 40;
                        }
                    }
                    mainGraph = graph;
                    foreach (var vertex in mainGraph.AdjacencyList)
                    {
                        AddEvents(vertex.Key);
                        MainCanvas.Children.Add(vertex.Key);
                        foreach (var edge in vertex.Value)
                        {
                            try
                            {
                                edge.UIShape.MouseLeftButtonUp += EdgeClick;
                                MainCanvas.Children.Add(edge.UIShape);
                            }
                            catch (Exception) { }

                        }
                    }
                    NumberOfVertex = graph.AdjacencyList.Count + 1;
                }
            }
            catch (Exception e) { }
        }

        private void ClearGraph()
        {
            mainGraph = new Graph();
            MainCanvas.Children.Clear();
            NumberOfVertex = 1;
        }

        private void SetAllCodeLinesInactive()
        {
            foreach (CodeLine line in codeStack.Children)
            {
                line.SetInactive();
            }
        }
        private void SetCodeLines(IAlgorithm algorithm)
        {
            if (algorithm.Lines != null)
            {
                foreach (var line in algorithm.Lines)
                {
                    codeStack.Children.Add(line);
                }
            }
        }
        private GraphGenerateWindow ShowGenerateWindow()
        {
            GraphGenerateWindow window = new GraphGenerateWindow();
            window.Left = this.Left + this.ActualWidth / 2;
            window.Top = this.Top + this.ActualHeight / 2;
            window.ShowDialog();
            return window;
        }
        
        private void ShowErrorMessage()
        {
            OpenErrorWindow error = new OpenErrorWindow("Wrong file format");
            error.Left = this.Left + this.ActualWidth / 2;
            error.Top = this.Top + this.ActualHeight / 2;
            error.ShowDialog();
        }

        private void AskForVertexChoose(bool isFast = false)
        {
            mode = isFast ? OperationMode.SelectVertexForFast : OperationMode.SelectVertex;
            if (!MainCanvas.Children.Contains(selectVertexLabel))
                MainCanvas.Children.Add(selectVertexLabel);
        }
    }
}