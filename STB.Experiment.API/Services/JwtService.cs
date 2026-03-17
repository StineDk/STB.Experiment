using Microsoft.IdentityModel.Tokens;
using STB.Experiment.API.Data;
using STB.Experiment.Domain.Models.Database;
using STB.Experiment.Domain.Models.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace STB.Experiment.API.Services
{
	public class JwtService(AppDbContext dbContext, JwtAuthenticationOptions jwtOptions)
	{
		public string GenerateJwtToken(User user)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim("Username", user.Username)
			};

			var token = new JwtSecurityToken(
				issuer: jwtOptions.Issuer,
				audience: jwtOptions.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(30),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<string> GenerateRefreshToken()
		{
			while(true)
			{
				var randomNumber = new byte[32];
				using (var rng = RandomNumberGenerator.Create())
				{
					rng.GetBytes(randomNumber);
					var refreshToken = Convert.ToBase64String(randomNumber);
					if (await dbContext.RefreshTokens.FindAsync(refreshToken) == null) return refreshToken;
				}
			}
		}
	}
}