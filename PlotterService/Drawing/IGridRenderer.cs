namespace PMC.PlotterService.Drawing
{
    interface IGridRenderer
    {
        void Start(IZoom zoom, Origin o, IMousePositionService lastMousePosition);
        void Draw();
    }
}