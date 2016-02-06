using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PMC.PlotterService
{
    /// <summary>
    /// Interaction logic for PlotterControl.xaml
    /// </summary>
    public partial class PlotterControl : UserControl
    {
        Drawing.PlotterControls _controls;
        Drawing.CanvasGridRenderer _renderer;
        Drawing.GLImmediateGraphicsController _graphics;

        List<IEnumerable<Drawing.CanvasPosition>> _drawings = 
            new List<IEnumerable<Drawing.CanvasPosition>>();

        public PlotterControl()
        {
            InitializeComponent();
        }

        public void AddSeries(IEnumerable<Drawing.PlotterPosition> series)
        {
            throw new System.NotImplementedException();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            _renderer.Draw();
            foreach (var pic in _drawings)
            {
                _graphics.DrawLines(pic, 3, System.Windows.Media.Brushes.Red);
            }
        }

        class GLThingSize : Drawing.GLImmediateGraphicsController.ICanvasSize
        {
            SharpGL.WPF.OpenGLControl _control;
            public GLThingSize(SharpGL.WPF.OpenGLControl control) { _control = control; }
            public int Height() { return (int)_control.ActualHeight; }
            public int Width() { return (int)_control.ActualWidth; }
        }

        private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            args.OpenGL.ClearColor(135.0f/255, 206.0f/255, 250.0f/255, 1.0f);
            _graphics = new Drawing.GLImmediateGraphicsController(args.OpenGL, new GLThingSize(glControl));
            _renderer = new Drawing.CanvasGridRenderer(_graphics);
            _controls = new Drawing.PlotterControls(glControl, _renderer);
        }

    }
}