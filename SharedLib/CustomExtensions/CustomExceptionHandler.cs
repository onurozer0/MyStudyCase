using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SharedLib.CustomExceptions;
using SharedLib.DTOs;
using System.Text.Json;

namespace SharedLib.CustomExtensions
{
	public static class CustomExceptionHandler
	{
		public static void UseCustomExceptionHandler(this IApplicationBuilder app)
		{
			app.UseExceptionHandler(cfg =>
			{
				cfg.Run(async context =>
				{
					context.Response.StatusCode = 500;
					context.Response.ContentType = "application/json";
					var errors = context.Features.Get<IExceptionHandlerFeature>();
					if (errors != null)
					{
						var exception = errors.Error;
						ErrorDto errorDto = null;
						if (exception is CustomException)
						{
							errorDto = new ErrorDto(exception.Message, true);
						}
						else
						{
							errorDto = new ErrorDto(exception.Message, false);
						}
						var response = Response<NoDataDto>.Fail(errorDto, 500);
						await context.Response.WriteAsync(JsonSerializer.Serialize(response));
					}
				});
			});
		}
	}
}
