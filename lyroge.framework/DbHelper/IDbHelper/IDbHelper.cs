using System.Data;
using System.Data.Common;

namespace lyroge.framework.DbHelper
{
    public interface IDbHelper 
    {
        /// <summary>
        /// 获取DataTable类型的查询结果
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        DataTable GetDataTable(string commandText, params DbParameter[] dbParameters);
        DataTable GetDataTable(DbCommand dbCommand);

        /// <summary>
        /// 获取DataSet类型的查询结果
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        DataSet GetDataSet(string commandText, params DbParameter[] dbParameters);
        DataSet GetDataSet(DbCommand dbCommand);

        /// <summary>
        /// 获取DataReader(记得用完要关闭Reader对象)
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        IDataReader GetDataReader(string commandText, params DbParameter[] dbParameters);
        IDataReader GetDataReader(DbCommand dbCommand);

        /// <summary>
        /// 判断是否查询到数据
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        bool IsExists(string commandText, params DbParameter[] dbParameters);
        bool IsExists(DbCommand dbCommand);

        /// <summary>
        /// 返回执行SQL语句影响的条数
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string commandText, params DbParameter[] dbParameters);
        int ExecuteNonQuery(DbCommand dbCommand);

        /// <summary>
        /// 返回一行一列的数据对象
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        T ExecuteScalar<T>(string commandText, params DbParameter[] dbParameters) where T : struct;
        T ExecuteScalar<T>(DbCommand dbCommand) where T : struct;
    }
}
