using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TKomp.Models;

namespace TKomp.Context
{
    public partial class DevDataContext : DbContext
    {
        public DevDataContext()
        {
        }

        public DevDataContext(DbContextOptions<DevDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TableA> TableAs { get; set; } = null!;
        public virtual DbSet<TableB> TableBs { get; set; } = null!;
        public virtual DbSet<TableC> TableCs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TableA>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Table_A");

                entity.Property(e => e.ColA1).HasColumnName("Col_A1");

                entity.Property(e => e.ColA2)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Col_A2");

                entity.Property(e => e.ColA3)
                    .HasColumnType("date")
                    .HasColumnName("Col_A3");
            });

            modelBuilder.Entity<TableB>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Table_B");

                entity.Property(e => e.ColB1).HasColumnName("Col_B1");

                entity.Property(e => e.ColB2)
                    .HasMaxLength(10)
                    .HasColumnName("Col_B2")
                    .IsFixedLength();

                entity.Property(e => e.ColB3).HasColumnName("Col_B3");
            });

            modelBuilder.Entity<TableC>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Table_C");

                entity.Property(e => e.ColC1)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Col_C1");

                entity.Property(e => e.ColC2)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("Col_C2");

                entity.Property(e => e.ColC3).HasColumnName("Col_C3");

                entity.Property(e => e.ColC4)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Col_C4")
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
