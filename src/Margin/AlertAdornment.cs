using System.Collections.Concurrent;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;

namespace PackageSecurity.Margin
{
    internal sealed class AlertAdornment : Image
    {
        private ConcurrentDictionary<VulnerabilityLevel, ImageSource> _cache = new ConcurrentDictionary<VulnerabilityLevel, ImageSource>();
        private const int size = 14;

        internal AlertAdornment(AlertTag alertTag)
        {
            Height = size;
            Width = size;
            Margin = new Thickness(5, 0, 0, 0);
            Cursor = Cursors.Arrow;

            Update(alertTag);
        }

        internal void Update(AlertTag alertTag)
        {
            if (alertTag == null)
            {
                Visibility = Visibility.Collapsed;
                return;
            }

            var vul = alertTag.Vulnerability;

            if (!_cache.ContainsKey(vul.Severity))
                _cache[vul.Severity] = GetMoniker(vul.Severity).GetImage(size);

            Visibility = Visibility.Visible;
            Source = _cache[vul.Severity];
            ToolTip = $"Risk level: {vul.Severity}\n\nMore info at {vul.Info.FirstOrDefault()}";
        }

        private ImageMoniker GetMoniker(VulnerabilityLevel level)
        {
            switch (level)
            {
                case VulnerabilityLevel.Info:
                case VulnerabilityLevel.Low:
                    return KnownMonikers.StatusInformation;

                case VulnerabilityLevel.Medium:
                    return KnownMonikers.StatusSecurityWarning;

                case VulnerabilityLevel.High:
                    return KnownMonikers.StatusSecurityCritical;
            }

            return KnownMonikers.StatusSecurityOK;
        }
    }

}
