using System.Drawing;

namespace IMRR.Lib
{
    internal class ImageOccurrence
    {
        public ImageOccurrence(Image image, int pageNum)
        {
            PageNum = pageNum;
            Image = image;
        }

        public Image Image { get; private set; }
        public int PageNum { get; private set; }
    }
}