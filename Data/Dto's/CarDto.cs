using Car_Rental_Backend_Application.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.Dto_s
{
    public class CarDto
    {

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
        public ICollection<BookingDto> Bookings { get; set; } // A car can be associated with zero or more bookings
        public ICollection<ReservationDto> Reservations { get; set; } // A car can be associated with zero or more reservations
        public AvailabilityDto Availability { get; set; } // A car has availability information
    }
}
