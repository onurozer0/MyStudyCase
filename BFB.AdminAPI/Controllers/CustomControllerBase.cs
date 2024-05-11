using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib.DTOs;

namespace BFB.AdminAPI.Controllers
{
	[Route("api/admin/[controller]/[action]")]
	[ApiController]
	[Authorize]
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
