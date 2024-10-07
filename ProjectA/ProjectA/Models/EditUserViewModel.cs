using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProjectA.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; } // Thêm Name
        public string? Address { get; set; } // Thêm Address
        public bool IsActive { get; set; }
        public string Role { get; set; } // Thêm thuộc tính Role
    }

}
