using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using System.ComponentModel.Design;
using Microsoft.VsSDK.IntegrationTestLibrary;
using PMC.PlotterService;
using PMC.PlotterService.Drawing;
using System.Collections.Generic;

namespace PlotterService_IntegrationTests
{
    [TestClass]
    public class ServiceTest
    {
        private delegate void ThreadInvoker();

        [TestMethod]
        [HostType("VS IDE")]
        public void TestPlottingIsAvailable()
        {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate()
            {
                var example = new List<PlotterPosition>();
                example.Add(new PlotterPosition{X=0,   Y=0});
                example.Add(new PlotterPosition{X=0,   Y=1});
                example.Add(new PlotterPosition{X=100, Y=1});
                example.Add(new PlotterPosition{X=200, Y=5});


                //Get the Service
                IPlotter2DService plottingService = VsIdeTestHostContext.ServiceProvider.GetService(typeof(IPlotter2DService)) as IPlotter2DService;
                Assert.IsNotNull(plottingService);

                plottingService.AddPointSeries(example);

            });
        }
    }
}
