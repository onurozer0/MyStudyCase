using Microsoft.AspNetCore.Mvc;
using SharedLib.DTOs;

namespace BFB.AuthApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CustomControllerBase : ControllerBase
	{
		public IActionResult ActionResultInstance<T>(Response<T> response) where T : class
		{
			return new ObjectResult(response)
			{
				StatusCode = response.StatusCode,
			};
		}
	}
}
