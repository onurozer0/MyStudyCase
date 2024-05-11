using BFB.Core.DTOs;
using SharedLib.DTOs;

namespace BFB.Core.Services
{
	public interface IUserService
	{
		Task<Response<UserDto>> CreateUserAsync(SignUpDto signUpDto);
		Task<Response<UserDto>> GetUserByNameAsync(string userName);
	}
}
