using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using System.Security.Cryptography;

namespace TASKHIVE.Controllers.TaskControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskFileDeleteController : ControllerBase
    {
        public readonly DataContext _context;

        public TaskFileDeleteController(DataContext _context)
        {
            this._context = _context;            
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFiles(int id)
        {
            try
            {
                var files = await _context.FileResources
                    .Where(e => e.TaskId == id)
                    .ToListAsync();
                
                for(int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    _context.FileResources.Remove(file);
                    await _context.SaveChangesAsync();
                }

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("deleteFile")]
        public async Task<IActionResult> DeleteFile(int fileId)
        {
            try
            {
                var file = await _context.FileResources.FindAsync(fileId);

                if (file == null)
                {
                    return NotFound();
                }

                _context.FileResources.Remove(file);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
