using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace lyroge.framework.Cryptography
{
    /// <summary>
    /// �ԳƼ����㷨 - DES���ܷ���
    /// ��Ҫ�ṩһ������һ����Կ ���ǲ�ͬ�㷨����Կ���Ȳ�ͬ �����㷨��DES��AES��TripleDES��    
    /// DES ��Կ����Ϊ64bit��ͬ�㷨����Կ���Ȳ�ͬ
    /// </summary>
    public static class DES
    {
        /// <summary>
        /// DES �����㷨
        /// </summary>
        /// <param name="data">�������ַ���</param>
        /// <param name="encryptkey">������Կ ������Ϊ64bit��</param>
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
        /// DES �����㷨        
        /// </summary>
        /// <param name="data">�������ַ���</param>
        /// <param name="encryptkey">������Կ ������Ϊ64bit��</param>
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
        /// ��֤��Կ�Ƿ�ϸ�
        /// </summary>
        /// <param name="encryptkey"></param>
        /// <returns></returns>
        private static byte[] CheckEncryptKey(string encryptkey)
        {
            if (String.IsNullOrEmpty(encryptkey))
                throw new ArgumentNullException("û���ṩ64bit�ļ�����Կ");

            byte[] bytes = Encoding.UTF8.GetBytes(encryptkey);

            if (bytes.Length < 8)
                throw new ArgumentException("�ṩ�ļ�����Կ����64bit");

            //ȡ8���ֽ�
            var byKey = new byte[8];
            Array.Copy(bytes, byKey, 8);

            return byKey;
        }

        #endregion
    }
}
