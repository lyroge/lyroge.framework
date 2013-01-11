using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lyroge.framework.ObjectExtension
{
    public static partial class ObjectExtension
    {
        #region 字符串类型对象扩展
        /// <summary>
        /// 如果字符串为空字符串或者null 那么返回数值0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EmptyOrNull2Zero(this string str)
        {
            return str.EmptyOrNull2String("0");
        }

        /// <summary>
        /// 如果字符串是空字符或者null， 那么返回val所指的值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string EmptyOrNull2String(this string str, string val)
        {
            return string.IsNullOrEmpty(str) ? val : str;
        }
        #endregion
    }
}
