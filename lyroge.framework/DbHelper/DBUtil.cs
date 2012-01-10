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
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

using lyroge.framework.DbHelper.Interfaces;

namespace lyroge.framework.DbHelper
{    
    public class DBUtil : IDbHelper, IDisposable
    {
        #region 属性
        public IDbConnection Conn { get; protected set; }        
        public IDbDataAdapter Adap { get; protected set; }
        public IDbCommand Comm { get; set; }
        public String Connectstring { get; set; }
        #endregion

        #region 内部方法
        /// <summary>
        /// 执行SQL操作前的准备工作
        /// </summary>
        /// <param name="commandTex"></param>
        /// <param name="dbParameters"></param>
        protected virtual void PrepareCommand(string commandTex, params DbParameter[] dbParameters)
        {
            AssertSetConnectstring();

            //当创建过连接的时候不会新建一个连接了
            if (Conn == null)
            {
                Conn = new OleDbConnection(Connectstring);
            }
            //确保连接是打开的
            ReOpen();

            Comm = new OleDbCommand();
            Comm.CommandText = commandTex;
            Comm.Connection = Conn;

            //添加所有的参数
            foreach (DbParameter param in dbParameters)
            {
                Comm.Parameters.Add(param);
            }            
            
            Adap = new OleDbDataAdapter();
            Adap.SelectCommand = Comm;
        }

        protected virtual void PrepareCommand(DbCommand dbCommand)
        {
            DbParameter[] dbParams = new DbParameter[dbCommand.Parameters.Count];
            dbCommand.Parameters.CopyTo(dbParams, 0);
            PrepareCommand(dbCommand.CommandText, dbParams);
        }

        /// <summary>
        /// 如果连接没有打开那么就打开连接
        /// </summary>
        protected void ReOpen()
        {
            if (Conn.State != ConnectionState.Open)
                Conn.Open();
        }

        /// <summary>
        /// 诊断是否已经设置了连接字符串信息
        /// </summary>
        protected void AssertSetConnectstring()
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
            catch (Exception e)
            {
                throw e;
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
            catch (Exception e)
            {
                throw e;
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
            catch (Exception e)
            {
                throw e;
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
            catch (Exception e)
            {
                throw e;
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
            catch (Exception e)
            {
                throw e;
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
            catch (Exception e)
            {
                throw e;
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
        public object ExecuteScalar(string commandText, params DbParameter[] dbParameters)
        {
            PrepareCommand(commandText, dbParameters);
            object obj = null;
            try
            {
                obj = Comm.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Conn.Close();
            }
            return obj;
        }

        public object ExecuteScalar(DbCommand dbCommand)
        {
            PrepareCommand(dbCommand);
            object obj = null;
            try
            {
                obj = Comm.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw e;
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






