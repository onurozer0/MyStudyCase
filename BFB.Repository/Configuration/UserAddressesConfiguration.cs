using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFB.Repository.Configuration
{
	internal class UserAddressesConfiguration : IEntityTypeConfiguration<UserAddresses>
	{
		public void Configure(EntityTypeBuilder<UserAddresses> builder)
		{
			builder.Property(x => x.City).HasColumnType("nvarchar(30)").HasMaxLength(30);
			builder.Property(x => x.Zipcode).HasColumnType("varchar(5)").IsFixedLength().HasMaxLength(5);
			builder.Property(x => x.Address).HasColumnType("nvarchar(300)").HasMaxLength(300);
			builder.HasOne(x => x.User).WithOne(x => x.UserAddress).HasForeignKey<UserAddresses>(x => x.UserId);
		}
	}
}
