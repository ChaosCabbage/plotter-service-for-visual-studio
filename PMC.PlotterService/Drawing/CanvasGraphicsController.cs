using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;

namespace PMC.PlotterService.Drawing
{
    /// <summary>
    /// Abstraction around a WPF canvas, to provide and interface more like the HTML5 canvas.
    /// </summary>
    class CanvasGraphicsController : ISimpleGraphics
    {
        Canvas _canvas;

        public CanvasGraphicsController(Canvas c)
        {
            _canvas = c;
        }

        private void DrawObject(UIElement element)
        {
            element.IsHitTestVisible = false;
            _canvas.Children.Add(element);
        }

        public double Height()
        {
            return _canvas.ActualHeight;
        }

        public double Width()
        {
            return _canvas.ActualWidth;
        }

        public void Clear()
        {
            _canvas.Children.Clear();
        }

        public void DrawFullVertical(double x, double lineWidth, Color linecolour)
        {
            x = AlignedToPixel(x, lineWidth);
            var line = new Line();
            line.StrokeThickness = lineWidth;
            line.Stroke = new SolidColorBrush(linecolour);
            line.X1 = x;
            line.Y1 = 0;
            line.X2 = x;
            line.Y2 = Height();

            DrawObject(line);
        }

        public void DrawCircle(CanvasPosition centre, double radius, Color colour)
        {
            var circle = new Ellipse();
            circle.Height = 2 * radius;
            circle.Width = 2 * radius;
            circle.Stroke = new SolidColorBrush(colour);
            circle.Fill = new SolidColorBrush(colour);
            Canvas.SetLeft(circle, centre.X - radius);
            Canvas.SetTop(circle, centre.Y - radius);
            DrawObject(circle);
        }

        public void DrawAlignedCircle(CanvasPosition centre, double radius, Color colour)
        {
            centre.X = AlignedToPixel(centre.X, 2 * radius);
            centre.Y = AlignedToPixel(centre.Y, 2 * radius);
            DrawCircle(centre, radius, colour);
        }

        public void DrawCenteredText(string text, CanvasPosition p, string font, int size)
        {
            DrawText(text, p, HorizontalAlignment.Center, font, size);
        }

        public void DrawEndAlignedText(string text, CanvasPosition p, string font, int size)
        {
            DrawText(text, p, HorizontalAlignment.Right, font, size);
        }

        private void DrawText(string text, CanvasPosition p, HorizontalAlignment align, string font, int size)
        {
            var label = new Label();
            label.FontSize = size;
            label.FontFamily = new FontFamily(font);
            label.Content = text;
            label.HorizontalAlignment = align;
            Canvas.SetLeft(label, p.X);
            Canvas.SetTop(label, p.Y);
            DrawObject(label);
        }

        public void DrawFullHorizontal(double y, double lineWidth, Color linecolour)
        {
            y = AlignedToPixel(y, lineWidth);
            var line = new Line();
            line.StrokeThickness = lineWidth;
            line.Stroke = new SolidColorBrush(linecolour);
            line.X1 = 0;
            line.Y1 = y;
            line.X2 = Width();
            line.Y2 = y;

            DrawObject(line);
        }

        public void DrawLine(CanvasPosition start, CanvasPosition end, double lineWidth, Color colour)
        {
            var line = new Line();
            line.StrokeThickness = lineWidth;
            line.Stroke = new SolidColorBrush(colour);
            line.X1 = start.X;
            line.Y1 = start.Y;
            line.X2 = end.X;
            line.Y2 = end.Y;

            DrawObject(line);
        }

        public void DrawCross(CanvasPosition position, double crossSize, double lineWidth, Color colour)
        {
            var leftX = position.X - crossSize;
            var rightX = position.X + crossSize;

            var topY = position.Y - crossSize;
            var bottomY = position.Y + crossSize;

            DrawLine(
                new CanvasPosition { X = leftX, Y = topY },
                new CanvasPosition { X = rightX, Y = bottomY },
                lineWidth, colour);

            DrawLine(
                new CanvasPosition { X = leftX, Y = bottomY },
                new CanvasPosition { X = rightX, Y = topY },
                lineWidth, colour);
        }

        private double AlignedToPixel(double value, double line_width)
        {
            if (line_width % 2 == 0)
            {
                return Math.Floor(value);
            }
            else
            {
                return Math.Floor(value) + 0.5;
            }
        }

        public void DrawLines(IEnumerable<CanvasPosition> points, double lineWidth, Color colour)
        {
            if (points.Count() == 0)
            {
                return;
            }

            CanvasPosition previous = points.First();
            foreach (var p in points)
            {
                DrawLine(previous, p, lineWidth, colour);
                previous = p;
            }
        }
    }
}
