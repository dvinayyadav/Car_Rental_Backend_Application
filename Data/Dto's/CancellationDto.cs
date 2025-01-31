using Car_Rental_Backend_Application.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.Dto_s
{
    public class CancellationDto
    {
        [Required]
        public int Booking_ID { get; set; } 

        public string Reason { get; set; }
    }
}
