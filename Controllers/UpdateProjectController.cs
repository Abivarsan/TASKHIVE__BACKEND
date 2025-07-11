using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateProjectController : ControllerBase
    {
        public readonly DataContext _context;

        public UpdateProjectController(DataContext _context)
        {
            this._context = _context;   
        }

        [HttpPut]
        public async Task<IActionResult> updateProject(int id, ProjectDto request)
        {
            if (request == null)
            {
                return NotFound();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();


        }
    }
}
