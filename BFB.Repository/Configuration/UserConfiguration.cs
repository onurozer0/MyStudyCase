using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFB.Repository.Configuration
{
	internal class UserConfiguration : IEntityTypeConfiguration<AppUser>
	{
		public void Configure(EntityTypeBuilder<AppUser> builder)
		{
			builder.Property(x => x.Name).HasColumnType("nvarchar(50)").HasMaxLength(50);
			builder.Property(x => x.Surname).HasColumnType("nvarchar(50)").HasMaxLength(50);
			builder.Property(x => x.LastLoginIp).HasColumnType("varchar(20)");
			builder.Property(x => x.PhoneNumber).HasColumnType("varchar(30)").HasMaxLength(30);
			builder.Property(x => x.PhoneNumber).HasColumnType("nvarchar(500)").HasMaxLength(500);
			builder.Property(x => x.Description).HasColumnType("nvarchar(500)").HasMaxLength(500);
			builder.Property(x => x.IdentityNumber).HasColumnType("char(11)").HasMaxLength(11);
			builder.HasMany(x => x.Products).WithOne(x => x.User).HasForeignKey(x => x.UserId);
		}
	}
}
