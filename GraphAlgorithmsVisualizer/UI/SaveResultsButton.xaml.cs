using GraphAlgorithmsVisualizer.Algorithms;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;

namespace GraphAlgorithmsVisualizer.UI
{
    public partial class SaveResultsButton : UserControl
    {
        private IAlgorithm _algorithm;
        public SaveResultsButton(IAlgorithm algorithm)
        {
            _algorithm = algorithm;
            InitializeComponent();
        }

        private void SaveToFile(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "Result";
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text files (.txt)|*.txt";

            var result = dialog.ShowDialog();

            if (result == true)
            {
                var fileName = dialog.FileName;
                using (StreamWriter file = new StreamWriter(fileName))
                {
                    var strings = _algorithm.GetOutput();
                    foreach (var str in strings)
                    {
                        file.WriteLine(str);
                    }
                }
            }
        }
    }
}
