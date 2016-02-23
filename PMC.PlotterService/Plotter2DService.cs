using System;

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

        public void AddPoint(Geometry.Position point)
        {
            _plotter.GeometryCollection.Points.Add(point);
            _window.Show();
        }

        public void AddPolyline(Geometry.Polyline polyline)
        {
            _plotter.GeometryCollection.Polylines.Add(polyline);
            _window.Show();
        }
    }
}
