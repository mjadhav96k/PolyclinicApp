using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PolyclinicApp.ClassLibrary.Models;
public partial class PolyclinicDbContext : DbContext
{
    public PolyclinicDbContext()
    {
    }

    public PolyclinicDbContext(DbContextOptions<PolyclinicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }
    public virtual DbSet<ListOfAppointments> ListOfAppointments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(
            new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build()
            .GetConnectionString("PolyclinicDB")
            );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentNo).HasName("pk_AppointmentNo");

            entity.Property(e => e.DoctorId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DoctorID");
            entity.Property(e => e.PatientId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PatientID");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DoctorID");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_PatientID");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("pk_DoctorID");

            entity.Property(e => e.DoctorId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DoctorID");
            entity.Property(e => e.DoctorName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fees).HasColumnType("money");
            entity.Property(e => e.Specialization)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("pk_PatientID");

            entity.Property(e => e.PatientId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PatientID");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PatientName)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ListOfAppointments>().HasNoKey();

        modelBuilder.HasDbFunction(typeof(PolyclinicDbContext).GetMethod(nameof(ufn_CalculateDoctorFees), new[] {typeof(string), typeof(DateTime)} ))
            .HasName("ufn_CalculateDoctorFees")
            .HasSchema("dbo");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    [DbFunction("ufn_CalculateDoctorFees", "dbo")]
    public static decimal ufn_CalculateDoctorFees(string doctorId, DateTime date)
    {
        throw new NotImplementedException();
    }
}
