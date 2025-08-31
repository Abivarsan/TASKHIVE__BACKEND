using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;

namespace TASKHIVE.Controllers.TaskControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskListController : ControllerBase
    {
        public readonly DataContext _context;

        public TaskListController(DataContext _context)
        {
            this._context = _context;
        }

        public class TaskListDTO
        {
            public int TaskId { get; set; }
            public string TaskName { get; set; }
            public int TaskStatus { get; set; }

            public int TimeDuration { get; set; }
            public string Priority { get; set; }

            public string DeveloperLName { get; set; }
            public string DeveloperFName { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTaskList(int ProId, int DevId)
        {
            var tasks = await _context.Tasks
                .Where(x => x.ProjectId == ProId && x.Developer.DeveloperId == DevId)
                .Select(x => new TaskListDTO
                {
                    TaskId = x.TaskId,
                    TaskName = x.TaskName,
                    TaskStatus = x.TaskStatus,
                    TimeDuration = x.TimeDuration,
                    Priority = x.Priority,
                    DeveloperFName = x.Developer.User.FirstName,
                    DeveloperLName = x.Developer.User.LastName

                }).ToListAsync();

            return Ok(tasks);
        }
    }
}
