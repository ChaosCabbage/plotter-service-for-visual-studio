﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PMC.PlotterService
{
    /// <summary>
    /// Interaction logic for PlotterControl.xaml
    /// </summary>
    internal partial class PlotterControl : UserControl
    {
        Drawing.PlotterControls _controls;
        Drawing.CanvasGridRenderer _grid;
        Drawing.GLImmediateGraphicsController _graphics;

        Geometry.Collection _geometryItems = new Geometry.Collection();

        public PlotterControl()
        {
            InitializeComponent();
        }

        public Geometry.Collection GeometryCollection
        {
            get { return _geometryItems; }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _geometryItems = new Geometry.Collection();
        }

        private void glControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            _grid.Draw(_geometryItems);
        }

        class GLThingSize : Drawing.GLImmediateGraphicsController.ICanvasSize
        {
            SharpGL.WPF.OpenGLControl _control;
            public GLThingSize(SharpGL.WPF.OpenGLControl control) { _control = control; }
            public int Height() { return (int)_control.ActualHeight; }
            public int Width() { return (int)_control.ActualWidth; }
        }

        private void glControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            args.OpenGL.ClearColor(135.0f/255, 206.0f/255, 250.0f/255, 1.0f);
            _graphics = new Drawing.GLImmediateGraphicsController(args.OpenGL, new GLThingSize(glControl));
            _grid = new Drawing.CanvasGridRenderer(_graphics);
            _controls = new Drawing.PlotterControls(glControl, _grid);
        }

    }
}