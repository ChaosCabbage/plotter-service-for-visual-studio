using System;

namespace PMC.PlotterService.Drawing
{
    interface IZoomController
    {
        void ZoomIn();
        void ZoomOut();
    }

    interface IZoom
    {
        double Scale();
    }

    class ZoomLogic : IZoom
    {
        private readonly double _defaultScale = 5;
        private readonly double _zoomSpeed = 0.1;

        private int _currentZoom = 0;

        private readonly int _minimumZoom = -int.MaxValue;
        private readonly int _maximumZoom = int.MaxValue;

        private double CurrentFactor()
        {
            return Math.Pow(1 + _zoomSpeed, _currentZoom);
        }

        public double Scale()
        {
            return _defaultScale * CurrentFactor();
        }

        public void ZoomIn()
        {
            _currentZoom = Math.Min(_currentZoom + 1, _maximumZoom);
        }

        public void ZoomOut()
        {
            _currentZoom = Math.Max(_currentZoom - 1, _minimumZoom);
        }

    }

    class ZoomController : IZoomController
    {
        ZoomLogic _zoom;
        Action<CanvasPosition, PlotterPosition> _focus;
        IMousePositionService _mouse;

        public ZoomController(ZoomLogic zoom, Action<CanvasPosition, PlotterPosition> focusCanvas, IMousePositionService mouse)
        {
            _zoom = zoom;
            _focus = focusCanvas;
            _mouse = mouse;
        }

        private void Update(Action zoom)
        {
            var focusPoint = _mouse.PlotterPosition();
            var canvasPoint = _mouse.CanvasPosition();
            zoom();
            _focus(canvasPoint, focusPoint);
        }

        public void ZoomIn()
        {
            Update(_zoom.ZoomIn);
        }

        public void ZoomOut()
        {
            Update(_zoom.ZoomOut);
        }
    }
}
