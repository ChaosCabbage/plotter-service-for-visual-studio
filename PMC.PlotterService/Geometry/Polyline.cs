using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMC.PlotterService.Geometry
{
    public class Polyline
    {
        public Polyline(IEnumerable<Position> points)
        {
            Points = points;
        }
        
        public IEnumerable<Position> Points
        {
            get;
            private set;
        }

    }
}
