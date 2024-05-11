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

namespace BFB.Service.Services
{
	internal class OfferService : Service<Offer>, IOfferService
	{
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly UserManager<AppUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<Product> _productRepository;
		private readonly IGenericRepository<AppUser> _userRepository;
		public OfferService(IUnitOfWork unitOfWork, IGenericRepository<Offer> repository, IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager, IGenericRepository<Product> productRepository, IGenericRepository<AppUser> userRepository) : base(unitOfWork, repository)
		{
			_contextAccessor = contextAccessor;
			_userManager = userManager;
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
			_userRepository = userRepository;
		}

		public async Task<Response<OfferDto>> AcceptOfferAsync(Guid id)
		{
			var offer = await GetByIdAsync(id);
			if (offer.IsHandled)
			{
				return Response<OfferDto>.Fail("Bu teklif daha önceden değerlendirilmiş!", 400, true);
			}

			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User!.Identity!.Name!);
			if (offer.ReceiverId != user!.Id)
			{
				return Response<OfferDto>.Fail("Bu işlemi gerçekleştirmek için yetkiniz yok!", 403, true);
			}
			offer.UpdatedDate = DateTime.Now;
			offer.IsAccepted = true;
			offer.IsHandled = true;
			await _unitOfWork.CommitAsync();
			var offerDto = ObjectMapper.Mapper.Map<OfferDto>(offer);
			return Response<OfferDto>.Success(offerDto, 200);
		}

		public async Task<Response<NoDataDto>> DeleteOfferAsync(Guid id)
		{
			var offer = await GetByIdAsync(id);
			await RemoveAsync(offer);
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<IEnumerable<UserDtoWithOfferCount>>> GetMostApprovedUsersAsync()
		{
			var users = await _userRepository.GetAll().Include(x => x.ReceivedOffers).OrderByDescending(x => x.ReceivedOffers.Count()).ToListAsync();
			var userDtos = new List<UserDtoWithOfferCount>();
			foreach (var user in users)
			{
				var userDto = new UserDtoWithOfferCount()
				{
					Id = user.Id,
					Name = user.Name,
					Surname = user.Surname,
					OffersCount = user.ReceivedOffers.Count,
					UserName = user.UserName
				};
				userDtos.Add(userDto);
			}
			return Response<IEnumerable<UserDtoWithOfferCount>>.Success(userDtos, 200);
		}

		public async Task<Response<IEnumerable<UserDtoWithOfferCount>>> GetMostRequestedUsersAsync()
		{
			var users = await _userRepository.GetAll().Include(x => x.RequestedOffers).OrderByDescending(x => x.RequestedOffers.Count()).ToListAsync();
			var userDtos = new List<UserDtoWithOfferCount>();
			foreach (var user in users)
			{
				var userDto = new UserDtoWithOfferCount()
				{
					Id = user.Id,
					Name = user.Name,
					Surname = user.Surname,
					OffersCount = user.RequestedOffers.Count,
					UserName = user.UserName
				};
				userDtos.Add(userDto);
			}
			return Response<IEnumerable<UserDtoWithOfferCount>>.Success(userDtos, 200);
		}

		public async Task<Response<IEnumerable<OfferDto>>> GetMyReceivedOffersAsync()
		{
			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User!.Identity!.Name!);
			var offers = await Where(x => x.ReceiverId == user!.Id).Include(x => x.Receiver).Include(x => x.Offerer).Include(x => x.Product).ToListAsync();
			var offerDtos = ObjectMapper.Mapper.Map<List<OfferDto>>(offers);
			return Response<IEnumerable<OfferDto>>.Success(offerDtos, 200);
		}

		public async Task<Response<IEnumerable<OfferDto>>> GetMyRequestedOffersAsync()
		{
			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User!.Identity!.Name!);
			var offers = await Where(x => x.OffererId == user!.Id).Include(x => x.Receiver).Include(x => x.Offerer).Include(x => x.Product).ToListAsync();
			var offerDtos = ObjectMapper.Mapper.Map<List<OfferDto>>(offers);
			return Response<IEnumerable<OfferDto>>.Success(offerDtos, 200);
		}

		public async Task<Response<IEnumerable<OfferDto>>> GetOffersByUserIdAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return Response<IEnumerable<OfferDto>>.Fail("Kullanıcı bulunamadı!", 404, true);
			}
			var offers = await Where(x => x.OffererId == user!.Id || x.ReceiverId == user!.Id).Include(x => x.Receiver).Include(x => x.Offerer).Include(x => x.Product).ToListAsync();
			var offerDtos = ObjectMapper.Mapper.Map<List<OfferDto>>(offers);
			return Response<IEnumerable<OfferDto>>.Success(offerDtos, 200);
		}

		public async Task<Response<OfferDto>> RejectOfferAsync(Guid id)
		{
			var offer = await GetByIdAsync(id);
			if (offer.IsHandled)
			{
				return Response<OfferDto>.Fail("Bu teklif daha önceden değerlendirilmiş!", 400, true);
			}

			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User!.Identity!.Name!);
			if (offer.ReceiverId != user!.Id)
			{
				return Response<OfferDto>.Fail("Bu işlemi gerçekleştirmek için yetkiniz yok!", 403, true);
			}
			offer.UpdatedDate = DateTime.Now;
			offer.IsAccepted = false;
			offer.IsHandled = true;
			await _unitOfWork.CommitAsync();
			var offerDto = ObjectMapper.Mapper.Map<OfferDto>(offer);
			return Response<OfferDto>.Success(offerDto, 200);
		}

		public async Task<Response<OfferDto>> SendOfferAsync(SendOfferDto sendOfferDto)
		{
			var product = await _productRepository.GetByIdAsync(sendOfferDto.ProductId);
			if (product == null)
			{
				return Response<OfferDto>.Fail("Ürün bulunamadı!", 400, true);
			}
			var offerer = await _userManager.FindByNameAsync(_contextAccessor!.HttpContext!.User.Identity!.Name!);
			if (product.UserId == offerer!.Id)
			{
				return Response<OfferDto>.Fail("Kendi ürününüze teklif veremezsiniz!", 400, true);
			}
			var offer = ObjectMapper.Mapper.Map<Offer>(sendOfferDto);
			offer.ReceiverId = product.UserId;
			offer.OffererId = offerer.Id;
			await AddAsync(offer);
			var offerDto = ObjectMapper.Mapper.Map<OfferDto>(offer);
			return Response<OfferDto>.Success(offerDto, 200);
		}
	}
}
