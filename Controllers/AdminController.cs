using Car_Rental_Backend_Application.Data;
using Car_Rental_Backend_Application.Data.Converters;
using Car_Rental_Backend_Application.Data.RequestDto_s;
using Car_Rental_Backend_Application.Data.ResponseDto_s;
using Car_Rental_Backend_Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Rental_Backend_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly CarRentalContext _context;

        public AdminController(CarRentalContext context)
        {
            _context = context;
        }

   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminResponseDto>>> GetAdmins()
        {
            var admins = await _context.Admin.ToListAsync();
            var adminDtos = admins.Select(AdminConverters.AdminToAdminResponseDto).ToList();
            return Ok(adminDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminResponseDto>> GetAdminById(int id)
        {
            var admin = await _context.Admin.FindAsync(id);
            if (admin == null)
                return NotFound($"Admin with ID {id} not found.");

            return Ok(AdminConverters.AdminToAdminResponseDto(admin));
        }

     
        [HttpPost]
        public async Task<ActionResult<AdminResponseDto>> CreateAdmin(AdminRequestDto adminRequestDto)
        {
            if (adminRequestDto == null)
                return BadRequest("Admin data is required.");

            if (UsersController.StrongPassword(adminRequestDto.Password) != true)
            {
                throw new PasswordMustBeStringException($"Passsword must cantain one UpperCase,One LowerCase,One Numeric,one Special and size must be greater than 7.");
            }

            var existingAdmin = await _context.Admin.FirstOrDefaultAsync(a => a.Email == adminRequestDto.Email);
            if (existingAdmin != null)
                return BadRequest("Admin with this email already exists.");

           
            var admin = AdminConverters.AdminRequestDtoToAdmin(adminRequestDto);

           
            _context.Admin.Add(admin);
            await _context.SaveChangesAsync();

            var createdAdminResponseDto = AdminConverters.AdminToAdminResponseDto(admin);
            return CreatedAtAction(nameof(GetAdminById), new { id = createdAdminResponseDto.Admin_ID }, createdAdminResponseDto);
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _context.Admin.FindAsync(id);
            if (admin == null)
                return NotFound($"Admin with ID {id} not found.");

            _context.Admin.Remove(admin);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
