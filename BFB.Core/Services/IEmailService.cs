namespace BFB.Core.Services
{
	public interface IEmailService
	{
		Task SendPwdResetEmail(string link, string toEmail);
	}
}
