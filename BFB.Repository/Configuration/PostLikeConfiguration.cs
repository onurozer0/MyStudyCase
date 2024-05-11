using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFB.Repository.Configuration
{
	internal class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
	{
		public void Configure(EntityTypeBuilder<PostLike> builder)
		{
			builder.HasOne(x => x.User).WithMany(x => x.PostLikes).HasForeignKey(x => x.UserId);
			builder.HasOne(x => x.Post).WithMany(x => x.Likes).HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.NoAction);
			builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
		}
	}
}
