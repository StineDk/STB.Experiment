using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using STB.Experiment.API.Data;
using STB.Experiment.API.Services;
using STB.Experiment.Domain.Models.Responses;

namespace STB.Experiment.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext dbContext;
		private readonly JwtService jwtService;
		private readonly AuthService authService;

		public AuthController(AppDbContext dbContext, JwtService jwtService, AuthService authService)
		{
			this.dbContext = dbContext;
			this.jwtService = jwtService;
			this.authService = authService;
		}

		[HttpPost("Register")]
		public async Task<ActionResult> RegisterAsync(RegisterRequest requestDto)
		{
			try
			{
				await authService.RegisterAsync(requestDto);
				return Created();
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest($"Missing object: {ex.Message}");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("Login")]
		public async Task<ActionResult<AuthResult>> LoginAsync(LoginRequest requestDto)
		{
			try
			{
				return await authService.LoginAsync(requestDto);
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest($"Missing object: {ex.Message}");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("LoggedIn"), Authorize]
		public async Task<ActionResult> LoggedIn()
		{
			return Ok("User is logged on.");
		}
	}
}