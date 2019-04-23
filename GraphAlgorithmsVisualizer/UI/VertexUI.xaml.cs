using System.Windows;
using System.Windows.Media;

namespace GraphAlgorithmsVisualizer.UI
{
    /// <summary>
    /// Логика взаимодействия для VertexUI.xaml
    /// </summary>
    public partial class VertexUI : PositionableControl
    {

        public static readonly DependencyProperty TextProperty;

        public static readonly DependencyProperty FillProperty;

        public static readonly DependencyProperty RadiusProperty;

        static VertexUI()
        {
            TextProperty = DependencyProperty.Register("Text",
                typeof(string), typeof(VertexUI),
                new PropertyMetadata(""));

            FillProperty =
            DependencyProperty.Register("Fill",
                typeof(SolidColorBrush), typeof(VertexUI),
                new PropertyMetadata(Brushes.Red));

            RadiusProperty =
            DependencyProperty.Register("Radius",
                typeof(double), typeof(VertexUI),
                new PropertyMetadata(double.NaN));
        }

        public VertexUI()
        {
            InitializeComponent();
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set
            {
                SetValue(RadiusProperty, value);
                OnPropertyChanged("Diameter");
            }
        }

        public double Diameter
        {
            get { return Radius * 2; }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public SolidColorBrush Fill
        {
            get { return (SolidColorBrush)GetValue(FillProperty); }
            set
            {
                if ((value.Color.R + value.Color.B + value.Color.G) / 3 > 128)
                    textDescription.Foreground = new SolidColorBrush(Colors.Black);
                else
                    textDescription.Foreground = new SolidColorBrush(Colors.White);
                SetValue(FillProperty, value);
            }
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
