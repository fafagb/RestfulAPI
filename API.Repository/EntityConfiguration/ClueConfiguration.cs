using System;
using System.Collections.Generic;
using System.Text;
using API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Repository.EntityConfiguration
{
    public class ClueConfiguration : IEntityTypeConfiguration<TxClues>
    {
        public void Configure(EntityTypeBuilder<TxClues> builder)
        {
            builder.Property(x => x.ReportCasePeople).HasMaxLength(30);
            
        }
    }
}
