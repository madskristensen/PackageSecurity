using Microsoft.VisualStudio.Text.Tagging;

namespace PackageSecurity
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
