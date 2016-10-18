using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace PackageSecurity.Margin
{
    public class AlertTag : ITag
    {
        public AlertTag(Vulnerability vulnerability)
        {
            Vulnerability = vulnerability;
        }

        public Vulnerability Vulnerability { get; }
    }
}
