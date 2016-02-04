using SharpGL;
using SharpGL.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMC.PlotterService.Drawing
{
    class GLGridRenderer : IGridRenderer
    {
        IZoom _zoom;
        Origin _origin;
        IMousePositionService _lastMousePosition;

        #region IGridRenderer members
        public void Start(IZoom zoom, Origin o, IMousePositionService lastMousePosition)
        {
            _zoom = zoom;
            _origin = o;
            _lastMousePosition = lastMousePosition;
        }

        public void Draw()
        {
            // Does nothing: The other Draw() is called automatically every frame.
        }

        #endregion

        public void Initialize(OpenGLEventArgs args)
        {
            if (_zoom == null || _origin == null || _lastMousePosition == null) throw new Exception("Start must be called first");

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
