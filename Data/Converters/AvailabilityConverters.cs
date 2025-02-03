using Car_Rental_Backend_Application.Data.Dto_s;
using Car_Rental_Backend_Application.Data.Entities;

namespace Car_Rental_Backend_Application.Data.Converters
{
    public static class AvailabilityConverters
    {
        public static AvailabilityDto AvailabilityToAvailabilityDto(Availability availability)
        {
            if (availability == null)
                throw new ArgumentNullException(nameof(availability));

            return new AvailabilityDto
            {
                Car_ID = availability.Car_ID,
                Pickup_Date = availability.Pickup_Date,
                Return_Date = availability.Return_Date,
                Available= availability.Available
            };
        }

        public static Availability AvailabilityDtoToAvailability(AvailabilityDto availabilityDto)
        {
            if (availabilityDto == null)
                throw new ArgumentNullException(nameof(availabilityDto));

            return new Availability
            {
                Car_ID = availabilityDto.Car_ID,
                Pickup_Date = availabilityDto.Pickup_Date,
                Return_Date = availabilityDto.Return_Date,
                Available = availabilityDto.Available
            };
        }
    }

}
