using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAssignedDevelopersController : ControllerBase
    {
        public readonly DataContext _context;

        public GetAssignedDevelopersController(DataContext _context)
        {
            this._context = _context;
        }

        public class AssignedDevDto
        {
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string JobRoleName { get; set; }

        }

        public class AssignedDevIdDTO
        {
            public int DeveloperId { get; set; }
        }



        [HttpGet("{para}")]
        public async Task<ActionResult<IEnumerable<User>>> getDevelopers(int para)
        {


            var developers = await _context.DeveloperProjects
                .Where(e => e.ProjectId == para)
                .Select(e => new AssignedDevDto
                {
                    UserId = e.DeveloperId,
                    FirstName = e.Developer.User.FirstName,
                    LastName = e.Developer.User.LastName,
                    JobRoleName = e.Developer.User.JobRole.JobRoleType

                }).ToListAsync();

            if (developers == null)
            {
                Console.WriteLine("Developers are not added");
            }


            return Ok(developers);

        }

    }
}
