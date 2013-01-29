using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using lyroge.framework.DateTimeX;

namespace lyroge.framework.Test
{
    [TestClass]
    public class DateTimeHelperTest
    {
        [TestMethod]
        public void Format()
        {
           var dt=  DateTimeHelper.Format(DateTime.Parse("2012-1-12"), DateTimeFormat.cn_yyyyMMdd24);
           Assert.AreEqual(dt, "2012年01月12日");

           dt = DateTimeHelper.Format(DateTime.Parse("2012-1-12 12:32:55"), DateTimeFormat.cn_yyyyMMdd12);
           Assert.AreEqual(dt, "2012年1月12日");

           dt = DateTimeHelper.Format(DateTime.Parse("2012-1-12"), DateTimeFormat.yyyyMMdd24);
           Assert.AreEqual(dt, "2012-01-12");

           dt = DateTimeHelper.Format(DateTime.Parse("2012-1-12"), DateTimeFormat.yyyyMMdd12);
           Assert.AreEqual(dt, "2012-1-12");
        }
    }
}
