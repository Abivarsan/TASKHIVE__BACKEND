using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.VisualBasic;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;


namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    public class DeveloperProjectController : ControllerBase
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DeveloperProjectController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet("{developerid}")]
        public async Task<ActionResult<IEnumerable<Project>>> getDeveloperProjects(int developerid)
        {

            var projects = await _context.DeveloperProjects
                .Where(e => e.DeveloperId == developerid)
                .Select(e => new GetDeveloperProjectDTO
                {
                    ProjectId = e.ProjectId,
                    ProjectName = e.Project.ProjectName,
                    ProjectStatus = e.Project.ProjectStatus

                }).ToListAsync();

            if (projects == null)
            {
                Console.WriteLine("Projects are not added");
            }


            return Ok(projects);

        }






        [HttpGet("ProjectDescription/{projectid}")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectDetails(int projectid)
        {


            var projectManagerId = await _context.Projects
            .Where(p => p.ProjectId == projectid)
            .Select(p => p.ProjectManagerId)
            .FirstOrDefaultAsync();




            var projectmanagerfname = await _context.Users
                .Where(e => e.UserId == projectManagerId)
                .Select(p => p.FirstName)
            .FirstOrDefaultAsync();


            var projectmanagerlname = await _context.Users
                .Where(e => e.UserId == projectManagerId)
                .Select(p => p.LastName)
            .FirstOrDefaultAsync();


            if (projectmanagerfname == null && projectmanagerlname == null)
            {
                Console.WriteLine("Project Manager details does not exist");
            }



            var projects = await _context.Projects
                .Where(e => e.ProjectId == projectid)
                .Select(e => new ProjectDescriptionDTO
                {
                    ProjectId = e.ProjectId,
                    ProjectName = e.ProjectName,
                    ProjectDescription = e.ProjectDescription,
                    Objectives = e.Objectives,
                    ProjectManagerId = e.ProjectManagerId,
                    ProjectManagerName = projectmanagerfname + " " + projectmanagerlname,
                    P_StartDate = e.P_StartDate,
                    Duration = e.Duration,
                    P_DueDate = e.P_DueDate

                }).ToListAsync();


            if (projects == null)
            {
                Console.WriteLine("Project details does not exist");
            }


            return Ok(projects);
        }





    }

}
