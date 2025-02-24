using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebProject.Models;

public partial class HospitalSurgeryContext : DbContext
{
    public HospitalSurgeryContext()
    {
    }

    public HospitalSurgeryContext(DbContextOptions<HospitalSurgeryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OperatingRoom> OperatingRooms { get; set; }

    public virtual DbSet<OperationSchedule> OperationSchedules { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<Surgeon> Surgeons { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        // => optionsBuilder.UseSqlServer("Server=192.168.147.54;database=hospital_surgery;User id=is;Password=1;TrustServerCertificate=true");
         => optionsBuilder.UseSqlServer("Server=WIN-3QCSQAGT3H0\\MSSQLSERVER01;database=hospital_surgery;TrustServerCertificate=true;Trusted_Connection=true");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OperatingRoom>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__operatin__19675A8A9633B652");

            entity.ToTable("operating_rooms");

            entity.HasIndex(e => e.RoomName, "UQ__operatin__1B7D99CD6F815C49").IsUnique();

            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.RoomName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("room_name");
            entity.Property(e => e.EquipmentDetails)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("equipment_details");
        });

        modelBuilder.Entity<OperationSchedule>(entity =>
        {
            entity.HasKey(e => e.OperationId).HasName("PK__operatio__9DE17123486BA0C1");

            entity.ToTable("operation_schedules");

            entity.Property(e => e.OperationId).HasColumnName("operation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.StartDateTime)
                .HasColumnName("start_time")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDateTime)
                .HasColumnName("end_time")
                .HasColumnType("datetime");
            entity.Property(e => e.OperatingRoomId).HasColumnName("operating_room_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Запланировано")
                .HasColumnName("status");
            entity.Property(e => e.SurgeonId).HasColumnName("surgeon_id");

            entity.HasOne(d => d.OperatingRoom).WithMany(p => p.OperationSchedules)
                .HasForeignKey(d => d.OperatingRoomId)
                .HasConstraintName("FK__operation__opera__4CA06362");

            entity.HasOne(d => d.Surgeon).WithMany(p => p.OperationSchedules)
                .HasForeignKey(d => d.SurgeonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__operation__surge__4BAC3F29");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__760965CC64C5CBCA");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "UQ__roles__783254B19ACB1F0E").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.SpecializationId).HasName("PK__speciali__0E5BB650BA752481");

            entity.ToTable("specializations");

            entity.HasIndex(e => e.SpecializationName, "UQ__speciali__A28CFD799189810A").IsUnique();

            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");
            entity.Property(e => e.SpecializationName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("specialization_name");
        });

        modelBuilder.Entity<Surgeon>(entity =>
        {
            entity.HasKey(e => e.SurgeonId).HasName("PK__surgeons__01A1C59D280A88E3");

            entity.ToTable("surgeons");

            entity.HasIndex(e => e.FullName, "UQ__surgeons__A79AD91F5FA52B85").IsUnique();

            entity.HasIndex(e => e.UserId, "UQ__surgeons__B9BE370E2E3A3DB4").IsUnique();

            entity.Property(e => e.SurgeonId).HasColumnName("surgeon_id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.ContactInfo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contact_info");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Specialization).WithMany(p => p.Surgeons)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__surgeons__specia__4316F928");

            entity.HasOne(d => d.User).WithOne(p => p.Surgeon)
                .HasForeignKey<Surgeon>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__surgeons__user_i__4222D4EF");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370F2851F8A2");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC5722BA272FD").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
