using BFB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFB.Repository.Configuration
{
	internal class SearchedWordsConfiguration : IEntityTypeConfiguration<SearchedWords>
	{
		public void Configure(EntityTypeBuilder<SearchedWords> builder)
		{
			builder.Property(x => x.Word).HasColumnType("nvarchar(30)").HasMaxLength(30);
			builder.Property(x => x.NormalizedWord).HasColumnType("nvarchar(30)").HasMaxLength(30);
		}
	}
}
