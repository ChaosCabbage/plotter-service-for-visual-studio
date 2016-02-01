using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PMC.PlotterService
{
    [Guid(GuidList.guidPlotter2DServiceString)]
    public interface IPlotter2DService 
    {
        void AddPointSeries(IEnumerable<Drawing.PlotterPosition> pointSeries);

        void ClearAll();
    }
}
