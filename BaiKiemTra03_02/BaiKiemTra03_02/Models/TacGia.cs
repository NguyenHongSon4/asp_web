using System.ComponentModel.DataAnnotations;

namespace BaiKiemTra03_02.Models
{
    public class TacGia
    {
        [Key]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Tên tác giả là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên tác giả không được dài hơn 99 ký tự.")]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "Quốc tịch là bắt buộc.")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Năm sinh là bắt buộc.")]
        public int BirthYear { get; set; } 
    }

}
