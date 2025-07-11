using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.IO;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    public class DeveloperTeamController : ControllerBase
    {


        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DeveloperTeamController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetAllTeams/{developerid}")]
        public async Task<ActionResult<IEnumerable<Project>>> getTeams(int developerid)
        {

            var teams = await _context.DeveloperProjects
                .Where(e => e.DeveloperId == developerid)
                .Select(e => new GetTeamDTO
                {
                    ProjectId = e.ProjectId,
                    ProjectName = e.Project.ProjectName,
                    TeamName = e.Project.TeamName,
                    ProjectStatus = e.Project.ProjectStatus

                }).ToListAsync();

            if (teams == null)
            {
                Console.WriteLine("Projects are not added");
            }


            return Ok(teams);

        }




        [HttpGet("TeamDescription/{projectid}")]
        public async Task<ActionResult<IEnumerable<GetTeamDetailsDTO>>> GetTeamDetails(int projectid)
        {


            var developerIds = await _context.DeveloperProjects
           .Where(p => p.ProjectId == projectid)
           .Select(p => p.DeveloperId)
           .ToListAsync();

            var userIds = developerIds.Distinct();
            var userDetails = await _context.Users
                .Where(u => userIds.Contains(u.UserId))
                .ToListAsync();



            var teamDetails = userDetails.Select(u => new GetTeamDetailsDTO
            {

                UserId = u.UserId,
                DeveloperName = u.FirstName + " " + u.LastName,
                Email = u.Email,
                ContactNumber = u.ContactNumber

            });


            if (teamDetails == null)
            {
                Console.WriteLine("Team details does not exist");
            }



            return Ok(teamDetails);

        }




        [HttpGet("DeveloperDescription/{developerid}")]
        public async Task<ActionResult<IEnumerable<GetTeamDetailsDTO>>> GetDeveloperDetails(int developerid)
        {


            var developer = await _context.Users
                .Where(p => p.UserId == developerid)
                 .Select(p => p.FirstName + " " + p.LastName)
                .ToListAsync();





            if (developer == null)
            {
                Console.WriteLine("Developer details does not exist");
            }



            return Ok(developer);

        }



        [HttpGet]
        [Route("GetDevelopeTotalTaskCount/{developerid}")]
        public async Task<ActionResult<int>> GetDevelopeTotalTaskCount(int developerid)
        {
            try
            {
                var count = await _context.Tasks
                    .CountAsync(t => t.DeveloperId == developerid);
                return Ok(count);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }

        }


        [HttpGet]
        [Route("GetDevelopeTotalProjectCount/{developerid}")]
        public async Task<ActionResult<int>> GetDevelopeTotalProjectCount(int developerid)
        {
            try
            {
                var count = await _context.DeveloperProjects
                    .CountAsync(t => t.DeveloperId == developerid);
                return Ok(count);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }

        }









    }
}

