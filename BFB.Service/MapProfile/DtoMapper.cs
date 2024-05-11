using AutoMapper;
using BFB.Core.DTOs;
using BFB.Core.Entities;

namespace BFB.Service.MapperConfiguration
{
	public class DtoMapper : Profile
	{
		public DtoMapper()
		{
			CreateMap<ProductDto, Product>().ReverseMap();
			CreateMap<UserDto, AppUser>().ReverseMap();
			CreateMap<UserIntroductionDto, UserIntroduction>().ReverseMap();
			CreateMap<UpdateMemberDto, AppUser>().ReverseMap();
			CreateMap<UnregisteredUserDto, AppUser>().ReverseMap();
			CreateMap<CreateMemberProfileDto, AppUser>().ReverseMap();
			CreateMap<UserAddressesDto, UserAddresses>().ReverseMap();
			CreateMap<CommentsDto, Comment>().ReverseMap();
			CreateMap<CommentsDto, PostComment>().ReverseMap();
			CreateMap<LikesDto, Like>().ReverseMap();
			CreateMap<LikesDto, PostLike>().ReverseMap();
			CreateMap<CategoryDto, Category>().ReverseMap();
			CreateMap<CategoryAddDto, Category>().ReverseMap();
			CreateMap<CategoryWithProductsDto, Category>().ReverseMap();
			CreateMap<CategoryUpdateDto, Category>().ReverseMap();
			CreateMap<CommentAddDto, Comment>().ReverseMap();
			CreateMap<CommentAddDto, PostComment>().ReverseMap();
			CreateMap<CommentsDto, Comment>().ReverseMap();
			CreateMap<CommentsDto, PostComment>().ReverseMap();
			CreateMap<PostAddDto, Post>().ReverseMap();
			CreateMap<PostUpdateDto, Post>().ReverseMap();
			CreateMap<PostDto, Post>().ReverseMap();
			CreateMap<SinglePostDto, Post>().ReverseMap();
			CreateMap<PrivateMessage, PrivateMessageDto>().ReverseMap();
			CreateMap<PrivateMessage, SendPrivateMessageDto>().ReverseMap();
			CreateMap<Conversation, ConversationDto>().ReverseMap();
			CreateMap<Conversation, StartConversationDto>().ReverseMap();
			CreateMap<Offer, OfferDto>().ReverseMap();
			CreateMap<Offer, SendOfferDto>().ReverseMap();
			CreateMap<SearchedWords, SearchedWordsDto>().ReverseMap();
		}
	}
}
