using Microsoft.VisualStudio.TestTools.UnitTesting;
using MASA.IoT.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.IoT.Core.Infrastructure.Tests
{
    [TestClass()]
    public class TimeSeriesDbClientTests
    {
        [TestMethod()]
        public void GetFluxRecordListAsyncTest()
        {
            var g = new Guid();
            int b = 0;
            string c = "";
            var sss= Type.GetTypeCode(g.GetType());
            var ss2s= Type.GetTypeCode(c.GetType());

           ;
            //var utcStartTime = TimeZoneInfo.ConvertTimeToUtc(stopDateTime, TimeZoneInfo.Local);
            Assert.IsNotNull(sss);
        }
    }
}