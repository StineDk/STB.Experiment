using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using STB.Experiment.API.Data;
using STB.Experiment.Domain.Models.Database;
using STB.Experiment.Domain.Models.Responses;

namespace STB.Experiment.API.Services
{
	public class AuthService
	{
		private readonly AppDbContext dbContext;
		private readonly JwtService jwtService;

		public AuthService(AppDbContext dbContext, JwtService jwtService)
		{
			this.dbContext = dbContext;
			this.jwtService = jwtService;
		}

		public async Task RegisterAsync(RegisterRequest requestDto)
		{
			if (requestDto == null) throw new ArgumentNullException(nameof(requestDto));
			if (string.IsNullOrEmpty(requestDto.Email)) throw new ArgumentNullException(nameof(requestDto.Email));
			if (string.IsNullOrEmpty(requestDto.Password)) throw new ArgumentNullException(nameof(requestDto.Password));

			var user = new User
			{
				Username = requestDto.Email,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(requestDto.Password)
			};

			await dbContext.Users.AddAsync(user);
			await dbContext.SaveChangesAsync();
		}

		public async Task<AuthResult> LoginAsync(LoginRequest requestDto)
		{
			if (requestDto == null) throw new ArgumentNullException(nameof(requestDto));
			if (string.IsNullOrEmpty(requestDto.Email)) throw new ArgumentNullException(nameof(requestDto.Email));
			if (string.IsNullOrEmpty(requestDto.Password)) throw new ArgumentNullException(nameof(requestDto.Password));

			var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == requestDto.Email);
			if (user == null) throw new KeyNotFoundException(nameof(user));
			if (!BCrypt.Net.BCrypt.Verify(requestDto.Password, user.PasswordHash)) return AuthResult.Fail("Invalid credentials.");

			var jwt = jwtService.GenerateJwtToken(user);
			var refreshToken = await jwtService.GenerateRefreshToken();
			dbContext.RefreshTokens.Add(new RefreshToken
			{
				Id = refreshToken,
				UserId = user.Id,
				Expires = DateTime.UtcNow.AddDays(7),
				IsRevoked = false
			});
			await dbContext.SaveChangesAsync();

			return AuthResult.Success(new LoginResponse
			{
				Token = jwt,
				Expires = 1800,
				RefreshToken = refreshToken
			});
		}
	}
}