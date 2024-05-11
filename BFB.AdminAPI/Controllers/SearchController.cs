using BFB.Core.Services;
using BFB.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BFB.AdminAPI.Controllers
{
	public class SearchController : CustomControllerBase
	{
		private readonly ISearchedWordsService _searchedWordsService;

		public SearchController(ISearchedWordsService searchedWordsService)
		{
			_searchedWordsService = searchedWordsService;
		}
		[HttpGet]
		public async Task<IActionResult> Most()
		{
			return ActionResultInstance(await _searchedWordsService.GetSearchedWordsAsync(true));
		}
		[HttpGet]
		public async Task<IActionResult> Least()
		{
			return ActionResultInstance(await _searchedWordsService.GetSearchedWordsAsync(false));
		}
	}
}
