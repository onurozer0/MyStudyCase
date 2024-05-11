using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Repositories;
using BFB.Core.Services;
using BFB.Core.UnitOfWorks;
using BFB.Service.MapProfile;
using Microsoft.EntityFrameworkCore;
using SharedLib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFB.Service.Services
{
	public class SearchedWordsService : Service<SearchedWords>, ISearchedWordsService
	{
		private readonly IGenericRepository<SearchedWords> _repository;
		public SearchedWordsService(IUnitOfWork unitOfWork, IGenericRepository<SearchedWords> repository) : base(unitOfWork, repository)
		{
			_repository = repository;
		}

		public async Task<Response<IEnumerable<SearchedWordsDto>>> GetSearchedWordsAsync(bool isMostSearched)
		{
			var wordsDto = new List<SearchedWordsDto>();
			if (isMostSearched)
			{
				var words = await _repository.GetAll().OrderByDescending(x => x.Count).ToListAsync();
				wordsDto = ObjectMapper.Mapper.Map<List<SearchedWordsDto>>(words);	
			}
			else
			{
				var words = await _repository.GetAll().OrderBy(x => x.Count).ToListAsync();
				wordsDto = ObjectMapper.Mapper.Map<List<SearchedWordsDto>>(words);
			}
			return Response<IEnumerable<SearchedWordsDto>>.Success(wordsDto, 200);
		}
	}
}
