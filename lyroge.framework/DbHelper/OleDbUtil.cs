using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Configuration;

namespace lyroge.framework.DbHelper
{
    public class OleDbUtil : DBUtil
    {
        const string ConnectionString = "Provider=OraOLEDB.Oracle;data source={0};user id={1};password={2}";

        #region 构造函数
        /// <summary>
        /// 从配置文件中读取连接字符串
        /// </summary>
        /// <param name="connStringName"></param>
        public OleDbUtil(string connStringName)
        {
            if (String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[connStringName].ToString()) == false)
                Connectstring = ConfigurationManager.ConnectionStrings[connStringName].ToString();
            else
                Connectstring = ConfigurationManager.AppSettings[connStringName];
        }

        public OleDbUtil(string host, string dbname, string password)
        {
            Connectstring = string.Format(ConnectionString, host, dbname, password);
        }
        #endregion
    }
}