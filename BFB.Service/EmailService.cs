using BFB.Core.Models;
using BFB.Core.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BFB.Service
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings _settings;

		public EmailService(IOptions<EmailSettings> settings)
		{
			_settings = settings.Value;
		}

		public async Task SendPwdResetEmail(string link, string toEmail)
		{
			var smtpClient = new SmtpClient();
			var mailMessage = new MailMessage();
			smtpClient.Host = _settings.Host;
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Port = 587;
			smtpClient.Credentials = new NetworkCredential(_settings.Email, _settings.Password);
			smtpClient.EnableSsl = true;

			mailMessage.From = new MailAddress(_settings.Email);
			mailMessage.To.Add(toEmail);

			mailMessage.Subject = "Bfb | Şife Sıfırlama Bağlantısı";
			mailMessage.Body = @$"Şifrenizi yenilemek için aşağıdaki linke tıklayınız.
                                      <p><a href='{link}'>TIKLA</a></p>.";
			mailMessage.IsBodyHtml = true;

			await smtpClient.SendMailAsync(mailMessage);

		}
	}
}
