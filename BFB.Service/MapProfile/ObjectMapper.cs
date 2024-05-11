using AutoMapper;
using BFB.Service.MapperConfiguration;

namespace BFB.Service.MapProfile
{
	public static class ObjectMapper
	{
		private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
		{
			var config = new AutoMapper.MapperConfiguration(cfg =>
			{
				cfg.AddProfile<DtoMapper>();
			});
			return config.CreateMapper();
		});
		public static IMapper Mapper => lazy.Value;
	}
}
