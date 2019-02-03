using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Media.Imaging;



namespace AR_Comic_Viewer.Services
{
    class ImageArchiveService : IImageArchiveService
    {
        private ZipArchive _zip;

        public void OpenArchive(string path)
        {
            _zip = ZipFile.OpenRead(path);
        }
        public List<ZipArchiveEntry> GetArchiveEntries()
        {
            List<ZipArchiveEntry> list = _zip.Entries
                .OrderBy(x => x.Name)
                .ToList();
            return list;
        }

        public void Dispose()
        {
            _zip.Dispose();
        }

        public bool isFileExtension(string fileName, string extension)
        {
            return fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
        }

        public BitmapSource ConvertEntryToBitmapSource(ZipArchiveEntry zae)
        {
            Stream stream = zae.Open();

            if (
                !(isFileExtension(zae.Name, ".jpg") ||
                isFileExtension(zae.Name, ".jpeg") ||
                isFileExtension(zae.Name, ".png") ||
                isFileExtension(zae.Name, ".gif"))
            ) return null;

            Image img = Image.FromStream(stream);
            stream = zae.Open();
            BitmapDecoder decoder = null;
            if (img.RawFormat.Equals(ImageFormat.Jpeg))
            {
                decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            }
            else if (img.RawFormat.Equals(ImageFormat.Png))
            {
                decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            }
            else if (img.RawFormat.Equals(ImageFormat.Gif))
            {
                decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            }
            else return null;

            BitmapSource bitmapSource = decoder.Frames[0];
            return bitmapSource;
        }

    }
}
