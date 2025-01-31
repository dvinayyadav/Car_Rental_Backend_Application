using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.Entities
{
    public class Admin
    {
        [Key]
        public int Admin_ID { get; set; } // Primary Key

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        // Relationships
        public ICollection<Booking> Bookings { get; set; } // An admin manages zero or more bookings
    }
}
