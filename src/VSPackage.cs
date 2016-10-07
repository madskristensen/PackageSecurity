using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace PackageSecurity
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400)]
    [Guid(PackageGuidString)]
    public sealed class PackageSecurityPackage : Package
    {
        public const string PackageGuidString = "abd5079c-ac41-4837-ace6-03b8c8070298";

        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}
