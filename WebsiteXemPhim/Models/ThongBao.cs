namespace WebsiteXemPhim.Models
{
    public class ThongBao
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? PhimBoId { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public PhimBo? PhimBo { get; set; }
        public AppUser User { get; set; }
    }
}
