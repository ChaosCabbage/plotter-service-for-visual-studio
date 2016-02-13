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

            //I am setting a state where I am editing the projection matrix...
            _gl.MatrixMode(OpenGL.GL_PROJECTION);

            //Clearing the projection matrix...
            _gl.LoadIdentity();

            //Creating an orthoscopic view matrix going from -1 -> 1 in each
            //dimension on the screen (x, y, z). 
            _gl.Ortho(-1, 1, -1, 1, -1, 1);

            //Now editing the model-view matrix.
            _gl.MatrixMode(OpenGL.GL_MODELVIEW);

            //Clearing the model-view matrix.
            _gl.LoadIdentity();

            //Disabling the depth test (z will not be used to tell what object 
            //will be shown above another, only the order in which I draw them.)
            _gl.Disable(OpenGL.GL_DEPTH_TEST);
        }

        public void DrawFullVertical(double x, double lineWidth, Color colour)
        {
            _gl.Color(colour.R, colour.G, colour.B, colour.A); 
            _gl.LineWidth((float)lineWidth);

            _gl.Begin(OpenGL.GL_LINES);
              _gl.Vertex(GLx(x), -1);
              _gl.Vertex(GLx(x),  1);
            _gl.End();
        }

        public void DrawCircle(CanvasPosition centre, double radius, Color colour)
        {
            _gl.Color(colour.R, colour.G, colour.B, colour.A); 
            _gl.PointSize((float)radius*2);

            _gl.Begin(OpenGL.GL_POINTS);
            _gl.Vertex(GLPosition(centre));
            _gl.End();
        }
        
        public void DrawText(string text, CanvasPosition p, string font, int size)
        {
            _gl.DrawText((int)p.X, (int)(Height() - p.Y), 0, 0, 0, font, size, text);
        }

        public void DrawFullHorizontal(double y, double lineWidth, Color colour)
        {
            _gl.Color(colour.R, colour.G, colour.B, colour.A); 
            _gl.LineWidth((float)lineWidth);

            _gl.Begin(OpenGL.GL_LINES);
              _gl.Vertex(-1, GLy(y));
              _gl.Vertex( 1, GLy(y));
            _gl.End();
        }

        public void DrawLine(CanvasPosition start, CanvasPosition end, double lineWidth, Color colour)
        {
            _gl.Color(colour.R, colour.G, colour.B, colour.A); 
            _gl.LineWidth((float)lineWidth);

            _gl.Begin(OpenGL.GL_LINES);
              _gl.Vertex(GLPosition(start));
              _gl.Vertex(GLPosition(end));
            _gl.End();
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

        public void DrawAlignedCircle(CanvasPosition centre, double radius, Color colour)
        {
            DrawCircle(centre, radius, colour);
        }

        public void DrawCenteredText(string text, CanvasPosition p, string font, int size)
        {
            DrawText(text, p, font, size);
        }

        public void DrawEndAlignedText(string text, CanvasPosition p, string font, int size)
        {
            DrawText(text, p, font, size);
        }

        public void DrawLines(IEnumerable<CanvasPosition> points, double lineWidth, Color colour)
        {
            _gl.Color(colour.R, colour.G, colour.B, colour.A); 
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
