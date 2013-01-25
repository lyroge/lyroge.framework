using System;
using System.Text.RegularExpressions;

namespace lyroge.framework.Common
{
    public class Validator
    {
        #region 邮件验证类

        const string REG_IS_EMAIL = @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$";
        const string REG_CONTAIN_EMAIL = @"[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})";

        //是否为邮件格式
        public static bool IsEmail(string source)
        {
            return Regex.IsMatch(source, REG_IS_EMAIL, RegexOptions.IgnoreCase);
        }

        //文本中是否与邮件字符串
        public static bool HasEmail(string source)
        {
            return Regex.IsMatch(source, REG_CONTAIN_EMAIL, RegexOptions.IgnoreCase);
        }

        #endregion

        #region 验证网址

        const string REG_IS_URL = @"^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?$";
        const string REG_CONTAIN_URL = @"(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?";

        public static bool IsUrl(string source)
        {
            return Regex.IsMatch(source, REG_IS_URL, RegexOptions.IgnoreCase);
        }

        public static bool HasUrl(string source)
        {
            return Regex.IsMatch(source, REG_CONTAIN_URL, RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证日期  
        /// <summary>
        /// 验证日期类型
        /// </summary>
        public static bool IsDateTime(string source)
        {
            DateTime dt;
            return DateTime.TryParse(source, out dt);
        }
        #endregion

        #region 验证手机号
        public static bool IsMobile(string source)
        {
            return Regex.IsMatch(source, @"^1[35]\d{9}$", RegexOptions.IgnoreCase);
        }
        public static bool HasMobile(string source)
        {
            return Regex.IsMatch(source, @"1[35]\d{9}", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证IP
        public static bool IsIP(string source)
        {
            return Regex.IsMatch(source, @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$", RegexOptions.IgnoreCase);
        }

        public static bool HasIP(string source)
        {
            return Regex.IsMatch(source, @"(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])",RegexOptions.IgnoreCase);
        }
        #endregion

        #region 是不是中国电话，格式010-85849685
        //// <summary>
        /// 是不是中国电话，格式010-85849685
        /// </summary>
        public static bool IsTel(string source)
        {
            return Regex.IsMatch(source, @"^\d{3,4}-?\d{6,8}$", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 邮政编码 6个数字
        //// <summary>
        /// 邮政编码 6个数字
        /// </summary>
        public static bool IsPostCode(string source)
        {
            return Regex.IsMatch(source, @"^\d{6}$", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 中文
        public static bool IsChinese(string source)
        {
            return Regex.IsMatch(source, @"^[\u4e00-\u9fa5]+$", RegexOptions.IgnoreCase);
        }
        public static bool hasChinese(string source)
        {
            return Regex.IsMatch(source, @"[\u4e00-\u9fa5]+", RegexOptions.IgnoreCase);
        }
        #endregion

        /// <summary>
        /// 是否为数字、字符、下划线组合
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNormalChar(string source)
        {
            return Regex.IsMatch(source, @"[\w\d_]+", RegexOptions.IgnoreCase);
        }

    }
}
