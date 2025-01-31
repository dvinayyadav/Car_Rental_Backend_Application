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
                Username = ud.Username,
                Password = ud.Password,  // You should hash the password before saving
                Email = ud.Email,
                Address = ud.Address,
                Phone_Number = ud.Phone_Number,

                // Since UserDto contains only IDs, we must initialize these as empty lists or retrieve from DB
                Bookings = ud.BookingIds?.Select(id => new Booking { Booking_ID = id }).ToList(),
                Reservations = ud.ReservationIds?.Select(id => new Reservation { Reservation_ID = id }).ToList()
            };

            return user;
        }

        // Convert User Entity to UserDto
        public static UserDto UserToUserDto(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            UserDto userDto = new UserDto()
            {
                User_Id=user.User_ID,
                Username = user.Username,
                Email = user.Email,
                Address = user.Address,
                Phone_Number = user.Phone_Number,
                Password=user.Password,

                // Correctly reference BookingIds and ReservationIds instead of missing Bookings/Reservations properties
                BookingIds = user.Bookings?.Select(booking => booking.Booking_ID).ToList(),
                ReservationIds = user.Reservations?.Select(reservation => reservation.Reservation_ID).ToList()
            };

            return userDto;
        }
    }
}
