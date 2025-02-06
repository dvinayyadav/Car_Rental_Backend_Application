namespace Car_Rental_Backend_Application.Data.ResponseDto_s
{
    public class UserResponseDto
    {
        public int User_ID { get; set; } 

        public string Username { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Phone_Number { get; set; }

        public List<int> BookingIds { get; set; }
    }
}
