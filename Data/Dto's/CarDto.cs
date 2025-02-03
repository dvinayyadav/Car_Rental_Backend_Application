using Car_Rental_Backend_Application.Data.Dto_s;
using System.ComponentModel.DataAnnotations;

public class CarDto
{
    public int Car_ID { get; set; }

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

    // ✅ Make these optional by using `?` and initializing empty collections
    public ICollection<int>? BookingIds { get; set; } = new List<int>();
    public ICollection<int>? ReservationIds { get; set; } = new List<int>();
    //public AvailabilityDto? Availability { get; set; }
}
