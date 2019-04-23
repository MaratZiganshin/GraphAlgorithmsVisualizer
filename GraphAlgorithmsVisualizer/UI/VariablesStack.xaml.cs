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
    /// Логика взаимодействия для VariablesStack.xaml
    /// </summary>
    public partial class VariablesStack : UserControl
    {
        public VariablesStack()
        {
            InitializeComponent();
        }
        
        public void SetVariables(Dictionary<string, string> variables)
        {
            if (variables == null)
                return; 

            foreach (var pair in variables)
            {
                bool found = false;
                foreach (VariableLine variableLine in stack.Children)
                {
                    if (variableLine.variableName.Text == pair.Key)
                    {
                        variableLine.variableValue.Text = pair.Value;
                        found = true;
                    }
                }
                if (!found)
                {
                    stack.Children.Add(new VariableLine(pair.Key, pair.Value));
                }
            }

            var linesToDelete = new List<VariableLine>();
            foreach (VariableLine line in stack.Children)
            {
                bool found = false;
                foreach (var pair in variables)
                {
                    if (line.variableName.Text == pair.Key)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    linesToDelete.Add(line);
                }
            }

            foreach (var line in linesToDelete)
            {
                stack.Children.Remove(line);
            }
        }
    }
}
