using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class RedisObject : IRedisObject
    {
        public DateTime LastTimeUpdated { get; set; }
        public string JsonData { get; set; }
    }
}
