using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using lyroge.framework.DbHelper;

namespace lyroge.framework.test.DbHelper.SqlDb
{
    [TestClass]
    public class CommandMethods
    {
        SqlDbUtil sqlutil;

        /// <summary>
        /// 可以在此处切换多种构造方式 Appsetting、 Connectstring 、Contructor
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            sqlutil = new SqlDbUtil("shorturldb");
        }

        [TestMethod]
        public void GetDataReader()
        {
            Assert.IsTrue(sqlutil.GetDataReader("select * from ShortUrlMap").Read());
        }

        [TestMethod]
        public void IsExists()
        {
            Assert.IsTrue(sqlutil.IsExists("select * from ShortUrlMap"));
        }

        [TestMethod]
        public void ExecuteNonQuery()
        {
            Assert.IsTrue(sqlutil.ExecuteNonQuery("select * from ShortUrlMap") == -1);
        }

        [TestMethod]
        public void ExecuteNonQuery_Delete()
        {
            Assert.IsTrue(sqlutil.ExecuteNonQuery("Delete from ShortUrlMap") > 0);
        }


        [TestMethod]
        public void ExecuteScalar()
        {
            Assert.IsTrue(sqlutil.ExecuteScalar("select * from ShortUrlMap") != null);
        }
    }
}
