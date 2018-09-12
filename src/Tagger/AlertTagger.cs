using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace PackageSecurity
{
    internal class AlertTagger : ITagger<AlertTag>
    {
        private static readonly Regex _regex = new Regex("\"(?<name>[^\" ]+)\"(\\s+)?:(\\s+)?\"(?<version>\\d.([a-z0-9-.]+))\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        IEnumerable<ITagSpan<AlertTag>> ITagger<AlertTag>.GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0 || PackageSecurityPackage.Vulnurabilities == null)
                yield break;

            SnapshotSpan span = spans.First();

            ITextSnapshotLine line = span.Snapshot.GetLineFromPosition(span.Start.Position);
            string text = line.Extent.GetText();

            Match match = _regex.Match(text);

            if (match.Success)
            {
                string name = match.Groups["name"].Value;
                string version = match.Groups["version"].Value;

                Vulnerability vul = PackageSecurityPackage.Vulnurabilities.CheckPackage(name, version);

                if (vul.Severity != VulnerabilityLevel.None)
                    yield return new TagSpan<AlertTag>(line.Extent, new AlertTag(vul));
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }
    }
}
