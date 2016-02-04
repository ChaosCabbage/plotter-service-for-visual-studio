using SharpGL;
using SharpGL.SceneGraph;
using System;
using System.Runtime.InteropServices;

namespace PMC.PlotterService.Drawing
{
    class GLGridRenderer : IGridRenderer
    {
        IZoom _zoom;
        Origin _origin;
        IMousePositionService _lastMousePosition;

        OpenGL _gl;

        public void Start(IZoom zoom, Origin o, IMousePositionService lastMousePosition)
        {
            _zoom = zoom;
            _origin = o;
            _lastMousePosition = lastMousePosition;
        }
      
        private void AllOfTheCrazySetup()
        {
            var glops = new OpenGLHelp.BasicOperations(_gl);

            //----- VAO
            uint vao = glops.GenBuffer();
            _gl.BindVertexArray(vao);


            //----- DATA
            float[] vertices =
            {
		    //    X      Y    Lum  
			     0.0f,  0.5f, 0.3f,
                 0.5f, -0.5f, 0.8f,
                -0.5f, -0.5f, 0.6f,
            };
            const int positionFloats = 2;
            const int colourFloats = 1;
            const int floatsPerVertex = positionFloats + colourFloats;

            uint vbo = glops.BindDataToNewBuffer(vertices, OpenGL.GL_ARRAY_BUFFER, OpenGL.GL_STATIC_DRAW);

            //----- ELEMENT BUFFER
            uint[] elements = {
                0, 1, 2
            };

            uint ebo = glops.BindDataToNewBuffer(elements, OpenGL.GL_ELEMENT_ARRAY_BUFFER, OpenGL.GL_STATIC_DRAW);

            //----- SHADERS
            uint vertexShader = glops.CreateAndCompileShader(OpenGL.GL_VERTEX_SHADER, Resources.VertexShader);
            uint fragmentShader = glops.CreateAndCompileShader(OpenGL.GL_FRAGMENT_SHADER, Resources.FragmentShader);
            uint shaderProgram = _gl.CreateProgram();
            _gl.AttachShader(shaderProgram, vertexShader);
            _gl.AttachShader(shaderProgram, fragmentShader);
            _gl.BindFragDataLocation(shaderProgram, 0, "outColour");
            _gl.LinkProgram(shaderProgram);
            _gl.UseProgram(shaderProgram);

            //----- ATTRIBUTES
            int positionAttribute = _gl.GetAttribLocation(shaderProgram, "position");
            _gl.VertexAttribPointer((uint)positionAttribute, 2, OpenGL.GL_FLOAT, false, floatsPerVertex * Marshal.SizeOf(typeof(float)), IntPtr.Zero);
            _gl.EnableVertexAttribArray((uint)positionAttribute);

            int colourAttribute = _gl.GetAttribLocation(shaderProgram, "colour");
            _gl.VertexAttribPointer((uint)colourAttribute, 3, OpenGL.GL_FLOAT, false, floatsPerVertex * Marshal.SizeOf(typeof(float)), new IntPtr(positionFloats * Marshal.SizeOf(typeof(float))));
            _gl.EnableVertexAttribArray((uint)colourAttribute);
        }

        public void Initialize(OpenGLEventArgs args)
        {
            if (_zoom == null || _origin == null || _lastMousePosition == null) throw new Exception("Start must be called first");

            _gl = args.OpenGL;
            
            // Background colour: Sky blue
            float r = 135;
            float g = 206;
            float b = 250;
            _gl.ClearColor(r / 255, g / 255, b / 255, 1.0f);
            
            AllOfTheCrazySetup();
        }

        private void ClearScreen()
        {
            _gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
        }


        public void Draw()
        {
            if (_gl == null)
            {
                // Can be called before initialization by the controller
                return;
            }

            ClearScreen();
            _gl.DrawElements(OpenGL.GL_TRIANGLES, 3, OpenGL.GL_UNSIGNED_INT, IntPtr.Zero);
        }
        

    }
}
