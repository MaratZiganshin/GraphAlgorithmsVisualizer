using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphAlgorithmsVisualizer.UI
{
    /// <summary>
    /// Логика взаимодействия для VertexDescriptionStack.xaml
    /// </summary>
    public partial class VertexDescriptionStack : UserControl
    {
        private Dictionary<string, TextBlock> _textBlocks;
        public VertexDescriptionStack()
        {
            InitializeComponent();
            _textBlocks = new Dictionary<string, TextBlock>();
        }

        public void Add(string name, string value)
        {
            var textBlock = new TextBlock() { Text = $"{name}: {value}" };
            descriptionList.Children.Add(textBlock);
            _textBlocks[name] = textBlock;
        }

        public void Change(string name, string value)
        {
            _textBlocks[name].Text = $"{name}: {value}";
            _textBlocks[name].Foreground = new SolidColorBrush(Colors.Red);
        }

        public void ClearDescriptions()
        {
            foreach(var textBlock in _textBlocks.Values)
            {
                textBlock.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
