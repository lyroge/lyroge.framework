using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SQLite;

namespace lyroge.framework
{
    public class SQLLiteUtil : IDBUtil
    {
        const string ConnectionString = @"Data Source={0}";

        public SQLLiteUtil(string dbpath)
        {
            Connectstring = string.Format(ConnectionString, dbpath);
        }

        public SQLLiteUtil(string dbpath, string password)
        {
            string connectString = ConnectionString + ";Password={1}";
            Connectstring = string.Format(connectString, dbpath, password);
        }


        /// <summary>
        /// 执行SQL操作前的准备工作
        /// </summary>
        /// <param name="commandTex"></param>
        /// <param name="dbParameters"></param>
        protected override void PrepareCommand(string commandTex, params DbParameter[] dbParameters)
        {
            //当创建过连接的时候不会新建一个连接了
            if (Conn == null)
            {
                Conn = new SQLiteConnection(Connectstring);
            }
            //确保连接是打开的
            ReOpen();

            Comm = new SQLiteCommand();
            Comm.CommandText = commandTex;
            Comm.Connection = Conn;

            //添加所有的参数
            foreach (DbParameter param in dbParameters)
            {
                Comm.Parameters.Add(param);
            }
            Adap = new SQLiteDataAdapter();
            Adap.SelectCommand = Comm;
        }
    }

}