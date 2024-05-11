using BFB.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BFB.Repository
{
	public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(builder);
		}
		public DbSet<Product> Products { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<PostLike> PostLikes { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<PostComment> PostComments { get; set; }
		public DbSet<UserIntroduction> UserIntroductions { get; set; }
		public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
		public DbSet<Offer> Offers { get; set; }
		public DbSet<SearchedWords> SearchedWords { get; set; }
	}
}
