using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Image = System.Drawing.Image;

namespace IMRR.Lib
{
    internal class Document
    {
        public static ScannedFile ReadFile(string filePath)
        {
            if (filePath.ToLower().EndsWith("pdf")) return ReadPdfFile(filePath);
            if (filePath.ToLower().EndsWith("tif")) return ReadTiffFile(filePath);
            throw new Exception("Unknown file type (supported files are PDF or TIF): " + filePath);
        }

        private static ScannedFile ReadPdfFile(string filePath)
        {
            var stream = (Stream)new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var pdf = ReadPdf(stream);
            return pdf;
        }

        private static ScannedFile ReadTiffFile(string filePath)
        {
            var stream = (Stream)new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var tif = ReadTif(stream);
            return tif;
        }

        #region HELP METHODS
        private static ScannedFile ReadTif(Stream stream)
        {
            using (var reader = new System.Drawing.Bitmap(stream))
            {
                var numberOfPages = reader.GetFrameCount(FrameDimension.Page);
                var imageOccurrences = new List<ImageOccurrence>(numberOfPages);
                for (var pageNum = 1; pageNum <= numberOfPages; pageNum++)
                {
                    reader.SelectActiveFrame(FrameDimension.Page, pageNum - 1);
                    var byteStream = new MemoryStream();
                    reader.Save(byteStream, ImageFormat.Tiff);
                    imageOccurrences.Add(new ImageOccurrence(Image.FromStream(byteStream), pageNum));
                }

                return new ScannedFile
                {
                    PageCount = numberOfPages,
                    ImageOccurrences = imageOccurrences
                };
            }
        }
        
        private static ScannedFile ReadPdf(Stream stream)
        {
            using (var reader = new PdfReader(stream))
            {
                var numberOfPages = reader.NumberOfPages;
                var imageOccurrences = new List<ImageOccurrence>(numberOfPages);
                var parser = new PdfReaderContentParser(reader);
                for (var pageNum = 1; pageNum <= numberOfPages; pageNum++)
                {
                    ImageRenderListener listener;
                    parser.ProcessContent(pageNum, (listener = new ImageRenderListener()));
                    if (listener.Images.Count <= 0) continue;
                    imageOccurrences.AddRange(listener.Images.Select(image => new ImageOccurrence(image, pageNum)));
                }
                return new ScannedFile
                {
                    PageCount = numberOfPages,
                    ImageOccurrences = imageOccurrences
                };
            }
        }

        private class ImageRenderListener : IRenderListener
        {
            readonly List<Image> _images = new List<Image>();
            public List<Image> Images
            {
                get { return _images; }
            }
            public void BeginTextBlock() { }
            public void EndTextBlock() { }
            public void RenderImage(ImageRenderInfo renderInfo)
            {
                var image = renderInfo.GetImage();
                var pdfObject = image.Get(PdfName.FILTER);

                PdfName filter;
                var array = pdfObject as PdfArray;
                if (array != null)
                    filter = (PdfName)array[0];
                else
                    filter = (PdfName)pdfObject;

                if (filter == null) return;
                var drawingImage = image.GetDrawingImage();
                Images.Add(drawingImage);
            }
            public void RenderText(TextRenderInfo renderInfo) { }
        }
        #endregion
    }
}
