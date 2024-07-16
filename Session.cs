using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisExample
{
    internal class Session
    {
        public string SessionId { get; set; } 
        public string UserId { get; set; } 
        public DateTime LoginTime { get; set; } 

      
        public Session(string sessionId, string userId, DateTime loginTime)
        {
            SessionId = sessionId; 
            UserId = userId; 
            LoginTime = loginTime; 
        }

        public void SaveToRedis(IDatabase db)
        {
            db.HashSet(SessionId, new HashEntry[] {
                new HashEntry("userId", UserId), 
                new HashEntry("loginTime", LoginTime.ToString("yyyy-MM-dd HH:mm:ss")) 
            });
        }


        public static Session LoadFromRedis(IDatabase db, string sessionId)
        {
            // Lấy tất cả các trường của phiên
            var sessionEntries = db.HashGetAll(sessionId);
            // Kiểm tra nếu không tìm thấy phiên
            if (sessionEntries.Length == 0) 
                return null; 

            // Lấy giá trị của userId và loginTime từ Redis
            string userId = sessionEntries.FirstOrDefault(x => x.Name == "userId").Value;
            DateTime loginTime = DateTime.Parse(sessionEntries.FirstOrDefault(x => x.Name == "loginTime").Value);

            return new Session(sessionId, userId, loginTime);
        }
    }
}
