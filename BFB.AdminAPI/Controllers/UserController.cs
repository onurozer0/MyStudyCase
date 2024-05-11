using BFB.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFB.AdminAPI.Controllers
{
	public class UserController : CustomControllerBase
	{
		private readonly IMemberService _memberService;

		public UserController(IMemberService memberService)
		{
			_memberService = memberService;
		}
		[HttpGet]
		[Route("{userId}")]
		public async Task<IActionResult> Activate(string userId)
		{
			return ActionResultInstance(await _memberService.ActivateUserAsync(userId));
		}
		[HttpGet]
		[Route("{userId}")]
		public async Task<IActionResult> Deactivate(string userId)
		{
			return ActionResultInstance(await _memberService.DeactivateUserAsync(userId));
		}
		[HttpGet]
		public async Task<IActionResult> All()
		{
			return ActionResultInstance(await _memberService.GetAllUsersAsync());
		}
		[HttpDelete]
		[Route("{userId}")]
		public async Task<IActionResult> Delete(string userId)
		{
			return ActionResultInstance(await _memberService.DeleteUserAsync(userId));
		}
		[HttpGet]
		[Route("{userId}")]
		public async Task<IActionResult> Confirm(string userId)
		{
			return ActionResultInstance(await _memberService.ConfirmUserAsync(userId));
		}
		[HttpGet]
		public async Task<IActionResult> Confirmed()
		{
			return ActionResultInstance(await _memberService.GetConfirmedUsersAsync());
		}
		[HttpGet]
		public async Task<IActionResult> Unconfirmed()
		{
			return ActionResultInstance(await _memberService.GetUnconfirmedUsersAsync());
		}
	}
}
