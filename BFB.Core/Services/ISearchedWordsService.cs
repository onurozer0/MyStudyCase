using BFB.Core.DTOs;
using BFB.Core.Entities;
using SharedLib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFB.Core.Services
{
	public interface ISearchedWordsService : IService<SearchedWords>
	{
		Task<Response<IEnumerable<SearchedWordsDto>>> GetSearchedWordsAsync(bool isMostSearched);

	}
}
