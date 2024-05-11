using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFB.Repository.Configuration
{
	internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
	{
		public void Configure(EntityTypeBuilder<Comment> builder)
		{
			builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.HasOne(x => x.User).WithMany(x => x.Comments).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
			builder.Property(x => x.Title).IsRequired().HasColumnType("nvarchar(100)").HasMaxLength(100);
			builder.Property(x => x.Content).IsRequired().HasColumnType("text").HasMaxLength(2000);
		}
	}
}
