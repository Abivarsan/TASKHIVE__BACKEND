using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetProjectManagerNameController : ControllerBase
    {
        public readonly DataContext _context;

        public GetProjectManagerNameController(DataContext _context)
        {
            this._context = _context;     
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetNames()
        {
            var pManagers = await _context.Users
                .Where(e => e.UserCategoryId == 2)
                .Select(e => new GetProjectManagerNameDTO
                {
                    UserId = e.UserId,
                    FirstName = e.FirstName,
                    LastName = e.LastName
                }
                ).ToListAsync();

            if(pManagers == null)
            {
                return NotFound();
            }
            return Ok(pManagers);
        }
    }
}
