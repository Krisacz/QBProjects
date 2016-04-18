using System.Collections.Generic;

namespace IMRR.Lib
{
    internal class ScannedFile
    {
        public int PageCount { get; set; }
        public List<ImageOccurrence> ImageOccurrences { get; set; }
    }
}