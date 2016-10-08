using Microsoft.VisualStudio.Text.Editor;

namespace PackageSecurity.Margin
{
    public class AlertTag : IGlyphTag
    {
        public AlertTag(Vulnerability vulnerability)
        {
            Vulnerability = vulnerability;
        }

        public Vulnerability Vulnerability { get; }
    }
}
