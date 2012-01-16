using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace lyroge.framework.HttpWeb.Extensions.ValidateCode
{
    public static class PageExtension
    {   
        /// <summary>
        /// 产生一个验证码页面
        /// </summary>
        public static void CreateValidateCode(System.Web.UI.Page page)
        {
            Random ran = new Random();
            int randomnum = ran.Next(10001, 99999);            
            
            string fontname = "Arial";            
            int fontsize = 9;
            Font forFont = new Font(fontname, fontsize, FontStyle.Bold);

            int picturewidth = 40;            
            int pictureheight = 20;
            //生成图片
            Bitmap newBitmap = new Bitmap(picturewidth, pictureheight, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(newBitmap);

            Color bgColor = ColorTranslator.FromHtml("#" + page.Request.QueryString["colorb"]);            
            Color foreColor = ColorTranslator.FromHtml("#" + page.Request.QueryString["colorf"]);                   
            
            Rectangle newRect = new Rectangle(0, 0, picturewidth, pictureheight);
            //填充背景色
            g.FillRectangle(new SolidBrush(bgColor), newRect);
            //写字
            g.DrawString(randomnum.ToString(), forFont, new SolidBrush(foreColor), 2, 2);

            MemoryStream mStream = new MemoryStream();            
            newBitmap.Save(mStream, ImageFormat.Gif);
            g.Dispose();
            newBitmap.Dispose();
            
            page.Response.ClearContent();
            page.Response.ContentType = "image/GIF";
            page.Response.BinaryWrite(mStream.ToArray());
            page.Response.End();            

        }
    }
}
