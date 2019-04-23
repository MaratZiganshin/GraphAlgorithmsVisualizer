using GraphAlgorithmsVisualizer.UI;

namespace GraphAlgorithmsVisualizer
{
    public class Edge
    {
        public VertexUI StartVertex { get; set; }
        public VertexUI EndVertex { get; set; }
        public EdgeUI UIShape { get; set; }
        public override string ToString()
        {
            return "Start: " + StartVertex.Text + " End: " + EndVertex.Text + " Weight: " + UIShape.Weight;
        }
    }
}
