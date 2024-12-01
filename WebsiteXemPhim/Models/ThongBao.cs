namespace WebsiteXemPhim.Models
{
    public class ThongBao
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public bool IsRead { get; set; } = false; // Để xác định thông báo đã đọc hay chưa
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
