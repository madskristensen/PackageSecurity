using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace PackageSecurity.Margin
{
    [Export(typeof(IGlyphFactoryProvider))]
    [Name("AlertGlyph")]
    [Order(Before = "VsTextMarker")]
    [ContentType("json")]
    [TagType(typeof(AlertTag))]
    public class AlertGlyphFactoryProvider : IGlyphFactoryProvider
    {
        [Import]
        public ITextDocumentFactoryService DocumentService { get; set; }

        public IGlyphFactory GetGlyphFactory(IWpfTextView view, IWpfTextViewMargin margin)
        {
            ITextDocument doc;

            if (!DocumentService.TryGetTextDocument(view.TextBuffer, out doc) || !doc.IsFileSupported())
                return null;

            return view.Properties.GetOrCreateSingletonProperty(() => new AlertGlyphFactory());
        }
    }
}
