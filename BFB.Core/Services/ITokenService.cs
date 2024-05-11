using BFB.Core.DTOs;
using BFB.Core.Entities;

namespace BFB.Core.Services
{
	public interface ITokenService
	{
		Task<TokenDto> CreateTokenAsync(AppUser user);
	}
}
