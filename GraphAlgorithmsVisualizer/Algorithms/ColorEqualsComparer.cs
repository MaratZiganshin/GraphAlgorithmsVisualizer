using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace GraphAlgorithmsVisualizer.Algorithms
{
    public class ColorEqualsComparer : EqualityComparer<SolidColorBrush>
    {
        private ColorEqualsComparer() { }

        public static ColorEqualsComparer Instance = new ColorEqualsComparer();
        public override bool Equals(SolidColorBrush x, SolidColorBrush y)
        {
            if (x.Color.B == y.Color.B && x.Color.R == y.Color.R && x.Color.G == y.Color.G)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode(SolidColorBrush obj)
        {
            return obj.GetHashCode();
        }
    }
}
