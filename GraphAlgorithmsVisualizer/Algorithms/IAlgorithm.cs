using GraphAlgorithmsVisualizer.UI;
using System.Collections.Generic;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public interface IAlgorithm
    {
        List<CodeLine> Lines { get; }
        EventStack EventStack { get; }
        bool HasOutput { get; }
        void Run(VertexUI startVertex);
        List<string> GetOutput();
    }
}
