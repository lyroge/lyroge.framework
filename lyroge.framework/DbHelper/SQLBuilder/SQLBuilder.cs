using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using lyroge.framework.LinqExtension;
using lyroge.framework.DbHelper;

namespace lyroge.framework.DbHelper
{
    #region - SQLBuilder -
    public class SQLBuilder
    {
        public string HashKey { get; set; }
        public SQLBuilder(string name, StringBuilder sql) : this(name, sql, DateTime.MinValue) { }
        public SQLBuilder(string name, StringBuilder sql, DateTime expire)
        {
            this.Name = name;
            this.SQL = sql;
            this.Parms = new List<SqlParameter>();
            this.ExpireTime = expire;
        }

        #region 保留
        /// <summary>
        /// 直接将结果保存
        /// </summary>
        /// <param name="name"></param>
        /// <param name="table"></param>
        //public SQLBuilder(string name, DataTable table)
        //{
        //    this.Name = name;
        //    this.Table = table;
        //    this.ExpireTime = DateTime.MinValue;
        //}
        #endregion

        public string Name { get; private set; }
        public StringBuilder SQL { get; private set; }
        public List<SqlParameter> Parms { get; private set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; private set; }
        public DataTable Table { get; private set; }
        /// <summary>
        /// 刷新缓存
        /// </summary>
        public bool Refresh { get; set; }

        public SQLBuilder AddParm(SqlParameter parm)
        {
            this.Parms.Add(parm);

            return this;
        }
    } 
    #endregion

    public class SQLBuilderCollection : List<SQLBuilder>
    {
        private string _connectionkey = string.Empty;

        public SQLBuilderCollection() { }
        public SQLBuilderCollection(string connectionkey)
        {
            _connectionkey = connectionkey;
        }

        public SQLBuilderCollection AddIt(SQLBuilder builder)
        {
            if (builder != null)
            {
                if (this.Select(x => x.Name).Contains(builder.Name) == true)
                {
                    throw new ArgumentException("名称不能相同 " + builder.Name);
                }

                this.Add(builder);
            }
            return this;
        }

        //#region - FromCache -
        ///// <summary>
        ///// 从缓存中拿数据
        ///// </summary>
        ///// <returns></returns>
        //private Dictionary<string, DataTable> FromCache()
        //{
        //    if (IsProductEnv() == false)
        //        return new Dictionary<string, DataTable>();

        //    return this.Where(builder =>
        //            builder.Table == null && //没有直接指定
        //            builder.ExpireTime > DateTime.Now && //还没有过缓存时间
        //            builder.Refresh == false) //如果强行刷缓存，则不从redis中取数据
        //            .Select(builder =>
        //                new
        //                {
        //                    Keys = BuildCacheKey(builder),
        //                    Index = (builder.HashKey ?? BuildCacheKey(builder)).RedisIndex(),
        //                    HashKey = builder.HashKey
        //                })
        //            .GroupBy(p => p.Index)
        //            .SelectMany(x =>
        //            {
        //                var hashkey = x.First().HashKey;
        //                var c = CachedHelper.GetAll<DataTable>(x.Select(y => y.Keys), hashkey);
        //                if (c == null)
        //                {
        //                    return new List<DataTable>();
        //                }
        //                return c.Values;
        //            })
        //            .ToDictionary(x => x.TableName, x => x);
        //} 
        //#endregion

        public DataSet Query()
        {
            return QueryDataBase(this);

            #region 保留
            //List<SQLBuilder> query = new List<SQLBuilder>();
            //List<DataTable> cacheTable = new List<DataTable>();
            ////svar redis = FromCache();

            ////开始分配
            //foreach (var builder in this)
            //{
            //    //如果已经给了结果直接使用
            //    if (builder.Table != null)
            //    {
            //        if (string.IsNullOrEmpty(builder.Table.TableName) == true)
            //            throw new NoNullAllowedException(builder.Name);

            //        cacheTable.Add(builder.Table);
            //        continue;
            //    }

            //    DataTable table = null;
            //    //先看redis有没有
            //    //if (redis != null && redis.Count() > 0)
            //    //{
            //    //    var cachekey = builder.Name;
            //    //    redis.TryGetValue(cachekey, out table);
            //    //}

            //    //没有加入查询列表中
            //    if (table == null)
            //        query.Add(builder);
            //    //else //有放入缓存列表中
            //    //    cacheTable.Add(table);
                

            //}

            ////查询数据库
            //var ds = QueryDataBase(query);

            ////添加缓存
            ////AddCache(cacheTable.Select(p => p.TableName), ds);
            
            ////缓存数据添加DataSet中
            //cacheTable.ForEach(t => ds.Tables.Add(t));

            //return ds;
            #endregion
        }

        #region - QueryDataBase -
        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private DataSet QueryDataBase(List<SQLBuilder> query)
        {
            DataSet ds = new DataSet() { CaseSensitive = false };
            if (query.Count > 0)
            {
                SqlCommand command = new SqlCommand(
                    //拼接SQL
                    query.Select(builder => builder.SQL)
                        .Aggregate((a, b) => a.Append(b).AppendLine()).ToString()
                        );

                //添加参数
                query.SelectMany(builder => builder.Parms)
                    .Distinct(new SqlParameterComparer())
                    .Each(p => command.Parameters.Add(p));

                //查询SQL名称列表
                var names = query.Select(builder => builder.Name).ToArray();
                var conn = new DbHelper.SqlDbUtil("");

                if (string.IsNullOrEmpty(_connectionkey) == true)
                {
                    throw new ArgumentNullException("_connectionkey 不能为空");
                }

                if (string.IsNullOrEmpty(_connectionkey) == false)
                {
                    conn = new DbHelper.SqlDbUtil(_connectionkey);
                }

                try
                {
                    using (var reader = conn.GetDataReader(command))
                    {
                        ds.Load(reader, LoadOption.PreserveChanges, names);
                    }
                }
                finally
                {
                    conn.Dispose();
                }
            }

            return ds;
        } 
        #endregion

        //#region - AddCache -
        ///// <summary>
        ///// 加入缓存中
        ///// </summary>
        ///// <param name="cacheTable"></param>
        ///// <param name="ds"></param>
        //private void AddCache(IEnumerable<string> cachetablenames, DataSet ds)
        //{
        //    foreach (var item in this)
        //    {
        //        if (
        //            item.ExpireTime > DateTime.Now && //希望缓存的时间大于当前时间
        //            ds.Tables[item.Name] != null && //查询列表中有
        //            cachetablenames.Contains(item.Name) == false //但缓存列表中没有
        //            )
        //        {
        //            var cachekey = BuildCacheKey(item);
        //            CachedHelper.Add(cachekey, ds.Tables[item.Name], item.ExpireTime, item.HashKey);
        //        }
        //    }
        //} 
        //#endregion

        //#region - BuildCacheKey -
        ///// <summary>
        ///// 构建缓存关键字
        ///// </summary>
        ///// <param name="builder"></param>
        ///// <returns></returns>
        //private static string BuildCacheKey(SQLBuilder builder)
        //{
        //    if (builder.Parms == null || builder.Parms.Count() == 0)
        //    {
        //        return "SQLBuilder_" + builder.Name;
        //    }
        //    else
        //    {
        //        return "SQLBuilder_" + builder.Name + "_" + builder.Parms
        //                    .Select(p => string.Format("{0}_{1}", p.ParameterName, p.Value))
        //                    .Aggregate((a, b) => a + "_" + b);
        //    }
        //} 
        //#endregion

        //#region - IsProductEnv -
        ///// <summary>
        ///// 是否是生产环境
        ///// </summary>
        ///// <returns></returns>
        //private bool IsProductEnv()
        //{
        //    return HttpContext.Current.Request.Url.Host.StartsWith("demo") == false
        //        && HttpContext.Current.Request.Url.Host.StartsWith("my") == false;
        //} 
        //#endregion
    }

    #region - SqlParameterComparer -
    /// <summary>
    /// SqlParameterComparer
    /// </summary>
    class SqlParameterComparer : IEqualityComparer<SqlParameter>
    {
        #region IEqualityComparer<SqlParameter> Members

        public bool Equals(SqlParameter x, SqlParameter y)
        {
            return x.ParameterName == y.ParameterName;
        }

        public int GetHashCode(SqlParameter obj)
        {
            return obj.ParameterName.GetHashCode();
        }

        #endregion
    } 
    #endregion

    #region - DataTableCollectionExtensions -
    public static class DataTableCollectionExtensions
    {
        public static DataTable Get(this DataTableCollection tables, string tablename)
        {
            if (tables[tablename] != null)
            {
                return tables[tablename];
            }
            return new DataTable();
        }
    } 
    #endregion
}
