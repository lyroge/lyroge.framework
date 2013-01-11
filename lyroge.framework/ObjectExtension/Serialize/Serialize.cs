using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Script.Serialization;

namespace lyroge.framework.ObjectExtension.Serialize
{
    public static class SerializeExtension
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Serialize(this object obj)
        {
            if (obj == null)
            { 
                throw new ArgumentNullException("序列化的对象不能为空"); 
            }
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object Deserialze(this byte[] data)
        {
            return Deserialze<object>(data);
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialze<T>(this byte[] data) where T : class
        {            
            using (MemoryStream stream = new MemoryStream(data))
            {
                return new BinaryFormatter().Deserialize(stream) as T;
            }
        }

        /// <summary>
        /// 将obj对象序列化为json格式的字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer JSerializer = new JavaScriptSerializer();
            return JSerializer.Serialize(obj);
        }

        /// <summary>
        /// 将json字符串转换为类型为T的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonTo<T>(this string json)
        {
            JavaScriptSerializer JSerializer = new JavaScriptSerializer();
            return JSerializer.Deserialize<T>(json);
        }
    }
}