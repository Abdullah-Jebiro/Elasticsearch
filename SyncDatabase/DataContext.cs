// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

public class DataContext :DbContext
{
    public DataContext() { }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<UpdateEntitie> UpdateEntities { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=PatientsDb;Integrated Security=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Patient>().HasData(GetPatients());
    }

    private List<Patient> GetPatients()
    {
        List<Patient> patients = new List<Patient>();

        patients.Add(new Patient
        {
            Id = 1,
            FirstName = "Abdullah",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = 1,
            Region = "Region 1",
            IsDeleted = false,
        });

        patients.Add(new Patient
        {
            Id = 2,
            FirstName = "Omar",
            LastName = "Doe",
            DateOfBirth = new DateTime(1985, 5, 15),
            Gender = 2,
            Region = "Region 2",
            IsDeleted = false,
        });

        // Add more patients as needed...
        patients.Add(new Patient
        {
            Id = 3,
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = new DateTime(1978, 10, 25),
            Gender = 1,
            Region = "Region 3",
            IsDeleted = false,
        });

        patients.Add(new Patient
        {
            Id = 4,
            FirstName = "Emily",
            LastName = "Johnson",
            DateOfBirth = new DateTime(1992, 3, 8),
            Gender = 2,
            Region = "Region 4",
            IsDeleted = false,
        });

        patients.Add(new Patient
        {
            Id = 5,
            FirstName = "Michael",
            LastName = "Brown",
            DateOfBirth = new DateTime(1980, 7, 12),
            Gender = 1,
            Region = "Region 5",
            IsDeleted = false,
        });

        // Add more patients as needed...

        return patients;
    }

}