using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectA.Models
{
    public class SanPhamYeuThich
    {
        [Key]
        public int Id { get; set; }

        // Khóa ngoại liên kết đến sản phẩm yêu thích
        public int SanPhamId { get; set; }

        [ForeignKey("SanPhamId")]
        [ValidateNever]
        public SanPham SanPham { get; set; }

        // Khóa ngoại liên kết đến người dùng
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        // Thời điểm thêm sản phẩm vào danh sách yêu thích (tuỳ chọn, nếu muốn lưu)
        public DateTime AddedDate { get; set; } = DateTime.Now;
    }
}
