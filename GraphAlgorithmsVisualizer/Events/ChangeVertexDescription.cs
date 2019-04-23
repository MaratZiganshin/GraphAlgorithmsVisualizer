using GraphAlgorithmsVisualizer.UI;
using System;
using System.Collections.Generic;

namespace GraphAlgorithmsVisualizer.Events
{
    public class ChangeVertexDescription : IEvent
    {
        private VertexUI _vertex;
        private string _name;
        private string _oldValue;
        private string _newValue;
        public Dictionary<string, string> Locals { get; }
        public CodeLine Line { get; }
        public bool IsVisualizable { get; set; }
        public ChangeVertexDescription(VertexUI vertex, string name, string oldValue, string newValue, CodeLine line, Dictionary<string, string> locals)
        {
            _vertex = vertex;
            _name = name;
            _oldValue = oldValue;
            _newValue = newValue;
            Line = line;
            Locals = locals;
            IsVisualizable = true;
        }
        public void Run()
        {
            _vertex.descriptions.Change(_name, _newValue ?? "null");
        }

        public void PlayBack()
        {
            _vertex.descriptions.Change(_name, _oldValue ?? "null");
        }
    }
}
