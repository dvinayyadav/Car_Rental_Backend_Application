using System;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Backend_Application.Data.DTOs
{
    public class CancellationRequestDto
    {
        [Required]
        public int Booking_ID { get; set; } // The booking that is being canceled

        [Required]
        public DateTime Cancellation_Date { get; set; } // The date when the cancellation request is made

        public string Reason { get; set; } // Optional reason for cancellation
    }
}
