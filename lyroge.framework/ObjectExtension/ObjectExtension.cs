using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lyroge.framework.ObjectExtension
{
    public static partial class ObjectExtension
    {
        #region Object对象扩展

        /// <summary>
        /// 如果对象为空，那么执行一个动作
        /// 调用示例
        /// object o = null;
        /// o.IfNull(() => Response.Write("here"));
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public static void IfNull(this object obj, Action action)
        {
            if (obj == null)
                action();
        }

        public static void IfNotNull(this object obj, Action<object> action)
        {
            if (obj != null)
                action(obj);
        }

        /// <summary>
        /// 对象如果是不空则返回值有func决定
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T IfNotNull<T>(this object obj, Func<object, T> func)
        {
            return obj == null ? default(T) : func(obj);
        }
        #endregion

        #region 对布尔类型的扩展，简化if else操作
        public static void IsTrue(this bool b, Action action)
        {
            if (b == true)
                action();
        }

        public static void IsFalse(this bool b, Action action)
        {
            if (b == false)
                action();
        }

        public static void IsTrueOrFalse(this bool b, Action action0, Action action1)
        {
            if (b == true)
                action0();
            else
                action1();
        }
        #endregion

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
