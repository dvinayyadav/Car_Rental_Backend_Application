using Car_Rental_Backend_Application.Data.Converters;
using Car_Rental_Backend_Application.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly CarRentalContext _context;

    public BookingController(CarRentalContext context)
    {
        _context = context;
    }

    // POST: api/bookings
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

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Car_ID == bookingDto.CarId);
            if (car == null)
                return NotFound("Car not found.");

            var booking = new Booking
            {
                User_ID = bookingDto.UserId,
                User = user,
                Car_ID = bookingDto.CarId,
                Car = car,
                BookingDate = bookingDto.BookingDate,
                PickupDate = bookingDto.PickupDate,
                ReturnDate = bookingDto.ReturnDate,
                TotalPrice = bookingDto.TotalPrice
            };

            user.Bookings.Add(booking);
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var createdBookingDto = BookingConverters.BookingToBookingDto(booking);
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
        try
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                return NotFound($"Booking with ID {id} not found.");

            var bookingDto = BookingConverters.BookingToBookingDto(booking);
            return Ok(bookingDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // PUT: api/bookings/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBooking(int id, BookingDto bookingDto)
    {
        if (id != bookingDto.BookingId)
            return BadRequest("Booking ID mismatch.");

        var booking = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Car)
            .FirstOrDefaultAsync(b => b.BookingId == id);

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
            if (!_context.Bookings.Any(b => b.BookingId == id))
                return NotFound();
            else
                throw;
        }
    }

    // DELETE: api/bookings/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBooking(int id)
    {
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.BookingId == id);

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
        try
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)  // Ensure the User data is loaded
                .Include(b => b.Car)   // Ensure the Car data is loaded
                .ToListAsync();

            if (bookings == null || !bookings.Any())
                return NotFound("No bookings found.");

            // Convert the bookings to DTOs
            var bookingDtos = bookings.Select(b => BookingConverters.BookingToBookingDto(b)).ToList();

            return Ok(bookingDtos);
        }
        catch (Exception ex)
        {
            // Log error (optional) and return a 500 status code with the exception message
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
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
        {
            query = query.Where(b => b.User_ID == userId.Value);
        }

        var bookings = await query.ToListAsync();

        if (!bookings.Any())
            return NotFound("No bookings found within the specified dates.");

        return Ok(bookings.Select(BookingConverters.BookingToBookingDto));
    }

}
