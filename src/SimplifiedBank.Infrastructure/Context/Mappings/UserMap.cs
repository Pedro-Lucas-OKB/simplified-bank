using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimplifiedBank.Domain;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Infrastructure.Context.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
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
        
        builder.Property(x => x.RowVersion)
            .HasColumnName("RowVersion")
            .IsRowVersion(); // Fazendo concurrency check
        
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasColumnName("FullName")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(DomainConfiguration.UserFullNameMaximumLength);
        
        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("Email")
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasColumnName("PasswordHash")
            .HasColumnType("VARCHAR")
            .HasMaxLength(255);
        
        builder.Property(x => x.Document)
            .IsRequired()
            .HasColumnName("CPForCNPJ")
            .HasColumnType("VARCHAR")
            .HasMaxLength(14);
        
        builder.Property(x => x.Balance)
            .IsRequired()
            .HasColumnName("Balance")
            .HasColumnType("DECIMAL(18,2)");

        builder.Property(x => x.Type)
            .IsRequired()
            .HasColumnName("Type")
            .HasConversion<string>()
            .HasMaxLength(40);
        
        // Índices
        builder
            .HasIndex(x => x.Email, "IX_User_Email")
            .IsUnique();
        
        builder
            .HasIndex(x => x.Document, "IX_User_CPForCNPJ")
            .IsUnique();
        
        // Relacionamentos
        builder.HasMany(u => u.TransactionsSent)
            .WithOne(t => t.Sender)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(u => u.TransactionsReceived)
            .WithOne(t => t.Receiver)
            .HasForeignKey(t => t.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}