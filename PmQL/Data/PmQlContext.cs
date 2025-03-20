using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PmQL.Models;

namespace PmQL.Data;

public partial class PmQlContext : DbContext
{
    public PmQlContext()
    {
    }

    public PmQlContext(DbContextOptions<PmQlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FmpA0> FmpA0s { get; set; }

    public virtual DbSet<FmpEvn> FmpEvns { get; set; }

    public virtual DbSet<FmpSai> FmpSais { get; set; }

    public virtual DbSet<NhậpGiáNm> NhậpGiáNms { get; set; }

    public virtual DbSet<PmA0> PmA0s { get; set; }

    public virtual DbSet<PmEvn> PmEvns { get; set; }

    public virtual DbSet<PmSai> PmSais { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-409OOSHC\\SQLEXPRESS;Initial Catalog=PmQL;Integrated Security=True;Encrypt=False;Trust Server Certificate=True;Connect Timeout=60");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FmpEvn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FMP_EVN__3214EC276D4638BE");
        });

        modelBuilder.Entity<FmpSai>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FMP_Sai__3214EC2740C9C5B8");
        });

        modelBuilder.Entity<PmEvn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pm_EVN__3214EC276B0DD338");
        });

        modelBuilder.Entity<PmSai>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pm_Sai__3214EC27797439AB");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });


        OnModelCreatingPartial(modelBuilder);
    }



    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
