using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectA.Models
{
    public class DanhGia
    {
        public int Id { get; set; }

        [Required]
        public int SanPhamId { get; set; }

        [ForeignKey("SanPhamId")]
        public SanPham SanPham { get; set; }

        public string UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string TenNguoiDung { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "So sao phai tu 1 den 5")]
        public int SoSao { get; set; }

        [Required(ErrorMessage = "Vui long nhap noi dung danh gia")]
        [StringLength(1000, ErrorMessage = "Noi dung khong duoc vuot qua 1000 ky tu")]
        public string NoiDung { get; set; }

        // Duong dan anh minh hoa (nullable - khong bat buoc)
        public string AnhUrl { get; set; }

        public DateTime NgayDang { get; set; } = DateTime.Now;
    }
}