using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
            obj.IfNull(() => { throw new ArgumentNullException("序列化的对象不能为空"); });
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, obj);
                return stream.ToArray();
            }
                             
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
    }
}