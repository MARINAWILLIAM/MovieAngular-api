using System.ComponentModel.DataAnnotations;

namespace Securtiy_api.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone] 
        public string PhoneNumber { get; set; }
        [Required]
        
        public string Password { get; set; }
       
    }
}
