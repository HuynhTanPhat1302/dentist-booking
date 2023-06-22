using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace DentistBooking.Infrastructure
{
    public partial class DentistBookingContext : DbContext
    {
        private readonly IConfiguration? _configuration;



        public DentistBookingContext(DbContextOptions<DentistBookingContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<AppointmentDetail> AppointmentDetails { get; set; } = null!;
        public virtual DbSet<Dentist> Dentists { get; set; } = null!;
        public virtual DbSet<DentistAvailability> DentistAvailabilities { get; set; } = null!;
        public virtual DbSet<Illness> Illnesses { get; set; } = null!;
        public virtual DbSet<MedicalRecord> MedicalRecords { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<ProposeAppointment> ProposeAppointments { get; set; } = null!;
        public virtual DbSet<Treatment> Treatments { get; set; } = null!;
        public virtual DbSet<staff> staff { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");

                entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");

                entity.Property(e => e.Datetime).HasColumnType("datetime");

                entity.Property(e => e.DentistId).HasColumnName("DentistID");

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.StaffId).HasColumnName("StaffID");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.Dentist)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.DentistId)
                    .HasConstraintName("FK__Appointme__Denti__38996AB5");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK__Appointme__Patie__37A5467C");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK__Appointme__Staff__398D8EEE");
            });

            modelBuilder.Entity<AppointmentDetail>(entity =>
            {
                entity.ToTable("AppointmentDetail");

                entity.Property(e => e.AppointmentDetailId).HasColumnName("AppointmentDetailID");

                entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");

                entity.Property(e => e.MedicalRecordId).HasColumnName("MedicalRecordID");

                entity.HasOne(d => d.Appointment)
                    .WithMany(p => p.AppointmentDetails)
                    .HasForeignKey(d => d.AppointmentId)
                    .HasConstraintName("FK__Appointme__Appoi__3C69FB99");

                entity.HasOne(d => d.MedicalRecord)
                    .WithMany(p => p.AppointmentDetails)
                    .HasForeignKey(d => d.MedicalRecordId)
                    .HasConstraintName("FK__Appointme__Medic__3D5E1FD2");
            });

            modelBuilder.Entity<Dentist>(entity =>
            {
                entity.ToTable("Dentist");

                entity.Property(e => e.DentistId).HasColumnName("DentistID");

                entity.Property(e => e.DentistName).HasMaxLength(150);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            });

            modelBuilder.Entity<DentistAvailability>(entity =>
            {
                entity.HasKey(e => e.AvailabilityId)
                    .HasName("PK__DentistA__DA397991E28B9C01");

                entity.ToTable("DentistAvailability");

                entity.Property(e => e.AvailabilityId).HasColumnName("AvailabilityID");

                entity.Property(e => e.DayOfWeek).HasMaxLength(20);

                entity.Property(e => e.DentistId).HasColumnName("DentistID");

                entity.HasOne(d => d.Dentist)
                    .WithMany(p => p.DentistAvailabilities)
                    .HasForeignKey(d => d.DentistId)
                    .HasConstraintName("FK__DentistAv__Denti__403A8C7D");
            });

            modelBuilder.Entity<Illness>(entity =>
            {
                entity.ToTable("Illness");

                entity.Property(e => e.IllnessId).HasColumnName("IllnessID");

                entity.Property(e => e.IllnessName).HasMaxLength(150);
            });

            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.ToTable("MedicalRecord");

                entity.Property(e => e.MedicalRecordId).HasColumnName("MedicalRecordID");

                entity.Property(e => e.DentistId).HasColumnName("DentistID");

                entity.Property(e => e.IllnessId).HasColumnName("IllnessID");

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.TreatmentId).HasColumnName("TreatmentID");

                entity.HasOne(d => d.Dentist)
                    .WithMany(p => p.MedicalRecords)
                    .HasForeignKey(d => d.DentistId)
                    .HasConstraintName("FK__MedicalRe__Denti__32E0915F");

                entity.HasOne(d => d.Illness)
                    .WithMany(p => p.MedicalRecords)
                    .HasForeignKey(d => d.IllnessId)
                    .HasConstraintName("FK__MedicalRe__Illne__33D4B598");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.MedicalRecords)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK__MedicalRe__Patie__31EC6D26");

                entity.HasOne(d => d.Treatment)
                    .WithMany(p => p.MedicalRecords)
                    .HasForeignKey(d => d.TreatmentId)
                    .HasConstraintName("FK__MedicalRe__Treat__34C8D9D1");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");

                entity.HasIndex(e => e.Email, "UQ__Patient__A9D10534EBCFE825")
                    .IsUnique();

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.PatientCode).HasMaxLength(20);

                entity.Property(e => e.PatientName).HasMaxLength(150);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            });

            modelBuilder.Entity<ProposeAppointment>(entity =>
            {
                entity.ToTable("ProposeAppointment");

                entity.Property(e => e.ProposeAppointmentId).HasColumnName("ProposeAppointmentID");

                entity.Property(e => e.Datetime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(e => e.Patient)
    .WithMany()
    .HasForeignKey(e => e.PatientId)
    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Treatment>(entity =>
            {
                entity.ToTable("Treatment");

                entity.Property(e => e.TreatmentId).HasColumnName("TreatmentID");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.TreatmentName).HasMaxLength(150);
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.ToTable("Staff");

                entity.HasIndex(e => e.Email, "UQ__Staff__A9D10534EFD242DF")
                    .IsUnique();

                entity.Property(e => e.StaffId).HasColumnName("StaffID");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.StaffName).HasMaxLength(150);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
