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
        readonly Drawing.GLGridRenderer _renderer = new Drawing.GLGridRenderer();

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
        }

        private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            if (_controls == null)
            {
                _controls = new Drawing.PlotterControls(glControl, _renderer);
            }
            _renderer.Initialize(args);
        }

    }
}