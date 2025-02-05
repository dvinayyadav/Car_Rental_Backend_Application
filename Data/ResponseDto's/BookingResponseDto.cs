namespace Car_Rental_Backend_Application.Data.ResponseDto_s
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; } // Primary key

        public int User_ID { get; set; } // Foreign key for User
        public string UserName { get; set; } // Optional: To show User Name

        public int Car_ID { get; set; } // Foreign key for Car
        public string CarDetails { get; set; } // Optional: To show Car details (Brand + Model)

        public DateTime BookingDate { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public decimal TotalPrice { get; set; }

        // List of cancellation IDs if any
        public List<int> CancellationIds { get; set; }
    }
}
