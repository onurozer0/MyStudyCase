using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFB.Repository.Configuration
{
	internal class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.Property(x => x.Name).IsRequired().HasColumnType("nvarchar(200)").HasMaxLength(200);
			builder.Property(x => x.Description).IsRequired().HasColumnType("nvarchar(350)").HasMaxLength(350);
			builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.HasMany(x => x.Likes).WithOne().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
			builder.HasMany(x => x.Comments).WithOne(x => x.Product).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
