namespace STB.Experiment.Domain.Models.Responses
{
	public class AuthResult
	{
		public bool IsSuccess { get; set; }
		public string? ErrorMessage { get; set; }
		public LoginResponse? LoginResponse { get; set; }

		public AuthResult(bool success, string? errorMessage, LoginResponse? loginResponse)
		{
			IsSuccess = success;
			ErrorMessage = errorMessage;
			LoginResponse = loginResponse;
		}

		public static AuthResult Fail(string errorMessage) => new(false, errorMessage, null);
		public static AuthResult Success(LoginResponse loginResponse) => new(true, null, loginResponse);
	}
	public class LoginResponse
	{
		public string Token { get; set; }
		public int Expires { get; set; }
		public string RefreshToken { get; set; }
		public bool MfaRequired { get; set; } = false;
	}
}