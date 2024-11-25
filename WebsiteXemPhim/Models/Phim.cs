namespace WebsiteXemPhim.Models
{
    public class PhimWithLoai
    {
        public int Id { get; set; }
        public string Loai { get; set; } // Bộ hoặc Lẻ
        public string TenPhim { get; set; }
        public string? Anh { get; set; }
        public string? AnhNen { get; set; }
        public string TrangThai { get; set; }
        public int? Like {  get; set; }
        public int? LuotXem { get; set; }
    }
}
