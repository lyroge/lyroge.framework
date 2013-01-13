using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lyroge.framework.Image
{
    public class GifImageHandler : ImageHandlerBase
    {
        public GifImageHandler(string imagePath) : base(imagePath)
        {
        }

        public GifImageHandler(System.Drawing.Image image) : base(image)
        {
        }

        public override System.Drawing.Bitmap Crop(System.Drawing.Rectangle rectangle)
        {
            return base.Crop(rectangle);
        }
    }
}
