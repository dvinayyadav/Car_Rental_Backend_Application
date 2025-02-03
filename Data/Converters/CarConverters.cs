using Car_Rental_Backend_Application.Data.Dto_s;
using Car_Rental_Backend_Application.Data.Entities;

namespace Car_Rental_Backend_Application.Data.Converters
{
    public class CarConverters
    {
        public static CarDto CarToCarDto(Car car)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car));

            // Mapping Car to CarDto
            var carDto = new CarDto
            {
                Car_ID=car.Car_ID,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                License_Plate = car.License_Plate,
                Availability_Status = car.Availability_Status,
                BookingIds = car.Bookings?.Select(b => b.BookingId).ToList(),

                // Store only Reservation IDs instead of full ReservationDto objects
                ReservationIds = car.Reservations?.Select(r => r.Reservation_ID).ToList(),

                //Availability = car.Availability == null ? null : AvailabilityConverters.AvailabilityToAvailabilityDto(car.Availability)
            };

            return carDto;
        }


        public static Car CarDtoToCar(CarDto carDto)
            {
                if (carDto == null)
                    throw new ArgumentNullException(nameof(carDto));

                // Mapping CarDto to Car
                var car = new Car
                {
                    Brand = carDto.Brand,
                    Model = carDto.Model,
                    Year = carDto.Year,
                    License_Plate = carDto.License_Plate,
                    Availability_Status = carDto.Availability_Status,
                    // Optionally handle relationships like Bookings and Reservations if required
                };

                return car;
            }
        

    }
}
