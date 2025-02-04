using Car_Rental_Backend_Application.Data;
using Car_Rental_Backend_Application.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Car_Rental_Backend_Application.Data.Dto_s;

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

        // GET: api/Admin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
            var admins = await _context.Admin.ToListAsync();
            return Ok(admins);
        }

        // GET: api/Admin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdminById(int id)
        {
            var admin = await _context.Admin.FindAsync(id);

            if (admin == null)
            {
                return NotFound($"Admin with ID {id} not found.");
            }

            return Ok(admin);
        }

        // POST: api/Admin
        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin(AdminDto adminDto)
        {
            if (adminDto == null)
                return BadRequest("Admin data is required.");

            // Check if admin email already exists
            var existingAdmin = await _context.Admin
                .FirstOrDefaultAsync(a => a.Email == adminDto.Email);
            if (existingAdmin != null)
            {
                return BadRequest("Admin with this email already exists.");
            }

            // Convert DTO to Entity
            Admin admin = new Admin
            {
                Username = adminDto.Username,
                Email = adminDto.Email,
                Password = adminDto.Password // In real-world, hash the password
            };

            _context.Admin.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdminById), new { id = admin.Admin_ID }, admin);
        }

        // PUT: api/Admin/5
        
        // DELETE: api/Admin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _context.Admin.FindAsync(id);
            if (admin == null)
            {
                return NotFound($"Admin with ID {id} not found.");
            }

            _context.Admin.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
