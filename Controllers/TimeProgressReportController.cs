using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    public class TimeProgressReportController : ControllerBase
    {


        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TimeProgressReportController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        [HttpGet("ProjectProgressReportRemain/{projectid}")]

        public async Task<ActionResult<IEnumerable<Project>>> getDeveloperProjectProgressRemain(int projectid)
        {

            var remain = await _context.Projects
        .Where(e => e.ProjectId == projectid)
        .Select(e => e.TotalProjectRemainingHours)
        .FirstOrDefaultAsync();



            if (remain == null)
            {
                Console.WriteLine("Projects are not added");
            }


            return Ok(remain);

        }



        [HttpGet("ProjectProgressReportComplete/{projectid}")]

        public async Task<ActionResult<IEnumerable<Project>>> getDeveloperProjectProgressComplete(int projectid)
        {



            var remain = await _context.Projects
        .Where(e => e.ProjectId == projectid)
        .Select(e => e.TotalProjectCompletedHours)
        .FirstOrDefaultAsync();



            if (remain == null)
            {
                Console.WriteLine("Projects are not added");
            }


            return Ok(remain);

        }






    }
}

