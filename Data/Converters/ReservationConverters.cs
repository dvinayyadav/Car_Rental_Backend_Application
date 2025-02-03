using Car_Rental_Backend_Application.Data.Dto_s;
using Car_Rental_Backend_Application.Data.Entities;

namespace Car_Rental_Backend_Application.Data.Converters
{
    public class ReservationConverters
    {
        public static Reservation ReservationDtoToEntity(ReservationDto dto)
        {
            return new Reservation
            {
                Reservation_ID = dto.Reservation_ID,
                User_ID = dto.User_ID,
                Car_ID = dto.Car_ID,
                Reservation_Date = dto.Reservation_Date,
                Pickup_Date = dto.Pickup_Date,
                Return_Date = dto.Return_Date
            };
        }

        public static ReservationDto EntityToReservationDto(Reservation entity)
        {
            return new ReservationDto
            {
                Reservation_ID = entity.Reservation_ID,
                User_ID = entity.User_ID,
                Car_ID = entity.Car_ID,
                Reservation_Date = entity.Reservation_Date,
                Pickup_Date = entity.Pickup_Date,
                Return_Date = entity.Return_Date
            };
        }
    }
}
