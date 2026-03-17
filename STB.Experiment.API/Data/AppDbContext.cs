using Microsoft.EntityFrameworkCore;
using STB.Experiment.Domain.Models.Database;

namespace STB.Experiment.API.Data
{
	public class AppDbContext : DbContext
	{
		private readonly IConfiguration configuration;

		public DbSet<User> Users { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
		{
			this.configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.UseNpgsql(configuration.GetConnectionString("STBExperimentDb"));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<RefreshToken>()
				.HasOne(rt => rt.User)
				.WithMany()
				.HasForeignKey(rt => rt.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}