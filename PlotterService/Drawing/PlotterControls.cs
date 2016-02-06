﻿using System;
using System.Windows;
using System.Windows.Input;

namespace PMC.PlotterService.Drawing
{
    class PlotterControls
    {
        readonly CanvasGridRenderer _gridRenderer;

        Origin _origin = new Origin();
        IZoom _zoom;

        MousePositionService _lastMousePosition;

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
        
        public PlotterControls(FrameworkElement c, CanvasGridRenderer graphics)
        {
            _gridRenderer = graphics;
            _origin.Point = new CanvasPosition
            {
                X = c.ActualWidth / 2,
                Y = c.ActualHeight / 2
            };

            var zoomLogic = new ZoomLogic();
            _zoom = zoomLogic;

            _lastMousePosition = new MousePositionService(this.PicturePosFromCanvasPos);

            _gridRenderer.Start(_zoom, _origin, _lastMousePosition);

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

        private void Draw()
        {
            _gridRenderer.Draw();
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
