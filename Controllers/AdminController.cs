using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASKHIVE.Data;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _datacontext;
        private readonly IMapper _mapper;

        public AdminController(DataContext datacontext, IMapper mapper)
        {
            _datacontext = datacontext;
            _mapper = mapper;
        }

        [HttpGet("Counts")]
        public async Task<ActionResult<List<GetDashboardDto>>> GetDashboard()
        {
            var totaladmins = await _datacontext.Admins.CountAsync();
            var totalmanagers = await _datacontext.ProjectManagers.CountAsync();
            var totaldevelopers = await _datacontext.Developers.CountAsync();
            var totalprojects = await _datacontext.Projects.CountAsync();

            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var totalIncome = await _datacontext.Transactions
                .Where(t => t.Type == "Income" && t.Date >= startOfMonth && t.Date <= endOfMonth)
            .SumAsync(t => t.Value);

            var totalExpense = await _datacontext.Transactions
                .Where(t => t.Type == "Expence" && t.Date >= startOfMonth && t.Date <= endOfMonth)
                .SumAsync(t => t.Value);
            var dashboarddata = new GetDashboardDto
            {
                TotalAdmins = totaladmins,
                TotalManagers = totalmanagers,
                TotalDevelopers = totaldevelopers,
                TotalProjects = totalprojects,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
            };
            return Ok(dashboarddata);
        }

        [HttpGet("{ClientId}")]
        public async Task<ActionResult<ViewClientDetailDto>> GetClientById(int ClientId)
        {
            var client = await _datacontext.Clients.FirstOrDefaultAsync(u => u.ClientId == ClientId);

            if (client == null)
            {
                return NotFound();
            }

            // Map the user data along with UserCategoryType and JobRoleType to ViewUserDetailDto
            var viewClientDto = new ViewClientDetailDto
            {
                ClientId = client.ClientId,
                UserName = client.UserName,
                ClientName = client.ClientName,
                Address = client.Address,
                NIC = client.NIC,
                IsActive = client.IsActive,
                ContactNumber = client.ContactNumber,
                Email = client.Email,
                ClientDescription = client.ClientDescription,
            };

            return Ok(viewClientDto);
        }

        [HttpGet("ClientList")]
        public ActionResult<IEnumerable<ViewClientListDto>> GetClientAll()
        {
            var clients = _datacontext.Clients.ToList();

            // Map each user to ViewUserListDto
            var viewClientListDtos = clients.Select(client => new ViewClientListDto
            {
                ClientId = client.ClientId,
                UserName = client.UserName,
                Email = client.Email
            }).ToList();

            return Ok(viewClientListDtos);
        }

        [HttpPost("ClientRegister")]
        public async Task<ActionResult<string>> RegisterClient(ClientRegisterDto request)
        {
            var randomPassword = CreateRandomPassword(10);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword);

            var existingClient = _datacontext.Clients.FirstOrDefault(c => c.UserName == request.UserName);
            if (existingClient != null)
            {

                return BadRequest(new { message = "ClientName already exists" });
            }

            Client newClient = new Client
            {
                UserName = request.UserName,
                PasswordHash = passwordHash,
                ClientName = request.ClientName,
                Address = request.Address,
                NIC = request.NIC,
                ContactNumber = request.ContactNumber,
                Email = request.Email,
                ClientDescription = request.ClientDescription,
                UserCategoryId = 4,

            };

            _datacontext.Clients.Add(newClient);
            await _datacontext.SaveChangesAsync();
            return randomPassword;
        }

        [HttpPost("Deactivate-Client")]
        public async Task<IActionResult> DeactivateClient([FromBody] DeactivateClientDto request)
        {
            // Fetch the user from the database using the username
            var client = _datacontext.Clients.FirstOrDefault(u => u.ClientId == request.ClientId);

            // Check if the user exists
            if (client == null)
            {
                return BadRequest(new { message = "Client not found." });
            }

            if (client.IsActive == false)
            {
                return BadRequest(new { message = "Client already deactivated from the system." });
            }

            client.IsActive = false;

            // Save the changes to the database
            _datacontext.Clients.Update(client);
            await _datacontext.SaveChangesAsync();

            return Ok(new { message = "Client successfully deactivate.!" });
        }

        [HttpPost("Reactivate-Client")]
        public async Task<IActionResult> ReactivateUser([FromBody] ReactivateClientDto request)
        {
            // Fetch the user from the database using the username
            var client = _datacontext.Clients.FirstOrDefault(u => u.ClientId == request.ClientId);

            // Check if the user exists
            if (client == null)
            {
                return BadRequest(new { message = "User not found." });
            }

            if (client.IsActive == true)
            {
                return BadRequest(new { message = "User already reactivated from the system." });
            }

            client.IsActive = true;

            // Save the changes to the database
            _datacontext.Clients.Update(client);
            await _datacontext.SaveChangesAsync();

            return Ok(new { message = "User successfully reactivate.!" });
        }

        [HttpGet("ClientSearch/{term}")]
        public ActionResult<List<ViewClientListDto>> SearchClients(string term)
        {
            // If the term is null, empty, or whitespace, return a bad request
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest(new { message = "Search term cannot be empty" });
            }

            // Trim and convert term to lowercase for case-insensitive comparison
            term = term.Trim().ToLower();

            var clients = _datacontext.Clients
                .Where(u => u.ClientId.ToString().ToLower().Contains(term) ||
                            u.UserName.ToLower().Contains(term))
                .Select(u => new ViewClientListDto
                {
                    ClientId = u.ClientId,
                    UserName = u.UserName,
                    Email = u.Email
                })
                .ToList();

            if (clients == null || clients.Count == 0)
            {
                return NotFound(new { message = "No users found" });
            }

            return Ok(clients);
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

        [HttpPost("UserRegister")]
        public async Task<ActionResult<string>> RegisterUser([FromBody] UserRegisterDto request)
        {
            var randomPassword = CreateRandomPassword(10);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword);

            var existingUser = _datacontext.Users.FirstOrDefault(u => u.UserName == request.UserName);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Username already exists" });
            }

            var userCategory = _datacontext.UsersCategories.FirstOrDefault(uc => uc.UserCategoryType == request.UserCategoryType);
            if (userCategory == null)
            {
                return BadRequest(new { message = "Invalid UserCategoryType" });
            }

            int UserCategoryId = userCategory.UserCategoryId;

            var jobRole = _datacontext.JobRoles.FirstOrDefault(jr => jr.JobRoleType == request.JobRoleType);
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

            _datacontext.Users.Add(newUser);
            await _datacontext.SaveChangesAsync();

            // Get the newly created user's UserId
            int newUserId = newUser.UserId;

            // Add the UserId to the relevant table based on UserCategoryType
            switch (request.UserCategoryType)
            {
                case "Admin":
                    var newAdmin = new Admin { AdminId = newUserId };
                    _datacontext.Admins.Add(newAdmin);
                    break;
                case "Manager":
                    var newProjectManager = new ProjectManager { ProjectManagerId = newUserId };
                    _datacontext.ProjectManagers.Add(newProjectManager);
                    break;
                case "Developer":
                    var newDeveloper = new Developer
                    {
                        DeveloperId = newUserId,
                        FinanceReceiptId = 1,
                        TotalDeveloperWorkingHours = 0
                    };
                    _datacontext.Developers.Add(newDeveloper);
                    break;
            }

            await _datacontext.SaveChangesAsync();
            return randomPassword;
        }

        [HttpGet("{UserId:int}")]
        public async Task<ActionResult<ViewUserDetailDto>> GetUserById(int UserId)
        {
            var user = await _datacontext.Users
                .Include(u => u.UserCategory)
                .Include(u => u.JobRole)
                .FirstOrDefaultAsync(u => u.UserId == UserId);

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

        [HttpGet("UserList")]
        public ActionResult<IEnumerable<ViewUserListDto>> GetUserAll()
        {
            var users = _datacontext.Users
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

        [HttpPost("Deactivate-User")]
        public async Task<IActionResult> DeactivateUser([FromBody] DeactivateUserDto request)
        {
            var user = await _datacontext.Users
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
                    var oldAdmin = _datacontext.Admins.FirstOrDefault(a => a.AdminId == user.UserId);
                    if (oldAdmin != null)
                    {
                        _datacontext.Admins.Remove(oldAdmin);

                    }
                    break;
                case "MANAGER":
                    var oldManager = _datacontext.ProjectManagers.FirstOrDefault(pm => pm.ProjectManagerId == user.UserId);
                    if (oldManager != null)
                    {
                        _datacontext.ProjectManagers.Remove(oldManager);

                    }
                    break;
                case "DEVELOPER":
                    var oldDeveloper = _datacontext.Developers.FirstOrDefault(d => d.DeveloperId == user.UserId);
                    if (oldDeveloper != null)
                    {
                        _datacontext.Developers.Remove(oldDeveloper);

                    }
                    break;
            }

            _datacontext.Users.Update(user);
            await _datacontext.SaveChangesAsync();

            Console.WriteLine($"User with ID {request.UserId} successfully deactivated.");
            return Ok(new { message = "User successfully deactivated." });
        }

        [HttpPost("Reactivate-User")]
        public async Task<IActionResult> ReactivateUser([FromBody] ReactivateUserDto request)
        {
            var user = await _datacontext.Users
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
                    _datacontext.Admins.Add(newAdmin);
                    break;
                case "MANAGER":
                    var newProjectManager = new ProjectManager { ProjectManagerId = user.UserId };
                    _datacontext.ProjectManagers.Add(newProjectManager);
                    break;
                case "DEVELOPER":
                    var newDeveloper = new Developer { DeveloperId = user.UserId, FinanceReceiptId = 1, TotalDeveloperWorkingHours = 0 };
                    _datacontext.Developers.Add(newDeveloper);
                    break;
            }

            _datacontext.Users.Update(user);
            await _datacontext.SaveChangesAsync();

            return Ok(new { message = "User successfully reactivated." });
        }

        [HttpGet("UserSearch/{term}")]
        public ActionResult<List<ViewUserListDto>> SearchUsers(string term)
        {
            // If the term is empty, return a bad request
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest(new { message = "Search term cannot be empty" });
            }

            // Convert term to lowercase for case-insensitive comparison
            term = term.ToLower();

            var users = _datacontext.Users
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


    }
}
