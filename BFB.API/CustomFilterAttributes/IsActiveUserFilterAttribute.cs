using BFB.Core.DTOs;
using BFB.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedLib.DTOs;

namespace BFB.AuthApi.CustomFilterAttributes
{
	public class IsActiveUserFilterAttribute : IAsyncActionFilter
	{
		private readonly UserManager<AppUser> _userManager;

		public IsActiveUserFilterAttribute(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (context.ActionArguments.TryGetValue("SignInDto", out var signInDtoObject) && signInDtoObject is SignInDto signInDto)
			{
				var user = await _userManager.FindByEmailAsync(signInDto.Email);
				if (user != null && !user!.IsActive)
				{
					var result = new ObjectResult(Response<NoDataDto>.Fail("Hesabınız Aktif Değil.", 403, true)) { StatusCode = 403 };
					context.Result = result;
					return;
				}
			}
			await next.Invoke();
			return;
		}
	}
}
