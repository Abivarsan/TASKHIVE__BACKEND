using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateProjectController : ControllerBase
    {
        public readonly DataContext _context;

        public CreateProjectController(DataContext _context)
        {
            this._context = _context;
        }

        [HttpPost]      //  https://localhost:44339/api/CreateProject
        public async Task<ActionResult<List<Project>>> Create(ProjectDto request, int projectManagerId)
        {
            var client = await _context.Clients.FindAsync(request.ClientId);
            if (client == null)
            {
                return NotFound("Client not found");
            }

            var pManager = await _context.ProjectManagers.FindAsync(projectManagerId);
            if (pManager == null)
            {
                return NotFound("Project Manager not found");
            }

            var newProject = new Project
            {
                ProjectName = request.ProjectName,
                ProjectDescription = request.ProjectDescription,
                Technologies = request.Technologies,
                BudgetEstimation = request.BudgetEstimation,
                P_StartDate = request.P_StartDate,
                P_DueDate = request.P_DueDate,
                Duration = request.Duration,
                TeamName = request.TeamName,
                TimeLine = request.TimeLine,
                Objectives = request.Objectives,

                ProjectStatus = "New",

                Client = client,
                ProjectManager = pManager
            };

            _context.Add(newProject);
            await _context.SaveChangesAsync();

            return Ok(newProject);
        }



        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateProject(int id, UpdateProjectDTO request)
        {
            var projectToUpdate = await _context.Projects.FindAsync(id);

            if (projectToUpdate == null)
            {
                return NotFound();
            }
            if (request.ProjectName != null)
            {
                projectToUpdate.ProjectName = request.ProjectName;
            }

            if (request.ProjectDescription != null)
            {
                projectToUpdate.ProjectDescription = request.ProjectDescription;
            }
            if (request.Technologies != null)
            {
                projectToUpdate.Technologies = request.Technologies;
            }
            if (request.BudgetEstimation != null)
            {
                projectToUpdate.BudgetEstimation = request.BudgetEstimation;
            }
            if (request.P_StartDate != null)
            {
                projectToUpdate.P_StartDate = request.P_StartDate;
            }
            if (request.P_DueDate != null)
            {
                projectToUpdate.P_DueDate = request.P_DueDate;
            }
            if (request.Duration != null)
            {
                projectToUpdate.Duration = request.Duration;
            }
            if (request.TeamName != null)
            {
                projectToUpdate.TeamName = request.TeamName;
            }
            if (request.TimeLine != null)
            {
                projectToUpdate.TimeLine = request.TimeLine;
            }
            if (request.Objectives != null)
            {
                projectToUpdate.Objectives = request.Objectives;
            }


            _context.Update(projectToUpdate);
            await _context.SaveChangesAsync();

            return Ok(); 
        }

        public class UpdateProjectDTO
        {
            public string ProjectName { get; set; }
            public string? ProjectDescription { get; set; }
            public string? Technologies { get; set; }
            public string? BudgetEstimation { get; set; }
            public DateTime P_StartDate { get; set; }
            public DateTime P_DueDate { get; set; }
            public int? Duration { get; set; }
            public string? TeamName { get; set; }
            public string? TimeLine { get; set; }
            public string? Objectives { get; set; }

        }

        [HttpGet("GetDetails")]
        public async Task<ActionResult<IEnumerable<Project>>> GetDetails(int ProId)
        {
            var projectDetails = await _context.Projects
                .Where(x => x.ProjectId == ProId)
                .Select(x => new UpdateProjectDTO
                {
                    ProjectName = x.ProjectName,
                    ProjectDescription = x.ProjectDescription,
                    Technologies = x.Technologies,
                    BudgetEstimation = x.BudgetEstimation,
                    P_DueDate = x.P_DueDate,
                    P_StartDate = x.P_StartDate,
                    Duration = x.Duration,
                    TeamName = x.TeamName,
                    TimeLine = x.TimeLine,
                    Objectives = x.Objectives

                }).ToListAsync();

            return Ok(projectDetails);
        }


    }
}
