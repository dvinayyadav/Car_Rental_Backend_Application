using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.Dto_s
{
    public class AdminDto
    {
            [Required]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
        }
    }
