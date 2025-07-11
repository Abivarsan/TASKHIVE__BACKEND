using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASKHIVE.Data;
using TASKHIVE.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TASKHIVE.Controllers
{


    [Route("api/[controller]")]
    public class DeveloperTaskProgressReportController : Controller
    {


        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DeveloperTaskProgressReportController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        [HttpGet]
        [Route("GetRemainingTasks/{developerid}")]
        public async Task<ActionResult<int>> GetDeveloperRemainingTasks(int developerid)
        {
            try
            {
                var count = await _context.Tasks
                    .CountAsync(t => t.DeveloperId == developerid && t.TaskStatus == 1);
                return Ok(count);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet]
        [Route("GetInProgressTasks/{developerid}")]
        public async Task<ActionResult<int>> GetDeveloperInProgressTasks(int developerid)
        {
            try
            {
                var count = await _context.Tasks
                    .CountAsync(t => t.DeveloperId == developerid && t.TaskStatus == 2);
                return Ok(count);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet]
        [Route("GetCompletedTasks/{developerid}")]
        public async Task<ActionResult<int>> GetDeveloperCompletedTasks(int developerid)
        {
            try
            {
                var count = await _context.Tasks
                    .CountAsync(t => t.DeveloperId == developerid && t.TaskStatus == 3);
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

