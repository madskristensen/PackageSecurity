using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace PackageSecurity.Margin
{
    internal sealed class AlertAdornmentTagger : IntraTextAdornmentTagger<AlertTag, AlertAdornment>
    {
        internal static ITagger<IntraTextAdornmentTag> GetTagger(IWpfTextView view, Lazy<ITagAggregator<AlertTag>> colorTagger)
        {
            return view.Properties.GetOrCreateSingletonProperty(() => new AlertAdornmentTagger(view, colorTagger.Value));
        }

        private ITagAggregator<AlertTag> colorTagger;

        private AlertAdornmentTagger(IWpfTextView view, ITagAggregator<AlertTag> colorTagger)
        : base(view, view.TextBuffer)
        {
            this.colorTagger = colorTagger;
        }

        public void Dispose()
        {
            this.colorTagger.Dispose();

            base.view.Properties.RemoveProperty(typeof(AlertAdornmentTagger));
        }

        // To produce adornments that don't obscure the text, the adornment tags
        // should have zero length spans. Overriding this method allows control
        // over the tag spans.
        protected override IEnumerable<Tuple<SnapshotSpan, PositionAffinity?, AlertTag>> GetAdornmentData(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0)
                yield break;

            ITextSnapshot snapshot = spans[0].Snapshot;

            var colorTags = this.colorTagger.GetTags(spans);

            foreach (IMappingTagSpan<AlertTag> dataTagSpan in colorTags)
            {
                NormalizedSnapshotSpanCollection colorTagSpans = dataTagSpan.Span.GetSpans(snapshot);

                // Ignore data tags that are split by projection.
                // This is theoretically possible but unlikely in current scenarios.
                if (colorTagSpans.Count != 1)
                    continue;

                SnapshotSpan adornmentSpan = new SnapshotSpan(colorTagSpans[0].End, 0);

                yield return Tuple.Create(adornmentSpan, (PositionAffinity?)PositionAffinity.Successor, dataTagSpan.Tag);
            }
        }

        protected override AlertAdornment CreateAdornment(AlertTag dataTag, SnapshotSpan span)
        {
            return new AlertAdornment(dataTag);
        }

        protected override bool UpdateAdornment(AlertAdornment adornment, AlertTag dataTag)
        {
            adornment.Update(dataTag);
            return true;
        }
    }
}