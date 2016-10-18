﻿using System;
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
        private IBufferTagAggregatorFactoryService BufferTagAggregatorFactoryService { get; set; }

        [Import]
        private ITextDocumentFactoryService DocumentService { get; set; }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            ITextDocument doc;

            if (!DocumentService.TryGetTextDocument(buffer, out doc) || !doc.IsFileSupported())
                return null;

            return AlertAdornmentTagger.GetTagger(
            (IWpfTextView)textView,
            new Lazy<ITagAggregator<AlertTag>>(
            () => BufferTagAggregatorFactoryService.CreateTagAggregator<AlertTag>(textView.TextBuffer)))
            as ITagger<T>;
        }
    }
}