using System.Windows.Controls;
using System.Windows.Media;

namespace GraphAlgorithmsVisualizer.UI
{
    /// <summary>
    /// Логика взаимодействия для CodeLine.xaml
    /// </summary>
    public partial class CodeLine : UserControl
    {
        private SolidColorBrush inActiveColor;
        public CodeLine()
        {
            InitializeComponent();
            inActiveColor = (SolidColorBrush)FindResource("MaterialDesignPaper");
        }

        public void SetText(string text)
        {
            textBlock.Text = text;
        }

        public void SetActive()
        {
            textBlock.Background = new SolidColorBrush(Colors.OrangeRed);
        }

        public void SetInactive()
        {
            textBlock.Background = inActiveColor;
        }
    }
}
