using BFB.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFB.Core.Entities
{
	public class SearchedWords
	{
        public Guid Id { get; set; }
        public string Word { get; set; }    
        public string NormalizedWord { get; set; }    
        public int Count { get; set; }
    }
}
