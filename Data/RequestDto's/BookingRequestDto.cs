using System;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.RequestDto_s
{
    public class BookingRequestDto
    {
        [Required]
        public int User_ID { get; set; } 

        [Required]
        public int Car_ID { get; set; } 

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total Price must be positive.")]
        public decimal TotalPrice { get; set; }
    }
}
