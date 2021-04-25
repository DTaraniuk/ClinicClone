using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ClinicWebApplication
{
    public partial class DBClinicContext : DbContext
    {
        public DBClinicContext()
        {
        }

        public DBClinicContext(DbContextOptions<DBClinicContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Diagnosis> Diagnoses { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<DoctorOffice> DoctorOffices { get; set; }
        public virtual DbSet<DoctorSpeciality> DoctorSpecialities { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<Hospitalization> Hospitalizations { get; set; }
        public virtual DbSet<Illness> Illnesses { get; set; }
        public virtual DbSet<IllnessSymptom> IllnessSymptoms { get; set; }
        public virtual DbSet<Office> Offices { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientSymptom> PatientSymptoms { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Speciality> Specialities { get; set; }
        public virtual DbSet<Symptom> Symptoms { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=NATALI\\SQLEXPRESS; Database=DBClinic; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_Appointments_Patients");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK_Appointments_Schedules");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.DepartmentHead)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.DepartmentHeadId)
                    .HasConstraintName("FK_Departments_Doctors");

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK_Departments_Hospitals");
            });

            modelBuilder.Entity<Diagnosis>(entity =>
            {
                entity.Property(e => e.Time)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Diagnoses)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_Diagnoses_Doctors");

                entity.HasOne(d => d.Illness)
                    .WithMany(p => p.Diagnoses)
                    .HasForeignKey(d => d.IllnessId)
                    .HasConstraintName("FK_Diagnoses_Illnesses");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Diagnoses)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_Diagnoses_Patients");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Doctors_Departments");
            });

            modelBuilder.Entity<DoctorOffice>(entity =>
            {
                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.DoctorOffices)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_DoctorOffices_Doctors");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.DoctorOffices)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK_DoctorOffices_Offices");
            });

            modelBuilder.Entity<DoctorSpeciality>(entity =>
            {
                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.DoctorSpecialities)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_DoctorSpecialities_Doctors");

                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.DoctorSpecialities)
                    .HasForeignKey(d => d.SpecialityId)
                    .HasConstraintName("FK_DoctorSpecialities_Specialities");
            });

            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Hospitals)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Hospitals_Cities");
            });

            modelBuilder.Entity<Hospitalization>(entity =>
            {
                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Hospitalizations)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_Hospitalizations_Patients");

                entity.HasOne(d => d.Ward)
                    .WithMany(p => p.Hospitalizations)
                    .HasForeignKey(d => d.WardId)
                    .HasConstraintName("FK_Hospitalizations_Wards");
            });

            modelBuilder.Entity<Illness>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<IllnessSymptom>(entity =>
            {
                entity.HasOne(d => d.Illness)
                    .WithMany(p => p.IllnessSymptoms)
                    .HasForeignKey(d => d.IllnessId)
                    .HasConstraintName("FK_IllnessSymptoms_Illnesses");

                entity.HasOne(d => d.Symptom)
                    .WithMany(p => p.IllnessSymptoms)
                    .HasForeignKey(d => d.SymptomId)
                    .HasConstraintName("FK_IllnessSymptoms_Symptoms");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Therapist)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.TherapistId)
                    .HasConstraintName("FK_Patients_Doctors");
            });

            modelBuilder.Entity<PatientSymptom>(entity =>
            {
                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientSymptoms)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_PatientSymptoms_Patients");

                entity.HasOne(d => d.Symptom)
                    .WithMany(p => p.PatientSymptoms)
                    .HasForeignKey(d => d.SymptomId)
                    .HasConstraintName("FK_PatientSymptoms_Symptoms");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.DoctorOffice)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.DoctorOfficeId)
                    .HasConstraintName("FK_Schedules_DoctorOffices");
            });

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Symptom>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Wards_Departments");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
