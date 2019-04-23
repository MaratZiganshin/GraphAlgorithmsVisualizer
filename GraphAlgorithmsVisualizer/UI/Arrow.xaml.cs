using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphAlgorithmsVisualizer.UI
{
    public partial class Arrow : UserControl, INotifyPropertyChanged
    {
        public const double RadianToDegree = 57.295779513;
        public const double DegreeToRadian = 1 / RadianToDegree;

        public static readonly DependencyProperty StartProperty;

        public static readonly DependencyProperty EndProperty;

        public static readonly DependencyProperty ArrowHeadLengthProperty;

        public static readonly DependencyProperty ArrowHeadAngleProperty;

        public static readonly DependencyProperty ArrowHeadVisibilityProperty;

        static Arrow()
        {
            StartProperty = DependencyProperty.Register("Start",
                typeof(Point), typeof(Arrow),
                new PropertyMetadata(default(Point), new PropertyChangedCallback(ArrowPropertyChangedCallback)));

            EndProperty = DependencyProperty.Register("End",
                typeof(Point), typeof(Arrow),
                new PropertyMetadata(default(Point), new PropertyChangedCallback(ArrowPropertyChangedCallback)));

            ArrowHeadLengthProperty = DependencyProperty.Register("ArrowHeadLength",
                typeof(double), typeof(Arrow),
                new PropertyMetadata(15.0d, new PropertyChangedCallback(ArrowPropertyChangedCallback)));

            ArrowHeadAngleProperty = DependencyProperty.Register("ArrowHeadAngle",
                typeof(double), typeof(Arrow),
                new PropertyMetadata(45.0d, new PropertyChangedCallback(ArrowPropertyChangedCallback)));

            ArrowHeadVisibilityProperty = DependencyProperty.Register("ArrowHeadVisibility",
                typeof(Visibility), typeof(Arrow),
                new PropertyMetadata(Visibility.Visible));
        }

        public Arrow()
        {
            InitializeComponent();
        }

        private double LineAngle
        {
            get
            {
                return Vector.AngleBetween(new Vector(1, 0), new Vector(End.X - Start.X, End.Y - Start.Y))
                    * DegreeToRadian;
            }
        }

        public Point Start
        {
            get { return (Point)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        public Point End
        {
            get { return (Point)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }

        public double ArrowHeadLength
        {
            get { return (double)GetValue(ArrowHeadLengthProperty); }
            set { SetValue(ArrowHeadLengthProperty, value); }
        }

        public double ArrowHeadAngle
        {
            get { return (double)GetValue(ArrowHeadAngleProperty); }
            set { SetValue(ArrowHeadAngleProperty, value); }
        }

        private double ArrowHeadAngleRad
        {
            get { return ArrowHeadAngle * DegreeToRadian; }
        }

        public Visibility ArrowHeadVisibility
        {
            get { return (Visibility)GetValue(ArrowHeadVisibilityProperty); }
            set { SetValue(ArrowHeadVisibilityProperty, value); }
        }

        public Point Center
        {
            get
            {
                return new Point
                {
                    X = (Start.X + End.X) / 2,
                    Y = (Start.Y + End.Y) / 2
                };
            }
        }

        public Point ArrowHeadBasePoint
        {
            get
            {
                return new Point
                {
                    X = End.X - ArrowHeadLength * Math.Cos(LineAngle),
                    Y = End.Y - ArrowHeadLength * Math.Sin(LineAngle)
                };
            }
        }

        public Point ArrowHeadRightPoint
        {
            get
            {
                return new Point
                {
                    X = ArrowHeadBasePoint.X - ArrowHeadLength * Math.Sin(LineAngle) * Math.Tan(ArrowHeadAngleRad / 2),
                    Y = ArrowHeadBasePoint.Y + ArrowHeadLength * Math.Cos(LineAngle) * Math.Tan(ArrowHeadAngleRad / 2)
                };
            }
        }

        public Point ArrowHeadLeftPoint
        {
            get
            {
                return new Point
                {
                    X = ArrowHeadBasePoint.X + ArrowHeadLength * Math.Sin(LineAngle) * Math.Tan(ArrowHeadAngleRad / 2),
                    Y = ArrowHeadBasePoint.Y - ArrowHeadLength * Math.Cos(LineAngle) * Math.Tan(ArrowHeadAngleRad / 2)
                };
            }
        }

        public PointCollection Points
        {
            get { return new PointCollection { ArrowHeadLeftPoint, End, ArrowHeadRightPoint }; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private static void ArrowPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Arrow;
            control.OnPropertyChanged(e.Property.Name);
            control.OnPropertyChanged("Points");
            control.OnPropertyChanged("Center");
            control.OnPropertyChanged("ArrowHeadBasePoint");
            control.OnPropertyChanged("LineAngle");
            control.OnPropertyChanged("ArrowHeadRightPoint");
            control.OnPropertyChanged("ArrowHeadLeftPoint");
        }
    }
}
