using STB.Experiment.API.Data;
using STB.Experiment.API.Extentions;
using STB.Experiment.API.Services;

namespace STB.Experiment.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.AddServiceDefaults();

			// Add services to the container.

			builder.Services.AddDbContext<AppDbContext>();
			builder.Services.AddJwtAuthentication(builder.Configuration);
			builder.Services.AddScoped<JwtService>();
			builder.Services.AddScoped<AuthService>();

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();

			var app = builder.Build();

			app.MapDefaultEndpoints();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.ApplyMigrations();
			}

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
