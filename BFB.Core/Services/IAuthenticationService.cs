using BFB.Core.DTOs;
using SharedLib.DTOs;

namespace BFB.Core.Services
{
	public interface IAuthenticationService
	{
		Task<Response<TokenDto>> CreateTokenAsync(SignInDto signInDto);
		Task<Response<TokenDto>> CreateTokenViaRefreshTokenAsync(string refreshToken);
		Task<Response<NoDataDto>> RevokeAsync(string refreshToken);
	}
}
