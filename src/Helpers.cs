using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;

namespace PackageSecurity
{
    static class Helpers
    {
        public static bool IsFileSupported(this ITextDocument document)
        {
            try
            {
                string file = Path.GetFileName(document.FilePath).ToLowerInvariant();

                if (file == "package.json" || file == "bower.json")
                    return true;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }

            return false;
        }

        public static ImageSource GetImage(this ImageMoniker imageMoniker, int size = 16)
        {
            var vsIconService = ServiceProvider.GlobalProvider.GetService(typeof(SVsImageService)) as IVsImageService2;

            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.Flags = (uint)_ImageAttributesFlags.IAF_RequiredFlags;
            imageAttributes.ImageType = (uint)_UIImageType.IT_Bitmap;
            imageAttributes.Format = (uint)_UIDataFormat.DF_WPF;
            imageAttributes.LogicalHeight = size;//IconHeight,
            imageAttributes.LogicalWidth = size;//IconWidth,
            imageAttributes.StructSize = Marshal.SizeOf(typeof(ImageAttributes));

            IVsUIObject result = vsIconService.GetImage(imageMoniker, imageAttributes);

            object data;
            result.get_Data(out data);
            ImageSource glyph = data as ImageSource;
            glyph.Freeze();

            return glyph;
        }
    }
}
