namespace STB.Experiment.Domain.Models.Database
{
	public class RefreshToken
	{
		public string Id { get; set; }
		public Guid UserId { get; set; }
		public DateTime	Expires { get; set; }
		public bool IsRevoked { get; set; }

		// Relations
		public User User { get; set; }
	}
}