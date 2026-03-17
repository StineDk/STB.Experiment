using Microsoft.AspNetCore.Mvc;
using STB.Experiment.API.Data;
using STB.Experiment.Domain.Models.Database;
using STB.Experiment.Domain.Models.Options;
using System.Runtime.CompilerServices;

namespace STB.Experiment.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext dbContext;
		private readonly JwtAuthenticationOptions jwtOptions;

		public AuthController(AppDbContext dbContext, JwtAuthenticationOptions jwtOptions)
		{
			this.dbContext = dbContext;
			this.jwtOptions = jwtOptions;
		}

		[HttpGet]
		public async Task<ActionResult> Test()
		{
			return Ok(jwtOptions);
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
	}
}