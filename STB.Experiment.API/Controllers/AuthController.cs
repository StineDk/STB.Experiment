using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STB.Experiment.API.Data;
using STB.Experiment.API.Services;
using STB.Experiment.Domain.Models.Database;
using STB.Experiment.Domain.Models.Options;
using STB.Experiment.Domain.Models.Responses;

namespace STB.Experiment.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext dbContext;
		private readonly JwtAuthenticationOptions jwtOptions;
		private readonly JwtService jwtService;

		public AuthController(AppDbContext dbContext, JwtAuthenticationOptions jwtOptions, JwtService jwtService)
		{
			this.dbContext = dbContext;
			this.jwtOptions = jwtOptions;
			this.jwtService = jwtService;
		}

		[HttpPost("Register")]
		public async Task<ActionResult> RegisterAsync()
		{
			var user = new User
			{
				Username = "test",
				PasswordHash = BCrypt.Net.BCrypt.HashPassword("password")
			};

			await dbContext.Users.AddAsync(user);
			await dbContext.SaveChangesAsync();

			return Created();
		}

		[HttpPost("Login")]
		public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest requestDto)
		{
			if (requestDto == null) return BadRequest("Request body is required.");
			if (string.IsNullOrEmpty(requestDto.Email)) return BadRequest("Username is required.");
			if (string.IsNullOrEmpty(requestDto.Password)) return BadRequest("Password is required.");

			var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == requestDto.Email);
			if (user == null) return StatusCode(500);
			if (string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(requestDto.Password, user.PasswordHash)) return BadRequest("Bad password");

			var jwtToken = jwtService.GenerateJwtToken(user);
			var refreshToken = await jwtService.GenerateRefreshToken();
			dbContext.RefreshTokens.Add(new RefreshToken
			{
				Id = refreshToken,
				UserId = user.Id,
				Expires = DateTime.UtcNow.AddDays(7),
				IsRevoked = false
			});
			await dbContext.SaveChangesAsync();

			return Ok(new LoginResponse(jwtToken, 1800, refreshToken));
		}

		[HttpGet("LoggedIn"), Authorize]
		public async Task<ActionResult> LoggedIn()
		{
			return Ok("User is logged on.");
		}
	}
}