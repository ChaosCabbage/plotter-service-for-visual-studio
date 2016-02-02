using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

namespace PMC.PlotterService
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(PlotterToolWindow))]
    [ProvideService(typeof(SPlotter2DService), ServiceName = "Plotter2DService")]
    [Guid(GuidList.guidPlotterServicePkgString)]
    public sealed class PlotterServicePackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// The package is not yet sited inside Visual Studio, so this is the place
        /// for any setup that doesn't need the VS environment.
        /// Here we register our service, so that it will be available to any other
        /// packages by the time their Initialize is called.
        /// </summary>
        public PlotterServicePackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));

            var serviceContainer = this as IServiceContainer;
            var serviceCreator = new ServiceCreatorCallback(CreatePlotterService);
            serviceContainer.AddService(typeof(SPlotter2DService), serviceCreator, true);            
        }

        /// <summary>
        /// This is called the first time any package wants our plotter service.
        /// </summary>
        private object CreatePlotterService(IServiceContainer container, Type serviceType)
        {
            if (serviceType != typeof(SPlotter2DService)) { return null; }
            return new Plotter2DService(FindPlotterToolWindow());
        }

        /// <summary>
        /// Get our one and only tool window, the plotter grid.
        /// </summary>
        /// <returns></returns>
        private PlotterToolWindow FindPlotterToolWindow()
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = this.FindToolWindow(typeof(PlotterToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }
            return window as PlotterToolWindow;
        }

        /// <summary>
        /// Show our one and only window.
        /// </summary>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            FindPlotterToolWindow().Show();
        }


        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                // Create the command for the tool window
                CommandID toolwndCommandID = new CommandID(GuidList.guidPlotterServiceCmdSet, (int)PkgCmdIDList.cmdid2DPlotter);
                MenuCommand menuToolWin = new MenuCommand(ShowToolWindow, toolwndCommandID);
                mcs.AddCommand( menuToolWin );
            }
        }

        #endregion

    }
}
