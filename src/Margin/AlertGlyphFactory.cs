using System.Collections.Concurrent;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace PackageSecurity.Margin
{
    public class AlertGlyphFactory : IGlyphFactory
    {
        private ConcurrentDictionary<VulnerabilityLevel, Image> _cache = new ConcurrentDictionary<VulnerabilityLevel, Image>();

        public UIElement GenerateGlyph(IWpfTextViewLine line, IGlyphTag tag)
        {
            var alertTag = tag as AlertTag;

            if (alertTag == null)
                return null;

            return GetImage(alertTag.Vulnerability);
        }

        private Image GetImage(Vulnerability vul)
        {
            if (!_cache.ContainsKey(vul.Severity))
            {
                var image = new Image
                {
                    Source = GetMoniker(vul.Severity).GetImage(),
                    Height = 16,
                    Width = 16,
                    ToolTip = $"Risk level: {vul.Severity}\n\nMore info at {vul.Info.FirstOrDefault()}"
                };

                _cache[vul.Severity] = image;
            }

            return _cache[vul.Severity];
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
