using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using task = System.Threading.Tasks.Task;

namespace PackageSecurity
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400)]
    [Guid(PackageGuidString)]

    public sealed class PackageSecurityPackage : AsyncPackage
    {
        public const string PackageGuidString = "abd5079c-ac41-4837-ace6-03b8c8070298";
        public const string UIContextGuid = "d39fc56c-a6a8-4491-b246-066e671b054f";

        public static Vulnerabilities Vulnurabilities
        {
            get;
            private set;
        }

        protected override async task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            Vulnurabilities = await Vulnerabilities.LoadFromExtensionPath();
        }

        public static void LoadPackage()
        {
            var shell = (IVsShell)GetGlobalService(typeof(SVsShell));

            IVsPackage package;
            var guid = new Guid(PackageGuidString);

            if (shell.IsPackageLoaded(ref guid, out package) != VSConstants.S_OK)
                ErrorHandler.Succeeded(shell.LoadPackage(ref guid, out package));
        }
    }
}
