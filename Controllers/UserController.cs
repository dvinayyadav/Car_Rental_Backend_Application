using Car_Rental_Backend_Application.Data;
using Car_Rental_Backend_Application.Data.Converters;
using Car_Rental_Backend_Application.Data.Dto_s;
using Car_Rental_Backend_Application.Data.Entities;
using Car_Rental_Backend_Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Rental_Backend_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CarRentalContext _context;
        private readonly EmailService _emailService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(CarRentalContext context, EmailService emailService, ILogger<UsersController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users.Select(UserConverters.UserToUserDto));
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<UserDto>> GetUserById([FromRoute] int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found.");
                throw new UserNotFoundException($"User with ID {id} not found.");
            }
            return Ok(UserConverters.UserToUserDto(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> RegisterUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
                return BadRequest("User data is required.");

            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                throw new EmailAlreadyExistsException($"User with Email {userDto.Email} already exists.");

            if (await _context.Users.AnyAsync(u => u.Phone_Number == userDto.Phone_Number))
                throw new MobileNumberAlreadyExistedException($"User with Phone Number {userDto.Phone_Number} already exists.");

            User user = UserConverters.UserDtoToUser(userDto);

            foreach (var booking in user.Bookings)
            {
                if (!await _context.Cars.AnyAsync(c => c.Car_ID == booking.Car_ID))
                    return BadRequest($"Car with ID {booking.Car_ID} does not exist.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // ✅ Send Confirmation Email
            string subject = "Welcome to Car Rental Service!";
            string message = $"<h3>Hello {user.Username},</h3><p>Thank you for registering with Car Rental Service.</p>";
            await _emailService.SendEmailAsync(user.Email, subject, message);

            return CreatedAtAction(nameof(GetUserById), new { id = user.User_ID }, UserConverters.UserToUserDto(user));
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail([FromRoute] string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new UserNotFoundException($"User with email {email} not found.");

            return Ok(UserConverters.UserToUserDto(user));
        }

        [HttpGet("phone/{phoneNumber}")]
        public async Task<ActionResult<UserDto>> GetUserByPhoneNumber([FromRoute] string phoneNumber)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Phone_Number == phoneNumber);
            if (user == null)
                throw new UserNotFoundException($"User with Phone Number {phoneNumber} not found.");

            return Ok(UserConverters.UserToUserDto(user));
        }

        [HttpGet("address/{address}")]
        public async Task<ActionResult<List<UserDto>>> GetUsersByAddress([FromRoute] string address)
        {
            var users = await _context.Users.Where(u => u.Address.Contains(address)).ToListAsync();
            if (!users.Any())
                return NotFound($"No users found at address: {address}");

            return Ok(users.Select(UserConverters.UserToUserDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var user = await _context.Users.Include(u => u.Bookings).Include(u => u.Reservations).FirstOrDefaultAsync(u => u.User_ID == id);
            if (user == null)
                throw new UserNotFoundException($"User with ID {id} not found.");

            if (user.Bookings.Any() || user.Reservations.Any())
                return BadRequest($"User with ID {id} has active bookings or reservations and cannot be deleted.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}