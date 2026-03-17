using Microsoft.EntityFrameworkCore;
using STB.Experiment.API.Data;

namespace STB.Experiment.API.Extentions
{
	public static class DatabaseExtentions
	{
		public static void ApplyMigrations(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			if (dbContext.Database.GetPendingMigrations().Any())
				dbContext.Database.Migrate();
		}
	}
}