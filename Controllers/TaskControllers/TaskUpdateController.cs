using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;

namespace TASKHIVE.Controllers.TaskControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskUpdateController : ControllerBase
    {
        public readonly DataContext _context;

        public TaskUpdateController(DataContext _context)
        {
            this._context = _context;
        }
        
        public class UpdateTaskDTO
        {
            public string TaskName { get; set; }
            public string TaskDescription { get; set; }
            public string? Technology { get; set; }
            public string? Dependancy { get; set; }
            public int Priority { get; set; }

            public DateTime CreatedDate { get; set; }
            public DateTime DueDate { get; set; }
            public int TimeDuration { get; set; }

        }

        
        [HttpGet("GetDetails")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetDetails(int taskId)
        {
            var taskDetails = await _context.Tasks
                .Where(x => x.TaskId == taskId)
                .Select(x => new UpdateTaskDTO
                {
                    TaskName = x.TaskName,
                    TaskDescription = x.TaskDescription,
                    Technology = x.Technology,
                    Priority = x.Priority,
                    CreatedDate = x.CreatedDate,
                    DueDate = x.DueDate,
                    Dependancy = x.Dependancy

                }).ToListAsync();

            return Ok(taskDetails);
        }
        
        /*
        [HttpPut("taskUpdate")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDTO request)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.TaskName = request.TaskName;
            task.TaskDescription = request.TaskDescription;
            task.Technology = request.Technology;
            task.Dependancy = request.Dependancy;
            task.Priority = request.Priority;
            task.CreatedDate = request.CreatedDate;
            task.DueDate = request.DueDate;
            task.TimeDuration = request.TimeDuration;

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }
        */
    }
}
