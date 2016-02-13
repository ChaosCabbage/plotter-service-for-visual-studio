using System;
using System.Windows;
using System.Windows.Input;

namespace PMC.PlotterService.Drawing
{
    class MousePanningHandler
    {
        readonly Origin _origin;
        readonly IZoom _zoom;

        public MousePanningHandler(Origin o, IZoom zoom)
        {
            _origin = o;
            _zoom = zoom;
        }

        public void AddDragHandler(UIElement canvas)
        {
            MouseButtonEventHandler startDragging = null;
            startDragging = (object sender, MouseButtonEventArgs downEvent) =>
            {
                if (downEvent.ChangedButton != MouseButton.Middle)
                {
                    downEvent.Handled = false;
                    return;
                }

                downEvent.Handled = true;

                canvas.CaptureMouse();

                var dragStartPos = PointConversions.CanvasFromMouse(downEvent.MouseDevice, canvas);
                var originStartPos = _origin.Point;

                MouseEventHandler MiddleDragListener = (object o, MouseEventArgs moveEvent) =>
                {
                    var mouse_pos = PointConversions.CanvasFromMouse(moveEvent.MouseDevice, canvas);

                    _origin.Point = new CanvasPosition
                    {
                        X = originStartPos.X + (mouse_pos.X - dragStartPos.X),
                        Y = originStartPos.Y + (mouse_pos.Y - dragStartPos.Y),
                    };
                };

                MouseButtonEventHandler mouseUpListener = null;
                mouseUpListener = (object bob, MouseButtonEventArgs upEvent) =>
                {
                    if (upEvent.ChangedButton != MouseButton.Middle)
                    {
                        upEvent.Handled = false;
                        return;
                    }
                    canvas.ReleaseMouseCapture();

                    upEvent.Handled = true;

                    canvas.MouseMove -= MiddleDragListener;
                    canvas.MouseUp -= mouseUpListener;
                    canvas.MouseDown += startDragging;
                };

                canvas.MouseMove += MiddleDragListener;
                canvas.MouseUp += mouseUpListener;
                canvas.MouseDown -= startDragging;
            };

            canvas.MouseDown += startDragging;
        }
    }
}
