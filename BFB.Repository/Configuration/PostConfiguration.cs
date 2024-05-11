using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFB.Repository.Configuration
{
	internal class PostConfiguration : IEntityTypeConfiguration<Post>
	{
		public void Configure(EntityTypeBuilder<Post> builder)
		{
			builder.Property(x => x.Title).IsRequired().HasColumnType("nvarchar(100)").HasMaxLength(100);
			builder.Property(x => x.Content).IsRequired().HasColumnType("text").HasMaxLength(2000);
			builder.HasOne(x => x.User).WithMany(x => x.Posts).HasForeignKey(x => x.UserId);
			builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
		}
	}
}
