using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RoadmapDesigner.Server.Models.Entity;

public partial class RoadmapDesignerContext : DbContext
{
    public RoadmapDesignerContext()
    {
    }

    public RoadmapDesignerContext(DbContextOptions<RoadmapDesignerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Editor> Editors { get; set; }

    public virtual DbSet<Program> Programs { get; set; }

    public virtual DbSet<ProgramDiscipline> ProgramDisciplines { get; set; }

    public virtual DbSet<ProgramEditor> ProgramEditors { get; set; }

    public virtual DbSet<ProgramVersion> ProgramVersions { get; set; }

    public virtual DbSet<Roadmap> Roadmaps { get; set; }

    public virtual DbSet<StudentDiscipline> StudentDisciplines { get; set; }

    public virtual DbSet<StudentProgram> StudentPrograms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=RoadmapDesigner;Username=postgres;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.HasKey(e => e.DisciplineId).HasName("Unique_Identifier5");

            entity.HasAnnotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

            entity.Property(e => e.DisciplineId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("DisciplineID");
            entity.Property(e => e.DisciplineName).HasMaxLength(50);
        });

        modelBuilder.Entity<Editor>(entity =>
        {
            entity.HasKey(e => new { e.EditorId, e.UserId });

            entity
                .ToTable("Editor")
                .HasAnnotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

            entity.Property(e => e.EditorId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("EditorID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Editors)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relationship1");
        });

        modelBuilder.Entity<Program>(entity =>
        {
            entity.HasKey(e => e.ProgramCode).HasName("Unique_Identifier2");

            entity.HasAnnotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

            entity.Property(e => e.ProgramCode).HasMaxLength(8);
            entity.Property(e => e.ProgramName).HasMaxLength(30);
        });

        modelBuilder.Entity<ProgramDiscipline>(entity =>
        {
            entity.HasKey(e => new { e.ProgramDisciplineId, e.ProgramVersionId }).HasName("Unique_Identifier6");

            entity
                .ToTable("ProgramDiscipline")
                .HasAnnotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

            entity.HasIndex(e => e.DisciplineId, "IX_ProgramDiscipline_DisciplineID");

            entity.Property(e => e.ProgramDisciplineId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("ProgramDisciplineID");
            entity.Property(e => e.ProgramVersionId).HasColumnName("ProgramVersionID");
            entity.Property(e => e.DisciplineId).HasColumnName("DisciplineID");

            entity.HasOne(d => d.Discipline).WithMany(p => p.ProgramDisciplines)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProgramDiscipline_Discipline");

            entity.HasOne(d => d.ProgramVersion).WithMany(p => p.ProgramDisciplines)
                .HasForeignKey(d => d.ProgramVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PrVersions_PrDiscipline");
        });

        modelBuilder.Entity<ProgramEditor>(entity =>
        {
            entity.HasKey(e => e.ProgramEditorId).HasName("Unique_Identifier9");

            entity
                .ToTable("ProgramEditor")
                .HasAnnotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

            entity.HasIndex(e => new { e.EditorId, e.UserId }, "IX_ProgramEditor_EditorID_UserID");

            entity.HasIndex(e => e.ProgramVersionId, "IX_ProgramEditor_ProgramVersionID");

            entity.Property(e => e.ProgramEditorId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("ProgramEditorID");
            entity.Property(e => e.EditorId).HasColumnName("EditorID");
            entity.Property(e => e.ProgramVersionId).HasColumnName("ProgramVersionID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.ProgramVersion).WithMany(p => p.ProgramEditors)
                .HasForeignKey(d => d.ProgramVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relationship4");

            entity.HasOne(d => d.Editor).WithMany(p => p.ProgramEditors)
                .HasForeignKey(d => new { d.EditorId, d.UserId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relationship2");
        });

        modelBuilder.Entity<ProgramVersion>(entity =>
        {
            entity.HasKey(e => e.ProgramVersionId).HasName("Unique_Identifier3");

            entity.HasAnnotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

            entity.HasIndex(e => e.ProgramCode, "IX_ProgramVersions_ProgramCode");

            entity.Property(e => e.ProgramVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("ProgramVersionID");
            entity.Property(e => e.ProgramCode).HasMaxLength(8);

            entity.HasOne(d => d.ProgramCodeNavigation).WithMany(p => p.ProgramVersions)
                .HasForeignKey(d => d.ProgramCode)
                .HasConstraintName("Relationship3");
        });

        modelBuilder.Entity<Roadmap>(entity =>
        {
            entity.HasKey(e => e.RoadmapId).HasName("Unique_Identifier4");

            entity.HasAnnotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

            entity.HasIndex(e => e.ProgramVersionId, "IX_Roadmaps_ProgramVersionID");

            entity.Property(e => e.RoadmapId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("RoadmapID");
            entity.Property(e => e.ProgramVersionId).HasColumnName("ProgramVersionID");

            entity.HasOne(d => d.ProgramVersion).WithMany(p => p.Roadmaps)
                .HasForeignKey(d => d.ProgramVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProgramVersions_Roadmaps");
        });

        modelBuilder.Entity<StudentDiscipline>(entity =>
        {
            entity.HasKey(e => new { e.StudentDisciplineId, e.StudentProgramId, e.ProgramVersionId }).HasName("Unique_Identifier8");

            entity
                .ToTable("StudentDiscipline")
                .HasAnnotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

            entity.Property(e => e.StudentDisciplineId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("StudentDisciplineID");
            entity.Property(e => e.StudentProgramId).HasColumnName("StudentProgramID");
            entity.Property(e => e.ProgramVersionId).HasColumnName("ProgramVersionID");
            entity.Property(e => e.Grade).HasMaxLength(3);
            entity.Property(e => e.StatusDiscipline).HasMaxLength(30);

            entity.HasOne(d => d.StudentProgram).WithMany(p => p.StudentDisciplines)
                .HasForeignKey(d => new { d.StudentProgramId, d.ProgramVersionId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("StProgram_StDiscipline");
        });

        modelBuilder.Entity<StudentProgram>(entity =>
        {
            entity.HasKey(e => new { e.StudentProgramId, e.ProgramVersionId }).HasName("Unique_Identifier7");

            entity
                .ToTable("StudentProgram")
                .HasAnnotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

            entity.HasIndex(e => e.UserId, "IX_StudentProgram_UserID");

            entity.Property(e => e.StudentProgramId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("StudentProgramID");
            entity.Property(e => e.ProgramVersionId).HasColumnName("ProgramVersionID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.ProgramVersion).WithMany(p => p.StudentPrograms)
                .HasForeignKey(d => d.ProgramVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("StPrograms_PrVersions");

            entity.HasOne(d => d.User).WithMany(p => p.StudentPrograms)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_StudentProgram");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Unique_Identifier1");

            entity.HasAnnotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

            entity.HasIndex(e => e.RoleId, "IX_Users_RoleID");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Login).HasMaxLength(30);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.SecondName).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Roles");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Unique_Identifier10");

            entity.HasAnnotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
