using BFB.AdminAPI.Controllers;
using BFB.Core;
using BFB.Core.DTOs;
using BFB.Core.Services;
using BFB.UserAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFB.UserAPI.Controllers
{
	public class MemberController : CustomControllerBase
	{
		private readonly IMemberService _service;

		public MemberController(IMemberService service)
		{
			_service = service;
		}

		[HttpPut]
		[Authorize]
		[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
		public async Task<IActionResult> Update(UpdateMemberDto updateMemberDto)
		{
			return ActionResultInstance(await _service.UpdateMemberDetailsAsync(updateMemberDto, User.Identity!.Name!));
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateProfile(CreateMemberProfileDto createMemberProfileDto)
		{
			return ActionResultInstance(await _service.CreateMemberProfileAsync(createMemberProfileDto, User.Identity!.Name!));
		}
		[HttpGet]
		[Route("/api/users/{id}")]
		public async Task<IActionResult> UserProfile(string id)
		{
			return ActionResultInstance(await _service.GetProfileDetailsAsync(id));
		}
		[HttpPost]
		public async Task<IActionResult> ForgotPassword(SendPasswordResetLinkDto dto)
		{
			return ActionResultInstance(await _service.SendPasswordResetLinkAsync(dto));
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
		{
			return ActionResultInstance(await _service.ResetPasswordAsync(dto));
		}
		[HttpPost]
		[Authorize]
		[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
		public async Task<IActionResult> Search(SearchDto dto)
		{
			return ActionResultInstance(await _service.SearchAsync(dto));
		}
	}
}
