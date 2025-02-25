using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Property_Manager_WebApp.Models;

public partial class PropertyRentalManagementContext : DbContext
{
    public PropertyRentalManagementContext()
    {
    }

    public PropertyRentalManagementContext(DbContextOptions<PropertyRentalManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Apartment> Apartments { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Report> Reports { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MIGUELANGEL\\SQL2022EXPRESS;Initial Catalog=PropertyRentalManagement;User ID=sa;Password=lasalle;Integrated Security=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Apartment>(entity =>
        {
            entity.HasKey(e => e.ApartmentId).HasName("PK__Apartmen__CBDF5764633DDD4E");

            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(10);

            entity.HasOne(d => d.Building).WithMany(p => p.Apartments)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("FK__Apartment__Build__52593CB8");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC232A9C2F4");

            entity.Property(e => e.ScheduledDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Apartment).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ApartmentId)
                .HasConstraintName("FK__Appointme__Apart__5812160E");

            entity.HasOne(d => d.Manager).WithMany(p => p.AppointmentManagers)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Manag__571DF1D5");

            entity.HasOne(d => d.Tenant).WithMany(p => p.AppointmentTenants)
                .HasForeignKey(d => d.TenantId)
                .HasConstraintName("FK__Appointme__Tenan__5629CD9C");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.BuildingId).HasName("PK__Building__5463CDC40D621372");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);

            // Configure Manager relationship
            entity.HasOne(d => d.Manager)
                .WithMany(p => p.Buildings)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Buildings__Manag__4E88ABD4");

            // Configure Owner relationship
            entity.HasOne(d => d.Owner) // New Owner relationship
                .WithMany()
                .HasForeignKey(d => d.OwnerId) // Foreign key for OwnerId
                .OnDelete(DeleteBehavior.Cascade) // Adjust delete behavior if necessary
                .HasConstraintName("FK__Buildings__Owner__4F7CD00D"); // Constraint name for clarity
        });


        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C0C9C02CE4A1B");

            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Receiv__5CD6CB2B");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK__Messages__Sender__5BE2A6F2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CA93ECD8F");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534C488525B").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
