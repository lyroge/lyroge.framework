using System;
using System.Linq;
using System.Security.Cryptography;

namespace lyroge.framework.Cryptography
{
	/// <summary>
	/// Hash�����㷨 - MD5���ܷ���
    /// fcl�з�װ��7�ֳ��ù�ϣ�����㷨 MD5��ShA1 ...
	/// </summary>
	public static class MD5
	{
		/// <summary>
		/// ʹ��MD5�����ֽ�����
		/// </summary>
		/// <param name="data">Ҫ���ܵ��ֽ�����</param>
		/// <returns>���ؼ��ܺ���ֽ�</returns>
		public static byte[] EncryptBytes(byte[] data)
		{
			MD5CryptoServiceProvider md5=new MD5CryptoServiceProvider();
			byte[] bytes=md5.ComputeHash(data);
			return bytes;
		}

		/// <summary>
		/// ʹ��MD5�����ַ���
		/// </summary>
		/// <param name="data">Ҫ���ܵ��ַ�������utf8�ַ�����</param>
		/// <returns>���ؼ��ܺ���ַ�</returns>
        public static string EncryptString(string data)
		{
            return EncryptString(data, System.Text.Encoding.UTF8);
		}

		/// <summary>
        /// ʹ��MD5�����ַ�����ָ�����룩
		/// </summary>
		/// <param name="data">Ҫ���ܵ��ַ���</param>
		/// <param name="encode">Ҫʹ�õı���</param>
		/// <returns>���ؼ��ܺ���ַ�</returns>
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
