using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewProjectListController : ControllerBase
    {
        public readonly DataContext _context;

        public ViewProjectListController(DataContext _context)
        {
            this._context = _context;
        }

        public class ViewProjctListDTO
        {
            public int proId { get; set; }
            public string projectName { get; set; }
            public string projectStatus { get; set; }

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetList()
        {
            var projects = await _context.Projects
                .Select(e => new ViewProjctListDTO
                {
                    proId = e.ProjectId,
                    projectName = e.ProjectName,
                    projectStatus = e.ProjectStatus
                })
                .ToListAsync();

            return Ok(projects);
        }

    }
}
