using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace lyroge.framework.Cryptography
{	
	/// <summary>
	/// RSA加密服务
	/// </summary>
	public class RSA
	{
		/// <summary>
		/// 生密匙对文件，私匙文件名:privateKey.ck，公匙文件名:publicKey.ck
		/// </summary>
		/// <param name="Folder">保存的文件路径,如C:\windows</param>
		public static void CreateKeyFile(string Folder)
		{
			string[] key=GetKeyInfo();
			Vancl.PMethod.IOOper.FileOper.WriteFile(Folder,"privateKey.ck",key[0],true);
			Vancl.PMethod.IOOper.FileOper.WriteFile(Folder,"publicKey.ck",key[1],true);
		}

		/// <summary>
		/// 得到私匙和公匙的信息，string[0]是私匙的信息，string[1]是公匙的信息
		/// </summary>
		/// <returns></returns>
		public static string[] GetKeyInfo()
		{
			string[] key=new string[2];
			RSACryptoServiceProvider crypt=new RSACryptoServiceProvider();
			key[0]=crypt.ToXmlString(true);
			key[1]=crypt.ToXmlString(false);
			crypt.Clear();  
			return key;
		}

		/// <summary>
		/// 用公匙加密
		/// </summary>
		/// <param name="data">要加密的数据</param>
		/// /// <param name="publicKey">公匙信息</param>
		/// <returns></returns>
		public static string PublicEncrypt(string data,string publicKey)
		{
			if(data!=""&&publicKey!="")
			{
				RSACryptoServiceProvider crypt=new RSACryptoServiceProvider();
				byte[] bytes=System.Text.Encoding.UTF8.GetBytes(data);
				crypt.FromXmlString(publicKey);
				bytes = crypt.Encrypt(bytes,false);
				data=Convert.ToBase64String(bytes); 
			}
			return data;
		}

		/// <summary>
		/// 使用私匙解密
		/// </summary>
		/// <param name="data">要解密的数据</param>
		/// <param name="privateKey">私匙信息</param>
		/// <returns>返回解密后的信息</returns>
		public static string PrivateDecrypt(string data,string privateKey)
		{
			if(data!=""&&privateKey!="")
			{
				RSACryptoServiceProvider crypt=new RSACryptoServiceProvider();				
				byte [] decryptbyte;
				crypt.FromXmlString ( privateKey ) ;
				decryptbyte = crypt.Decrypt(Convert.FromBase64String(data), false);
				data=System.Text.Encoding.UTF8.GetString( decryptbyte );
			}
			return data;
		}
	
		/// <summary>
		/// 使用私匙加密
		/// </summary>
		/// <param name="data">要加密的数据</param>
		/// <param name="privateKey">私匙信息</param>
		/// <returns>返回加密后的信息</returns>
		public static string PrivateEncrypt(string data,string privateKey)
		{
			if(data!=""&&privateKey!="")
			{
				RSACryptoServiceProvider crypt=new RSACryptoServiceProvider();
				byte [] bytes=System.Text.Encoding.UTF8.GetBytes(data);
				crypt.FromXmlString ( privateKey ) ;
				bytes=crypt.Encrypt(bytes,false);
				data=System.Text.Encoding.UTF8.GetString(bytes);
			}
			return data;
		}

		/// <summary>
		/// 使用公匙解密
		/// </summary>
		/// <param name="data">要解密的数据</param>
		/// <param name="publicKey">公匙信息</param>
		/// <returns>返回解密后的信息</returns>
		public static string PublicDecrypt(string data,string publicKey)
		{
			if(data!=""&&publicKey!="")
			{
				RSACryptoServiceProvider crypt=new RSACryptoServiceProvider();				
				byte [] decryptbyte;
				crypt.FromXmlString ( publicKey ) ;
				decryptbyte = crypt.Decrypt(System.Text.Encoding.UTF8.GetBytes(data), false);
				data=System.Text.Encoding.UTF8.GetString( decryptbyte );
			}
			return data;
		}
	}
}
