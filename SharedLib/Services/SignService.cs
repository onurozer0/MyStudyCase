using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SharedLib.Services
{
	public static class SignService
	{
		public static SecurityKey GetSymmetricKey(string securityKey)
		{
			return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
		}
	}
}
