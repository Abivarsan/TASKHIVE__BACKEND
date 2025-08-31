using Microsoft.AspNetCore.Mvc;
using System;
using TASKHIVE.Data;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRoleController : ControllerBase
    {
        private readonly DataContext _context;

        public JobRoleController(DataContext context)
        {
            _context = context;
        }

        // GET: api/JobRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobRole>>> GetAll()
        {
            return await _context.JobRoles.ToListAsync();
        }

        // POST: api/JobRole
        [HttpPost]
        public async Task<ActionResult<JobRole>> Post(JobRole role)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.JobRoleType))
                return BadRequest("Invalid data.");

            _context.JobRoles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = role.JobRoleId }, role);
        }

        // DELETE: api/JobRole/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _context.JobRoles.FindAsync(id);
            if (role == null)
            {
                return NotFound($"JobRole with ID {id} not found.");
            }

            _context.JobRoles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
