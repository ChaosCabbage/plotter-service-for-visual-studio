using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMC.PlotterService
{
    class Plotter2DService : IPlotter2DService
    {
        PlotterControl _plotter;
        public Plotter2DService(PlotterToolWindow window)
        {
            if (window == null) throw new ArgumentNullException("window");
            _plotter = window.Content as PlotterControl;
        }

        public void AddPointSeries(IEnumerable<Drawing.PlotterPosition> pointSeries)
        {
            _plotter.AddSeries(pointSeries);
        }

        public void ClearAll()
        {
            throw new NotImplementedException();
        }
    }
}
