using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MVCInterProject.Models
{
    public partial class AW_TestContext : DbContext
    {
        public AW_TestContext()
        {
        }

        public AW_TestContext(DbContextOptions<AW_TestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Package> Packages { get; set; } = null!;
        public virtual DbSet<Shipping> Shippings { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-68KN1FC\SQLEXPRESS;Database=PackageProject;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasKey(e => e.PacId)
                    .HasName("PK__Package__37D45AFD21080290");

                entity.ToTable("Package");

                entity.Property(e => e.PacId).HasColumnName("PAC_ID");

                entity.Property(e => e.PacCity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PAC_City");

                entity.Property(e => e.PacCloseDate)
                    .HasColumnType("date")
                    .HasColumnName("PAC_CloseDate");

                entity.Property(e => e.PacCreateDate)
                    .HasColumnType("date")
                    .HasColumnName("PAC_CreateDate");

                entity.Property(e => e.PacIsOpen).HasColumnName("PAC_IsOpen");

                entity.Property(e => e.PacName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PAC_Name");
            });

            modelBuilder.Entity<Shipping>(entity =>
            {
                entity.HasKey(e => e.ShiId)
                    .HasName("PK__Shipping__326BBA32520FC57B");

                entity.ToTable("Shipping");

                entity.Property(e => e.ShiId).HasColumnName("SHI_ID");

                entity.Property(e => e.ShiAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SHI_Address");

                entity.Property(e => e.ShiCreateDate)
                    .HasColumnType("date")
                    .HasColumnName("SHI_CreateDate");

                entity.Property(e => e.ShiMass).HasColumnName("SHI_Mass");

                entity.Property(e => e.ShiName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SHI_Name");

                entity.Property(e => e.ShiPackageId).HasColumnName("SHI_PackageID");

                entity.HasOne(d => d.ShiPackage)
                    .WithMany(p => p.Shippings)
                    .HasForeignKey(d => d.ShiPackageId)
                    .HasConstraintName("FK__Shipping__SHI_Pa__267ABA7A");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.PerId)
                    .HasName("PK__Person__8FE383B353C293FB");

                entity.ToTable("Person");

                entity.Property(e => e.PerId).HasColumnName("PER_ID");

                entity.Property(e => e.PerIsAdmin).HasColumnName("PER_IsAdmin");

                entity.Property(e => e.PerName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PER_Name");

                entity.Property(e => e.PerPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PER_Password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
