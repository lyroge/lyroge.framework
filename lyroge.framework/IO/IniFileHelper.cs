using System;
using System.Runtime.InteropServices;
using System.Text;

namespace lyroge.framework.IO
{
    class IniFileHelper
    {
		private string _filePath;

        #region 构造函数
        private IniFileHelper() { }

		public IniFileHelper(string iniFilePath)
		{
			_filePath = iniFilePath;
		}
        #endregion

        #region DllImport
        [DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section,string key,string val,string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section,string key,string def, StringBuilder retVal,int size,string filePath);
        	
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);
        #endregion

        /// <summary>
        /// 写入配置信息
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this._filePath);
        }

        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, this._filePath);
            return temp.ToString();
        }

		public byte[] ReadValues(string section, string key)
		{
			byte[] temp = new byte[255];
			int i = GetPrivateProfileString(section, key, "", temp, 255, this._filePath);
			return temp;
		}

		/// <summary>
		/// 删除ini文件下所有段落
		/// </summary>
		public void ClearAllSection()
		{
			WriteValue(null,null,null);
		}

		/// <summary>
		/// 删除ini文件下personal段落下的所有键
		/// </summary>
		/// <param name="section"></param>
		public void ClearSection(string section)
		{
			WriteValue(section,null,null);
		}
	}
}

