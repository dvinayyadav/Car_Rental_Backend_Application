using Car_Rental_Backend_Application.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.Dto_s
{
    public class UserDto
    {
        public int User_ID;

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
        public List<int> BookingIds { get; set; } =   new List<int>();   // A user can make zero or more bookings
        public List<int> ReservationIds { get; set; } = new List<int>();
    }
}
