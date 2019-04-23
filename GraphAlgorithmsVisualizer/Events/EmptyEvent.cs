using GraphAlgorithmsVisualizer.UI;
using System.Collections.Generic;

namespace GraphAlgorithmsVisualizer.Events
{
    public class EmptyEvent : IEvent
    {
        public Dictionary<string, string> Locals { get; }
        public CodeLine Line { get; }
        public bool IsVisualizable { get; set; }
        public EmptyEvent(CodeLine line, Dictionary<string, string> locals)
        {
            Locals = locals;
            Line = line;
            IsVisualizable = true;
        }

        public void Run()
        {
        }
        public void PlayBack()
        {
        }
    }
}
