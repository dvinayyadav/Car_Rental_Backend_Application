using Car_Rental_Backend_Application.Data.ENUMS;

namespace Car_Rental_Backend_Application.Data.ResponseDto_s
{
    public class CarResponseDto
    {
        public int Car_ID { get; set; } // Primary Key
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string License_Plate { get; set; }
        public string Availability_Status { get; set; } // e.g., Available, Booked

        // List of related Booking and Reservation IDs
        public List<int> BookingIds { get; set; }
        public List<int> ReservationIds { get; set; }
    }
}
