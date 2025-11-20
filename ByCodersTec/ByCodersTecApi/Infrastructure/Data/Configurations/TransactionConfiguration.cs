using ByCodersTecApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByCodersTecApi.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Type)
            .IsRequired();

        builder.Property(t => t.OccurredAt)
            .IsRequired();

        builder.Property(t => t.Value)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(t => t.Cpf)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(t => t.Card)
            .IsRequired()
            .HasMaxLength(16);

        builder.HasIndex(t => t.StoreId);

        builder.HasOne(t => t.Store)
            .WithMany(s => s.Transactions)
            .HasForeignKey(t => t.StoreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
