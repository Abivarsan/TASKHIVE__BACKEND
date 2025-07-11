using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;

namespace TASKHIVE.Controllers.TaskControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskDetailsViewController : ControllerBase
    {
        public readonly DataContext _context;

        public TaskDetailsViewController(DataContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTaskDetails(int Tid)
        {
            var AddedTask = await _context.Tasks
                .Where(x => x.TaskId == Tid)
                .Select(x => new TaskDetailsViewDTO
                {
                    TaskId = x.TaskId,
                    TaskName = x.TaskName,
                    TaskDescription = x.TaskDescription,
                    TaskStatus = x.TaskStatus,
                    TimeDuration = x.TimeDuration,
                    Technologies = x.Technology,
                    CreatedDate = x.CreatedDate,
                    DueDate = x.DueDate,
                    Dependancies = x.Dependancy,
                    Priority = x.Priority
                }
                ).ToListAsync();

            return Ok(AddedTask);
        }
    }
}
