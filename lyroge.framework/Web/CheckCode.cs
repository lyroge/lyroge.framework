using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;

namespace lyroge.framework.Web
{
    public partial class CheckCode : System.Web.UI.Page
    {
        /// <summary>
        /// 创建验证码图片
        /// </summary>
        public static void Creat()
        {
            GenerateCheckCodeImage(GenerateCheckCode());
        }

        /// <summary>
        /// 验证验证码是否匹配
        /// </summary>
        /// <param name="code">用户输入的验证码</param>
        /// <returns></returns>
        public static bool CodeIsRight(string code)
        {
            var session = HttpContext.Current.Session;
            return session["CheckCode"] == null
                   ? false : session["CheckCode"].ToString() == code.ToLower();
        }

        #region private method

        /// <summary>
        /// 生成验证码字符
        /// </summary>
        /// <returns></returns>
        private static string GenerateCheckCode(int length = 4)
        {
            int number;//数字
            char code; //字符
            var sbCheckCode = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + number % 10);
                else
                    code = (char)('A' + number % 26);
                sbCheckCode.Append(code.ToString());
            }

            var checkCode = sbCheckCode.ToString().ToLower();
            HttpContext.Current.Session["CheckCode"] = checkCode;
            return checkCode;
        }

        /// <summary>
        /// 根据验证码字符生成验证码图片
        /// </summary>
        /// <param name="checkCode">验证码</param>
        private static void GenerateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                throw new ArgumentNullException(checkCode, "验证码字符参数为空");

            Bitmap bitmap = new Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics g = Graphics.FromImage(bitmap);

            try
            {
                Random random = new Random(); //生成随机生成器
                g.Clear(Color.White);         //清空图片背景色

                //画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(bitmap.Width);
                    int x2 = random.Next(bitmap.Width);
                    int y1 = random.Next(bitmap.Height);
                    int y2 = random.Next(bitmap.Height);
                    g.DrawLine(new Pen(Color.GreenYellow), x1, y1, x2, y2);
                }

                Font font = new Font("Verdana", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, bitmap.Width, bitmap.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点
                for (int i = 0; i < 80; i++)
                {
                    int x = random.Next(bitmap.Width);
                    int y = random.Next(bitmap.Height);
                    bitmap.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Red), 0, 0, bitmap.Width - 1, bitmap.Height - 1);

                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Gif);

                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Gif";
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                bitmap.Dispose();
            }
        }

        #endregion
    }
}


