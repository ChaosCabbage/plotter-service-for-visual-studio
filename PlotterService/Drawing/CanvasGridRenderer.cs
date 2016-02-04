using System;
using System.Windows.Controls;
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

    class CanvasGridRenderer : IGridRenderer
    {
        readonly CanvasGraphicsController _graphics;

        IZoom _zoom;
        Origin _origin;
        IMousePositionService _mouse;

        public CanvasGridRenderer(Canvas c)
        {
            _graphics = new CanvasGraphicsController(c);
        }

        public void Start(IZoom zoom, Origin o, IMousePositionService mouse)
        {
            _zoom = zoom;
            _origin = o;
            _mouse = mouse;
        }

        public void Draw()
        {
            _graphics.Clear();
            DrawGrid();
            DrawMousePos();
        }

        private void DrawMousePos()
        {
            //          DrawSnapPoint();
            DrawCoordinates();
        }

        private void DrawCoordinates()
        {
            //           var snapped = adjustedPos(pos);
            var snapped = _mouse.PlotterPosition();
            //           context.font = "16px Century Gothic";
            //           context.fillStyle = colours.mouse.coordinates;
            //            context.textAlign = "left";


            var x = snapped.X.ToString("F2");
            var y = snapped.Y.ToString("F2");
            _graphics.DrawCenteredText(
                "(" + x + "," + y + ")",
                new CanvasPosition { X = 1, Y = 1 },
                "Century Gothic", 16
            );
        }

        //        private void DrawSnapPoint(PlotterPosition pos) {	
        //		    if (_snap.HasSnapped(pos)) {
        //               var canvasPos = CanvasPosFromPicturePos(_snap.Snap(pos));
        //              _graphics.DrawAlignedCircle(canvasPos, 5, ColourScheme.SnapGrid);
        //		    } 
        //	    }

        private void DrawGrid()
        {
            DrawMinorAxisDivisions();
            DrawMajorAxes();
            DrawGridLabels();
        }

        private double nextPowerOfTen(double x)
        {
            var exp = Math.Floor(Math.Log10(x));
            if (x == Math.Pow(10, exp))
            {
                return Math.Pow(10, exp);
            }
            else
            {
                return Math.Pow(10, exp + 1);
            }
        }

        private double gridDivisionUnit()
        {
            var smallest_allowed_division = 25; //No closer than 25 pixels
            var units_in_smallest = smallest_allowed_division / ZoomScale();
            return nextPowerOfTen(units_in_smallest);
        }

        private int currentGridDecimalPlaces()
        {
            return -(int)Math.Floor(Math.Log10(gridDivisionUnit()));
        }

        private void DrawGridLabels()
        {
            int dp = Math.Max(0, currentGridDecimalPlaces());
            string fixedDp = "F" + dp.ToString();   // e.g. "F6" means decimal with exactly 6 d.p.

            Func<double, string> format = (x =>
            {
                return x.ToString(fixedDp);
            });

            drawGridXLabels(format);
            drawGridYLabels(format);
        }

        private void drawGridYLabels(Func<double, string> format)
        {
            var unit = gridDivisionUnit();
            var division_width = unit * ZoomScale();

            var x = _origin.Point.X - 5;

            var y = Math.Floor(_origin.Point.Y % division_width) + 0.5;
            while (y < 0)
            {
                y += division_width;
            }
            var y_label = unit * Math.Floor(_origin.Point.Y / division_width);
            while (y < _graphics.Height())
            {
                _graphics.DrawEndAlignedText(
                    format(y_label),
                    new CanvasPosition { X = x, Y = y - 12 },
                    "Georgia", 12
                );
                y_label -= unit;
                y += division_width;
            }
        }

        private void drawGridXLabels(Func<double, string> format)
        {
            var unit = gridDivisionUnit();
            var division_width = unit * ZoomScale();

            double x = Math.Floor(_origin.Point.X % division_width) + 0.5;
            double y = _origin.Point.Y;


            while (x < 0)
            {
                x += division_width;
            }

            var x_label = -unit * Math.Floor(_origin.Point.X / division_width);
            while (x < _graphics.Width())
            {
                _graphics.DrawCenteredText(
                    format(x_label),
                    new CanvasPosition { X = x - 10, Y = y },
                    "Georgia", 12
                );
                x_label += unit;
                x += division_width;
            }
        }

        private void DrawMajorAxes()
        {
            double x = _origin.Point.X;
            double y = _origin.Point.Y;
            if (x >= -2 && x <= _graphics.Width() +2)
            {
                _graphics.DrawFullVertical(_origin.Point.X, 4, ColourScheme.MajorAxis);
            }
            if (y >= -2 && y <= _graphics.Height() + 2)
            {
                _graphics.DrawFullHorizontal(_origin.Point.Y, 4, ColourScheme.MajorAxis);
            }
        }

        private void DrawMinorAxisDivisions()
        {
            var division_width = gridDivisionUnit() * ZoomScale();
            var vertical_x = _origin.Point.X % division_width;
            while (vertical_x < _graphics.Width())
            {
                _graphics.DrawFullVertical(vertical_x, 1, ColourScheme.MinorAxis);
                vertical_x += division_width;
            }

            var horizontal_y = _origin.Point.Y % division_width;
            while (horizontal_y < _graphics.Height())
            {
                _graphics.DrawFullHorizontal(horizontal_y, 1, ColourScheme.MinorAxis);
                horizontal_y += division_width;
            }
        }

        private double ZoomScale()
        {
            return _zoom.Scale();
        }

        private PlotterPosition PicturePosFromCanvasPos(CanvasPosition pos)
        {
            return PointConversions.PlotterFromCanvas(pos, ZoomScale(), _origin.Point);
        }

        private CanvasPosition CanvasPosFromPicturePos(PlotterPosition pos)
        {
            return PointConversions.CanvasFromPlotter(pos, ZoomScale(), _origin.Point);
        }

    }
}
