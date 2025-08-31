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

        

        [HttpPost("register")]
        public async Task<ActionResult<string>> RegisterUser([FromBody] UserRegisterDto request)
        {
            var randomPassword = CreateRandomPassword(10);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword);

            var existingUser = _dataContext.Users.FirstOrDefault(u => u.UserName == request.UserName);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Username already exists" });
            }

            var userCategory = _dataContext.UsersCategories.FirstOrDefault(uc => uc.UserCategoryType == request.UserCategoryType);
            if (userCategory == null)
            {
                return BadRequest(new { message = "Invalid UserCategoryType" });
            }

            int UserCategoryId = userCategory.UserCategoryId;

            var jobRole = _dataContext.JobRoles.FirstOrDefault(jr => jr.JobRoleType == request.JobRoleType);
            if (jobRole == null)
            {
                return BadRequest(new { message = "Invalid jobRoleType" });
            }

            int JobRoleId = jobRole.JobRoleId;

            User newUser = new User
            {
                UserName = request.UserName,
                PasswordHash = passwordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                Gender = request.Gender,
                NIC = request.NIC,
                DOB = request.DOB,
                ContactNumber = request.ContactNumber,
                Email = request.Email,
                JobRoleId = JobRoleId,
                UserCategoryId = UserCategoryId,
                ProfileImageUrl = request.ProfileImageUrl // Firebase URL
            };

            _dataContext.Users.Add(newUser);
            await _dataContext.SaveChangesAsync();

            // Get the newly created user's UserId
            int newUserId = newUser.UserId;

            // Add the UserId to the relevant table based on UserCategoryType
            switch (request.UserCategoryType)
            {
                case "Admin":
                    var newAdmin = new Admin { AdminId = newUserId };
                    _dataContext.Admins.Add(newAdmin);
                    break;
                case "Manager":
                    var newProjectManager = new ProjectManager { ProjectManagerId = newUserId };
                    _dataContext.ProjectManagers.Add(newProjectManager);
                    break;
                case "Developer":
                    var newDeveloper = new Developer
                    {
                        DeveloperId = newUserId,
                        FinanceReceiptId = 1,
                        TotalDeveloperWorkingHours = 0
                    };
                    _dataContext.Developers.Add(newDeveloper);
                    break;
            }

            await _dataContext.SaveChangesAsync();
            return randomPassword;
        }
        //public async Task<ActionResult<string>> RegisterUser([FromForm] UserRegisterDto request)
        //{
        //    var randomPassword = CreateRandomPassword(10);
        //    string passwordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword);

        //    var existingUser = _dataContext.Users.FirstOrDefault(u => u.UserName == request.UserName);
        //    if (existingUser != null)
        //    {
        //        return BadRequest(new { message = "Username already exists" });
        //    }

        //    var userCategory = _dataContext.UsersCategories.FirstOrDefault(uc => uc.UserCategoryType == request.UserCategoryType);
        //    if (userCategory == null)
        //    {
        //        return BadRequest(new { message = "Invalid UserCategoryType" });
        //    }

        //    int UserCategoryId = userCategory.UserCategoryId;

        //    var jobRole = _dataContext.JobRoles.FirstOrDefault(jr => jr.JobRoleType == request.JobRoleType);
        //    if (jobRole == null)
        //    {
        //        return BadRequest(new { message = "Invalid jobRoleType" });
        //    }

        //    int JobRoleId = jobRole.JobRoleId;

        //    string imageName = null;
        //    if (request.ImageFile != null)
        //    {
        //        imageName = await SaveImage(request.ImageFile);
        //    }

        //    User newUser = new User
        //    {
        //        UserName = request.UserName,
        //        PasswordHash = passwordHash,
        //        FirstName = request.FirstName,
        //        LastName = request.LastName,
        //        Address = request.Address,
        //        Gender = request.Gender,
        //        NIC = request.NIC,
        //        DOB = request.DOB,
        //        ContactNumber = request.ContactNumber,
        //        Email = request.Email,
        //        JobRoleId = JobRoleId,
        //        UserCategoryId = UserCategoryId,
        //        ProfileImageName = imageName

        //    };

        //    _dataContext.Users.Add(newUser);
        //    await _dataContext.SaveChangesAsync();

        //    // Get the newly created user's UserId
        //    int newUserId = newUser.UserId;

        //    // Add the UserId to the relevant table based on UserCategoryType
        //    switch (request.UserCategoryType)
        //    {
        //        case "Admin":
        //            var newAdmin = new Admin
        //            {
        //                AdminId = newUserId,

        //            };
        //            _dataContext.Admins.Add(newAdmin);
        //            break;

        //        case "Manager":
        //            var newProjectManager = new ProjectManager
        //            {
        //                ProjectManagerId = newUserId,

        //            };
        //            _dataContext.ProjectManagers.Add(newProjectManager);
        //            break;

        //        case "Developer":
        //            var newDeveloper = new Developer
        //            {
        //                DeveloperId = newUserId,
        //                FinanceReceiptId = 1,
        //                TotalDeveloperWorkingHours = 0,

        //            };
        //            _dataContext.Developers.Add(newDeveloper);
        //            break;

        //    }

        //    await _dataContext.SaveChangesAsync();

        //    return randomPassword;
        //}

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ViewUserDetailDto>> GetById(int id)
        {
            var user = await _dataContext.Users
                .Include(u => u.UserCategory)
                .Include(u => u.JobRole)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            // Map the user data along with UserCategoryType and JobRoleType to ViewUserDetailDto
            var viewUserDto = new ViewUserDetailDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Gender = user.Gender,
                NIC = user.NIC,
                DOB = user.DOB,
                ProfileImageUrl = user.ProfileImageUrl,
                IsActive = user.IsActive,
                ContactNumber = user.ContactNumber,
                Email = user.Email,
                UserCategoryType = user.UserCategory?.UserCategoryType,
                JobRoleType = user.JobRole?.JobRoleType
            };

            return Ok(viewUserDto);
        }

        [HttpGet("list")]
        public ActionResult<IEnumerable<ViewUserListDto>> GetAll()
        {
            var users = _dataContext.Users
                .Include(u => u.UserCategory)
                .ToList();

            // Map each user to ViewUserListDto
            var viewUserListDtos = users.Select(user => new ViewUserListDto
            {

                ProfileImageUrl = user.ProfileImageUrl,
                UserId = user.UserId,
                FirstName = user.FirstName,
                UserName = user.UserName,
                Email = user.Email,
                UserCategoryType = user.UserCategory != null ? user.UserCategory.UserCategoryType : null,
                IsActive = user.IsActive
            }).ToList();

            return Ok(viewUserListDtos);
        }

        [HttpPost("deactivate-user")]
        public async Task<IActionResult> DeactivateUser([FromBody] DeactivateUserDto request)
        {
            var user = await _dataContext.Users
                .Include(u => u.UserCategory)
                .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
            {
                Console.WriteLine($"User with ID {request.UserId} not found.");
                return BadRequest(new { message = "User not found." });
            }

            if (!user.IsActive)
            {
                Console.WriteLine($"User with ID {request.UserId} is already deactivated.");
                return BadRequest(new { message = "User already deactivated from the system." });
            }

            user.IsActive = false;

            // Remove user from relevant user category table
            switch (user.UserCategory.UserCategoryType)
            {
                case "ADMIN":
                    var oldAdmin = _dataContext.Admins.FirstOrDefault(a => a.AdminId == user.UserId);
                    if (oldAdmin != null)
                    {
                        _dataContext.Admins.Remove(oldAdmin);
                        
                    }
                    break;
                case "MANAGER":
                    var oldManager = _dataContext.ProjectManagers.FirstOrDefault(pm => pm.ProjectManagerId == user.UserId);
                    if (oldManager != null)
                    {
                        _dataContext.ProjectManagers.Remove(oldManager);
                        
                    }
                    break;
                case "DEVELOPER":
                    var oldDeveloper = _dataContext.Developers.FirstOrDefault(d => d.DeveloperId == user.UserId);
                    if (oldDeveloper != null)
                    {
                        _dataContext.Developers.Remove(oldDeveloper);
                        
                    }
                    break;
            }

            _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync();

            Console.WriteLine($"User with ID {request.UserId} successfully deactivated.");
            return Ok(new { message = "User successfully deactivated." });
        }


        [HttpPost("reactivate-user")]
        public async Task<IActionResult> ReactivateUser([FromBody] ReactivateUserDto request)
        {
            var user = await _dataContext.Users
                .Include(u => u.UserCategory)
                .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
            {
                return BadRequest(new { message = "User not found." });
            }

            if (user.IsActive)
            {
                return BadRequest(new { message = "User already reactivated from the system." });
            }

            user.IsActive = true;

            // Add user back to relevant user category table
            switch (user.UserCategory.UserCategoryType)
            {
                case "ADMIN":
                    var newAdmin = new Admin { AdminId = user.UserId };
                    _dataContext.Admins.Add(newAdmin);
                    break;
                case "MANAGER":
                    var newProjectManager = new ProjectManager { ProjectManagerId = user.UserId };
                    _dataContext.ProjectManagers.Add(newProjectManager);
                    break;
                case "DEVELOPER":
                    var newDeveloper = new Developer { DeveloperId = user.UserId, FinanceReceiptId = 1, TotalDeveloperWorkingHours = 0 };
                    _dataContext.Developers.Add(newDeveloper);
                    break;
            }

            _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync();

            return Ok(new { message = "User successfully reactivated." });
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

        [HttpGet("search")]
        public ActionResult<List<ViewUserListDto>> SearchUsers([FromQuery] string term)
        {
            // If the term is empty, return a bad request
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest(new { message = "Search term cannot be empty" });
            }

            // Convert term to lowercase for case-insensitive comparison
            term = term.ToLower();

            var users = _dataContext.Users
                .Where(u => u.UserId.ToString().ToLower().Contains(term) ||
                            u.FirstName.ToLower().Contains(term) ||
                            u.UserName.ToLower().Contains(term) ||
                            u.UserCategory.UserCategoryType.ToLower().Contains(term))
                .Select(u => new ViewUserListDto
                {
                   
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    UserName = u.UserName,
                    Email = u.Email,
                    UserCategoryType = u.UserCategory.UserCategoryType
                })
                .ToList();

            if (users == null || users.Count == 0)
            {
                return NotFound(new { message = "No users found" });
            }

            return Ok(users);
        }

        public static string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }





    }
}



