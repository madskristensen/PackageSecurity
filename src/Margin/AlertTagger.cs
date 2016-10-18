using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace PackageSecurity.Margin
{
    internal class AlertTagger : ITagger<AlertTag>
    {
        private static readonly Regex _regex = new Regex("\"(?<name>[^\" ]+)\"(\\s+)?:(\\s+)?\"(?<version>\\d.([a-z0-9-.]+))\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        IEnumerable<ITagSpan<AlertTag>> ITagger<AlertTag>.GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0 || PackageSecurityPackage.Vulnurabilities == null)
                yield break;

            var span = spans.First();

            var line = span.Snapshot.GetLineFromPosition(span.Start.Position);
            var text = line.Extent.GetText();

            var match = _regex.Match(text);

            if (match.Success)
            {
                var name = match.Groups["name"].Value;
                var version = match.Groups["version"].Value;

                var vul = PackageSecurityPackage.Vulnurabilities.CheckPackage(name, version);

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
