using Car_Rental_Backend_Application.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.Dto_s
{
    public class ReservationDto
    {
        public int Reservation_ID { get; set; }

        [Required]
        public int User_ID { get; set; } // Foreign Key to User

        [Required]
        public int Car_ID { get; set; } // Foreign Key to Car

        [Required]
        public DateTime Reservation_Date { get; set; }

        [Required]
        public DateTime Pickup_Date { get; set; }

        [Required]
        public DateTime Return_Date { get; set; }
    }
}
