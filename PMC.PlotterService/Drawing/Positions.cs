using System.Windows.Controls;
using System.Windows.Input;

namespace PMC.PlotterService.Drawing
{
    public struct PlotterPosition { public double X; public double Y; }
    struct CanvasPosition { public double X; public double Y; }

    interface IMousePositionService
    {
        CanvasPosition CanvasPosition();
        PlotterPosition PlotterPosition();
    }

    interface ICanvasFocuser
    {
        /// <summary>
        /// Move the drawing so that position p in the drawing coincides with
        /// position c on the canvas.
        /// </summary>
        /// <param name="c">Canvas coordinate</param>
        /// <param name="p">Plotter diagram coordinate</param>
        void FocusCanvas(CanvasPosition c, PlotterPosition p);
    }

    interface ICoordinateConverter
    {
        PlotterPosition PlotterFromCanvas(CanvasPosition pos);
        CanvasPosition CanvasFromPlotter(PlotterPosition pos);
    }

    /// <summary>
    /// A position on the canvas that always corresponds to (0,0) of the plot.
    /// </summary>
    class Origin
    {
        public CanvasPosition Point { get; set; }
    }

    static class PointConversions
    {
        public static CanvasPosition CanvasFromMouse(MouseDevice m, System.Windows.IInputElement c)
        {
            var mousePos = m.GetPosition(c);
            return new CanvasPosition { X = mousePos.X, Y = mousePos.Y };
        }


        public static CanvasPosition CanvasFromPlotter(PlotterPosition point, double zoomScale, CanvasPosition origin)
        {
            return new CanvasPosition
            {
                X = (point.X * zoomScale) + origin.X,
                Y = origin.Y - (point.Y * zoomScale)
            };
        }

        public static PlotterPosition PlotterFromCanvas(CanvasPosition point, double zoomScale, CanvasPosition origin)
        {
            return new PlotterPosition
            {
                X = (point.X - origin.X) / zoomScale,
                Y = (origin.Y - point.Y) / zoomScale
            };
        }

    }

    /// <summary>
    /// A helpful class to wrap around the PointConversions when always using the same 
    /// zoom and origin.
    /// </summary>
    class CoordinateConverter : ICoordinateConverter
    {
        private readonly IZoom _zoom;
        private readonly Origin _origin;

        public CoordinateConverter(IZoom zoom, Origin o)
        {
            _zoom = zoom;
            _origin = o;
        }

        public PlotterPosition PlotterFromCanvas(CanvasPosition pos)
        {
            return PointConversions.PlotterFromCanvas(pos, _zoom.Scale(), _origin.Point);
        }

        public CanvasPosition CanvasFromPlotter(PlotterPosition pos)
        {
            return PointConversions.CanvasFromPlotter(pos, _zoom.Scale(), _origin.Point);
        }
    }
}
