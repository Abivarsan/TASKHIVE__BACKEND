using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    public class ModuleProgressReportController : ControllerBase
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ModuleProgressReportController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        [HttpGet]
        [Route("GetProjectRemainingTasks/{projectid}")]
        public async Task<ActionResult<int>> GetProjectRemainingTasks(int projectid)
        {
            try
            {
                var count = await _context.Tasks
                    .CountAsync(t => t.ProjectId == projectid && t.TaskStatus == 1);
                return Ok(count);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet]
        [Route("GetProjectInProgressTasks/{projectid}")]
        public async Task<ActionResult<int>> GetProjectInProgressTasks(int projectid)
        {
            try
            {
                var count = await _context.Tasks
                    .CountAsync(t => t.ProjectId == projectid && t.TaskStatus == 2);
                return Ok(count);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet]
        [Route("GetProjectCompletedTasks/{projectid}")]
        public async Task<ActionResult<int>> GetProjectCompletedTasks(int projectid)
        {
            try
            {
                var count = await _context.Tasks
                    .CountAsync(t => t.ProjectId == projectid && t.TaskStatus == 3);
                return Ok(count);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return StatusCode(500, "Internal server error");
            }

        }





    }
}

