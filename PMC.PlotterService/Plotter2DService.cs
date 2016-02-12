using System;
using System.Collections.Generic;

namespace PMC.PlotterService
{
    class Plotter2DService : IPlotter2DService
    {
        PlotterControl _plotter;
        PlotterToolWindow _window;
        public Plotter2DService(PlotterToolWindow window)
        {
            if (window == null) throw new ArgumentNullException("window");
            _window = window;
            _plotter = window.Content as PlotterControl;
        }

        public void AddPointSeries(IEnumerable<Drawing.PlotterPosition> pointSeries)
        {
            _window.Show();
            _plotter.AddSeries(pointSeries);
        }

        public void ClearAll()
        {
            throw new NotImplementedException();
        }
    }
}
