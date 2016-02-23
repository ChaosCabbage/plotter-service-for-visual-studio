using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PMC.PlotterService
{

    [Guid(GuidList.guidPlotter2DServiceTypeString)]
    public interface SPlotter2DService { }


    [Guid(GuidList.guidPlotter2DServiceString)]
    public interface IPlotter2DService 
    {
        void AddPolyline(Geometry.Polyline polyline);
        void AddPoint(Geometry.Position point);
    }
}
