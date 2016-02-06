using SharpGL;
using System.Windows.Media;
using System;
using System.Collections.Generic;

namespace PMC.PlotterService.Drawing
{
    /// <summary>
    /// Abstraction around a WPF canvas, to provide and interface more like the HTML5 canvas.
    /// </summary>
    class GLImmediateGraphicsController : ISimpleGraphics
    {
        public interface ICanvasSize
        {
            int Width();
            int Height();
        }

        private readonly OpenGL _gl;
        private readonly ICanvasSize _size;

        public GLImmediateGraphicsController(OpenGL gl, ICanvasSize size)
        {
            _gl = gl;
            _size = size;
            
            _gl.MatrixMode(OpenGL.GL_PROJECTION);
            _gl.LoadIdentity();
            _gl.Ortho(-1, 1, -1, 1, -1, 1);
            _gl.Disable(OpenGL.GL_DEPTH_TEST);
        }

        private float[] GLPosition(CanvasPosition p)
        {
            return new float[] { GLx(p.X), GLy(p.Y) };
        }

        private float GLx(double x)
        {
            return (float)(2 * x / Width()) - 1;
        }

        private float GLy(double y)
        {
            return 1 - (float)(2 * y / Height());
        }

        public double Height()
        {
            return _size.Height();
        }

        public double Width()
        {
            return _size.Width();
        }

        public void Clear()
        {
            _gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
        }

        public void DrawFullVertical(double x, double lineWidth, Brush lineStyle)
        {
            _gl.Color(1.0f, 1.0f, 1.0f); //White
            _gl.LineWidth((float)lineWidth);

            _gl.Begin(OpenGL.GL_LINES);
              _gl.Vertex(GLx(x), -1);
              _gl.Vertex(GLx(x),  1);
            _gl.End();
        }

        public void DrawCircle(CanvasPosition centre, double radius, Brush style)
        {
            _gl.Color(1.0f, 1.0f, 1.0f); //White
            _gl.PointSize((float)radius*2);

            _gl.Begin(OpenGL.GL_POINTS);
            _gl.Vertex(GLPosition(centre));
            _gl.End();
        }
        
        public void DrawText(string text, CanvasPosition p, string font, int size)
        {
            _gl.DrawText((int)p.X, (int)(Height() - p.Y), 0, 0, 0, font, size, text);
        }

        public void DrawFullHorizontal(double y, double lineWidth, Brush lineStyle)
        {
            _gl.Color(1.0f, 1.0f, 1.0f); //White
            _gl.LineWidth((float)lineWidth);

            _gl.Begin(OpenGL.GL_LINES);
              _gl.Vertex(-1, GLy(y));
              _gl.Vertex( 1, GLy(y));
            _gl.End();
        }

        public void DrawLine(CanvasPosition start, CanvasPosition end, double lineWidth, Brush style)
        {
            _gl.Color(1.0f, 1.0f, 1.0f); //White
            _gl.LineWidth((float)lineWidth);

            _gl.Begin(OpenGL.GL_LINES);
              _gl.Vertex(GLPosition(start));
              _gl.Vertex(GLPosition(end));
            _gl.End();
        }

        public void DrawCross(CanvasPosition position, double crossSize, double lineWidth, Brush style)
        {
            var leftX = position.X - crossSize;
            var rightX = position.X + crossSize;

            var topY = position.Y - crossSize;
            var bottomY = position.Y + crossSize;

            DrawLine(
                new CanvasPosition { X = leftX, Y = topY },
                new CanvasPosition { X = rightX, Y = bottomY },
                lineWidth, style);

            DrawLine(
                new CanvasPosition { X = leftX, Y = bottomY },
                new CanvasPosition { X = rightX, Y = topY },
                lineWidth, style);
        }

        public void DrawAlignedCircle(CanvasPosition centre, double radius, Brush style)
        {
            DrawCircle(centre, radius, style);
        }

        public void DrawCenteredText(string text, CanvasPosition p, string font, int size)
        {
            DrawText(text, p, font, size);
        }

        public void DrawEndAlignedText(string text, CanvasPosition p, string font, int size)
        {
            DrawText(text, p, font, size);
        }

        public void DrawLines(IEnumerable<CanvasPosition> points, double lineWidth, Brush style)
        {
            _gl.Color(1.0f, 1.0f, 1.0f); //White
            _gl.LineWidth((float)lineWidth);

            _gl.Begin(OpenGL.GL_LINE_STRIP);
            foreach (var p in points)
            {
                _gl.Vertex(GLPosition(p));
            }
            _gl.End();
        }
    }
}
