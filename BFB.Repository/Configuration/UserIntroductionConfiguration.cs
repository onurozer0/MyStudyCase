using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFB.Repository.Configuration
{
	internal class UserIntroductionConfiguration : IEntityTypeConfiguration<UserIntroduction>
	{
		public void Configure(EntityTypeBuilder<UserIntroduction> builder)
		{
			builder.Property(x => x.Message).IsRequired().HasColumnType("text").HasMaxLength(2000);
			builder.HasOne(x => x.User).WithOne(x => x.UserIntroduction).HasForeignKey<UserIntroduction>(x => x.UserId);
		}
	}
}
