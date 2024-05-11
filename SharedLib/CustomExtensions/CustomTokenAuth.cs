using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SharedLib.Configuration;
using SharedLib.Services;

namespace SharedLib.CustomExtensions
{
	public static class CustomTokenAuth
	{
		public static void AddCustomTokenAuth(this IServiceCollection services, CustomTokenOptions tokenOpt)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
			{
				opts.TokenValidationParameters = new()
				{
					ValidIssuer = tokenOpt.Issuer,
					ValidAudience = tokenOpt.Audiences[0],
					IssuerSigningKey = SignService.GetSymmetricKey(tokenOpt.SecurityKey),
					ValidateIssuerSigningKey = true,
					ValidateAudience = true,
					ValidateIssuer = true,
					ValidateLifetime = true,

					ClockSkew = TimeSpan.Zero
				};
			});
		}
	}
}
