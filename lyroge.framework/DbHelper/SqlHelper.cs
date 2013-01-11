using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;

namespace lyroge.framework.DbHelper
{
    public class SqlHelper : Dbhelper<SqlConnection, SqlCommand, SqlDataAdapter>
    {
        const string CONNECTIONSTRING = "server={0};database={1};uid={2};pwd={3}";

        #region 构造函数
        /// <summary>
        /// 从配置文件中读取连接字符串
        /// </summary>
        /// <param name="str"></param>
        public SqlHelper(string str, bool isKey=true)
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
        /// 直接指定主机，数据库名字，用户名，密码信息
        /// </summary>
        /// <param name="host"></param>
        /// <param name="dbname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public SqlHelper(string host, string dbname, string username, string password)            
        {
            Connectstring = string.Format(CONNECTIONSTRING, host, dbname, username, password);
        }

        /// <summary>
        /// 默认为本地主机
        /// </summary>
        /// <param name="dbname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public SqlHelper(string dbname, string username, string password)            
        {
            Connectstring = string.Format(CONNECTIONSTRING, ".", dbname, username, password);
        }


        /// <summary>
        /// 默认为本地主机且密码为空
        /// </summary>
        /// <param name="dbname"></param>
        /// <param name="username"></param>
        public SqlHelper(string dbname, string username)            
        {
            Connectstring = string.Format(CONNECTIONSTRING, ".", dbname, username);
        }
        #endregion

        /// <summary>
        /// 返回插入
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int ExecuteInsert(string commandText, params DbParameter[] dbParameters)
        {
            PrepareCommand(commandText, dbParameters);
            int obj = -1;
            try
            {
                obj = ExecuteScalar<int>(commandText + ";select @@IDENTITY");
            }
            catch
            {
                throw;
            }   
            finally
            {
                Conn.Close();
            }
            return obj;
        }

        public int ExecuteInsert(DbCommand dbCommand)
        {
            PrepareCommand(dbCommand);
            int obj = -1;
            try
            {
                obj = ExecuteScalar<int>(dbCommand.CommandText + ";select @@IDENTITY");
            }
            catch
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }            
            return obj;
        }
    }
}