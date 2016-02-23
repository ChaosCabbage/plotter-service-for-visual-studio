using System.Windows.Controls;
using System.Windows.Input;

namespace PMC.PlotterService.Drawing
{
    struct CanvasPosition { public double X; public double Y; }

    interface IMousePositionService
    {
        CanvasPosition CanvasPosition();
        Geometry.Position PlotterPosition();
    }

    interface ICanvasFocuser
    {
        /// <summary>
        /// Move the drawing so that position p in the drawing coincides with
        /// position c on the canvas.
        /// </summary>
        /// <param name="c">Canvas coordinate</param>
        /// <param name="p">Plotter diagram coordinate</param>
        void FocusCanvas(CanvasPosition c, Geometry.Position p);
    }

    interface ICoordinateConverter
    {
        Geometry.Position PlotterFromCanvas(CanvasPosition pos);
        CanvasPosition CanvasFromPlotter(Geometry.Position pos);
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


        public static CanvasPosition CanvasFromPlotter(Geometry.Position point, double zoomScale, CanvasPosition origin)
        {
            return new CanvasPosition
            {
                X = (point.X * zoomScale) + origin.X,
                Y = origin.Y - (point.Y * zoomScale)
            };
        }

        public static Geometry.Position PlotterFromCanvas(CanvasPosition point, double zoomScale, CanvasPosition origin)
        {
            return new Geometry.Position
            {
                X = (point.X - origin.X) / zoomScale,
                Y = (origin.Y - point.Y) / zoomScale
            };
        }

    }
}
