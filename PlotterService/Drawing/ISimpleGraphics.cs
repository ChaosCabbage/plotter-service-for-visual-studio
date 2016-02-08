using System.Collections.Generic;
using System.Windows.Media;

namespace PMC.PlotterService.Drawing
{
    interface ISimpleGraphics
    {
        void Clear();
        void DrawAlignedCircle(CanvasPosition centre, double radius, Color style);
        void DrawCenteredText(string text, CanvasPosition p, string font, int size);
        void DrawCircle(CanvasPosition centre, double radius, Color style);
        void DrawCross(CanvasPosition position, double crossSize, double lineWidth, Color style);
        void DrawEndAlignedText(string text, CanvasPosition p, string font, int size);
        void DrawFullHorizontal(double y, double lineWidth, Color lineStyle);
        void DrawFullVertical(double x, double lineWidth, Color lineStyle);
        void DrawLine(CanvasPosition start, CanvasPosition end, double lineWidth, Color style);
        void DrawLines(IEnumerable<CanvasPosition> points, double lineWidth, Color style);
        double Height();
        double Width();
    }
}