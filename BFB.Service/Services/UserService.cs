using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Repositories;
using BFB.Core.Services;
using BFB.Core.UnitOfWorks;
using BFB.Service.MapProfile;
using Microsoft.AspNetCore.Identity;
using SharedLib.DTOs;
using SharedLib.GeneralTools;

namespace BFB.Service.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly IGenericRepository<AppUser> _genericRepository;
		private readonly IUnitOfWork _unitOfWork;
		public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IGenericRepository<AppUser> genericRepository, IUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_genericRepository = genericRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Response<UserDto>> CreateUserAsync(SignUpDto signUpDto)
		{
			var user = new AppUser()
			{
				Email = signUpDto.Email,
				UserName = StringManipulationTools.GetUsernameByEmail(signUpDto.Email),
				CreatedDate = DateTime.Now,
				UserIntroduction = new()
				{
					Message = signUpDto.UserIntroduction.Message,
					CreatedDate = DateTime.Now,
				}
			};
			var result = await _userManager.CreateAsync(user, signUpDto.Password);
			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(x => x.Description).ToList();

				return Response<UserDto>.Fail(new ErrorDto(errors, true), 400);
			}
			var userCount = await _genericRepository.CountAsync();
			if (userCount == 1)
			{
				await _roleManager.CreateAsync(new AppRole { Name = "Admin" });
				await _roleManager.CreateAsync(new AppRole { Name = "User" });
				await _userManager.AddToRoleAsync(user, "Admin");
				user.IsActive = true;
				user.IsConfirmed = true;
				await _unitOfWork.CommitAsync();
			}
			return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
		}

		public async Task<Response<UserDto>> GetUserByNameAsync(string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user == null)
			{
				return Response<UserDto>.Fail("User Not Found.", 400, true);
			}
			return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
		}
	}
}
