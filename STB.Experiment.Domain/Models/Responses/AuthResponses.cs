namespace STB.Experiment.Domain.Models.Responses
{
	public record LoginResponse(string? Token, int? Expires, string? RefreshToken, bool MfaRequired = false);
}