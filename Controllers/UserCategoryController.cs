using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TASKHIVE.Data;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCategoryController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public UserCategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: api/UserCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCategory>>> GetAll()
        {
            return await _dataContext.UsersCategories.ToListAsync();
        }

        // POST: api/UserCategory
        [HttpPost]
        public async Task<ActionResult<UserCategory>> Post(UserCategory category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.UserCategoryType))
                return BadRequest("Invalid data.");

            _dataContext.UsersCategories.Add(category);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = category.UserCategoryId }, category);
        }

        // DELETE: api/UserCategory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _dataContext.UsersCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"UserCategory with ID {id} not found.");
            }

            _dataContext.UsersCategories.Remove(category);
            await _dataContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
