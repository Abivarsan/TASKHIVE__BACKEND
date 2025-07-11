using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ClientController(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;

        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> RegisterClient(ClientRegisterDto request)
        {
            var randomPassword = CreateRandomPassword(10);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword);

            var existingClient = _dataContext.Clients.FirstOrDefault(c => c.UserName == request.UserName);
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

            _dataContext.Clients.Add(newClient);
            await _dataContext.SaveChangesAsync();
            return randomPassword;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ViewClientDetailDto>> GetById(int id)
        {
            var client = await _dataContext.Clients.FirstOrDefaultAsync(u => u.ClientId == id);

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

        [HttpGet("list")]
        public ActionResult<IEnumerable<ViewClientListDto>> GetAll()
        {
            var clients = _dataContext.Clients.ToList();

            // Map each user to ViewUserListDto
            var viewClientListDtos = clients.Select(client => new ViewClientListDto
            {
                ClientId = client.ClientId,
                UserName = client.UserName,
                Email = client.Email
            }).ToList();

            return Ok(viewClientListDtos);
        }

        [HttpPost("deactivate-client")]
        public async Task<IActionResult> DeactivateClient([FromBody] DeactivateClientDto request)
        {
            // Fetch the user from the database using the username
            var client = _dataContext.Clients.FirstOrDefault(u => u.ClientId == request.ClientId);

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
            _dataContext.Clients.Update(client);
            await _dataContext.SaveChangesAsync();

            return Ok(new { message = "Client successfully deactivate.!" });
        }

        [HttpPost("reactivate-client")]
        public async Task<IActionResult> ReactivateUser([FromBody] ReactivateClientDto request)
        {
            // Fetch the user from the database using the username
            var client = _dataContext.Clients.FirstOrDefault(u => u.ClientId == request.ClientId);

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
            _dataContext.Clients.Update(client);
            await _dataContext.SaveChangesAsync();

            return Ok(new { message = "User successfully reactivate.!" });
        }

        [HttpGet("search")]
        public ActionResult<List<ViewClientListDto>> SearchUsers([FromQuery] string term)
        {
            // If the term is null, empty, or whitespace, return a bad request
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest(new { message = "Search term cannot be empty" });
            }

            // Trim and convert term to lowercase for case-insensitive comparison
            term = term.Trim().ToLower();

            var clients = _dataContext.Clients
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
    }
}