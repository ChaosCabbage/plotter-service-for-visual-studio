/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.Collections;
using System.Text;
using System.Reflection;
using Microsoft.VsSDK.UnitTestLibrary;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMC.PlotterService;

namespace PlotterService_UnitTests
{
    [TestClass()]
    public class PackageTest
    {
        private void AddMockActivityLogService(OleServiceProvider provider)
        {
            BaseMock activityLogMock =
                new GenericMockFactory(
                    "MockVsActivityLog",
                    new[] { typeof(Microsoft.VisualStudio.Shell.Interop.IVsActivityLog) }
                ).GetInstance();

            provider.AddService(
                typeof(Microsoft.VisualStudio.Shell.Interop.SVsActivityLog),
                activityLogMock,
                true
            );
        }

        [TestMethod()]
        public void CreateInstance()
        {
            PlotterServicePackage package = new PlotterServicePackage();
        }

        [TestMethod()]
        public void IsIVsPackage()
        {
            PlotterServicePackage package = new PlotterServicePackage();
            Assert.IsNotNull(package as IVsPackage, "The object does not implement IVsPackage");
        }

        [TestMethod()]
        public void SetSite()
        {
            // Create the package
            IVsPackage package = new PlotterServicePackage() as IVsPackage;
            Assert.IsNotNull(package, "The object does not implement IVsPackage");

            // Create a basic service provider
            OleServiceProvider serviceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices();
            AddMockActivityLogService(serviceProvider);

            // Site the package
            Assert.AreEqual(0, package.SetSite(serviceProvider), "SetSite did not return S_OK");

            // Unsite the package
            Assert.AreEqual(0, package.SetSite(null), "SetSite(null) did not return S_OK");
        }
    }
}
