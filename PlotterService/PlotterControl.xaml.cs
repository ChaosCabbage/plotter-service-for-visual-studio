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
        Drawing.Plotter plotter = null;

        public PlotterControl()
        {
            InitializeComponent();
        }

        void plot_Loaded(object sender, RoutedEventArgs e)
        {
            if (plotter == null)
            {
                plotter = new Drawing.Plotter(this.plot);
            }
        }

        public void AddSeries(IEnumerable<Drawing.PlotterPosition> series)
        {
            plotter.AddPointSeries(series);
        }
    }
}