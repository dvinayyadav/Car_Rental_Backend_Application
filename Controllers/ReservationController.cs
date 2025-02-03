using Car_Rental_Backend_Application.Data.Converters;
using Car_Rental_Backend_Application.Data.Dto_s;
using Car_Rental_Backend_Application.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_Backend_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly CarRentalContext _context;

        public ReservationsController(CarRentalContext context)
        {
            _context = context;
        }

        // POST: api/Reservations
        [HttpPost]
        public async Task<ActionResult<ReservationDto>> CreateReservation(ReservationDto reservationDto)
        {
            if (reservationDto == null)
            {
                return BadRequest("Reservation data is required.");
            }

            try
            {
                var reservation = ReservationConverters.ReservationDtoToEntity(reservationDto);
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                var createdReservationDto = ReservationConverters.EntityToReservationDto(reservation);
                return CreatedAtAction(nameof(GetReservationById), new { id = createdReservationDto.Reservation_ID }, createdReservationDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations()
        {
            var reservations = await _context.Reservations.ToListAsync();

            if (reservations == null || !reservations.Any())
            {
                return NotFound("No reservations found.");
            }

            var reservationDtos = reservations.Select(r => ReservationConverters.EntityToReservationDto(r)).ToList();
            return Ok(reservationDtos);
        }

        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetReservationById(int id)
        {
            var reservation = await _context.Reservations
                                            .FirstOrDefaultAsync(r => r.Reservation_ID == id);

            if (reservation == null)
            {
                return NotFound($"Reservation with ID {id} not found.");
            }

            var reservationDto = ReservationConverters.EntityToReservationDto(reservation);
            return Ok(reservationDto);
        }

        // PUT: api/Reservations/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReservation(int id, ReservationDto reservationDto)
        {
            if (id != reservationDto.Reservation_ID)
            {
                return BadRequest("Reservation ID mismatch.");
            }

            var existingReservation = await _context.Reservations
                                                     .FirstOrDefaultAsync(r => r.Reservation_ID == id);

            if (existingReservation == null)
            {
                return NotFound($"Reservation with ID {id} not found.");
            }

            existingReservation.Pickup_Date = reservationDto.Pickup_Date;
            existingReservation.Return_Date = reservationDto.Return_Date;
            existingReservation.Reservation_Date = reservationDto.Reservation_Date;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations
                                            .FirstOrDefaultAsync(r => r.Reservation_ID == id);

            if (reservation == null)
            {
                return NotFound($"Reservation with ID {id} not found.");
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(r => r.Reservation_ID == id);
        }
    }

}
