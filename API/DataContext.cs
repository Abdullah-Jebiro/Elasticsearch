using Microsoft.EntityFrameworkCore;
using System;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<UpdateEntitie> UpdateEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity properties, relationships, etc.
        modelBuilder.Entity<Patient>().HasKey(p => p.Id);

        // Seed initial data
        modelBuilder.Entity<Patient>().HasData(
          new Patient
          {
              Id = 1,
              FirstName = "Abdullah",
              LastName = "Doe",
              DateOfBirth = new DateTime(1990, 1, 1),
              Gender = 1,
              Region = "Region 1",
              IsDeleted = false,
          },
          new Patient
          {
              Id = 2,
              FirstName = "Omar",
              LastName = "Doe",
              DateOfBirth = new DateTime(1985, 5, 15),
              Gender = 2,
              Region = "Region 2",
              IsDeleted = false,
          },
          // Add more seed data as needed...
          new Patient
          {
              Id = 3,
              FirstName = "John",
              LastName = "Smith",
              DateOfBirth = new DateTime(1978, 10, 25),
              Gender = 1,
              Region = "Region 3",
              IsDeleted = false,
          },
          new Patient
          {
              Id = 4,
              FirstName = "Emily",
              LastName = "Johnson",
              DateOfBirth = new DateTime(1992, 3, 8),
              Gender = 2,
              Region = "Region 4",
              IsDeleted = false,
          },
          new Patient
          {
              Id = 5,
              FirstName = "Michael",
              LastName = "Brown",
              DateOfBirth = new DateTime(1980, 7, 12),
              Gender = 1,
              Region = "Region 5",
              IsDeleted = false,
          }
      );
    }
}
