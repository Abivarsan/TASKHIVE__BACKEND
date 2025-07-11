using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using TASKHIVE.DTOs;
using TASKHIVE.Models;
using TASKHIVE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, DataContext dataContext, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _dataContext = dataContext;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponseDto>> Login(UserLoginDto request)
        {
            // Retrieve user data from the database
            User user = _dataContext.Users.FirstOrDefault(u => u.UserName == request.UserName);

            // Check if the user exists
            if (user == null)
            {
                return BadRequest(new { message = "Enter valid User name." });
            }

            if(user.IsActive ==  false)
            {
                return BadRequest(new { message = "Hi! You cann't login to the System." });
            }

            // Check if the user's password hash is not null
            if (user.PasswordHash == null)
            {
                return BadRequest(new { message = "Password hash is missing." });
            }

            // Check if password matches
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest(new { message = "Incorrect password" });
            }

            // Generate JWT token
            string accessToken = GenerateAccessJwtToken(user);
            string refreshToken = GenerateRefreshToken();

            // Handle refresh token
            var userRefreshToken = _dataContext.RefreshTokens.FirstOrDefault(rt => rt.UserId == user.UserId);
            if (userRefreshToken == null)
            {
                userRefreshToken = new RefreshToken
                {
                    Token = refreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
                    UserId = user.UserId
                };
                _dataContext.RefreshTokens.Add(userRefreshToken);
            }
            else
            {
                userRefreshToken.Token = refreshToken;
                userRefreshToken.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            }

            _dataContext.SaveChanges();

            //Set the Last loginDate
            user.LastLoginDate = DateTime.Now;
            _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync();

            var response = new AuthenticationResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            

            return Ok(response);
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] UserLogoutDto request)
        {
            // Log the received token for debugging
            _logger.LogInformation("Received refresh token: {Token}", request.RefreshToken);

            // Find the refresh token entry
            var refreshTokenEntry = _dataContext.RefreshTokens.FirstOrDefault(rt => rt.Token == request.RefreshToken);

            if (refreshTokenEntry == null)
            {
                _logger.LogWarning("Invalid refresh token: {Token}", request.RefreshToken);
                return Unauthorized(new { message = "Invalid refresh token" });
            }

            // Remove the refresh token entry
            _dataContext.RefreshTokens.Remove(refreshTokenEntry);
            _dataContext.SaveChanges();

            _logger.LogInformation("Refresh token invalidated successfully: {Token}", request.RefreshToken);
            return Ok(new { message = "Logout successful" });
        }

        [HttpPost("refresh")]
        public ActionResult<AuthenticationResponseDto> Refresh(TokenRefreshRequestDto request)
        {
            var refreshTokenEntry = _dataContext.RefreshTokens.FirstOrDefault(rt => rt.Token == request.RefreshToken);

            if (refreshTokenEntry == null || refreshTokenEntry.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token" });
            }

            var user = _dataContext.Users.FirstOrDefault(u => u.UserId == refreshTokenEntry.UserId);

            string newAccessToken = GenerateAccessJwtToken(user);
            string newRefreshToken = GenerateRefreshToken();

            refreshTokenEntry.Token = newRefreshToken;
            refreshTokenEntry.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            _dataContext.SaveChanges();

            var response = new AuthenticationResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return Ok(response);
        }

        private string GenerateAccessJwtToken(User user)
        {
            var userCategory = GetUserCategoryById(user.UserCategoryId);
            List<Claim> claims = new List<Claim>
            {
                new Claim("UserID", user.UserId.ToString()),
                new Claim("UserName", user.UserName),
                new Claim("UserCategoryId", user.UserCategoryId.ToString()),
                new Claim("UserCategory", userCategory.UserCategoryType)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: creds
                 );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private UserCategory GetUserCategoryById(int userCategoryId)
        {
            return _dataContext.UsersCategories.FirstOrDefault(uc => uc.UserCategoryId == userCategoryId);
        }
    }
}
