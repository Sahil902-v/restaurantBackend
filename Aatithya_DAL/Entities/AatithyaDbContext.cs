using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Aatithya_DAL.Entities;

public partial class AatithyaDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public AatithyaDbContext(DbContextOptions<AatithyaDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WebApiLog> WebApiLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;User ID=sa;Password=Rudr4321@69;Database=Aatithya_DB;TrustServerCertificate=True;");

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

        modelBuilder.Entity<WebApiLog>(entity =>
        {
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
