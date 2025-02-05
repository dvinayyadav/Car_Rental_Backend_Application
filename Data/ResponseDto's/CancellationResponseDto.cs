using System;

namespace Car_Rental_Backend_Application.Data.DTOs
{
    public class CancellationResponseDto
    {
        public int Cancellation_ID { get; set; } // The ID of the canceled booking

        public int Booking_ID { get; set; } // The booking that was canceled

        public DateTime Cancellation_Date { get; set; } // The date of cancellation

        public string Reason { get; set; } // The reason provided for cancellation
    }
}
