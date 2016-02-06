using System.Collections.Generic;
using System.Windows.Media;

namespace PMC.PlotterService.Drawing
{
    interface ISimpleGraphics
    {
        void Clear();
        void DrawAlignedCircle(CanvasPosition centre, double radius, Brush style);
        void DrawCenteredText(string text, CanvasPosition p, string font, int size);
        void DrawCircle(CanvasPosition centre, double radius, Brush style);
        void DrawCross(CanvasPosition position, double crossSize, double lineWidth, Brush style);
        void DrawEndAlignedText(string text, CanvasPosition p, string font, int size);
        void DrawFullHorizontal(double y, double lineWidth, Brush lineStyle);
        void DrawFullVertical(double x, double lineWidth, Brush lineStyle);
        void DrawLine(CanvasPosition start, CanvasPosition end, double lineWidth, Brush style);
        void DrawLines(IEnumerable<CanvasPosition> points, double lineWidth, Brush style);
        double Height();
        double Width();
    }
}