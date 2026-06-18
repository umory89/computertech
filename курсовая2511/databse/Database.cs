
using Microsoft.EntityFrameworkCore;
using TechAccounting.Models.Entities;
using TechAccounting.Models.Enums;
using курсовая2511.Models;

namespace TechAccounting.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<EquipmentAssignment> EquipmentAssignments { get; set; }
        public DbSet<AssignmentHistory> AssignmentHistories { get; set; }

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
                new EquipmentType { Id = Guid.Parse("1"), Name = "Computer",  Description = "ПК" },
                new EquipmentType { Id = Guid.Parse("2"), Name = "Laptop",  Description = "Ноутбук" },
                new EquipmentType { Id = Guid.Parse("3"), Name = "Monitor",  Description = "Монитор" },
                new EquipmentType { Id = Guid.Parse("4"), Name = "Printer",  Description = "Принтер" },
                new EquipmentType { Id = Guid.Parse("5"), Name = "MFP",  Description = "МФУ" },
                new EquipmentType { Id = Guid.Parse("6"), Name = "Scanner",  Description = "Сканер" },
                new EquipmentType { Id = Guid.Parse("7"), Name = "Server",  Description = "Сервер" },
                new EquipmentType { Id = Guid.Parse("8"), Name = "UPS", Description = "ИБП" },
                new EquipmentType { Id = Guid.Parse("9"), Name = "Router",  Description = "Маршрутизатор" },
                new EquipmentType { Id = Guid.Parse("10"), Name = "Switch",  Description = "Коммутатор" },
                new EquipmentType { Id = Guid.Parse("11"), Name = "Periphery",  Description = "Периферия" },
                new EquipmentType { Id = Guid.Parse("12"), Name = "Other",  Description = "Прочее" }
            );
        }
    }
}