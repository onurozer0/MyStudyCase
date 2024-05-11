using BFB.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedLib.DTOs;

namespace BFB.UserAPI.Filters
{
	public class IsCreatedProfileFilterAttribute : IAsyncActionFilter
	{
		private readonly UserManager<AppUser> _userManager;

		public IsCreatedProfileFilterAttribute(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (context.HttpContext.User.Identity.Name == null)
			{
				await next.Invoke();
				return;
			}
			var user = await _userManager.FindByNameAsync(context.HttpContext.User.Identity!.Name!);

			if (!user!.IsCreatedProfile)
			{
				var result = new BadRequestObjectResult(Response<NoDataDto>.Fail("Profilinizi oluşturmalısınız!", 403, true)) { StatusCode = 403 };
				context.Result = result;
				return;
			}
			await next.Invoke();
		}
	}
}
