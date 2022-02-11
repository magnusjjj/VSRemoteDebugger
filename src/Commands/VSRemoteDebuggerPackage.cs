﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace VSRemoteDebugger
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(VSRemoteDebugger.Windows.SettingsToolWindow), DocumentLikeTool = true)]
    public sealed class VSRemoteDebuggerPackage : AsyncPackage
    { 
        public string IP => ConfigFile.Current.Hostname;
        public string UserName => ConfigFile.Current.Username;// RemotePage.UserName;
        public string GroupName => ConfigFile.Current.GroupName; // RemotePage.GroupName;
        public string VsDbgPath => ConfigFile.Current.VsdbgLocation; //RemotePage.VsDbgPath;
        public string DotnetPath => ConfigFile.Current.DotnetLocation;// RemotePage.DotnetPath;
        public string AppFolderPath => ConfigFile.Current.OutputDirectory; // RemotePage.AppFolderPath;
        public string DebugFolderPath => AppFolderPath + "/debug";
        public string ReleaseFolderPath => AppFolderPath + "/release";
        public bool Publish => ConfigFile.Current.Publish; // LocalPage.Publish;
        public bool UseCommandLineArgs => ConfigFile.Current.UseCommandLineFromProject; // LocalPage.UseCommandLineArgs;
        public bool NoDebug => ConfigFile.Current.DontDebug; // LocalPage.NoDebug;

        /// <summary>
        /// VSRemoteDebuggerPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "27375819-9dd0-4292-a612-2300250b06b8";

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await RemoteDebugCommand.InitializeAsync(this).ConfigureAwait(false);
            await VSRemoteDebugger.Windows.SettingsToolWindowCommand.InitializeAsync(this);
        }
        #endregion
    }
}
