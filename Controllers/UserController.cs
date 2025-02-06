using Car_Rental_Backend_Application.Data;
using Car_Rental_Backend_Application.Data.Converters;
using Car_Rental_Backend_Application.Data.Entities;
using Car_Rental_Backend_Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Car_Rental_Backend_Application.Data.RequestDto_s;
using Car_Rental_Backend_Application.Data.ResponseDto_s;

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
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users.Select(UserConverters.UserToResponseUserDto));
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById([FromRoute] int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found.");
                throw new UserNotFoundException($"User with ID {id} not found.");
            }
            return Ok(UserConverters.UserToResponseUserDto(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> RegisterUser([FromBody] UserRequestDto userRequestDto)
        {
            if (userRequestDto == null)
                return BadRequest("User data is required.");
            if (StrongPassword(userRequestDto.Password) != true)
            {
                throw new PasswordMustBeStringException($"Passsword must cantain one UpperCase,One LowerCase,One Numeric,one Special and size must be greater than 7.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == userRequestDto.Email))
                throw new EmailAlreadyExistsException($"User with Email {userRequestDto.Email} already exists.");

            if (await _context.Users.AnyAsync(u => u.Phone_Number == userRequestDto.Phone_Number))
                throw new MobileNumberAlreadyExistedException($"User with Phone Number {userRequestDto.Phone_Number} already exists.");

            User user = UserConverters.RequestUserDtoToUser(userRequestDto);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // ✅ Send Confirmation Email
            string subject = "Welcome to Car Rental Service!";
            string message = $"<h3>Hello {user.Username},</h3><p>Thank you for registering with Car Rental Service.</p>";
            await _emailService.SendEmailAsync(user.Email, subject, message);

            return CreatedAtAction(nameof(GetUserById), new { id = user.User_ID }, UserConverters.UserToResponseUserDto(user));
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserResponseDto>> GetUserByEmail([FromRoute] string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new UserNotFoundException($"User with email {email} not found.");

            return Ok(UserConverters.UserToResponseUserDto(user));
        }

        [HttpGet("phone/{phoneNumber}")]
        public async Task<ActionResult<UserResponseDto>> GetUserByPhoneNumber([FromRoute] string phoneNumber)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Phone_Number == phoneNumber);
            if (user == null)
                throw new UserNotFoundException($"User with Phone Number {phoneNumber} not found.");

            return Ok(UserConverters.UserToResponseUserDto(user));
        }

        [HttpGet("address/{address}")]
        public async Task<ActionResult<List<UserResponseDto>>> GetUsersByAddress([FromRoute] string address)
        {
            var users = await _context.Users.Where(u => u.Address.Contains(address)).ToListAsync();
            if (!users.Any())
                return NotFound($"No users found at address: {address}");

            return Ok(users.Select(UserConverters.UserToResponseUserDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.User_ID == id);
            if (user == null)
                throw new UserNotFoundException($"User with ID {id} not found.");

            if (user.Bookings.Any() )
                return BadRequest($"User with ID {id} has active bookings or reservations and cannot be deleted.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        public static bool StrongPassword(string pwd)
        {
            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasLength = pwd.Length >= 8; 
            bool hasDigit = false;
            bool hasSpecialChar = false;

            foreach (char c in pwd)
            {
                if (char.IsUpper(c))
                    hasUpperCase = true;
                if (char.IsLower(c))
                    hasLowerCase = true;
                if (char.IsDigit(c))
                    hasDigit = true;
                if (!char.IsLetterOrDigit(c))
                    hasSpecialChar = true;
            }

            return hasUpperCase && hasLowerCase && hasLength && hasDigit && hasSpecialChar;
        }

    }
}
