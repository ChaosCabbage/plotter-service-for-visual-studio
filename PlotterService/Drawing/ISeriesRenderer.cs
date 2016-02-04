using System.Collections.Generic;

namespace PMC.PlotterService.Drawing
{
    interface ISeriesRenderer
    {
        void Draw(IEnumerable<PlotterPosition> seriesEnumerable);
    }
}