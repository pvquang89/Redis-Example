using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisExample
{
    internal class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public User(string userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }

        public void SaveToRedis(IDatabase db)
        {
            db.HashSet(UserId, new HashEntry[]
            {
                new HashEntry("username", Username),
                new HashEntry("email", Email)
            });
        }
        public static User LoadFromRedis(IDatabase db, string userId)
        {
            // Lấy tất cả các trường của người dùng
            var userEntries = db.HashGetAll(userId);
            // Kiểm tra nếu không tìm thấy người dùng
            if (userEntries.Length == 0) 
                return null;

            //Lấy giá trị name, email từ Redis
            string userName = userEntries.FirstOrDefault(u => u.Name == "username").Value;
            string email = userEntries.FirstOrDefault(x => x.Name == "email").Value;

            return new User(userId,userName, email);
        }
    }
}
