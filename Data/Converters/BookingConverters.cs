using Car_Rental_Backend_Application.Data.Entities;

namespace Car_Rental_Backend_Application.Data.Converters
{
    public class BookingConverters
    {
        public static Booking BookingDtoToBooking(BookingDto bookingDto)
        {
            if (bookingDto == null)
                throw new ArgumentNullException(nameof(bookingDto));

            return new Booking
            {
                BookingId = bookingDto.BookingId,
                User_ID = bookingDto.UserId,
                Car_ID = bookingDto.CarId,
                BookingDate = bookingDto.BookingDate,
                PickupDate = bookingDto.PickupDate,
                ReturnDate = bookingDto.ReturnDate,
                TotalPrice = bookingDto.TotalPrice,

                // Cancellations should not be mapped here; handled separately
                Cancellations = new List<Cancellation>()
            };
        }

        public static BookingDto BookingToBookingDto(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking));

            return new BookingDto
            {
                BookingId = booking.BookingId,
                UserId = booking.User_ID,
                CarId = booking.Car_ID,
                BookingDate = booking.BookingDate,
                PickupDate = booking.PickupDate,
                ReturnDate = booking.ReturnDate,
                TotalPrice = booking.TotalPrice,

                // Store only Cancellation IDs
                CancellationIds = booking.Cancellations?
                    .Select(cancellation => cancellation.Cancellation_ID)
                    .ToList() ?? new List<int>()
            };
        }

    }
}
