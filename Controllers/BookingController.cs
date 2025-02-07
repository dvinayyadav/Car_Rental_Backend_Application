using Car_Rental_Backend_Application.Data;
using Car_Rental_Backend_Application.Data.Converters;
using Car_Rental_Backend_Application.Data.RequestDto_s;
using Car_Rental_Backend_Application.Data.ResponseDto_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly CarRentalContext _context;
    private readonly EmailService _emailService;

    public BookingController(CarRentalContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<ActionResult<BookingResponseDto>> CreateBooking(BookingRequestDto bookingRequestDto)
    {
        if (bookingRequestDto == null)
            return BadRequest("Booking data is required.");

        try
        {
            var user = await _context.Users
                .Include(u => u.Bookings)
                .FirstOrDefaultAsync(u => u.User_ID == bookingRequestDto.User_ID);

            if (user == null)
                return NotFound("User not found.");

            var car = await _context.Cars.FindAsync(bookingRequestDto.Car_ID);
            if (car == null)
                return NotFound("Car not found.");

            if (car.Availability_Status == "Booked")
                return BadRequest("Car is currently rented and not available for booking.");

            // Convert DTO to Booking entity
            var booking = BookingConverters.BookingRequestDtoToBooking(bookingRequestDto);
            booking.User = user;  // Directly associate user
            booking.Car = car;    // Directly associate car

            _context.Bookings.Add(booking);

            car.Availability_Status = "Booked";
            _context.Cars.Update(car);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingId },
                BookingConverters.BookingToBookingResponseDto(booking));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetAllBookings()
    {
        var bookings = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Car)
            .ToListAsync();

        if (!bookings.Any())
            return NotFound("No bookings found.");

        return Ok(bookings.Select(BookingConverters.BookingToBookingResponseDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingResponseDto>> GetBookingById(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Car)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking == null)
            return NotFound($"Booking with ID {id} not found.");

        return Ok(BookingConverters.BookingToBookingResponseDto(booking));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, BookingRequestDto bookingRequestDto)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
            return NotFound($"Booking with ID {id} not found.");

        booking.PickupDate = bookingRequestDto.PickupDate;
        booking.ReturnDate = bookingRequestDto.ReturnDate;
        booking.TotalPrice = bookingRequestDto.TotalPrice;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(500, "Database concurrency issue. Please try again.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
            return NotFound($"Booking with ID {id} not found.");

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}