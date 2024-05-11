using BFB.Core.DTOs;
using BFB.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFB.AuthApi.Controllers
{
	public class UserController : CustomControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}
		[HttpPost]
		public async Task<IActionResult> CreateUser(SignUpDto signUpDto)
		{
			return ActionResultInstance(await _userService.CreateUserAsync(signUpDto));
		}
		[HttpGet]
		public async Task<IActionResult> GetUser()
		{
			return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
		}
	}
}
