using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lyroge.framework.ObjectExtension
{
    public static class ObjectExtension
    {
        //调用示例
        //object o = null;
        //o.IfNull(() => Response.Write("here"));
        public static void IsNull(this object obj, Action action)
        {
            if (obj == null)
                action();
        }

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

        //public static TResult IfNull<TResult>(this object obj, Func<TResult> func)
        //{
        //    if (obj == null)
        //        return func();
        //}
    }
}
