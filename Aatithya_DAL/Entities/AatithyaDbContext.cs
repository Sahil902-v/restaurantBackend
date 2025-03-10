using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Aatithya_DAL.Entities;

public partial class AatithyaDbContext : DbContext
{
    public AatithyaDbContext()
    {
    }

    public AatithyaDbContext(DbContextOptions<AatithyaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-IGJJOSTT;User ID=sa;Password=sa@123;Database=Aatithya_DB;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC2708F822CA");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MobileNO");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
