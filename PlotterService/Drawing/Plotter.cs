using System;
using System.Collections.Generic;

using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PMC.PlotterService.Drawing
{
    class ColourScheme
    {
        static public readonly Brush Background = Brushes.SkyBlue;
        static public readonly Brush MajorAxis = Brushes.White;
        static public readonly Brush MinorAxis = Brushes.Lavender;
        static public readonly Brush Text = Brushes.Black;
        static public readonly Brush SnapGrid = Brushes.Blue;
    }

    class Plotter
    {
        GridRenderer _gridRenderer;
        SeriesRenderer _seriesRenderer;

        Origin _origin = new Origin();
        IZoom _zoom;

        MousePositionService _lastMousePosition;

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
        class MousePositionService : IMousePositionService
        {
            public CanvasPosition P;

            Func<CanvasPosition, PlotterPosition> _convert;

            public MousePositionService(Func<CanvasPosition, PlotterPosition> conversion)
            {
                _convert = conversion;
            }

            public CanvasPosition CanvasPosition()
            {
                return P;
            }

            public PlotterPosition PlotterPosition()
            {
                return _convert(P);
            }
        }

        List<IEnumerable<PlotterPosition>> _pointSerieses = new List<IEnumerable<PlotterPosition>>();

        public Plotter(Canvas c)
        {
            _origin.Point = new CanvasPosition
            {
                X = c.ActualWidth / 2,
                Y = c.ActualHeight / 2
            };

            var zoomLogic = new ZoomLogic();
            _zoom = zoomLogic;

            _lastMousePosition = new MousePositionService(this.PicturePosFromCanvasPos);

            _gridRenderer = new GridRenderer(c, _zoom, _origin, _lastMousePosition);
            _seriesRenderer = new SeriesRenderer(c, new CoordinateConverter(_zoom, _origin));

            var zoomer = new ZoomController(zoomLogic, this.FocusPosition, _lastMousePosition);

            //----- I'm just gonna set up all the input control here because why not

            c.SizeChanged +=
                (object sender, System.Windows.SizeChangedEventArgs e) =>
                {
                    Draw();
                };

            c.MouseMove +=
                (object sender, MouseEventArgs e) =>
                {
                    _lastMousePosition.P = PointConversions.CanvasFromMouse(e.MouseDevice, c);
                    Draw();
                };

            c.MouseWheel +=
                (object sender, MouseWheelEventArgs e) =>
                {
                    var delta = e.Delta;

                    if (delta > 0)
                    {
                        zoomer.ZoomIn();
                    }
                    else
                    {
                        zoomer.ZoomOut();
                    }

                    Draw();
                    e.Handled = true;
                };

            new MousePanningHandler(_origin, _zoom, Draw).AddDragHandler(c);


            Draw();
        }

        public void AddPointSeries(IEnumerable<PlotterPosition> series)
        {
            _pointSerieses.Add(series);
            Draw();
        }

        private void Draw()
        {
            _gridRenderer.Draw();
            DrawSerieses();
        }

        private void DrawSerieses()
        {
            foreach (var series in _pointSerieses)
            {
                _seriesRenderer.Draw(series);
            }
        }

        private void FocusPosition(CanvasPosition canvas_pos, PlotterPosition picture_pos)
        {
            var focus_pos = CanvasPosFromPicturePos(picture_pos);
            var dx = canvas_pos.X - focus_pos.X;
            var dy = canvas_pos.Y - focus_pos.Y;

            _origin.Point = new CanvasPosition
            {
                X = _origin.Point.X + dx,
                Y = _origin.Point.Y + dy
            };
        }

        private PlotterPosition PicturePosFromCanvasPos(CanvasPosition pos)
        {
            return PointConversions.PlotterFromCanvas(pos, _zoom.Scale(), _origin.Point);
        }

        private CanvasPosition CanvasPosFromPicturePos(PlotterPosition pos)
        {
            return PointConversions.CanvasFromPlotter(pos, _zoom.Scale(), _origin.Point);
        }


    }
}
