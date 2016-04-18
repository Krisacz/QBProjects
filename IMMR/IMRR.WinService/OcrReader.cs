using System;
using Tesseract;

namespace IMRR.Lib
{
    internal class OcrReader
    {
        private readonly ILogger _logger;

        public OcrReader(ILogger logger)
        {
            _logger = logger;
        }

        public string ProcessImage(string imageFile)
        {
            var textFound = string.Empty;

            try
            {
                _logger.AddInfo(string.Format("Running OCR on the image: {0}", imageFile));
                if(!imageFile.ToLower().EndsWith("jpg")) throw new Exception("Unknown file type (only supported file is JPG): " + imageFile);
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(imageFile))
                    {
                        using (var page = engine.Process(img))
                        {
                            textFound = page.GetText();
                        }
                    }
                }

                return textFound;
            }
            catch (Exception ex)
            {
                _logger.AddError(ex.Message);
            }

            return textFound;
        }
    }
}
