using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.Entities
{
    public class Availability
    {
        [Key]
        public int Car_ID { get; set; } // Primary Key and Foreign Key to Car
        public Car Car { get; set; } // Navigation property

        [Required]
        public DateTime Pickup_Date { get; set; }

        [Required]
        public DateTime Return_Date { get; set; }

        [Required]
        public int Available_Quantity { get; set; }
    }
}
