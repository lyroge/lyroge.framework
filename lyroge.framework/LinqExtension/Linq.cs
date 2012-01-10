using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace lyroge.framework.LinqExtension
{
    public static class EnumerableExtension
    {
        #region = Each =
        /// <summary>
        /// ForEach的3.5版
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static void Each<TSource>(this IEnumerable<TSource> array, Action<TSource> action)
        {
            if (array == null)
            {
                return;
            }
            IEnumerator<TSource> enumerator = array.GetEnumerator();
            while (enumerator.MoveNext())
            {
                action(enumerator.Current);
            }
        }

        /// <summary>
        /// ForEach的3.5版
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static void Each<TSource>(this IEnumerable<TSource> array, Action<TSource, int> action)
        {
            if (array == null)
            {
                return;
            }
            IEnumerator<TSource> enumerator = array.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                action(enumerator.Current, i);
                i++;
            }
        }

        /// <summary>
        /// ForEach的3.5版
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static void Each<TSource>(this IEnumerable<TSource> array, Func<TSource, bool> action)
        {
            if (array == null)
            {
                return;
            }
            IEnumerator<TSource> enumerator = array.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (action(enumerator.Current) == false)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// ForEach的3.5版
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static void Each<TSource>(this IEnumerable<TSource> array, Func<TSource, int, bool> action)
        {
            if (array == null)
            {
                return;
            }
            IEnumerator<TSource> enumerator = array.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                if (action(enumerator.Current, i) == false)
                {
                    break;
                }

                i++;
            }
        }        

        #endregion
    }
}
