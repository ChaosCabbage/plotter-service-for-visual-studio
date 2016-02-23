using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMC.PlotterService.Geometry
{
    internal class Collection
    {
        public Collection()
        {
            Polylines = new List<Polyline>();
            Points = new List<Position>();
        }

        public List<Polyline> Polylines { get; private set; }
        public List<Position> Points { get; private set; }
    }
}
