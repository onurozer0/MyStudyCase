using BFB.AdminAPI.Controllers;
using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Services;
using BFB.UserAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFB.UserAPI.Controllers
{
	[Authorize]
	[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
	public class OfferController : CustomControllerBase
	{
		private readonly IOfferService _offerService;

		public OfferController(IOfferService offerService)
		{
			_offerService = offerService;
		}
		[HttpGet]
		[Route("/api/offer/my/sent")]
		public async Task<IActionResult> MySentOffers()
		{
			return ActionResultInstance(await _offerService.GetMyRequestedOffersAsync());
		}
		[HttpGet]
		[Route("/api/offer/my/approved")]
		public async Task<IActionResult> MyApprovedOffers()
		{
			return ActionResultInstance(await _offerService.GetMyReceivedOffersAsync());
		}
		[HttpPost]
		public async Task<IActionResult> New(SendOfferDto dto)
		{
			return ActionResultInstance(await _offerService.SendOfferAsync(dto));
		}
		[Route("{id}")]
		[ServiceFilter(typeof(NotFoundFilter<Offer>))]
		[HttpGet]

		public async Task<IActionResult> Accept(Guid id)
		{
			return ActionResultInstance(await _offerService.AcceptOfferAsync(id));
		}
		[HttpGet]
		[Route("{id}")]
		[ServiceFilter(typeof(NotFoundFilter<Offer>))]
		public async Task<IActionResult> Reject(Guid id)
		{
			return ActionResultInstance(await _offerService.RejectOfferAsync(id));
		}
	}
}
