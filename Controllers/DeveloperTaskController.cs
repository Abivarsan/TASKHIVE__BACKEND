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
using Task = TASKHIVE.Models.Task;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    public class DeveloperTaskController : ControllerBase
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DeveloperTaskController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        // GET: api/values
        [HttpGet]
        [Route("GetAllTasks/{para}")]
        public async Task<ActionResult<IEnumerable<GetTaskDTO>>> GetDeveloperTasks(int para)
        {
            var tasks = await _context.Tasks.
                Where(e => e.DeveloperId == para)
                .Select(e => new GetTaskDTO
                {
                    TaskId = e.TaskId,
                    TaskName = e.TaskName,
                    DueDate = e.DueDate,
                    TaskStatus = e.TaskStatus
                }).ToListAsync();

            if (tasks == null)
            {
                throw new Exception("Not Found");
            }

            //return var projects to ProjectDTO class
            return _mapper.Map<List<GetTaskDTO>>(tasks);
        }




        [HttpGet("TaskDescription/{taskid}")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTaskDetails(int taskid)
        {

            var projectId = await _context.Tasks
           .Where(p => p.TaskId == taskid)
           .Select(p => p.ProjectId)
           .FirstOrDefaultAsync();


            var projectName = await _context.Projects
                .Where(e => e.ProjectId == projectId)
                .Select(p => p.ProjectName)
            .FirstOrDefaultAsync();



            var task = await _context.Tasks
                .Where(e => e.TaskId == taskid)
                .Select(e => new TaskDescriptionDTO
                {
                    ProjectId = e.ProjectId,
                    ProjectName = projectName,
                    TaskName = e.TaskName,
                    TaskId = e.TaskId,
                    TaskStatus = e.TaskStatus,
                    TaskDescription = e.TaskDescription,
                    Priority = e.Priority,
                    Technology = e.Technology,
                    Dependancy = e.Dependancy,
                    CreatedDate = e.CreatedDate,
                    TimeDuration = e.TimeDuration,
                    DueDate = e.DueDate

                }).ToListAsync();

            if (task == null)
            {
                Console.WriteLine("Task details does not exist");
            }

            return Ok(task);
        }

    }
}

