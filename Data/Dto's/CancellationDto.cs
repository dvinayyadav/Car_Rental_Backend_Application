using Car_Rental_Backend_Application.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.Dto_s
{
    public class CancellationDto
    {
        public int CancellationId { get; set; }
        public int BookingId { get; set; }
        public DateTime CancellationDate { get; set; }
        public string Reason { get; set; }
    }
}
