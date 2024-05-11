using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFB.Core.DTOs
{
	public class SearchedWordsDto
	{
        public Guid Id { get; set; }
        public string Word { get; set; }
		public int Count { get; set; }
	}
}
