using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Infrastructure.Context.Mappings;

public class TransactionMap : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.DateCreated)
            .IsRequired()
            .HasColumnName("DateCreated")
            .HasColumnType("DATETIME");
        
        builder.Property(x => x.DateModified)
            .IsRequired(false)
            .HasColumnName("DateModified")
            .HasColumnType("DATETIME");
        
        builder.Property(x => x.Value)
            .IsRequired()
            .HasColumnName("Value")
            .HasColumnType("DECIMAL(18,2)");
        
        builder.Property(x => x.SenderId)
            .IsRequired()
            .HasColumnName("SenderId");
        
        builder.Property(x => x.ReceiverId)
            .IsRequired()
            .HasColumnName("ReceiverId");
        // Índices
        builder.HasIndex(x => x.SenderId, "IX_Transaction_SenderId");
        builder.HasIndex(x => x.ReceiverId, "IX_Transaction_ReceiverId");

        // Relacionamentos
        builder.HasOne(t => t.Sender)
            .WithMany(u => u.TransactionsSent)
            .HasForeignKey(t => t.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(t => t.Receiver)
            .WithMany(u => u.TransactionsReceived)
            .HasForeignKey(t => t.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}