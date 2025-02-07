using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_Backend_Application.Data.Entities
{
    public class User
    {

        [Key]
        public int User_ID { get; set; } 

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Address { get; set; }

        [Phone]
        public string Phone_Number { get; set; }


        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
