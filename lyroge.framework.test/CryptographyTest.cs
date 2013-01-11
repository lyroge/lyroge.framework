using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using lyroge.framework.Cryptography;

namespace lyroge.framework.Test
{
    /// <summary>
    /// Cryptography 的摘要说明
    /// </summary>
    [TestClass]
    public class CryptographyTest
    {
        #region MD5 Test
        [TestMethod]
        public void MD5_Test()
        {
            var data = "songzhengang";
            Assert.AreEqual(MD5.EncryptString(data), MD5.EncryptString(data, System.Text.Encoding.UTF8));
        }
        #endregion

        #region DES Test
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DES_NullKey_Test()
        {
            DES.Encrypt("abc", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DES_Not64bitKey_Test()
        {
            DES.Encrypt("abc", "1234567");
        }
        #endregion
    }
}
