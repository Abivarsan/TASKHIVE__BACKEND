using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MimeKit;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X9;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(DataContext dataContext, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            this._hostEnvironment = hostEnvironment;
        }

        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] UserUpdateDto request)
        {
            var user = await _dataContext.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Update user fields
            user.Email = request.Email;
            user.ContactNumber = request.ContactNumber;
            user.Address = request.Address;

            // Update profile image URL if provided
            if (!string.IsNullOrEmpty(request.ProfileImageUrl))
            {
                user.ProfileImageUrl = request.ProfileImageUrl;
            }

            _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully" });
        }

        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UserRoleUpdateDto model)
        {
            var user = await _dataContext.Users
                .Include(u => u.UserCategory)
                .Include(u => u.JobRole)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return NotFound(new { message = "User not found" });

            var oldRole = user.UserCategory.UserCategoryType;
            if (string.IsNullOrEmpty(oldRole))
                return BadRequest(new { message = "User has no assigned role" });

            // Update UserCategory
            var newUserCategory = await _dataContext.UsersCategories
                .FirstOrDefaultAsync(uc => uc.UserCategoryType == model.UserCategoryType);

            if (newUserCategory == null)
                return BadRequest(new { message = "Invalid role" });

            user.UserCategoryId = newUserCategory.UserCategoryId;
            _dataContext.Users.Update(user);

            // Update role-specific tables
            int userId = user.UserId;
            switch (oldRole)
            {
                case "ADMIN":
                    var oldAdmin = _dataContext.Admins.FirstOrDefault(a => a.AdminId == userId);
                    if (oldAdmin != null)
                    {
                        _dataContext.Admins.Remove(oldAdmin);
                    }
                    break;
                case "MANAGER":
                    var oldManager = _dataContext.ProjectManagers.FirstOrDefault(pm => pm.ProjectManagerId == userId);
                    if (oldManager != null)
                    {
                        _dataContext.ProjectManagers.Remove(oldManager);
                    }
                    break;
                case "DEVELOPER":
                    var oldDeveloper = _dataContext.Developers.FirstOrDefault(d => d.DeveloperId == userId);
                    if (oldDeveloper != null)
                    {
                        _dataContext.Developers.Remove(oldDeveloper);
                    }
                    break;
            }
            await _dataContext.SaveChangesAsync();

            switch (model.UserCategoryType)
            {
                case "ADMIN":
                    var newAdmin = new Admin { AdminId = userId };
                    _dataContext.Admins.Add(newAdmin);
                    break;
                case "MANAGER":
                    var newProjectManager = new ProjectManager { ProjectManagerId = userId };
                    _dataContext.ProjectManagers.Add(newProjectManager);
                    break;
                case "DEVELOPER":
                    var newDeveloper = new Developer { DeveloperId = userId, FinanceReceiptId = 1, TotalDeveloperWorkingHours = 0 };
                    _dataContext.Developers.Add(newDeveloper);
                    break;
            }

            await _dataContext.SaveChangesAsync();

            return Ok(new { message = "User role updated successfully" });
        }

        // GET: api/UserCategory
        [HttpGet("UserCategories")]
        public async Task<ActionResult<IEnumerable<UserCategory>>> GetUserCategoryAll()
        {
            return await _dataContext.UsersCategories.ToListAsync();
        }
        // POST: api/UserCategory
        [HttpPost("UserCategory")]
        public async Task<ActionResult<UserCategory>> Post(UserCategory category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.UserCategoryType))
                return BadRequest("Invalid data.");

            _dataContext.UsersCategories.Add(category);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserCategoryAll), new { id = category.UserCategoryId }, category);
        }

        // DELETE: api/UserCategory/{id}
        [HttpDelete("UserCategory/{id}")]
        public async Task<IActionResult> DeleteUserCategory(int id)
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

        // GET: api/JobRole
        [HttpGet("jobRoles")]
        public async Task<ActionResult<IEnumerable<JobRole>>> GetJobRoleAll()
        {
            return await _dataContext.JobRoles.ToListAsync();
        }

        // POST: api/JobRole
        [HttpPost("jobRole")]
        public async Task<ActionResult<JobRole>> Post(JobRole role)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.JobRoleType))
                return BadRequest("Invalid data.");

            _dataContext.JobRoles.Add(role);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJobRoleAll), new { id = role.JobRoleId }, role);
        }

        // DELETE: api/JobRole/{id}
        [HttpDelete("JobRole/{id}")]
        public async Task<IActionResult> DeleteJobRole(int id)
        {
            var role = await _dataContext.JobRoles.FindAsync(id);
            if (role == null)
            {
                return NotFound($"JobRole with ID {id} not found.");
            }

            _dataContext.JobRoles.Remove(role);
            await _dataContext.SaveChangesAsync();

            return NoContent();
        }
    }
}



