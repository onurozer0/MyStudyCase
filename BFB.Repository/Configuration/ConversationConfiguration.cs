using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFB.Repository.Configuration
{
	internal class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
	{
		public void Configure(EntityTypeBuilder<Conversation> builder)
		{
			builder.Property(x => x.Title).IsRequired().HasColumnType("nvarchar(50)").HasMaxLength(50);
			builder.HasMany(x => x.Messages).WithOne(x => x.Conversation).HasForeignKey(x => x.ConversationId);
			builder.HasOne(x => x.Starter).WithMany(x => x.StartedConversations).HasForeignKey(x => x.StarterId).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(x => x.Receiver).WithMany(x => x.ReceivedConversations).HasForeignKey(x => x.ReceiverId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
