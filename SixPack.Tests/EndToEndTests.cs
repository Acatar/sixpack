using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SixPack.Tests
{
    [TestClass]
    public class EndToEndTests : _BaseTest
    {
        HashSet<string> cdnBundle = new HashSet<string> { 
            "http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js",
            "http://ajax.googleapis.com/ajax/libs/prototype/1.7.1.0/prototype.js"
        };

        [TestMethod]
        public void GoogleCdnTest()
        {
            var _bundle = Task.Run<string>(() => _sixpack.GetBundle("testbundle", cdnBundle)).Result;
            Assert.IsFalse(String.IsNullOrWhiteSpace(_bundle));

            // second time should come from cache
            //_bundle = Task.Run<string>(() => _sixpack.GetBundle("testbundle", cdnBundle)).Result;
            //Assert.IsFalse(String.IsNullOrWhiteSpace(_bundle));
        }
    }
}
