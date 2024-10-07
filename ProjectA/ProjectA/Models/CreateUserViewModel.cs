using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ProjectA.Models   
{
    public class CreateUserViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; } 
        public string? Address { get; set; } 
        public string Role { get; set; }
    }
}
