using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace PackageSecurity.Margin
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("json")]
    [TagType(typeof(AlertTag))]
    class AlertTaggerProvider : ITaggerProvider
    {
        [Import]
        private ITextDocumentFactoryService DocumentService { get; set; }

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            ITextDocument doc;

            if (!DocumentService.TryGetTextDocument(buffer, out doc) || !doc.IsFileSupported())
                return null;

            PackageSecurityPackage.LoadPackage();

            return new AlertTagger() as ITagger<T>;
            //return buffer.Properties.GetOrCreateSingletonProperty(() => new AlertTagger()) as ITagger<T>;
        }
    }

}
