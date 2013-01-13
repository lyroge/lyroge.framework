using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace lyroge.framework.Image
{
    public interface IImageHandler
    {
        //裁减
        Bitmap Crop(Point point, Size size);
        Bitmap Crop(Rectangle rectangle);

        //旋转
        void Rotate(RotateFlipType rotateFlipType);

        //等比例调整图片大小
        void Resize(int width, int height);
    }
}
