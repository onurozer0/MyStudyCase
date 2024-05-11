using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Services;
using BFB.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLib.DTOs;

namespace BFB.Service.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly ITokenService _tokenService;
		private readonly UserManager<AppUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IService<UserRefreshToken> _userRefreshTokenService;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthenticationService(ITokenService tokenService, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IService<UserRefreshToken> userRefreshTokenService, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor)
		{
			_tokenService = tokenService;
			_userManager = userManager;
			_unitOfWork = unitOfWork;
			_userRefreshTokenService = userRefreshTokenService;
			_signInManager = signInManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<Response<TokenDto>> CreateTokenAsync(SignInDto signInDto)
		{
			var user = await _userManager.FindByEmailAsync(signInDto.Email);
			if (user == null)
			{
				return Response<TokenDto>.Fail("Email or Password is wrong.", 400, true);
			}
			if (!user.IsActive)
			{
				return Response<TokenDto>.Fail("Email or Password is wrong.", 400, true);
			}
			var result = await _signInManager.PasswordSignInAsync(user, signInDto.Password, false, true);
			if (result.IsLockedOut)
			{
				return Response<TokenDto>.Fail("Account has been locked(15 minutes).", 400, true);
			}
			if (!result.Succeeded)
			{
				return Response<TokenDto>.Fail("Email or Password is wrong.", 400, true);
			}
			var token = await _tokenService.CreateTokenAsync(user);
			var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();
			if (userRefreshToken == null)
			{
				await _userRefreshTokenService.AddAsync(new UserRefreshToken
				{
					UserId = user.Id,
					Code = token.RefreshToken,
					ExpireDate = token.RefreshTokenExpiration
				});
			}
			else
			{
				userRefreshToken.Code = token.RefreshToken;
				userRefreshToken.ExpireDate = token.RefreshTokenExpiration;
			}
			var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
			user.LastLoginIp = ipAddress.ToString();
			user.LastLoginDate = DateTime.Now;
			await _unitOfWork.CommitAsync();
			return Response<TokenDto>.Success(token, 200);
		}

		public async Task<Response<TokenDto>> CreateTokenViaRefreshTokenAsync(string refreshToken)
		{
			var isTokenExist = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
			if (isTokenExist == null)
			{
				return Response<TokenDto>.Fail("RefreshToken not found.", 404, true);
			}
			var user = await _userManager.FindByIdAsync(isTokenExist.UserId);
			if (user == null)
			{
				return Response<TokenDto>.Fail("User not found.", 404, true);
			}
			var tokenDto = await _tokenService.CreateTokenAsync(user);

			isTokenExist.Code = tokenDto.RefreshToken;
			isTokenExist.ExpireDate = tokenDto.RefreshTokenExpiration;
			await _unitOfWork.CommitAsync();
			return Response<TokenDto>.Success(tokenDto, 200);
		}

		public async Task<Response<NoDataDto>> RevokeAsync(string refreshToken)
		{
			var isTokenExist = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
			if (isTokenExist == null)
			{
				return Response<NoDataDto>.Fail("RefreshToken not found.", 404, true);
			}
			await _userRefreshTokenService.RemoveAsync(isTokenExist);
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(200);
		}
	}
}
