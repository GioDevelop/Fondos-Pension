using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FPV.API.Core.Context;

public partial class AmarisDbContext : DbContext
{
    public AmarisDbContext()
    {
    }

    public AmarisDbContext(DbContextOptions<AmarisDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoryType> CategoryTypes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerFund> CustomerFunds { get; set; }

    public virtual DbSet<Fund> Funds { get; set; }

    public virtual DbSet<FundTransaction> FundTransactions { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=AmarisDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerFund>(entity =>
        {
            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerFunds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerFunds_Customer");

            entity.HasOne(d => d.Fund).WithMany(p => p.CustomerFunds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerFunds_Funds");
        });

        modelBuilder.Entity<Fund>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.Funds).HasConstraintName("FK_Funds_CategoryTypes");
        });

        modelBuilder.Entity<FundTransaction>(entity =>
        {
            entity.HasOne(d => d.Customer).WithMany(p => p.FundTransactions).HasConstraintName("FK_FundTransactions_Customer");

            entity.HasOne(d => d.Fund).WithMany(p => p.FundTransactions).HasConstraintName("FK_FundTransactions_Funds");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.FundTransactions).HasConstraintName("FK_FundTransactions_TransactionTypes");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
