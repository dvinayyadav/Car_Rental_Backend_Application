﻿using Car_Rental_Backend_Application.Data;
using Car_Rental_Backend_Application.Data.Converters;
using Car_Rental_Backend_Application.Data.Dto_s;
using Car_Rental_Backend_Application.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Rental_Backend_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly CarRentalContext _context;

        public CarController(CarRentalContext context)
        {
            _context = context;
        }

        // GET: api/car
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetCars()
        {
            var cars = await _context.Cars
                .Include(c => c.Bookings)
                .Include(c => c.Reservations)
                .ToListAsync();

            var carDtos = cars.Select(car => CarConverters.CarToCarDto(car)).ToList();

            return Ok(carDtos);
        }

        // GET: api/car/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetCarById(int id)
        {
            var car = await _context.Cars
                .Include(c => c.Bookings)
                .Include(c => c.Reservations)
                .FirstOrDefaultAsync(c => c.Car_ID == id);

            if (car == null)
                return NotFound($"Car with ID {id} not found.");

            return Ok(CarConverters.CarToCarDto(car));
        }

        // POST: api/car
        [HttpPost]
        public async Task<ActionResult<CarDto>> CreateCar(CarDto carDto)
        {
            if (carDto == null)
                return BadRequest("Car data is required.");

            var car = CarConverters.CarDtoToCar(carDto);
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            var createdCarDto = CarConverters.CarToCarDto(car);

            return CreatedAtAction(nameof(GetCarById), new { id = createdCarDto.Car_ID }, createdCarDto);
        }

        // PUT: api/car/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, CarDto carDto)
        {
            if (id != carDto.Car_ID)
                return BadRequest("Car ID mismatch.");

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound($"Car with ID {id} not found.");

            // Update fields
            car.Brand = carDto.Brand;
            car.Model = carDto.Model;
            car.Year = carDto.Year;
            car.License_Plate = carDto.License_Plate;
            car.Availability_Status = carDto.Availability_Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/car/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound($"Car with ID {id} not found.");

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/car/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetAvailableCars()
        {
            var availableCars = await _context.Cars
                .Where(c => c.Availability_Status == "Available")
                .ToListAsync();

            if (!availableCars.Any())
                return NotFound("No available cars at the moment.");

            var carDtos = availableCars.Select(CarConverters.CarToCarDto).ToList();
            return Ok(carDtos);
        }

        [HttpGet("rented")]
        public async Task<ActionResult<IEnumerable<object>>> GetRentedCars()
        {
            var rentedCars = await _context.Cars
                .Include(c => c.Bookings)
                .Where(c => c.Availability_Status == "Rented")
                .Select(c => new
                {
                    Car_ID = c.Car_ID,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    License_Plate = c.License_Plate,
                    Availability_Status = c.Availability_Status,
                    BookingId = c.Bookings.OrderByDescending(b => b.BookingDate)
                                          .Select(b => b.BookingId)
                                          .FirstOrDefault() // Get the latest booking ID
                })
                .ToListAsync();

            if (!rentedCars.Any())
                return NotFound("No rented cars at the moment.");

            return Ok(rentedCars);
        }


    }
}
