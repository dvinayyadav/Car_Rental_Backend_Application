
using Car_Rental_Backend_Application.Data.Converters;
using Car_Rental_Backend_Application.Data.Dto_s;
using Car_Rental_Backend_Application.Data.Entities;
using Car_Rental_Backend_Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Car_Rental_Backend_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CarRentalContext _context;

        public UsersController(CarRentalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            var userDtos = users.Select(u => new UserDto
            {
                User_Id=u.User_ID,
                Password=u.Password,
                Username = u.Username,
                Email = u.Email,
                Address = u.Address,
                Phone_Number = u.Phone_Number,
                BookingIds = u.Bookings?.Select(b => b.Booking_ID).ToList(),
                ReservationIds = u.Reservations?.Select(r => r.Reservation_ID).ToList()
            }).ToList();

            return Ok(userDtos);
        }



        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
           
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {id} not found.");
            }

            return UserConverters.UserToUserDto(user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDto userdto)
        {
            if (userdto == null)
                return BadRequest("User data is required.");

            // Convert DTO to Entity
            User user = UserConverters.UserDtoToUser(userdto);

            // Check if Car_IDs exist before inserting bookings
            foreach (var booking in user.Bookings)
            {
                bool carExists = await _context.Cars.AnyAsync(c => c.Car_ID == booking.Car_ID);
                if (!carExists)
                {
                    return BadRequest($"Car with ID {booking.Car_ID} does not exist.");
                }
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new { id = user.User_ID }, user);
        }

        // PUT: api/Users/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id)
        //{
            
        //    return NoContent();
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<UserDto> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        throw new UserNotFoundException($"User with ID {id} not found.");
        //    }

        //    return UserConverters.UserToUserDto(user);
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.User_ID == id);
        //}
    }
}