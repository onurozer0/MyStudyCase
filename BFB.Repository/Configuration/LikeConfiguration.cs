using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFB.Repository.Configuration
{
	internal class LikeConfiguration : IEntityTypeConfiguration<Like>
	{
		public void Configure(EntityTypeBuilder<Like> builder)
		{
			builder.HasOne(x => x.User).WithMany(x => x.Likes).HasForeignKey(x => x.UserId);
			builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
		}
	}
}
