using System;
using System.Configuration;
using System.Data.OleDb;

namespace lyroge.framework.DbHelper
{
    public class OleHelpUtil : Dbhelper<OleDbConnection, OleDbCommand, OleDbDataAdapter>
    {
        const string CONNECTIONSTRING = "Provider=OraOLEDB.Oracle;data source={0};user id={1};password={2}";

        #region 构造函数
        /// <summary>
        /// 从配置文件中读取连接字符串
        /// </summary>
        /// <param name="connStringName"></param>
        public OleHelpUtil(string str, bool isKey=true) 
        {
            if (isKey)
            {
                if (ConfigurationManager.ConnectionStrings[str] != null)
                    Connectstring = ConfigurationManager.ConnectionStrings[str].ToString();
                else
                    Connectstring = ConfigurationManager.AppSettings[str];
            }
            else
            {
                Connectstring = str;
            }            
        }

        /// <summary>
        /// 直接设置连接串的相关信息
        /// </summary>
        /// <param name="host"></param>
        /// <param name="dbname"></param>
        /// <param name="password"></param>
        public OleHelpUtil(string host, string dbname, string password)
        {
            Connectstring = string.Format(CONNECTIONSTRING, host, dbname, password);
        } 
        #endregion
    }
}