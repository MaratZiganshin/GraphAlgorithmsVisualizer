using System.Windows;

namespace GraphAlgorithmsVisualizer.UI
{
    public partial class OpenErrorWindow : Window
    {
        public OpenErrorWindow(string text)
        {
            InitializeComponent();
            btnClose.Focus();
            Label.Content = text;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
