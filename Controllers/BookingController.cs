using Car_Rental_Backend_Application.Data;
using Car_Rental_Backend_Application.Data.Converters;
using Car_Rental_Backend_Application.Data.Entities;
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
    public async Task<ActionResult<BookingDto>> CreateBooking(BookingDto bookingDto)
    {
        if (bookingDto == null)
            return BadRequest("Booking data is required.");

        try
        {
            var user = await _context.Users
                .Include(u => u.Bookings)
                .FirstOrDefaultAsync(u => u.User_ID == bookingDto.UserId);

            if (user == null)
                return NotFound("User not found.");

            var car = await _context.Cars.FindAsync(bookingDto.CarId);
            if (car == null)
                return NotFound("Car not found.");

            // Check if the car is already rented
            if (car.Availability_Status == "Rented")
                return BadRequest("Car is currently rented and not available for booking.");

            // Proceed with booking since the car is available
            var booking = new Booking
            {
                User_ID = bookingDto.UserId,
                Car_ID = bookingDto.CarId,
                BookingDate = bookingDto.BookingDate,
                PickupDate = bookingDto.PickupDate,
                ReturnDate = bookingDto.ReturnDate,
                TotalPrice = bookingDto.TotalPrice
            };

            _context.Bookings.Add(booking);

            // Update car status to "Rented"
            car.Availability_Status = "Rented";
            _context.Cars.Update(car);

            await _context.SaveChangesAsync();

            var createdBookingDto = BookingConverters.BookingToBookingDto(booking);

            // Send Booking Confirmation Email
            string emailSubject = "Booking Confirmation - Car Rental";
            string emailBody = $@"
            <h2>Dear {user.Username},</h2>
            <p>Your booking has been confirmed successfully!</p>
            <h3>Booking Details:</h3>
            <ul>
                <li><strong>Booking ID:</strong> {booking.BookingId}</li>
                <li><strong>Car Model:</strong> {car.Model}</li>
                <li><strong>Pickup Date:</strong> {booking.PickupDate:yyyy-MM-dd}</li>
                <li><strong>Return Date:</strong> {booking.ReturnDate:yyyy-MM-dd}</li>
                <li><strong>Total Cost:</strong> ${booking.TotalPrice}</li>
            </ul>
            <p>Thank you for choosing our service!</p>
            <p>Best Regards, <br/>Car Rental Team</p>
        ";

            await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

            return CreatedAtAction(nameof(GetBookingById), new { id = createdBookingDto.BookingId }, createdBookingDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    // GET: api/bookings/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetBookingById(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Car)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking == null)
            return NotFound($"Booking with ID {id} not found.");

        return Ok(BookingConverters.BookingToBookingDto(booking));
    }

    // PUT: api/bookings/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBooking(int id, BookingDto bookingDto)
    {
        if (id != bookingDto.BookingId)
            return BadRequest("Booking ID mismatch.");

        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
            return NotFound($"Booking with ID {id} not found.");

        booking.PickupDate = bookingDto.PickupDate;
        booking.ReturnDate = bookingDto.ReturnDate;
        booking.TotalPrice = bookingDto.TotalPrice;

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

    // DELETE: api/bookings/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
            return NotFound($"Booking with ID {id} not found.");

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/bookings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetAllBookings()
    {
        var bookings = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Car)
            .ToListAsync();

        if (!bookings.Any())
            return NotFound("No bookings found.");

        return Ok(bookings.Select(BookingConverters.BookingToBookingDto));
    }

    // GET: api/bookings/between-dates
    [HttpGet("between-dates")]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsBetweenDates(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] int? userId = null)
    {
        if (startDate > endDate)
            return BadRequest("Start date must be earlier than end date.");

        var query = _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Car)
            .Where(b => b.PickupDate >= startDate && b.ReturnDate <= endDate);

        if (userId.HasValue)
            query = query.Where(b => b.User_ID == userId.Value);

        var bookings = await query.ToListAsync();

        if (!bookings.Any())
            return NotFound("No bookings found within the specified dates.");

        return Ok(bookings.Select(BookingConverters.BookingToBookingDto));
    }
}
