using System;

namespace IMRR.Lib
{
    internal class ImageExtractor
    {
        private readonly ILogger _logger;

        public ImageExtractor(ILogger logger)
        {
            _logger = logger;
        }

        public ScannedFile ExtractImages(string scannedFile)
        {
            try
            {
                _logger.AddInfo(string.Format("Extracting images from file: {0}", scannedFile));
                var extractImages = Document.ReadFile(scannedFile);
                if (extractImages.ImageOccurrences.Count < 0) throw new Exception(string.Format("No images found in the file: {0}", scannedFile));
                return extractImages;
            }
            catch (Exception ex)
            {
                _logger.AddError(ex.Message);
            }

            return null;
        }
    }
}
