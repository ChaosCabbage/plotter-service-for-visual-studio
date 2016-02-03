using SharpGL;
using SharpGL.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMC.PlotterService.Drawing
{
    class GLPlotRenderer
    {
        public void Initialize(OpenGLEventArgs args)
        {
            OpenGL gl = args.OpenGL;

            // Background colour: Sky blue
            float r = 135;
            float g = 206;
            float b = 250;
            gl.ClearColor(r / 255, g / 255, b / 255, 1.0f);
        }

        private void ClearScreen(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
        }

        public void Draw(OpenGLEventArgs args)
        {
            OpenGL gl = args.OpenGL;

            ClearScreen(gl);

        }


    }
}
