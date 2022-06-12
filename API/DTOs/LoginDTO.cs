using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}