using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace lyroge.framework.Cryptography
{	
	/// <summary>
	/// RSA���ܷ���
	/// </summary>
	public class RSA
	{
		/// <summary>
		/// ���ܳ׶��ļ���˽���ļ���:privateKey.ck�������ļ���:publicKey.ck
		/// </summary>
		/// <param name="Folder">������ļ�·��,��C:\windows</param>
		public static void CreateKeyFile(string Folder)
		{
			string[] key=GetKeyInfo();
			Vancl.PMethod.IOOper.FileOper.WriteFile(Folder,"privateKey.ck",key[0],true);
			Vancl.PMethod.IOOper.FileOper.WriteFile(Folder,"publicKey.ck",key[1],true);
		}

		/// <summary>
		/// �õ�˽�׺͹��׵���Ϣ��string[0]��˽�׵���Ϣ��string[1]�ǹ��׵���Ϣ
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
		/// �ù��׼���
		/// </summary>
		/// <param name="data">Ҫ���ܵ�����</param>
		/// /// <param name="publicKey">������Ϣ</param>
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
		/// ʹ��˽�׽���
		/// </summary>
		/// <param name="data">Ҫ���ܵ�����</param>
		/// <param name="privateKey">˽����Ϣ</param>
		/// <returns>���ؽ��ܺ����Ϣ</returns>
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
		/// ʹ��˽�׼���
		/// </summary>
		/// <param name="data">Ҫ���ܵ�����</param>
		/// <param name="privateKey">˽����Ϣ</param>
		/// <returns>���ؼ��ܺ����Ϣ</returns>
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
		/// ʹ�ù��׽���
		/// </summary>
		/// <param name="data">Ҫ���ܵ�����</param>
		/// <param name="publicKey">������Ϣ</param>
		/// <returns>���ؽ��ܺ����Ϣ</returns>
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
