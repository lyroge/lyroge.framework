using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;

namespace lyroge.framework.DbHelper
{
    public class SqlDbUtil : DBUtil
    {
        const string ConnectionString = "Server={0};Database={1};uid={2};pwd={3}";

        #region 构造函数
        /// <summary>
        /// 从配置文件中读取连接字符串
        /// </summary>
        /// <param name="connStringName"></param>
        public SqlDbUtil(string connStringName)
        {
            if (ConfigurationManager.ConnectionStrings[connStringName] != null)
                Connectstring = ConfigurationManager.ConnectionStrings[connStringName].ToString();
            else
                Connectstring = ConfigurationManager.AppSettings[connStringName];
        }

        /// <summary>
        /// 直接指定主机，数据库名字，用户名，密码信息
        /// </summary>
        /// <param name="host"></param>
        /// <param name="dbname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public SqlDbUtil(string host, string dbname, string username, string password)
        {
            Connectstring = string.Format(ConnectionString, host, dbname, username, password);
        }

        /// <summary>
        /// 默认为本地主机
        /// </summary>
        /// <param name="dbname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public SqlDbUtil(string dbname, string username, string password)
        {
            Connectstring = string.Format(ConnectionString, ".", dbname, username, password);
        }

        /// <summary>
        /// 默认为本地主机且密码为空
        /// </summary>
        /// <param name="dbname"></param>
        /// <param name="username"></param>
        public SqlDbUtil(string dbname, string username)
        {
            Connectstring = string.Format(ConnectionString, ".", dbname, username);
        }
        #endregion

        /// <summary>
        /// 执行SQL操作前的准备工作
        /// </summary>
        /// <param name="commandTex"></param>
        /// <param name="dbParameters"></param>
        protected override void PrepareCommand(string commandTex, params DbParameter[] dbParameters)
        {
            AssertSetConnectstring();

            //当创建过连接的时候不会新建一个连接了
            if (Conn == null)
            {
                Conn = new SqlConnection(Connectstring);
            }
            //确保连接是打开的
            ReOpen();

            Comm = new SqlCommand();
            Comm.CommandText = commandTex;
            Comm.Connection = Conn;

            //添加所有的参数
            foreach (DbParameter param in dbParameters)
            {
                Comm.Parameters.Add(param);
            }
            Adap = new SqlDataAdapter();
            Adap.SelectCommand = Comm;
        }

        protected override void PrepareCommand(DbCommand dbCommand)
        {
            DbParameter[] dbParams = new DbParameter[dbCommand.Parameters.Count];
            dbCommand.Parameters.CopyTo(dbParams, 0);
            PrepareCommand(dbCommand.CommandText, dbParams);
        }

        /// <summary>
        /// 返回插入
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int ExecuteInsert(string commandText, params DbParameter[] dbParameters)
        {
            PrepareCommand(commandText, dbParameters);
            object obj = null;
            try
            {
                obj = ExecuteScalar(commandText + ";select @@IDENTITY");
            }
            catch (Exception e)
            {
                throw e;
            }   
            finally
            {
                Conn.Close();
            }
            if (obj != null)
                return System.Convert.ToInt32(obj);
            return -1;
        }

        public int ExecuteInsert(DbCommand dbCommand)
        {
            PrepareCommand(dbCommand);
            object obj = null;
            try
            {
                obj = ExecuteScalar(dbCommand.CommandText + ";select @@IDENTITY");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Conn.Close();
            }
            if (obj != null)
                return System.Convert.ToInt32(obj);
            return -1;
        }
    }
}