using Car_Rental_Backend_Application.Data.Dto_s;
using Car_Rental_Backend_Application.Data.Entities;

namespace Car_Rental_Backend_Application.Data.Converters
{
    public class UserConverters
    {
        public static User UserDtoToUser(UserDto ud)
        {
            if (ud == null)
                throw new ArgumentNullException(nameof(ud));

            User user = new User
            {
                User_ID=ud.User_ID,
                Username = ud.Username,
                Password = ud.Password,  // You should hash the password before saving
                Email = ud.Email,
                Address = ud.Address,
                Phone_Number = ud.Phone_Number,

                // Since UserDto contains only IDs, we must initialize these as empty lists or retrieve from DB
                Bookings = ud.BookingIds?.Select(id => new Booking { BookingId = id }).ToList(),
                Reservations = ud.ReservationIds?.Select(id => new Reservation { Reservation_ID = id }).ToList()
            };

            return user;
        }

        // Convert User Entity to UserDto
        public static UserDto UserToUserDto(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return new UserDto()
            {
                User_ID = user.User_ID,
                Username = user.Username,
                Password=user.Password,
                Email = user.Email,
                Address = user.Address,
                Phone_Number = user.Phone_Number,  // Ensure consistent naming

                // Mapping Booking IDs and Reservation IDs safely
                BookingIds = user.Bookings?.Select(booking => booking.BookingId).ToList() ?? new List<int>(),
                ReservationIds = user.Reservations?.Select(reservation => reservation.Reservation_ID).ToList() ?? new List<int>()
            };
        }

    }
}
