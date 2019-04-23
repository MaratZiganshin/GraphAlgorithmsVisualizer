using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphAlgorithmsVisualizer.UI
{
    public partial class EdgeUI : UserControl, INotifyPropertyChanged
    {
        private VertexUI startVertex;

        private VertexUI endVertex;

        public static readonly DependencyProperty IsDirectedProperty;

        public static readonly DependencyProperty IsWeightedProperty;

        public static readonly DependencyProperty WeightBoxHeightProperty;

        public static readonly DependencyProperty WeightBoxWidthProperty;

        public static readonly DependencyProperty IsRevertedProperty;

        static EdgeUI()
        {
            IsDirectedProperty =
                DependencyProperty.Register("IsDirected",
                typeof(bool), typeof(EdgeUI),
                new PropertyMetadata(false));

            IsRevertedProperty =
                DependencyProperty.Register("IsReverted",
                typeof(bool), typeof(EdgeUI),
                new PropertyMetadata(false));

            IsWeightedProperty =
                DependencyProperty.Register("IsWeighted",
                typeof(bool), typeof(EdgeUI),
                new PropertyMetadata(false));

            WeightBoxHeightProperty =
            DependencyProperty.Register("WeightBoxHeight",
                typeof(double), typeof(EdgeUI),
                new PropertyMetadata(20.0d));

            WeightBoxWidthProperty =
            DependencyProperty.Register("WeightBoxWidth",
                typeof(double), typeof(EdgeUI),
                new PropertyMetadata(30.0d));
        }

        public EdgeUI()
        {
            InitializeComponent();
        }

        public bool IsDirected
        {
            get { return (bool)GetValue(IsDirectedProperty); }
            set
            {
                SetValue(IsDirectedProperty, value);
                OnPropertyChanged("DirectionVisibility");
            }
        }

        public bool IsReverted
        {
            get { return (bool)GetValue(IsRevertedProperty); }
            set
            {
                SetValue(IsRevertedProperty, value);
                OnPropertyChanged("RevertedVisibility");
                SetValue(IsDirectedProperty, !value);
                OnPropertyChanged("DirectionVisibility");
            }
        }
        
        public bool IsWeighted
        {
            get { return (bool)GetValue(IsWeightedProperty); }
            set
            {
                SetValue(IsWeightedProperty, value);
                OnPropertyChanged("WeightVisibility");
            }
        }
        
        public double WeightBoxWidth
        {
            get { return (double)GetValue(WeightBoxWidthProperty); }
            set
            {
                SetValue(WeightBoxWidthProperty, value);
                OnPropertyChanged("TextTopLeftCorner");
            }
        }
        
        public double WeightBoxHeight
        {
            get { return (double)GetValue(WeightBoxHeightProperty); }
            set
            {
                SetValue(WeightBoxHeightProperty, value);
                OnPropertyChanged("TextTopLeftCorner");
            }
        }
        
        public VertexUI StartVertex
        {
            get { return startVertex; }
            set
            {
                if (value != null)
                {
                    value.PropertyChanged -= VertexCoordsChangedEventHandler;
                    value.PropertyChanged += VertexCoordsChangedEventHandler;
                }
                startVertex = value;
                VertexCoordsChangedEventHandler(value, new PropertyChangedEventArgs("StartVertex"));
            }
        }
        public VertexUI EndVertex
        {
            get { return endVertex; }
            set
            {
                if (value != null)
                {
                    value.PropertyChanged -= VertexCoordsChangedEventHandler;
                    value.PropertyChanged += VertexCoordsChangedEventHandler;
                }
                endVertex = value;
                VertexCoordsChangedEventHandler(value, new PropertyChangedEventArgs("EndVertex"));
            }
        }
        
        public Segment LinePoints
        {
            get
            {
                if (StartVertex == null || EndVertex == null)
                    return new Segment();
                double x1 = StartVertex.CenterPoint.X;
                double y1 = StartVertex.CenterPoint.Y;
                double x2 = EndVertex.CenterPoint.X;
                double y2 = EndVertex.CenterPoint.Y;
                double R1 = StartVertex.Radius;
                double R2 = StartVertex.Radius;
                double angle = Vector.AngleBetween(new Vector(1, 0), new Vector(EndVertex.CenterPoint.X - StartVertex.CenterPoint.X, EndVertex.CenterPoint.Y - StartVertex.CenterPoint.Y))
                    * (1 / 57.295779513);
                return new Segment
                {
                    From = new Point(x1 + R1 * Math.Cos(angle), y1 + R1 * Math.Sin(angle)),
                    To = new Point(x2 - R2 * Math.Cos(angle), y2 - R2 * Math.Sin(angle))
                };
            }
        }
        public Thickness TextTopLeftCorner
        {
            get { return new Thickness(arrow.Center.X - WeightBoxWidth / 2, arrow.Center.Y - WeightBoxHeight / 2, 0, 0); }
        }

        public Visibility DirectionVisibility
        {
            get { return IsDirected ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility RevertedVisibility
        {
            get { return IsReverted ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility WeightVisibility
        {
            get { return IsWeighted ? Visibility.Visible : Visibility.Collapsed; }
        }

        public SolidColorBrush Fill
        {
            get
            {
                return arrow.line.Stroke as SolidColorBrush;
            }
            set
            {
                arrow.polyLine.Stroke = value;
                arrow.line.Stroke = value;
                revertArrow.polyLine.Stroke = value;
                revertArrow.line.Stroke = value;
            }
        }

        public int Weight
        {
            get
            {
                int result = 1;
                int.TryParse(textbox.Text, out result);
                return result;
            }
            set
            {
                textbox.Text = value.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void VertexCoordsChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                OnPropertyChanged(e.PropertyName);
                OnPropertyChanged("LinePoints");
                OnPropertyChanged("TextTopLeftCorner");
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
