using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;

namespace TASKHIVE.Controllers.ClientSideController
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetProjectDetailsController : ControllerBase
    {
        public readonly DataContext _context;

        public GetProjectDetailsController(DataContext _context)
        {
            this._context = _context;
        }

        public class ClientViewDTO
        {
            public int ProjectId { get; set; }
            public string ProjectName { get; set; }
            public string Description { get; set; }
        }

        [HttpGet("{para}")]
        public async Task<ActionResult<IEnumerable<ClientViewDTO>>> GetClientView(int para)
        {

            var SelectedProject = await _context.Projects
                .Where(e => e.ProjectId == para)
                .Select(e => new ClientViewDTO
                {
                    ProjectId = e.ProjectId,
                    ProjectName = e.ProjectName,
                    Description = e.ProjectDescription
                
                }).ToListAsync();

            if (SelectedProject == null)
            {
                return NotFound();
            }


            return SelectedProject;


        }
    }
}
