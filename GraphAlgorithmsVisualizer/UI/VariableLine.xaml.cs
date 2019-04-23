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
    /// Логика взаимодействия для VariableLine.xaml
    /// </summary>
    public partial class VariableLine : UserControl
    {
        public VariableLine(string name, string value)
        {
            InitializeComponent();
            variableName.Text = name;
            variableValue.Text = value;
        }
    }
}
