// Guids.cs
// MUST match guids.h
using System;

namespace PMC.PlotterService
{
    static class GuidList
    {
        public const string guidPlotterServicePkgString = "935cb23f-5c8a-472b-95f7-c9f31c444be3";
        public const string guidPlotterServiceCmdSetString = "533aa122-d5b8-4b5c-a69e-1ec4941d9993";
        public const string guidToolWindowPersistanceString = "d74aff5a-3305-438c-801f-521d2c8fa425";
        public const string guidPlotter2DServiceString = "AB302E73-9B83-464C-A61C-93FED933F634";

        public static readonly Guid guidPlotterServiceCmdSet = new Guid(guidPlotterServiceCmdSetString);
    };
}