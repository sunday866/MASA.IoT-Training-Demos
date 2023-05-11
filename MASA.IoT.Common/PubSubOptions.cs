using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.IoT.Common
{
    public class PubSubOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public string Mode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public ulong Timestamp { get; set; }

        /// <summary>
        /// MQ数据
        /// </summary>
        public JObject MQData => !string.IsNullOrWhiteSpace(Msg) ? JObject.Parse(Msg) : new();

        /// <summary>
        /// 跟踪Id
        /// </summary>
        public Guid TrackId { get; set; }

        /// <summary>
        /// Pub时间
        /// </summary>
        public long PubTime { get; set; }
    }
}
