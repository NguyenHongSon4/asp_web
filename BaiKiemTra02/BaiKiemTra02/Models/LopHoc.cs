using System.ComponentModel.DataAnnotations;
namespace BaiKiemTra02.Models
{
    public class LopHoc
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Không được để trống tên lớp!")]
        [StringLength(10, ErrorMessage = "{0} phải có độ dài phải từ {2} đến {1} ký tự.", MinimumLength = 1)]
        [Display(Name = "Tên lớp học")]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime DateOut { get; set; } = DateTime.Now;

        public int Soluong { get; set; }
    }
}
