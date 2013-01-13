using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace lyroge.framework.Image
{
    public static class RawFormatJudge
    {
        public static string FromFile(string filePath)
        {
            Bitmap bitMap = new Bitmap(filePath);
            return FromBitMap(bitMap);
        }

        public static string FromStream(Stream stream)
        {
            Bitmap bitMap = new Bitmap(stream);
            return FromBitMap(bitMap);
        }

        public static string FromImage(System.Drawing.Image image)
        {            
            Bitmap bitMap = new Bitmap(image);
            return FromBitMap(bitMap);
        }

        #region private method 
        private static string FromBitMap(Bitmap bitMap)
        {
            ImageFormat format = bitMap.RawFormat;
            bitMap.Dispose();

            if (bitMap.RawFormat.Equals(ImageFormat.Jpeg))
            {
                return "jpg";
            }
            else if (bitMap.RawFormat.Equals(ImageFormat.Png))
            {
                return "png";
            }
            else if (bitMap.RawFormat.Equals(ImageFormat.Gif))
            {
                return "gif";
            }
            else if (bitMap.RawFormat.Equals(ImageFormat.Bmp))
            {
                return "bmp";
            }
            else
                return bitMap.RawFormat.ToString();

        }
        #endregion
    }
}
