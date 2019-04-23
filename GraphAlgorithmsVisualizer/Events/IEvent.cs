using GraphAlgorithmsVisualizer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithmsVisualizer.Events
{
    public interface IEvent
    {
        Dictionary<string, string> Locals { get; }
        CodeLine Line { get; }
        void Run();
        void PlayBack();
        bool IsVisualizable { get; set; }
    }
}
