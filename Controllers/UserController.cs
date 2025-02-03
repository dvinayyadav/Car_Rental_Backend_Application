
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

            var userDtos = users.Select(u => UserConverters.UserToUserDto(u)).ToList();

            return Ok(userDtos);
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        //{
        //    var users = await _context.Users
        //        .Include(u => u.Bookings) // Load bookings
        //        .ThenInclude(b => b.Car) // Load car details if needed
        //        .Include(u => u.Reservations) // Load reservations
        //        .ToListAsync();

        //    return Ok(users);
        //}





        // GET: api/Users/5
        [HttpGet("id/{id}")]
        public async Task<ActionResult<UserDto>> GetUserByIdS([FromRoute] int id)
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

        // GET: api/Users/5
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail([FromRoute] string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new UserNotFoundException($"User with email {email} not found.");
            }

            return Ok(UserConverters.UserToUserDto(user));
        }

      
        [HttpGet("phone/{phoneNumber}")]
        public async Task<ActionResult<UserDto>> GetUserByPhoneNumber([FromRoute] string phoneNumber)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Phone_Number == phoneNumber);

            if (user == null)
            {
                throw new UserNotFoundException($"User with Phone Number {phoneNumber} not found.");
            }

            return Ok(UserConverters.UserToUserDto(user));
        }


        [HttpGet("address/{address}")]
        public async Task<ActionResult<List<UserDto>>> GetUsersByAddress([FromRoute] string address)
        {
            var users = await _context.Users
                .Where(u => u.Address.Contains(address))
                .ToListAsync();

            if (users == null || users.Count == 0)
            {
                return NotFound($"No users found at address: {address}");
            }

            return Ok(users.Select(UserConverters.UserToUserDto).ToList());
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