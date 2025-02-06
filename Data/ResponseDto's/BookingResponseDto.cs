namespace Car_Rental_Backend_Application.Data.ResponseDto_s
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; } 

        public int User_ID { get; set; }
        public string UserName { get; set; } 

        public int Car_ID { get; set; }
        public string CarDetails { get; set; } 

        public DateTime BookingDate { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public decimal TotalPrice { get; set; }

       
        public List<int> CancellationIds { get; set; }
    }
}
