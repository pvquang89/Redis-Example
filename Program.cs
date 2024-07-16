using StackExchange.Redis;
using System.Text;

namespace RedisExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Thiết lập mã hóa UTF-8 cho console để hiển thị tiếng Việt
            Console.OutputEncoding = Encoding.UTF8;

            // Kết nối đến Redis server
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase(); // Lấy đối tượng database từ kết nối

            Console.WriteLine("Đăng ký người dùng:");
            string userId = "user:1001"; 
            string username = "quangpham"; 
            string email = "quangpham@email.com"; 

            // Tạo đối tượng User mới
            User user = new User(userId, username, email);
            // Lưu thông tin người dùng vào Redis
            user.SaveToRedis(db); 
            Console.WriteLine($"Người dùng {username} đã được đăng ký.");

            // Đăng nhập người dùng
            Console.WriteLine("\nĐăng nhập:");
            // Tải thông tin người dùng từ Redis
            User loggedUser = User.LoadFromRedis(db, userId);
            // Kiểm tra nếu người dùng tồn tại
            if (loggedUser != null) 
            {
                Console.WriteLine($"Đăng nhập thành công cho {loggedUser.Username}!");

                // In ra thông tin người dùng
                Console.WriteLine($"ID: {loggedUser.UserId}");
                Console.WriteLine($"Tên: {loggedUser.Username}");
                Console.WriteLine($"Email: {loggedUser.Email}");

                // Lưu thông tin phiên làm việc
                string sessionId = "session:abc123"; 
                DateTime loginTime = DateTime.UtcNow; 
                Session session = new Session(sessionId, userId, loginTime);
                // Lưu thông tin phiên vào Redis
                session.SaveToRedis(db);
                Console.WriteLine($"\nPhiên làm việc {sessionId} đã được lưu trữ.");

                // In ra thông tin phiên
                Console.WriteLine($"Session ID: {session.SessionId}");
                Console.WriteLine($"User ID: {session.UserId}");
                Console.WriteLine($"Login Time: {session.LoginTime.ToString("yyyy-MM-dd HH:mm:ss")}");
            }
            else
            {
                Console.WriteLine("Đăng nhập thất bại."); // Nếu không tìm thấy người dùng
            }


        }
    }
}
