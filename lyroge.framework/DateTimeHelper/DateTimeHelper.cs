using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lyroge.framework.DateTimeHelper
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 获取时间的描述，如几小时前， 58分钟前
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string Describe(System.DateTime datetime)
        {
            var timespan = System.DateTime.Now - datetime;
            if (timespan.Days > 1)
                return datetime.ToString("yyyy-MM-dd");
            if (timespan.Hours > 0.5)
                return string.Format("{0}小时前", timespan.Hours);
            return string.Format("{0}分钟前", timespan.Minutes);
        }

        /// <summary>
        /// 日期格式化为一定的格式
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string Format(DateTime datetime, DateTimeFormat format)
        {           
            return datetime.ToString(_formatArray[(int)(format)]);
        }

        #region 日期格式字符串
        /// <summary>
        /// 日期格式字符串数组
        /// </summary>
        private static string[] _formatArray = new string[] 
        {
            "yyyy-MM-dd",
            "yyyy-MM-dd HH:mm",
            "yyyy-MM-dd HH:mm:ss",

            "yyyy年MM月dd日",
            "yyyy年MM月dd日 HH时mm分",
            "yyyy年MM月dd日 HH时mm分ss秒",

            "yyyy-M-d",
            "yyyy-M-d H:m",
            "yyyy-M-d H:m:s",

            "yyyy年M月d日",
            "yyyy年M月d日 H时m分",
            "yyyy年M月d日 H时m分s秒",
        };
        #endregion
    }

    /// <summary>
    /// 日期格式枚举
    /// </summary>
    public enum DateTimeFormat
    {
        yyyyMMdd24,
        yyyyMMdd_HH_mm24,
        yyyyMMdd_HH_mm_ss24,

        cn_yyyyMMdd24,
        cn_yyyyMMdd_HH_mm24,
        cn_yyyyMMdd_HH_mm_ss24,

        yyyyMMdd12,
        yyyyMMdd_HH_mm12,
        yyyyMMdd_HH_mm_ss12,

        cn_yyyyMMdd12,
        cn_yyyyMMdd_HH_mm12,
        cn_yyyyMMdd_HH_mm_ss12
    }  
}
