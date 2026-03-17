namespace STB.Experiment.Domain.Models.Options
{
	public class JwtAuthenticationOptions
	{
		public string SecretKey { get; set; } = null!;
		public string Issuer { get; set; } = null!;
		public string Audience { get; set; } = null!;
	}
}