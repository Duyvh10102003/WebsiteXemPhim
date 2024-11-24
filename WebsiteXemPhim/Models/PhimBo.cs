using System.ComponentModel.DataAnnotations;

namespace WebsiteXemPhim.Models
{
    public class PhimBo
    {
        public int PhimBoId { get; set; }
        [StringLength(150)]
        public string TenPhim {  get; set; }
        [StringLength(1500)]
        public string NoiDung { get; set; }
        [StringLength(200)]
        public string? Anh { get; set; }
        [StringLength(200)]
        public string? AnhNen {  get; set; }
        public int LuotXem { get; set; }
        public int Like {  get; set; }
        public int? NamID { get; set; }
        public int? QuocGiaId { get; set; }
        public int? TrangThaiId { get; set; }
       
        public Nam? Nam { get; set; }
        public QuocGia? QuocGia { set; get; }
        public TrangThai? TrangThai { get; set; }
        public List<HopPhim>? Hops { get; set; }
        public List<LichSuXem>? lichSuXems { get; set; }
        public List<BinhLuan>? binhLuans { get; set; }
        public List<DanhGia>? danhGias { get; set; }
      
        public List<TapPhim>? TapPhims { get; set; }
        public List<ChiTietTheLoaiPhimBo>? ChiTietTheLoaiPhimBos { get; set; }
    }
}