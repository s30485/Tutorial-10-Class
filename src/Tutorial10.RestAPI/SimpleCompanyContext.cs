using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Tutorial10.RestAPI;

public partial class SimpleCompanyContext : DbContext
{
    public SimpleCompanyContext()
    {
    }

    public SimpleCompanyContext(DbContextOptions<SimpleCompanyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departemnt> Departemnts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("s30485");

        modelBuilder.Entity<Departemnt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departem__3214EC07416CBC04");

            entity.ToTable("Departemnt");

            entity.HasIndex(e => e.Name, "UQ__Departem__737584F65F6CEFCE").IsUnique();

            entity.Property(e => e.Location)
                .HasMaxLength(33)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0769F53D4E");

            entity.ToTable("Employee");

            entity.Property(e => e.Commission).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Salary).HasColumnType("decimal(7, 2)");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__Depart__3852A05C");

            entity.HasOne(d => d.Job).WithMany(p => p.Employees)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__JobId__366A57EA");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Employee__Manage__375E7C23");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Job__3214EC07ED46D326");

            entity.ToTable("Job");

            entity.HasIndex(e => e.Name, "UQ__Job__737584F6B975339C").IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
