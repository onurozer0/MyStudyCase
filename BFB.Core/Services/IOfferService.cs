using BFB.Core.DTOs;
using BFB.Core.Entities;
using SharedLib.DTOs;

namespace BFB.Core.Services
{
	public interface IOfferService : IService<Offer>
	{
		Task<Response<IEnumerable<OfferDto>>> GetMyRequestedOffersAsync();
		Task<Response<IEnumerable<OfferDto>>> GetMyReceivedOffersAsync();
		Task<Response<OfferDto>> AcceptOfferAsync(Guid id);
		Task<Response<OfferDto>> RejectOfferAsync(Guid id);
		Task<Response<OfferDto>> SendOfferAsync(SendOfferDto sendOfferDto);
		Task<Response<IEnumerable<UserDtoWithOfferCount>>> GetMostRequestedUsersAsync();
		Task<Response<IEnumerable<UserDtoWithOfferCount>>> GetMostApprovedUsersAsync();
		Task<Response<NoDataDto>> DeleteOfferAsync(Guid id);
		Task<Response<IEnumerable<OfferDto>>> GetOffersByUserIdAsync(string userId);
	}
}
