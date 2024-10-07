using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiKiemTra03_02.Models
{
    public class Sach
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Tiêu đề sách là bắt buộc.")]
      
        public string Title { get; set; }

        [Required(ErrorMessage = "Năm xuất bản là bắt buộc.")]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Tác giả là bắt buộc.")]
        public TacGia AuthorId { get; set; }
        [ForeignKey("AuthorID")]

        [Required(ErrorMessage = "Thể loại là bắt buộc.")]
        public string Genre { get; set; }

        public string? ImageUrl { get; set; }
    }   
}
