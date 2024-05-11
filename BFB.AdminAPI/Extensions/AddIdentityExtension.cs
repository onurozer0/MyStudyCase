using BFB.Core.Entities;
using BFB.Repository;
using BFB.Service.Localization;
using BFB.Service.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;

namespace BFB.AdminAPI.Extensions
{
	public static class AddIdentityExtension
	{
		public static void AddIdentityViaExtension(this IServiceCollection services)
		{
			services.AddIdentity<AppUser, AppRole>(x =>
			{
				x.User.RequireUniqueEmail = true;
				x.Password.RequireNonAlphanumeric = true;
				x.Password.RequireDigit = true;
				x.Password.RequiredLength = 8;
				x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
				x.Lockout.MaxFailedAccessAttempts = 5;
			}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders().AddErrorDescriber<LocalizationIdentityErrorDescriber>();
			services.AddFluentValidationClientsideAdapters();
			services.AddFluentValidationAutoValidation();
			services.AddValidatorsFromAssemblyContaining<SignUpDtoValidator>();
			services.Configure<SecurityStampValidatorOptions>(opt =>
			{
				opt.ValidationInterval = TimeSpan.FromMinutes(30);
			});
		}
	}
}
