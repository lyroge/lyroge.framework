using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;

namespace lyroge.framework.ObjectExtension.StringValidate
{
    public static partial class StringValidateExtension
    {
        /// <summary>
        /// 判断是否可以在字符串中找到一个匹配正则式的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern">正则表达式</param>
        /// <returns></returns>
        public static bool IsMatchRegex(this string str, string pattern)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 判断用户名格式是否有效 [长度（4-20个；字符）及内容（只能是字母、下划线、数字）是否合法]
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>true:有效；false:无效</returns>
        public static bool IsValidUserName(this string userName)
        {
            //用户名的真实长度、中文算2个字符哦
            int len = System.Text.Encoding.Default.GetBytes(userName).Length;
            return len >= 4 && len <= 20 && userName.IsMatchRegex(@"^([A-Za-z_0-9]{0,})$");
        }

        /// <summary>
        /// 判断密码是否有效（6-16位,字母、下划线、数字）
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>true:有效；false:无效</returns>
        public static bool IsValidPassword(this string password)
        {
            return password.IsMatchRegex(@"^[A-Za-z_0-9]{6,16}$");
        }

        /// <summary>
        /// 判断是否为有效的email格式
        /// </summary>
        public static bool IsValidEmail(this string email)
        {
            return email.IsMatchRegex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))(cn|com|net|gov|mil|edu|org)(\]?)$");
        }

        /// <summary>
        /// 判断邮编有效性
        /// </summary>
        public static bool IsValidZip(this string zip)
        {
            return zip.IsMatchRegex(@"^\d{6}$");
        }

        /// <summary>
        /// 固定电话有效性
        /// </summary>
        public static bool IsValidPhone(this string phone)
        {
            return phone.IsMatchRegex(@"^\d{3,4}-\d{7,8}(-\d{1,5})?$");
        }

        /// <summary>
        /// 手机号码有效性
        /// </summary>
        public static bool IsValidMobile(this string mobile)
        {
            return mobile.IsMatchRegex(@"^1\d{10}$");
        }

        /// <summary>
        /// Url有效性
        /// </summary>
        public static bool IsValidURL(this string url)
        {
            return url.IsMatchRegex(@"^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        ///// <summary>
        ///// 是否为Int类型
        ///// </summary>
        //public static bool IsValidInt(this string val)
        //{
        //    return val.IsMatchRegex(@"^[1-9]\d*\.?[0]*$");
        //}

        //    /// <summary>
        //    /// Float有效性
        //    /// </summary>
        //    /// <param name="val">Float数据</param>
        //    /// <returns>true:有效；false:无效</returns>
        //    public static bool IsValidFloat(string val)
        //    {
        //        return RegexIsMatch(val, @"^[1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0$");
        //    }

        //    /// <summary>
        //    /// 十进制数有效性
        //    /// </summary>
        //    /// <param name="val">十进制数</param>
        //    /// <returns>true:有效；false:无效</returns>
        //    public static bool IsValidDecimal(string val)
        //    {
        //        return RegexIsMatch(val, @"^[0-9]+\.?[0-9]{0,2}$");
        //    }

        //    /// <summary>
        //    /// 时间有效性
        //    /// </summary>
        //    /// <param name="strDate">输入字符串</param>
        //    /// <returns>true:有效；false:无效</returns>
        //    public static bool IsDateTime(string strDate)
        //    {
        //        string regex = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";
        //        return RegexIsMatch(strDate, regex);
        //    }

        //    /// <summary>
        //    /// 判断给定的字符串(strNumber)是否是数值型
        //    /// </summary>
        //    /// <param name="strNumber">要确认的字符串</param>
        //    /// <returns>是则返加true 不是则返回 false</returns>
        //    public static bool IsNumber(string strNumber)
        //    {
        //        return new Regex(@"^([0-9])[0-9]*(\.\w*)?$").IsMatch(strNumber);
        //    }

        /// <summary>
        /// IP有效性
        /// </summary>
        public static bool IsValidIP(this string ip)
        {
            return ip.IsMatchRegex(@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }


        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="strInput">原始字符串</param>
        /// <returns>true:有；false:无</returns>
        public static bool IsHasCHZN(this string strInput)
        {
            return strInput.IsMatchRegex(@"[\u4e00-\u9fa5]");
        }


        ///// <summary>
        ///// 检测是否有Sql危险字符
        ///// </summary>
        ///// <param name="str">要判断字符串</param>
        ///// <returns>true:有；false:无</returns>
        //public static bool IsSafeSqlString(string str)
        //{
        //    return !RegexIsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        //}

        ///// <summary>
        ///// 检测是否有危险的可能用于链接的字符串
        ///// </summary>
        ///// <param name="str">要判断字符串</param>
        ///// <returns>true:有；false:无</returns>
        //public static bool IsSafeUserInfoString(string str)
        //{
        //    return !RegexIsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        //}

        /// <summary>
        /// 验证身份证是否合法
        /// </summary>
        public static bool IsIdCard(this string idCard)
        {
            // 清除要验证字符串中的空格
            idCard = idCard.Trim();

            // 模式字符串
            StringBuilder pattern = new StringBuilder();

            pattern.Append(@"^(11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|");
            pattern.Append(@"50|51|52|53|54|61|62|63|64|65|71|81|82|91)");
            pattern.Append(@"(\d{13}|\d{15}[\dx])$");

            // 验证
            return idCard.IsMatchRegex(pattern.ToString());
        }

        /// <summary>
        /// 检测输入的字符串是否有对数据库操作的敏感词汇            
        /// </summary>
        public static bool IsValidInput(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return true;

            // 替换单引号
            input = input.Replace("'", "''").Trim();
            // 检测攻击性危险字符串
            string testString = @"and |or |exec |insert |select |delete |update |count |chr 
                        |mid |master |truncate |char |declare ";
            string[] testArray = testString.Split('|');
            return testArray.Any(warning => input.IndexOf(warning) > -1);
        }


        /// <summary>
        /// 合法qq号码
        /// </summary>
        public static bool IsValidQQ(this string qq)
        {
            return qq.IsMatchRegex(@"^\d{4,12}$");
        }
    }
}