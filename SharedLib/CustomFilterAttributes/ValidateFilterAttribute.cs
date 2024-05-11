using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedLib.DTOs;

namespace BFB.AuthApi.CustomFilterAttributes
{
	public class ValidateFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				var errorDescriptions = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

				context.Result = new BadRequestObjectResult(Response<ErrorDto>.Fail(new ErrorDto(errorDescriptions, true), 400));
				return;
			}
		}
	}
}
