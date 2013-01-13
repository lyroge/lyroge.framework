using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace lyroge.framework.Image
{
    public class ImageHandlerBase : IImageHandler
    {
        #region 内部成员变量
        protected System.Drawing.Image _image;
        #endregion

        #region 构造函数
        private ImageHandlerBase()
        {
        }

        public ImageHandlerBase(string imagePath)
        {
            this._image = System.Drawing.Image.FromFile(imagePath, true);
        }

        public ImageHandlerBase(System.Drawing.Image image)
        {
            this._image = image;
        }
        #endregion

        public virtual Bitmap Crop(Point point, Size size)
        {
            return Crop(new Rectangle(point, size));
        }

        public virtual Bitmap Crop(Rectangle rectangle)
        {            
            //创建画图板及设备
            Bitmap outputImage = new Bitmap(rectangle.Width, rectangle.Height);
            Graphics graphics = Graphics.FromImage(outputImage);

            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.None;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.CompositingMode = CompositingMode.SourceCopy;

            System.Drawing.Image sourceImage = this._image;
            graphics.DrawImage(sourceImage, new Rectangle(new Point(0, 0), rectangle.Size), rectangle, GraphicsUnit.Pixel);

            sourceImage.Dispose();
            sourceImage = null;
            graphics.Dispose();
            graphics = null;
            return outputImage;
        }

        public virtual void Rotate(RotateFlipType rotateFlipType)
        {

        }

        public virtual void Resize(int width, int height)
        {
        }
    }
}
