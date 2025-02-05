using Car_Rental_Backend_Application.Data.Dto_s;
using Car_Rental_Backend_Application.Data.Entities;
using Car_Rental_Backend_Application.Data.RequestDto_s;
using Car_Rental_Backend_Application.Data.ResponseDto_s;
using Car_Rental_Backend_Application.Data.ENUMS;

namespace Car_Rental_Backend_Application.Data.Converters
{
    public class CarConverters
    {
        // Convert Car to CarResponseDto (for sending back data to the client)
        public static CarResponseDto CarToCarResponseDto(Car car)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car));

            return new CarResponseDto
            {
                Car_ID = car.Car_ID,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                License_Plate = car.License_Plate,
                Availability_Status = car.Availability_Status.ToString(),  // Convert enum to string
                BookingIds = car.Bookings?.Select(b => b.BookingId).ToList(),
                ReservationIds = car.Reservations?.Select(r => r.Reservation_ID).ToList()
            };
        }

        // Convert CarRequestDto to Car (for creating or updating car)
        public static Car CarRequestDtoToCar(CarRequestDto carRequestDto)
        {
            if (carRequestDto == null)
                throw new ArgumentNullException(nameof(carRequestDto));

            // Ensure that Availability_Status from the request is properly mapped to the enum
            if (!Enum.IsDefined(typeof(AvailabilityStatus), carRequestDto.Availability_Status))
            {
                throw new ArgumentException($"Invalid Availability Status value: {carRequestDto.Availability_Status}");
            }

            // Mapping CarRequestDto to Car
            var car = new Car
            {
                Brand = carRequestDto.Brand,
                Model = carRequestDto.Model,
                Year = carRequestDto.Year,
                License_Plate = carRequestDto.License_Plate,
                Availability_Status = carRequestDto.Availability_Status, // Cast to enum
                // Note: Bookings and Reservations may not be part of the request DTO
            };

            return car;
        }
    }
}
