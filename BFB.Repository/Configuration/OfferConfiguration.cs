using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFB.Repository.Configuration
{
	internal class OfferConfiguration : IEntityTypeConfiguration<Offer>
	{
		public void Configure(EntityTypeBuilder<Offer> builder)
		{
			builder.HasOne(x => x.Offerer).WithMany(x => x.RequestedOffers).HasForeignKey(x => x.OffererId).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(x => x.Receiver).WithMany(x => x.ReceivedOffers).HasForeignKey(x => x.ReceiverId).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(x => x.Product).WithMany(x => x.Offers).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
			builder.Property(x => x.Message).IsRequired().HasColumnType("text").HasMaxLength(2000);
			builder.Property(x => x.Title).IsRequired().HasColumnType("nvarchar(150)").HasMaxLength(150);
		}
	}
}
