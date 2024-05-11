using BFB.AdminAPI.Filters;
using BFB.Core.Entities;
using BFB.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFB.AdminAPI.Controllers
{
	public class OfferController : CustomControllerBase
	{
		private readonly IOfferService _offerService;

		public OfferController(IOfferService offerService)
		{
			_offerService = offerService;
		}
		[HttpGet]
		public async Task<IActionResult> MostRequestedUsers()
		{
			return ActionResultInstance(await _offerService.GetMostRequestedUsersAsync());
		}
		[HttpGet]
		public async Task<IActionResult> MostApprovedUsers()
		{
			return ActionResultInstance(await _offerService.GetMostApprovedUsersAsync());
		}
		[HttpDelete]
		[Route("{id}")]
		[ServiceFilter(typeof(NotFoundFilter<Offer>))]
		public async Task<IActionResult> Delete(Guid id)
		{
			return ActionResultInstance(await _offerService.DeleteOfferAsync(id));
		}
		[HttpGet]
		[Route("{userId}")]
		public async Task<IActionResult> Users(string userId)
		{
			return ActionResultInstance(await _offerService.GetOffersByUserIdAsync(userId));
		}
	}
}
