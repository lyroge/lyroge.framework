#region 条件编译实例
//#define DEBUG
//#if DEBUG
//            Adap.Fill(ds);
//            Conn.Close();
//#else
//            try
//            {
//                Adap.Fill(ds);
//            }
//            finally{
//                Conn.Close();
//            }
//#endif
#endregion

using System;
using System.Data;
using System.Data.Common;

namespace lyroge.framework.DbHelper
{
    /// <summary>
    /// 声明数据帮助类的基类，可以通过类型参数直接生成子类
    /// </summary>
    /// <typeparam name="TDbConnection"></typeparam>
    /// <typeparam name="TDbCommand"></typeparam>
    /// <typeparam name="TDbDataAdapter"></typeparam>
    public class Dbhelper<TDbConnection, TDbCommand, TDbDataAdapter> : IDbHelper, IDisposable 
        where TDbConnection : IDbConnection, new ()
        where TDbCommand : IDbCommand, new ()
        where TDbDataAdapter : IDbDataAdapter, new ()
    {
        /// <summary>
        /// 构造函数 创建好三个对象 DbConnection DbCommand DbDataAdapter
        /// </summary>
        public Dbhelper()
        {   
            Conn = new TDbConnection();

            //创建Command对象
            Comm = new TDbCommand() { Connection = Conn };

            Adap = new TDbDataAdapter();
            Adap.SelectCommand = Comm;
        }

        #region 属性
        public IDbConnection Conn { get; protected set; }        
        public IDbDataAdapter Adap { get; protected set; }
        public IDbCommand Comm { get; set; }
        public String Connectstring { get; set; }
        #endregion    

        #region protect method
        /// <summary>
        /// 执行SQL操作前的准备工作 
        /// </summary>
        /// <param name="commandTex"></param>
        /// <param name="dbParameters"></param>
        protected void PrepareCommand(string commandTex, params DbParameter[] dbParameters)
        {
            //检查连接字符串是否为空
            AssertConnectstring();

            //设置连接字符串
            Conn.ConnectionString = Connectstring;

            //确保连接是打开的
            ReOpen();            

            Comm.CommandText = commandTex;            
            //添加所有的参数
            foreach (DbParameter param in dbParameters)
            {
                Comm.Parameters.Add(param);
            }                                   
            Adap.SelectCommand = Comm;
        }

        protected void PrepareCommand(DbCommand dbCommand)
        {
            DbParameter[] dbParams = new DbParameter[dbCommand.Parameters.Count];
            dbCommand.Parameters.CopyTo(dbParams, 0);
            PrepareCommand(dbCommand.CommandText, dbParams);
        }
        #endregion

        #region private method
        /// <summary>
        /// 如果连接没有打开那么就打开连接
        /// </summary>
        private void ReOpen()
        {
            if (Conn.State != ConnectionState.Open)
                Conn.Open();
        }

        /// <summary>
        /// 诊断是否已经设置了连接字符串信息
        /// </summary>
        private void AssertConnectstring()
        {
            if (String.IsNullOrEmpty(Connectstring) == true)
                throw new ArgumentNullException("没有设置连接字符串信息");
        }
        #endregion

        #region 接口方法的实现
        /// <summary>
        /// 获取DataTable类型的查询结果
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string commandText, params DbParameter[] dbParameters)
        {
            return GetDataSet(commandText, dbParameters).Tables[0];
        }

        public DataTable GetDataTable(DbCommand dbCommand)
        {
            return GetDataSet(dbCommand).Tables[0];
        }

        /// <summary>
        /// 获取DataSet类型的查询结果
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string commandText, params DbParameter[] dbParameters)
        {
            PrepareCommand(commandText, dbParameters);
            DataSet ds = new DataSet();

            try
            {
                Adap.Fill(ds);                
            }
            catch
            {
                throw;
            }
            finally{
                Conn.Close();
            }
            return ds;
        }

        public DataSet GetDataSet(DbCommand dbCommand)
        {
            PrepareCommand(dbCommand);
            DataSet ds = new DataSet();
            try
            {
                Adap.Fill(ds);
            }
            catch
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        /// <summary>
        /// 获取DataReader(记得用完要关闭Reader对象)
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public IDataReader GetDataReader(string commandText, params DbParameter[] dbParameters)
        {
            PrepareCommand(commandText, dbParameters);
            return Comm.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public IDataReader GetDataReader(DbCommand dbCommand)
        {
            PrepareCommand(dbCommand);
            return Comm.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 判断是否查询到数据
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public bool IsExists(string commandText, params DbParameter[] dbParameters)
        {
            PrepareCommand(commandText, dbParameters);
            IDataReader reader = null;
            bool b = false;
            try
            {
                reader = Comm.ExecuteReader();
                b = reader.Read();
            }
            catch
            {
                throw;
            }
            finally
            {
                reader.Close();
                Conn.Close();                
            }
            return b;
        }

        public bool IsExists(DbCommand dbCommand)
        {
            PrepareCommand(dbCommand);
            IDataReader reader = null;
            bool b = false;
            try
            {
                reader = Comm.ExecuteReader();
                b = reader.Read();
            }
            catch
            {
                throw;
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
            return b;
        }

        /// <summary>
        /// 返回执行SQL语句影响的条数
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbCommand dbCommand)
        {
            PrepareCommand(dbCommand);
            var i = 0;            
            try
            {
                i = Comm.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {                
                Conn.Close();
            }            
            return i;
        }

        public int ExecuteNonQuery(string commandText, params DbParameter[] dbParameters)
        {
            PrepareCommand(commandText, dbParameters);
            var i = 0;
            try
            {
                i = Comm.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }
            return i;
        }

        /// <summary>
        /// 返回一行一列的数据对象
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string commandText, params DbParameter[] dbParameters) where T : struct
        {
            PrepareCommand(commandText, dbParameters);
            T obj = default(T);
            try
            {
                var result = Comm.ExecuteScalar();
                if (result != null)
                    obj = (T)result;
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

        public T ExecuteScalar<T>(DbCommand dbCommand) where T : struct
        {
            PrepareCommand(dbCommand);
            T obj = default(T);
            try
            {
                var result = Comm.ExecuteScalar();
                if (result != null)
                    obj = (T)result;
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

        /// <summary>
        /// 对象销毁时关闭连接
        /// </summary>
        public void Dispose()
        {
            Conn.Close();
            Conn = null;
        }
        #endregion
    }
}






