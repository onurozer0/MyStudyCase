using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFB.Core.DTOs
{
	public class SearchResponseDto
	{
        public List<ProductDto> Products { get; set; }
        public List<UserDto> Users { get; set; }
    }
}
