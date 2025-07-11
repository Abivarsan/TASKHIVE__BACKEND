using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddDevelopersController : ControllerBase
    {
        public readonly DataContext _context;

        public AddDevelopersController(DataContext _context)
        {
            this._context = _context;
        }

        public class DeveloperListDTO
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string? JobRoleName { get; set; }

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetDeveloperList()
        {
            var developers = await _context.Users
                .Where(e => e.UserCategoryId == 3)
                .Select(e => new DeveloperListDTO
                {
                    UserId = e.UserId,
                    UserName = e.UserName,
                    JobRoleName = e.JobRole.JobRoleType
                })
                .ToListAsync();

            if (developers == null)
            {

            }

            return Ok(developers);

        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<DeveloperProject>>> PostAssignedDev(DeveloperProjectDTO request)
        {
            var proId = await _context.Projects.FindAsync(request.ProjectId);
            if (proId == null)
            {
                return NotFound();
            }

            var devId = await _context.Developers.FindAsync(request.DeveloperId);
            if (devId == null)
            {
                return NotFound();
            }


            var newDevProject = new DeveloperProject
            {
                ProjectId = request.ProjectId,
                DeveloperId = request.DeveloperId
            };

            _context.Add(newDevProject);
            await _context.SaveChangesAsync();
            return Ok();




        }


        [HttpDelete("{projectId}/{developerId}")]
        public async Task<ActionResult> DeleteAssignedDev(int projectId, int developerId)
        {
            try
            {
                var devProject = await _context.DeveloperProjects
                                               .FirstOrDefaultAsync(dp => dp.ProjectId == projectId && dp.DeveloperId == developerId);

                if (devProject == null)
                {
                    return NotFound();
                }

                _context.DeveloperProjects.Remove(devProject);
                await _context.SaveChangesAsync();

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
