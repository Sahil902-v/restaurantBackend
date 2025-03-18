using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AatithyaB_DAL.Entities;

public partial class AatithyaDbContext : DbContext
{
    public AatithyaDbContext()
    {
    }

    public AatithyaDbContext(DbContextOptions<AatithyaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-IGJJOSTT;User ID=sa;Password=sa@123;Database=Aatithya_DB;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.ToTable("Contact_Us");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.Property(e => e.ImgCategory).HasColumnName("Img_Category");
            entity.Property(e => e.ImgTitle)
                .HasMaxLength(50)
                .HasColumnName("Img_Title");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Img_url");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
