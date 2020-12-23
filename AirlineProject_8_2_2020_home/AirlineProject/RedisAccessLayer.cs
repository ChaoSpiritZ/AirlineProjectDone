using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class RedisAccessLayer
    {
        public static bool SaveWithTimeStamp(string key, string value)
        {
            bool isSuccessful = false;
            using(RedisClient redisClient = new RedisClient("localhost"))
            {
                isSuccessful = redisClient.Set(key, JsonConvert.SerializeObject(new RedisObject() { JsonData = value, LastTimeUpdated = DateTime.Now }));
            }
            return isSuccessful;
        }

        public static IRedisObject GetWithTimeStamp(string key)
        {
            using(RedisClient redisClient = new RedisClient("localhost"))
            {
                string result = redisClient.Get<string>(key);
                if (result != null)
                    return JsonConvert.DeserializeObject<RedisObject>(result);
                return null;
            }
        }
    }
}
