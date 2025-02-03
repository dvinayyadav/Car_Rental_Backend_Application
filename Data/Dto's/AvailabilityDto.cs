using Car_Rental_Backend_Application.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.Dto_s
{
    public class AvailabilityDto
    {
        
        public int Car_ID { get; set; } // Primary Key and Foreign Key to Car

        [Required]
        public DateTime Pickup_Date { get; set; }

        [Required]
        public DateTime Return_Date { get; set; }

        [Required]
        public Boolean Available{ get; set; }
    }
}
