using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.RequestDto_s
{
    public class AdminRequestLoginDto
    {
        [Required]
        public string UsernameOrEmail { get; set; } 

        [Required]
        public string Password { get; set; }
    }
}
