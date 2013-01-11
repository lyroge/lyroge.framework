using System;
using System.Linq;
using System.Security.Cryptography;

namespace lyroge.framework.Cryptography
{
	/// <summary>
	/// Hash加密算法 - MD5加密服务
    /// fcl中封装了7种常用哈希加密算法 MD5、ShA1 ...
	/// </summary>
	public static class MD5
	{
		/// <summary>
		/// 使用MD5加密字节数组
		/// </summary>
		/// <param name="data">要加密的字节数组</param>
		/// <returns>返回加密后的字节</returns>
		public static byte[] EncryptBytes(byte[] data)
		{
			MD5CryptoServiceProvider md5=new MD5CryptoServiceProvider();
			byte[] bytes=md5.ComputeHash(data);
			return bytes;
		}

		/// <summary>
		/// 使用MD5加密字符串
		/// </summary>
		/// <param name="data">要加密的字符串，以utf8字符编码</param>
		/// <returns>返回加密后的字符</returns>
        public static string EncryptString(string data)
		{
            return EncryptString(data, System.Text.Encoding.UTF8);
		}

		/// <summary>
        /// 使用MD5加密字符串（指定编码）
		/// </summary>
		/// <param name="data">要加密的字符串</param>
		/// <param name="encode">要使用的编码</param>
		/// <returns>返回加密后的字符</returns>
        public static string EncryptString(string data, System.Text.Encoding encode)
		{
			byte[] bytes=encode.GetBytes(data);
            bytes = EncryptBytes(bytes);
            System.Text.StringBuilder sb = new System.Text.StringBuilder(bytes.Length * 2);
            bytes.ToList().ForEach(b=> sb.Append(b.ToString("X2")));
			return sb.ToString();
		}
	}

}
