using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace lyroge.framework.Cryptography
{
    /// <summary>
    /// 对称加密算法 - DES加密服务
    /// 需要提供一个或者一组密钥 但是不同算法的密钥长度不同 常用算法如DES、AES、TripleDES等    
    /// DES 密钥长度为64bit不同算法的密钥长度不同
    /// </summary>
    public static class DES
    {
        /// <summary>
        /// DES 加密算法
        /// </summary>
        /// <param name="data">待加密字符串</param>
        /// <param name="encryptkey">加密密钥 （必须为64bit）</param>
        /// <returns></returns>
        public static string Encrypt(string data, string encryptkey)
        {
            byte[] byKey = CheckEncryptKey(encryptkey);            
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(data);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                data = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                ms = null;
            }
            catch (Exception e)
            {
                throw e;
            }
            return data;
        }

        /// <summary>
        /// DES 解密算法        
        /// </summary>
        /// <param name="data">待解密字符串</param>
        /// <param name="encryptkey">解密密钥 （必须为64bit）</param>
        /// <returns></returns>
        public static string Decrypt(string inputString, string decryptkey)
        {
            byte[] byKey = CheckEncryptKey(decryptkey);
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };            
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Convert.FromBase64String(inputString);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = new System.Text.UTF8Encoding();
                inputString = encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                throw e;
            }
            return inputString;
        }

        #region private method

        /// <summary>
        /// 验证密钥是否合格
        /// </summary>
        /// <param name="encryptkey"></param>
        /// <returns></returns>
        private static byte[] CheckEncryptKey(string encryptkey)
        {
            if (String.IsNullOrEmpty(encryptkey))
                throw new ArgumentNullException("没有提供64bit的加密密钥");

            byte[] bytes = Encoding.UTF8.GetBytes(encryptkey);

            if (bytes.Length < 8)
                throw new ArgumentException("提供的加密密钥少于64bit");

            //取8个字节
            var byKey = new byte[8];
            Array.Copy(bytes, byKey, 8);

            return byKey;
        }

        #endregion
    }
}
