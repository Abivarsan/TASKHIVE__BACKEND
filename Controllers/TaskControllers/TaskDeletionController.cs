using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;

namespace TASKHIVE.Controllers.TaskControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskDeletionController : ControllerBase
    {
        public readonly DataContext _context;

        public TaskDeletionController(DataContext _context)
        {
            this._context = _context;    
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int TId)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(TId);
                if (task == null)
                {
                    return NotFound();
                }

                if(task.TaskStatus == 1)
                {
                    return Ok("Task is ongoing");
                }
                else
                {
                    _context.Tasks.Remove(task);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
