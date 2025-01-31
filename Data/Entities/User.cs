using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.Entities
{
    public class User
    {

        [Key]
        public int User_ID { get; set; } // Primary Key

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

        // Relationships
        public ICollection<Booking> Bookings { get; set; } // A user can make zero or more bookings
        public ICollection<Reservation> Reservations { get; set; } // A user can make zero or more reservations
    }
}
