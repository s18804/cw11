using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Models
{
    public class ClinicDbContext : DbContext
    {
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<Medicament> Medicament { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicament { get; set; }

        public ClinicDbContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var docList = new List<Doctor>();
            docList.Add(new Doctor { IdDoctor = 1, FirstName = "Kacper", LastName = "Kowalski", Email = "kk@wp.pl"});
            docList.Add(new Doctor { IdDoctor = 2, FirstName = "Adam", LastName = "Awacki", Email = "aa@wp.pl"});
            
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.IdDoctor);
                entity.Property(e => e.FirstName).HasMaxLength(30).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(70).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.HasData(docList);
            });

            var patList = new List<Patient>();
            patList.Add(new Patient {IdPatient = 1, FirstName = "Aleksander", LastName = "Boguto", BirthDate = new DateTime(1990, 2, 28)});
            patList.Add(new Patient {IdPatient = 2, FirstName = "Kila", LastName = "Mogila", BirthDate = new DateTime(2000, 1, 20)});
            
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.IdPatient);
                entity.Property(e => e.FirstName).HasMaxLength(30).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(70).IsRequired();
                entity.Property(e => e.BirthDate).IsRequired();
                entity.HasData(patList);
            });
            
            var preList = new List<Prescription>();
            preList.Add(new Prescription {IdPrescription = 1, Date = new DateTime(2020, 2, 1), DueDate = new DateTime(2020, 4, 2), IdDoctor = 1, IdPatient = 1});
            preList.Add(new Prescription {IdPrescription = 2, Date = new DateTime(2020, 2, 1), DueDate = new DateTime(2020, 4, 2), IdDoctor = 2, IdPatient = 2});
            
            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.IdPrescription);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.DueDate).IsRequired();
                entity.HasOne(e => e.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(p => p.IdPatient)
                    .OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Prescription_Patient");
                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(p => p.IdDoctor)
                    .OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Prescription_Doctor");
                entity.HasData(preList);
            });

            var medList = new List<Medicament>();
            medList.Add(new Medicament{IdMedicament = 1, Description = "cos tam robi", Name = "Gazotromil", Type = "typ"});
            medList.Add(new Medicament{IdMedicament = 2, Description = "cos tam robi", Name = "Apomil", Type = "typ2"});

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(e => e.IdMedicament);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Type).HasMaxLength(100).IsRequired();
                entity.HasData(medList);
            });

            var preMedList = new List<PrescriptionMedicament>();
            preMedList.Add(new PrescriptionMedicament {IdMedicament = 1, Details = "", Dose = null, IdPrescription = 1});
            preMedList.Add(new PrescriptionMedicament {IdMedicament = 2, Details = "cos tam", Dose = null, IdPrescription = 2});

            modelBuilder.Entity<PrescriptionMedicament>(entity =>
            {
                entity.HasKey(e => e.IdMedicament);
                entity.HasKey(e => e.IdPrescription);
                entity.HasOne(e => e.Medicament)
                    .WithMany(p => p.PrescriptionMedicament)
                    .HasForeignKey(p => p.IdMedicament)
                    .OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Prescription_Medicament_Medicament");
                entity.HasOne(e => e.Prescription)
                    .WithMany(p => p.PrescriptionMedicament)
                    .HasForeignKey(p => p.IdPrescription)
                    .OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Prescription_Medicament_Prescription");
                entity.Property(e => e.Details).HasMaxLength(100).IsRequired();
                entity.HasData(preMedList);
            });
            modelBuilder.Entity<PrescriptionMedicament>().ToTable("Prescription_Medicament");
        }
    }
}