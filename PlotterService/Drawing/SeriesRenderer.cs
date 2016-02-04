using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace PMC.PlotterService.Drawing
{
    class SeriesRenderer : ISeriesRenderer
    {
        CanvasGraphicsController _graphics;

        readonly ICoordinateConverter _coords;

        public SeriesRenderer(Canvas c, ICoordinateConverter coords)
        {
            _graphics = new CanvasGraphicsController(c);
            _coords = coords;
        }

        public void Draw(IEnumerable<PlotterPosition> seriesEnumerable)
        {
            var series = seriesEnumerable.ToArray();
            if (series.Length == 0)
            {
                return;
            }

            var p0 = _coords.CanvasFromPlotter(series[0]);

            _graphics.DrawCircle(p0, 3, Brushes.Red);

            foreach (var point in series)
            {
                var p1 = _coords.CanvasFromPlotter(point);
                _graphics.DrawLine(p0, p1, 3, Brushes.Red);
                p0 = p1;
            }
        }
    }
}
