using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.Entities
{
    public class Reservation
    {
        [Key]
        public int Reservation_ID { get; set; } // Primary Key

        [Required]
        public int User_ID { get; set; } // Foreign Key to User
        public User User { get; set; } // Navigation property

        [Required]
        public int Car_ID { get; set; } // Foreign Key to Car
        public Car Car { get; set; } // Navigation property

        [Required]
        public DateTime Reservation_Date { get; set; }

        [Required]
        public DateTime Pickup_Date { get; set; }

        [Required]
        public DateTime Return_Date { get; set; }
    }
}
