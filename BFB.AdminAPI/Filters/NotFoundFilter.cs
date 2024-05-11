using BFB.Core.Entities.BaseEntities;
using BFB.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedLib.DTOs;

namespace BFB.AdminAPI.Filters
{
	public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
	{
		private readonly IService<T> _service;

		public NotFoundFilter(IService<T> service)
		{
			_service = service;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var idValue = context.ActionArguments.Values.FirstOrDefault();
			if (idValue == null)
			{
				await next.Invoke();
				return;
			}
			var id = new Guid();
			try
			{
				id = Guid.Parse(idValue.ToString()!);
			}
			catch (Exception)
			{
				await next.Invoke();
				return;
			}
			var anyEntity = await _service.AnyAsync(x => x.Id == id);
			if (anyEntity)
			{
				await next.Invoke();
				return;
			}
			context.Result = new NotFoundObjectResult(Response<NoDataDto>.Fail($"{typeof(T).Name} not found.", 404, true));
		}
	}
}
