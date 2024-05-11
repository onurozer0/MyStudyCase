using BFB.AuthApi.CustomFilterAttributes;
using BFB.Core.DTOs;
using BFB.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFB.AuthApi.Controllers
{
	[ServiceFilter(typeof(IsActiveUserFilterAttribute))]
	public class AuthenticationController : CustomControllerBase
	{
		private readonly IAuthenticationService _authenticationService;

		public AuthenticationController(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}
		[HttpPost]
		public async Task<IActionResult> SignIn(SignInDto signInDto)
		{
			return ActionResultInstance(await _authenticationService.CreateTokenAsync(signInDto));
		}
		[HttpPost]
		public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
		{
			return ActionResultInstance(await _authenticationService.RevokeAsync(refreshTokenDto.RefreshToken));
		}
		[HttpPost]
		public async Task<IActionResult> CreateTokenViaRefreshToken(RefreshTokenDto refreshTokenDto)
		{
			return ActionResultInstance(await _authenticationService.CreateTokenViaRefreshTokenAsync(refreshTokenDto.RefreshToken));
		}
	}
}
