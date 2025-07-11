using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    public class DeveloperTimeController : ControllerBase
    {


        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public DeveloperTimeController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpPost("taskTimes/{taskid}/{developerid}")]

        public async Task<IActionResult> CreateTaskTime(int taskid, int developerid, [FromBody] CreateTaskTimeDTO taskTimeData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {


                var newTaskTime = new TaskTime
                {
                    DeveloperId = developerid,
                    TaskId = taskid,
                    TaskTimeStartTime = taskTimeData.TaskTimeStartTime,
                    TaskTimeCompleteTime = taskTimeData.TaskTimeCompleteTime,
                    TotalTimeTaskTimeDuration = (int)(taskTimeData.TaskTimeCompleteTime - taskTimeData.TaskTimeStartTime).TotalMinutes,

                };



                _context.TaskTimes.Add(newTaskTime);
                await _context.SaveChangesAsync();


                //CalculateAndUpdateTaskTimeDuration(taskid, projectid);

                return Created("TaskTime", newTaskTime);

            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred while saving task time.");
            }
        }









        [HttpPut("tasks/{taskid}")]
        public async Task<ActionResult<IEnumerable<UpdateTaskDTO>>> UpdateTask(int taskid, [FromBody] UpdateTaskDTO updateTask)
        {

            try
            {
                var taskToUpdate = await _context.Tasks.FirstOrDefaultAsync(p => p.TaskId == taskid);

                if (taskToUpdate == null)
                {
                    return NotFound();
                }

                var currentTime = await _context.TaskTimes
                .Where(i => i.TaskId == taskid)
                .SumAsync(i => i.TotalTimeTaskTimeDuration);





                taskToUpdate.TotalTaskTimeDuration = currentTime;


                _context.Update(taskToUpdate);
                _context.SaveChanges();

                return Ok(taskToUpdate.TotalTaskTimeDuration);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred while saving task time.");
            }
        }





        [HttpPut("projects/{taskid}")]
        public async Task<ActionResult<IEnumerable<UpdateProjectTimeDTO>>> UpdateProjectTime(int taskid, [FromBody] UpdateProjectTimeDTO updateProjectTime)
        {

            try
            {
                var taskToUpdate = await _context.Tasks.FirstOrDefaultAsync(p => p.TaskId == taskid);

                if (taskToUpdate == null)
                {
                    return NotFound();
                }

                var projectid = taskToUpdate.ProjectId;

                var projectToUpdate = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectid);

                if (projectToUpdate == null)
                {
                    return NotFound();
                }

                var currentTotalTime = await _context.Tasks
                .Where(i => i.ProjectId == projectid)
                .SumAsync(i => i.TotalTaskTimeDuration);





                projectToUpdate.TotalProjectCompletedHours = currentTotalTime;


                _context.Update(projectToUpdate);
                _context.SaveChanges();

                return Ok(projectToUpdate.TotalProjectCompletedHours);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred while saving task time.");
            }
        }





        [HttpPut("tasksStatusStart/{taskid}")]
        public async Task<ActionResult<IEnumerable<UpdateTaskStatusDTO>>> UpdateTaskStatusStart(int taskid, [FromBody] UpdateTaskStatusDTO updateTask)
        {

            try
            {
                var statusToUpdate = await _context.Tasks.FirstOrDefaultAsync(p => p.TaskId == taskid);

                if (statusToUpdate == null)
                {
                    return NotFound();
                }


                statusToUpdate.TaskStatus = 2;


                _context.Update(statusToUpdate);
                _context.SaveChanges();

                return Ok(statusToUpdate.TaskStatus);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred while saving task time.");
            }
        }



















        [HttpPut("tasksStatusStop/{taskid}")]
        public async Task<ActionResult<IEnumerable<UpdateTaskStatusDTO>>> UpdateTaskStatusStop(int taskid, [FromBody] UpdateTaskStatusDTO updateTask)
        {

            try
            {
                var statusToUpdate = await _context.Tasks.FirstOrDefaultAsync(p => p.TaskId == taskid);

                if (statusToUpdate == null)
                {
                    return NotFound();
                }


                statusToUpdate.TaskStatus = 3;


                _context.Update(statusToUpdate);
                _context.SaveChanges();

                return Ok(statusToUpdate.TaskStatus);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred while saving task time.");
            }
        }










    }
}

