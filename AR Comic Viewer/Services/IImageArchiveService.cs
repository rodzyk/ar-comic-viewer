using System.Collections.Generic;
using System.IO.Compression;
using System.Windows.Media.Imaging;

namespace AR_Comic_Viewer.Services
{
    public interface IImageArchiveService
    {
        void OpenArchive(string path);
        void Dispose();
        List<ZipArchiveEntry> GetArchiveEntries();
        BitmapSource ConvertEntryToBitmapSource(ZipArchiveEntry zipArchiveEntry);
    }
}
