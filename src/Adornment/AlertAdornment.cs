using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;

namespace PackageSecurity
{
    internal sealed class AlertAdornment : Button
    {
        private const int _size = 14;
        private Image _image = new Image();

        internal AlertAdornment(AlertTag alertTag)
        {
            Height = _size;
            Width = _size;
            Margin = new Thickness(5, 0, 0, 0);
            Padding = new Thickness(0);
            BorderThickness = new Thickness(0);
            Background = Brushes.Transparent;
            Cursor = Cursors.Arrow;

            AddChild(_image);

            Update(alertTag);
        }

        internal void Update(AlertTag alertTag)
        {
            if (alertTag == null)
                return;

            var vul = alertTag.Vulnerability;
            Uri url;

            if (vul.Info.Any() && Uri.TryCreate(vul.Info.FirstOrDefault(), UriKind.Absolute, out url))
            {
                Click += (s, e) =>
                {
                    e.Handled = true;
                    Process.Start(vul.Info.First());
                };
            }

            _image.Source = GetMoniker(vul.Severity).GetImage(_size);
            ToolTip = $"Risk level: {vul.Severity}\n\nClick for more info";
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
