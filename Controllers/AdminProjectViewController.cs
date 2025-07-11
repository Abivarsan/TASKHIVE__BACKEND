using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminProjectViewController : ControllerBase
    {
        public readonly DataContext _context;

        public AdminProjectViewController(DataContext _context)
        {
            this._context = _context;
        }


        public class ClickedId
        {
            public int SelectedId { get; set; }
        }

        public readonly ClickedId c1;

        int SelectedId;

        [HttpPost]
        public IActionResult Getid([FromBody] int value)
        {
            return Ok(value);

        }



        [HttpGet("{para}")]
        public async Task<ActionResult<IEnumerable<AdminViewDTO>>> GetAdminView(int para)
        {


            // int SelectedProId = c1;

            var SelectedProject = await _context.Projects
                .Where(e => e.ProjectId == para)
                .Select(e => new AdminViewDTO
                {
                    ProjectId = e.ProjectId,
                    ProjectName = e.ProjectName,
                    ProjectDescription = e.ProjectDescription,
                    Technologies = e.Technologies,
                    BudgetEstimation = e.BudgetEstimation,
                    P_StartDate = e.P_StartDate,
                    P_DueDate = e.P_DueDate,
                    Duration = e.Duration,
                    TeamName = e.TeamName,
                    TimeLine = e.TimeLine,
                    Objectives = e.Objectives,
                    ClientId = e.ClientId,
                    ProjectManagerFName = e.ProjectManager.User.FirstName,
                    ProjectManagerLName = e.ProjectManager.User.LastName,
                    ProjectManagerId = e.ProjectManagerId
                }).ToListAsync();

            if (SelectedProject == null)
            {
                return NotFound();
            }


            return SelectedProject;


        }

    }
}
