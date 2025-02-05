using System.ComponentModel.DataAnnotations;
using Car_Rental_Backend_Application.Data.ENUMS;

namespace Car_Rental_Backend_Application.Data.Entities
{
    public class Car
    {
        [Key]
        public int Car_ID { get; set; } // Primary Key

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string License_Plate { get; set; }

        [Required]
        public string Availability_Status { get; set; } // e.g., Available, Booked

        // Relationships
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public Availability Availability { get; internal set; }
        //public Availability Availability { get; set; } // A car has availability information

    }
}
