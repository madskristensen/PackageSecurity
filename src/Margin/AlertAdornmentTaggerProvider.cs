using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace PackageSecurity.Margin
{
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("json")]
    [TagType(typeof(IntraTextAdornmentTag))]
    internal sealed class ColorAdornmentTaggerProvider : IViewTaggerProvider
    {
        [Import]
        private IBufferTagAggregatorFactoryService TagAggregatorService { get; set; }

        [Import]
        private ITextDocumentFactoryService DocumentService { get; set; }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            ITextDocument doc;

            if (!DocumentService.TryGetTextDocument(buffer, out doc) || !doc.IsFileSupported())
                return null;

            var lazy = new Lazy<ITagAggregator<AlertTag>>(() => TagAggregatorService.CreateTagAggregator<AlertTag>(textView.TextBuffer));

            return AlertAdornmentTagger.GetTagger((IWpfTextView)textView, lazy) as ITagger<T>;
        }
    }
}