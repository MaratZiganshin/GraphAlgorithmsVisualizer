using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace GraphAlgorithmsVisualizer.UI
{
    public class PositionableControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty LeftOffsetProperty;

        public static readonly DependencyProperty TopOffsetProperty;

        static PositionableControl()
        {
            LeftOffsetProperty =
            DependencyProperty.Register("LeftOffset",
                typeof(double), typeof(PositionableControl),
                new PropertyMetadata(double.NaN, new PropertyChangedCallback(OffsetPropertyChangedCallback)));

            TopOffsetProperty =
            DependencyProperty.Register("TopOffset",
                typeof(double), typeof(PositionableControl),
                new PropertyMetadata(double.NaN, new PropertyChangedCallback(OffsetPropertyChangedCallback)));
        }

        public PositionableControl()
        {

        }
        public double LeftOffset
        {
            get { return (double)GetValue(LeftOffsetProperty); }
            set { SetValue(LeftOffsetProperty, value); }
        }

        public double TopOffset
        {
            get { return (double)GetValue(TopOffsetProperty); }
            set { SetValue(TopOffsetProperty, value); }
        }

        public Point CenterPoint
        {
            get
            {
                double x = LeftOffset + Width / 2;
                double y = TopOffset + Height / 2;
                return new Point(x, y);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static void OffsetPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PositionableControl;
            control.OnPropertyChanged(e.Property.Name);
            control.OnPropertyChanged("CenterPoint");
        }
        protected void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
