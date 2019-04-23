using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithmsVisualizer
{
    /// <summary>
    /// List of available operations. 
    /// </summary>
    enum OperationMode
    {
        None,
        CreateVertex,
        CreateOrientedEdge,
        CreateEdge,
        Remove,
        Move,
        Generate,
        Algorithm,
        AlgorithmFast,
        SelectVertex,
        SelectVertexForFast
    };
}
