using Microsoft.EntityFrameworkCore;
using System;
using курсовая2511.Models;

namespace TechAccounting.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<EquipmentAssignment> EquipmentAssignments { get; set; }
        public DbSet<AssignmentHistory> AssignmentHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TechAccountingDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(100);

                entity.HasOne(e => e.User)
                      .WithOne(u => u.Employee)
                      .HasForeignKey<Employee>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(100);
                entity.Property(e => e.InventoryNumber).HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

                entity.HasIndex(e => e.SerialNumber).IsUnique();
                entity.HasIndex(e => e.InventoryNumber).IsUnique();

                entity.HasOne(e => e.EquipmentType)
                      .WithMany(t => t.Equipment)
                      .HasForeignKey(e => e.EquipmentTypeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Supplier)
                      .WithMany(s => s.Equipment)
                      .HasForeignKey(e => e.SupplierId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EquipmentAssignment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Employee)
                      .WithMany(emp => emp.Assignments)
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Equipment)
                      .WithMany(eq => eq.Assignments)
                      .HasForeignKey(e => e.EquipmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AssignmentHistory>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Equipment)
                      .WithMany(eq => eq.History)
                      .HasForeignKey(e => e.EquipmentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Employee)
                      .WithMany(emp => emp.History)
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<EquipmentType>().HasData(
                new EquipmentType { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Computer", Icon = "", Description = "ПК" },
                new EquipmentType { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Laptop", Icon = "", Description = "Ноутбук" },
                new EquipmentType { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Monitor", Icon = "", Description = "Монитор" },
                new EquipmentType { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Printer", Icon = "", Description = "Принтер" },
                new EquipmentType { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "MFP", Icon = "", Description = "МФУ" },
                new EquipmentType { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "Scanner", Icon = "", Description = "Сканер" },
                new EquipmentType { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "Server", Icon = "", Description = "Сервер" },
                new EquipmentType { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), Name = "UPS", Icon = "", Description = "ИБП" },
                new EquipmentType { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), Name = "Router", Icon = "", Description = "Маршрутизатор" },
                new EquipmentType { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Switch", Icon = "", Description = "Приставка" },
                new EquipmentType { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Periphery", Icon = "", Description = "Периферия" },
                new EquipmentType { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "Other", Icon = "", Description = "Прочее" }
            );
        }
    }
}