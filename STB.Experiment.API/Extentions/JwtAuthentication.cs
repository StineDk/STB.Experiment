using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using STB.Experiment.Domain.Models.Options;
using System.Runtime.CompilerServices;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class JwtAuthentication
	{
		public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtOptions = configuration.GetSection("Jwt").Get<JwtAuthenticationOptions>();
			if (jwtOptions == null) throw new InvalidOperationException("JWT configuration is missing.");
			services.AddSingleton(jwtOptions);

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtOptions.Issuer,
					ValidAudience = jwtOptions.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
				};
			});

			services.AddAuthorization();
		}
	}
}