using SharpGL;
using System;
using System.Runtime.InteropServices;

namespace PMC.PlotterService.Drawing.OpenGLHelp
{
    class BasicOperations
    {
        private readonly OpenGL _gl;

        public BasicOperations(OpenGL gl)
        {
            _gl = gl;
        }

        public uint GenBuffer()
        {
            uint[] buffers = new uint[1];
            _gl.GenBuffers(1, buffers);
            return buffers[0];
        }

        /// <summary>
        /// Put data on the currently bound buffer.
        /// </summary>
        /// <param name="bufferType">GL_ARRAY_BUFFER, GL_ELEMENT_ARRAY_BUFFER, etc.</param>
        /// <param name="drawType">GL_STATIC_DRAW, GL_STREAM_DRAW, etc.</param>
        public void SetBufferData<DataType>(DataType[] data, uint bufferType, uint drawType)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr vertexPtr = handle.AddrOfPinnedObject();
            int bufferSize = Marshal.SizeOf(typeof(DataType)) * data.Length;
            _gl.BufferData(bufferType, bufferSize, vertexPtr, drawType);
        }

        /// <summary>
        /// Create a shader and compile it. Throws an exception if the shader doesn't compile.
        /// </summary>
        /// <returns>The OpenGL handle of the new shader.</returns>
        public uint CreateAndCompileShader(uint shaderType, string shaderSource)
        {
            uint shader = _gl.CreateShader(shaderType);

            _gl.ShaderSource(shader, shaderSource);
            _gl.CompileShader(shader);

            if (!ShaderCompiledOK(shader))
            {
                throw new Exception("Shader compilation failed");
            }

            return shader;
        }

        /// <summary>
        /// Generate a new buffer, bind it, then load the data to it.
        /// </summary>
        /// <param name="bufferType">GL_ARRAY_BUFFER, GL_ELEMENT_ARRAY_BUFFER, etc.</param>
        /// <param name="drawType">GL_STATIC_DRAW, GL_STREAM_DRAW, etc.</param>
        /// <returns>The OpenGL handle of the new buffer.</returns>
        public uint BindDataToNewBuffer<DataType>(DataType[] data, uint bufferType, uint drawType)
        {
            uint buffer = GenBuffer();
            _gl.BindBuffer(bufferType, buffer);
            BindDataToNewBuffer(data, bufferType, drawType);
            return buffer;
        }

        private bool ShaderCompiledOK(uint shader)
        {
            int[] parameters = new int[] { 0 };
            _gl.GetShader(shader, OpenGL.GL_COMPILE_STATUS, parameters);
            return parameters[0] == OpenGL.GL_TRUE;
        }


    }
}
