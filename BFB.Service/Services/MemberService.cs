using BFB.Core;
using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Repositories;
using BFB.Core.Services;
using BFB.Core.UnitOfWorks;
using BFB.Service.MapProfile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLib.DTOs;
using TCKimlikNoDogrulama.Core;

namespace BFB.Service.Services
{
	public class MemberService : Service<AppUser>, IMemberService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IGenericRepository<UserIntroduction> _userIntroductionRepository;
		private readonly IGenericRepository<Product> _productRepository;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IGenericRepository<AppUser> _genericRepository;
		private readonly IGenericRepository<UserAddresses> _userAddressesRepository;
		private readonly IEmailService _emailService;
		private readonly IGenericRepository<SearchedWords> _searchedWordsRepository;

		public MemberService(IGenericRepository<AppUser> repository, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IGenericRepository<UserAddresses> userAddressesRepository, IGenericRepository<AppUser> genericRepository, IHttpContextAccessor contextAccessor, IGenericRepository<UserIntroduction> userIntroductionRepository, RoleManager<AppRole> roleManager, IEmailService emailService, IGenericRepository<Product> productRepository, IGenericRepository<SearchedWords> searchedWordsRepository) : base(unitOfWork, repository)
		{
			_userManager = userManager;
			_userAddressesRepository = userAddressesRepository;
			_genericRepository = genericRepository;
			_unitOfWork = unitOfWork;
			_contextAccessor = contextAccessor;
			_userIntroductionRepository = userIntroductionRepository;
			_roleManager = roleManager;
			_emailService = emailService;
			_productRepository = productRepository;
			_searchedWordsRepository = searchedWordsRepository;
		}

		public async Task<Response<UserDto>> ActivateUserAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return Response<UserDto>.Fail("Kullanıcı bulunamadı", 404, true);
			}
			user.IsActive = true;
			await _unitOfWork.CommitAsync();
			return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
		}

		public async Task<Response<UserDto>> ConfirmUserAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return Response<UserDto>.Fail("Kullanıcı bulunamadı", 404, true);
			}
			if (user.IsConfirmed)
			{
				return Response<UserDto>.Fail("Kullanıcı daha önce aktifleştirilmiş!", 400, true);
			}
			user.IsConfirmed = true;
			user.IsActive = true;
			var userInstroduction = await _userIntroductionRepository.Where(x => x.UserId == userId).FirstOrDefaultAsync();
			if (userInstroduction != null)
			{
				userInstroduction.ApprovedDate = DateTime.Now;
			}
			var result = await _userManager.AddToRoleAsync(user, "USER");
			await _unitOfWork.CommitAsync();
			return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
		}

		public async Task<Response<CreateMemberProfileDto>> CreateMemberProfileAsync(CreateMemberProfileDto createMemberProfileDto, string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user!.IsCreatedProfile)
			{
				return Response<CreateMemberProfileDto>.Fail("Profil daha önceden oluşturulmuş!", 409, true);
			}
			var isIdExist = new TCKimlikNoDogrulamaCORE(long.Parse(createMemberProfileDto.IdentityNumber)).TCAlgoritmasi();
			if (!isIdExist)
			{
				return Response<CreateMemberProfileDto>.Fail("Lütfen geçerli bir kimlik numarası giriniz!", 400, true);
			}
			var isProfileMatch = new TCKimlikNoDogrulamaCORE(long.Parse(createMemberProfileDto?.IdentityNumber),createMemberProfileDto.Name,createMemberProfileDto.Surname,createMemberProfileDto.DateOfBirth.Year).KisiVarMi();
			if (!isProfileMatch)
			{
				return Response<CreateMemberProfileDto>.Fail("Lütfen bilgilerinizin doğruluğunu kontrol ediniz kimlik numaranız ile bilgileriniz eşleşmiyor(Ad,Soyad,Doğum Tarihi)!", 400, true);
			}
			user.Name = createMemberProfileDto.Name;
			user.Surname = createMemberProfileDto.Surname;
			user.UserAddress = ObjectMapper.Mapper.Map<UserAddresses>(createMemberProfileDto.UserAddress);
			user.UserAddress.UserId = user.Id;
			user.IdentityNumber = createMemberProfileDto.IdentityNumber;
			user.PhoneNumber = createMemberProfileDto.PhoneNumber;
			user.DateOfBirth = createMemberProfileDto.DateOfBirth;
			user.Description = createMemberProfileDto.Description;
			user.UpdatedDate = DateTime.Now;
			user.IsCreatedProfile = true;
			await UpdateAsync(user);
			createMemberProfileDto = ObjectMapper.Mapper.Map<CreateMemberProfileDto>(user);
			return Response<CreateMemberProfileDto>.Success(createMemberProfileDto, 200);
		}

		public async Task<Response<UserDto>> DeactivateUserAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return Response<UserDto>.Fail("Kullanıcı bulunamadı", 404, true);
			}
			user.IsActive = false;
			await _unitOfWork.CommitAsync();
			return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
		}

		public async Task<Response<NoDataDto>> DeleteUserAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return Response<NoDataDto>.Fail("Kullanıcı bulunamadı", 404, true);
			}
			await RemoveAsync(user);
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<IEnumerable<UserDto>>> GetAllUsersAsync()
		{
			var users = await GetAllAsync();
			var userDtos = ObjectMapper.Mapper.Map<List<UserDto>>(users);
			return Response<IEnumerable<UserDto>>.Success(userDtos, 200);
		}

		public async Task<Response<IEnumerable<UserDto>>> GetConfirmedUsersAsync()
		{
			var users = await _genericRepository.Where(x => x.IsConfirmed == true).ToListAsync();
			var userDtos = ObjectMapper.Mapper.Map<List<UserDto>>(users);
			return Response<IEnumerable<UserDto>>.Success(userDtos, 200);
		}

		public async Task<Response<UserProfileDto>> GetProfileDetailsAsync(string id)
		{
			var user = await _genericRepository.Where(x => x.Id == id).Include(x => x.Comments).Include(x => x.PostComments).Include(x => x.SendedMessages).Include(x => x.Likes).Include(x => x.Products).Include(x => x.ReceivedOffers).Include(x => x.RequestedOffers).FirstAsync();
			if (user == null)
			{
				return Response<UserProfileDto>.Fail("Kullanıcı bulunamadı!", 404, true);
			}
			var userProfileDto = new UserProfileDto()
			{
				Id = id,
				Name = user.Name,
				Surname = user.Surname,
				MessagesSent = user.SendedMessages.Count,
				CommentsCount = user.PostComments.Count + user.Comments.Count,
				OffersSent = user.RequestedOffers.Count,
				DateOfBirth = user.DateOfBirth,
				LastLoginDate = user.LastLoginDate,
				LikesCount = user.Likes.Count,
				Description = user.Description,
				Products = ObjectMapper.Mapper.Map<List<ProductDto>>(user.Products),
				CreatedDate = user.CreatedDate,
				OffersApproved = user.ReceivedOffers.Count,
				ProfileVisitsCount = user.ProfileVisitsCount,
			};
			if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated && user.UserName != _contextAccessor.HttpContext.User.Identity.Name)
			{
				user.ProfileVisitsCount += 1;
				await _unitOfWork.CommitAsync();
			}
			return Response<UserProfileDto>.Success(userProfileDto, 200);
		}

		public async Task<Response<IEnumerable<UnregisteredUserDto>>> GetUnconfirmedUsersAsync()
		{
			var users = await _genericRepository.Where(x => x.IsConfirmed == false).Include(x => x.UserIntroduction).ToListAsync();
			var userDtos = ObjectMapper.Mapper.Map<List<UnregisteredUserDto>>(users);
			return Response<IEnumerable<UnregisteredUserDto>>.Success(userDtos, 200);
		}

		public async Task<Response<IdentityResult>> ResetPasswordAsync(ResetPasswordDto dto)
		{
			var user = await _userManager.FindByIdAsync(dto.UserId);
			if (user != null)
			{
				var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
				if (result.Succeeded)
				{
					await _userManager.UpdateSecurityStampAsync(user);
					return Response<IdentityResult>.Success(result, 200);
				}
				else
				{
					return Response<IdentityResult>.Fail("Bir hata oluştu", 400, true);
				}
			}
			else
			{
				return Response<IdentityResult>.Fail("Kullanıcı bulunamadı!", 404, true);
			}
		}

		public async Task<Response<SearchResponseDto>> SearchAsync(SearchDto searchDto)
		{
			var word = searchDto.Word;
			var normalizedWord = word.Trim().ToUpper();
			var isExist = await _searchedWordsRepository.Where(x => x.NormalizedWord == normalizedWord).FirstOrDefaultAsync();
			
			var products = await _productRepository.Where(x => x.Name.Contains(word) || x.Description.Contains(word)).ToListAsync();
			var users = await _genericRepository.Where(x => x.Name.Contains(word) || x.Description.Contains(word)).ToListAsync();
			var productDtos = ObjectMapper.Mapper.Map<List<ProductDto>>(products);
			var userDtos = ObjectMapper.Mapper.Map<List<UserDto>>(users);
			var searchResponseDto = new SearchResponseDto()
			{
				Products = productDtos,
				Users = userDtos
			};
			if(!productDtos.Any() && !userDtos.Any())
			{
				return Response<SearchResponseDto>.Fail("İlgili Ürün/Hizmet ya da kullanıcı bulunamadı!",404,true);
			}
			if (isExist == null)
			{
				await _searchedWordsRepository.AddAsync(new SearchedWords()
				{
					Count = 1,
					Word = word,
					NormalizedWord = normalizedWord
				});
			}
			else
			{
				isExist.Count++;
			}
			await _unitOfWork.CommitAsync();
			return Response<SearchResponseDto>.Success(searchResponseDto, 200);

		}

		public async Task<Response<NoDataDto>> SendPasswordResetLinkAsync(SendPasswordResetLinkDto sendPasswordResetLinkDto)
		{
			var user = await _userManager.FindByEmailAsync(sendPasswordResetLinkDto.Email);
			if (user == null)
			{
				return Response<NoDataDto>.Success(404);
			}
			string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

			var request = _contextAccessor!.HttpContext!.Request;
			var baseUrl = $"{request.Scheme}://{request.Host}";
			var link = $"{baseUrl}/userId={user.Id}&token={resetToken}";

			await _emailService.SendPwdResetEmail(link, user.Email);
			return Response<NoDataDto>.Success(204);

		}

		public async Task<Response<UpdateMemberDto>> UpdateMemberDetailsAsync(UpdateMemberDto updateMemberDto, string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);
			user!.UserAddress = await _userAddressesRepository.GetByIdAsync(user.Id);
			var checkPass = await _userManager.CheckPasswordAsync(user!, updateMemberDto.Password);
			if (!checkPass)
			{
				return Response<UpdateMemberDto>.Fail("Parolanızı kontrol edip tekrar deneyiniz.", 400, true);
			}
			user.Name = updateMemberDto.Name;
			user.Surname = updateMemberDto.Surname;
			user.UserAddress = ObjectMapper.Mapper.Map<UserAddresses>(updateMemberDto.UserAddress);
			user.UserAddress.UserId = user.Id;
			user.Description = updateMemberDto.Description;
			user.PhoneNumber = updateMemberDto.PhoneNumber;
			user.UpdatedDate = DateTime.Now;
			await UpdateAsync(user);
			updateMemberDto = ObjectMapper.Mapper.Map<UpdateMemberDto>(user);
			return Response<UpdateMemberDto>.Success(updateMemberDto, 200);
		}
	}
}
