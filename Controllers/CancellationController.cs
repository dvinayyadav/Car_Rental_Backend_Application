using Car_Rental_Backend_Application.Data;
using Car_Rental_Backend_Application.Data.Dto_s;
using Car_Rental_Backend_Application.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CancellationController : ControllerBase
{
    private readonly CarRentalContext _context;
    private readonly EmailService _emailService; // Inject Email Service

    public CancellationController(CarRentalContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<ActionResult<CancellationDto>> CreateCancellation(CancellationDto cancellationDto)
    {
        if (cancellationDto == null)
            return BadRequest("Cancellation data is required.");

        try
        {
            var booking = await _context.Bookings
                .Include(b => b.Car)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingId == cancellationDto.BookingId);

            if (booking == null)
                return NotFound("Booking not found.");

            var existingCancellation = await _context.Cancellations
                .FirstOrDefaultAsync(c => c.Booking_ID == cancellationDto.BookingId);

            if (existingCancellation != null)
                return BadRequest("This booking has already been canceled.");

            // Step 1: Create & Save Cancellation Record
            var cancellation = new Cancellation
            {
                Booking_ID = cancellationDto.BookingId,
                Cancellation_Date = DateTime.UtcNow,
                Reason = cancellationDto.Reason
            };

            _context.Cancellations.Add(cancellation);
            await _context.SaveChangesAsync(); // ✅ Save Cancellation First

            // Step 2: Update Car Availability
            if (booking.Car != null)
            {
                booking.Car.Availability_Status = "Available";
                _context.Cars.Update(booking.Car);
            }

            // Step 3: Remove Booking
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync(); // ✅ Save Changes After Deleting Booking

            // Step 4: Send Cancellation Email
            string emailSubject = "Booking Cancellation Confirmation - Car Rental";
            string emailBody = $@"
            <h2>Dear {booking.User.Username},</h2>
            <p>Your booking has been successfully canceled.</p>
            <h3>Cancellation Details:</h3>
            <ul>
                <li><strong>Booking ID:</strong> {booking.BookingId}</li>
                <li><strong>Car Model:</strong> {booking.Car.Model}</li>
                <li><strong>Pickup Date:</strong> {booking.PickupDate:yyyy-MM-dd}</li>
                <li><strong>Return Date:</strong> {booking.ReturnDate:yyyy-MM-dd}</li>
                <li><strong>Cancellation Date:</strong> {DateTime.UtcNow:yyyy-MM-dd}</li>
                <li><strong>Reason:</strong> {cancellation.Reason}</li>
            </ul>
            <p>We hope to serve you again in the future!</p>
            <p>Best Regards, <br/>Car Rental Team</p>
        ";

            await _emailService.SendEmailAsync(booking.User.Email, emailSubject, emailBody);

            // Return the cancellation details
            return CreatedAtAction(nameof(GetCancellationById), new { id = cancellation.Cancellation_ID }, new CancellationDto
            {
                CancellationId = cancellation.Cancellation_ID,
                BookingId = cancellation.Booking_ID,
                CancellationDate = cancellation.Cancellation_Date,
                Reason = cancellation.Reason
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    // GET: api/cancellations/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CancellationDto>> GetCancellationById(int id)
    {
        var cancellation = await _context.Cancellations
            .FirstOrDefaultAsync(c => c.Cancellation_ID == id);

        if (cancellation == null)
            return NotFound($"Cancellation with ID {id} not found.");

        return Ok(new CancellationDto
        {
            CancellationId = cancellation.Cancellation_ID,
            BookingId = cancellation.Booking_ID,
            CancellationDate = cancellation.Cancellation_Date,
            Reason = cancellation.Reason
        });
    }

    // GET: api/cancellations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CancellationDto>>> GetAllCancellations()
    {
        var cancellations = await _context.Cancellations.ToListAsync();

        if (!cancellations.Any())
            return NotFound("No cancellations found.");

        return Ok(cancellations.Select(c => new CancellationDto
        {
            CancellationId = c.Cancellation_ID,
            BookingId = c.Booking_ID,
            CancellationDate = c.Cancellation_Date,
            Reason = c.Reason
        }));
    }
}
