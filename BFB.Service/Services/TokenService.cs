using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Configuration;
using SharedLib.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BFB.Service.Services
{
	public class TokenService : ITokenService
	{
		private readonly CustomTokenOptions _customTokenOptions;
		private readonly UserManager<AppUser> _userManager;

		public TokenService(IOptions<CustomTokenOptions> customTokenOptions, UserManager<AppUser> userManager)
		{
			_customTokenOptions = customTokenOptions.Value;
			_userManager = userManager;
		}

		public async Task<TokenDto> CreateTokenAsync(AppUser user)
		{
			var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);
			var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.RefreshTokenExpiration);
			var securityKey = SignService.GetSymmetricKey(_customTokenOptions.SecurityKey);

			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
			var roles = await _userManager.GetRolesAsync(user);
			JwtSecurityToken jwtSecurityToken = new(
				issuer: _customTokenOptions.Issuer,
				expires: accessTokenExpiration,
				notBefore: DateTime.Now,
				claims: GetClaims(user, _customTokenOptions.Audiences, roles.ToList()),
				signingCredentials: signingCredentials);
			var handler = new JwtSecurityTokenHandler();
			var token = handler.WriteToken(jwtSecurityToken);
			var tokenDto = new TokenDto()
			{
				AccessToken = token,
				AccessTokenExpiration = accessTokenExpiration,
				RefreshToken = CreateRefreshToken(),
				RefreshTokenExpiration = refreshTokenExpiration
			};
			return tokenDto;
		}
		private string CreateRefreshToken()
		{
			var numberByte = new byte[32];
			var rnd = RandomNumberGenerator.Create();
			rnd.GetBytes(numberByte);
			return Convert.ToBase64String(numberByte);
		}
		private IEnumerable<Claim> GetClaims(AppUser user, List<String> audiences, List<string> userRoles)
		{
			if (audiences.Count == 2)
			{
				audiences.Add("www.adminapi.com");
			}
			// audiences [0]"www.authapi.com" ,[1]"www.userapi.com",[2]"www.adminapi.com"
			var userClaimsList = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier,user.Id),
				new Claim(JwtRegisteredClaimNames.Email,user.Email),
				new Claim(ClaimTypes.Name,user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
			};
			if (!userRoles.Contains("Admin"))
			{
				audiences.RemoveAt(2);
			}
			userClaimsList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
			return userClaimsList;
		}
	}
}
