using BFB.Core.DTOs;
using BFB.Core.Entities;
using Microsoft.AspNetCore.Identity;
using SharedLib.DTOs;

namespace BFB.Core.Services
{
	public interface IMemberService : IService<AppUser>
	{
		Task<Response<UpdateMemberDto>> UpdateMemberDetailsAsync(UpdateMemberDto updateMemberDto, string userName);
		Task<Response<CreateMemberProfileDto>> CreateMemberProfileAsync(CreateMemberProfileDto createMemberProfileDto, string userName);
		Task<Response<UserProfileDto>> GetProfileDetailsAsync(string id);
		Task<Response<UserDto>> ActivateUserAsync(string userId);
		Task<Response<UserDto>> DeactivateUserAsync(string userId);
		Task<Response<IEnumerable<UserDto>>> GetAllUsersAsync();
		Task<Response<IEnumerable<UserDto>>> GetConfirmedUsersAsync();
		Task<Response<IEnumerable<UnregisteredUserDto>>> GetUnconfirmedUsersAsync();
		Task<Response<UserDto>> ConfirmUserAsync(string userId);
		Task<Response<NoDataDto>> SendPasswordResetLinkAsync(SendPasswordResetLinkDto sendPasswordResetLinkDto);
		Task<Response<NoDataDto>> DeleteUserAsync(string userId);
		Task<Response<IdentityResult>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
		Task<Response<SearchResponseDto>> SearchAsync(SearchDto searchDto);	
	}
}
