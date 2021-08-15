using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Vensa.Api.Entities
{
    public partial class VensaContext : DbContext
    {
        public VensaContext()
        {
        }

        public VensaContext(DbContextOptions<VensaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Consumer> Consumers { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionMethod> TransactionMethods { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Consumer>(entity =>
            {
                entity.ToTable("Consumer");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.MiddleName).HasMaxLength(400);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PreferredName).HasMaxLength(200);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.Value).HasColumnType("numeric(16, 2)");
            });

            modelBuilder.Entity<TransactionMethod>(entity =>
            {
                entity.ToTable("TransactionMethod");

                entity.Property(e => e.TransactionMethodName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.ToTable("TransactionType");

                entity.Property(e => e.TransactionTypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
            {
                optionsBuilder.LogTo(Console.WriteLine);
            }
            
        }
    }
}
