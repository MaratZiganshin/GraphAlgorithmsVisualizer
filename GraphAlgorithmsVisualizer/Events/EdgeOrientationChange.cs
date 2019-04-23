using System;
using System.Collections.Generic;
using GraphAlgorithmsVisualizer.UI;

namespace GraphAlgorithmsVisualizer.Events
{
    public class EdgeOrientationChange : IEvent
    {
        private Edge _edge;
        public bool IsVisualizable { get; set; }
        public CodeLine Line { get; }
        public Dictionary<string, string> Locals { get; }

        public EdgeOrientationChange(Edge edge)
        {
            _edge = edge;
        }

        public void PlayBack()
        {
            _edge.UIShape.IsReverted = false;
        }

        public void Run()
        {
            _edge.UIShape.IsReverted = true;
        }
    }
}
