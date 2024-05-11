using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFB.Repository.Configuration
{
	internal class PrivateMessageConfiguration : IEntityTypeConfiguration<PrivateMessage>
	{
		public void Configure(EntityTypeBuilder<PrivateMessage> builder)
		{
			builder.Property(x => x.Message).HasColumnType("text").HasMaxLength(2000);
			builder.HasOne(x => x.Sender).WithMany(x => x.SendedMessages).HasForeignKey(x => x.SenderId).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(x => x.Receiver).WithMany(x => x.ReceivedMessages).HasForeignKey(x => x.ReceiverId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
