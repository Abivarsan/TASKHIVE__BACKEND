using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Controllers.Email;
using TASKHIVE.Data;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers.PManagerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PMCreateProjectController : ControllerBase
    {
        public readonly DataContext _context;
        private readonly IEmailService _emailService;

        public PMCreateProjectController(DataContext _context, IEmailService emailService)
        {
            this._context = _context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<ActionResult<List<Project>>> Create(ProjectDto request, int id)
        {

            

            var client = await _context.Clients.FindAsync(request.ClientId);
            if (client == null)
            {
                return NotFound();
            }

            var pManager = await _context.ProjectManagers.FindAsync(id);
            if (pManager == null)
            {
                return NotFound();
            }



            var newProject = new Project
            {
                //ProjectId = request.ProjectId,
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
                ProjectManager = pManager,
               
            };

            _context.Add(newProject);
            await _context.SaveChangesAsync();

            await _emailService.NotifyClientAsync(newProject.ClientId, newProject.ProjectId);


            return Ok();
        }
    }
}
