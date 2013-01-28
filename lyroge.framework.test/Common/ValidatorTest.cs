using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using lyroge.framework.Common;

namespace lyroge.framework.Test.Common
{
    [TestClass]
    public class ValidatorTest
    {
        [TestMethod]
        public void ValidateEmail()
        {
            Assert.IsTrue(Validator.IsEmail("lyroge@foxmail.com"));
            Assert.IsFalse(Validator.IsEmail("lyroge#foxmail.c"));
            Assert.IsFalse(Validator.IsEmail("lyroge@foxmailc"));
        }
    }
}
