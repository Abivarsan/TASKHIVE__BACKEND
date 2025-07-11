using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAccountController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public ClientAccountController(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }



        [HttpPost("password-forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] PasswordForgotDto request)
        {
            // Fetch the user from the database using the username
            var client = _dataContext.Clients.FirstOrDefault(u => u.UserName == request.UserName);

            // Check if the user exists
            if (client == null)
            {
                return BadRequest(new { message = "User not found." });
            }

            var randomPassword = CreateRandomPassword(10);
            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword);

            // Update the existing user's password
            client.PasswordHash = newPasswordHash;

            // Save the changes to the database
            _dataContext.Clients.Update(client);
            await _dataContext.SaveChangesAsync();

            return Ok(randomPassword);
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