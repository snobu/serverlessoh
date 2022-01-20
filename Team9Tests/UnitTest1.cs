using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Newtonsoft.Json;
using IcecreamRatings.Models;

namespace Team9Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var json = File.ReadAllText("./../../../combined.json");
            var data = JsonConvert.DeserializeObject<CombinedJsonRequest[]>(json);
            Assert.IsNotNull(data);
        }
    }
}
