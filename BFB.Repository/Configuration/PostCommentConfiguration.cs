using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFB.Repository.Configuration
{
	internal class PostCommentConfiguration : IEntityTypeConfiguration<PostComment>
	{
		public void Configure(EntityTypeBuilder<PostComment> builder)
		{
			builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.HasOne(x => x.Post).WithMany(x => x.Comments).HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(x => x.User).WithMany(x => x.PostComments).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
			builder.Property(x => x.Title).IsRequired().HasColumnType("nvarchar(100)").HasMaxLength(100);
			builder.Property(x => x.Content).IsRequired().HasColumnType("text").HasMaxLength(2000);
		}
	}
}
