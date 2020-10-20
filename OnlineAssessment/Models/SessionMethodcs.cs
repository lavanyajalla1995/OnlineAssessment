using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAssessment.Models
{
    public  static class SessionMethod
    {
        public static void Set<T>(this ISession session,string key,List<T> value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static List<T> Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<List<T>>(value);
        }
    }
}
