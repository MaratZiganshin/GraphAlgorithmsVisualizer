using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GraphAlgorithmsVisualizer.UI
{
    /// <summary>
    /// Логика взаимодействия для GraphGenerateWindow.xaml
    /// </summary>
    public partial class GraphGenerateWindow : Window
    {
        public double Probability { get; private set; }
        public int Count { get; private set; }
        public GraphGenerateWindow()
        {
            InitializeComponent();
            Probability = 0.25;
            Count = -1;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            if (Count > 0)
            {
                this.Close();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ChangeGraphType(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            switch ((comboBox.SelectedItem as ComboBoxItem).Name)
            {
                case "Sparse":
                    Probability = 0.1;
                    break;
                case "Normal":
                    Probability = 0.25;
                    break;
                case "Tight":
                    Probability = 0.5;
                    break;
            }
        }

        private void CountChange(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            int value = -1;
            int.TryParse(textBox.Text, out value);
            Count = value;
        }
    }
}
