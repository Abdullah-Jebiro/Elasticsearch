using Microsoft.EntityFrameworkCore;
using Nest;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var indexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}" +
    $"-{environment?.ToLower().Replace(".","-")}-{DateTime.UtcNow:yyyy-MM}";

Log.Logger = new LoggerConfiguration()
   .Enrich.FromLogContext()
   .Enrich.WithExceptionDetails()
   .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
       new Uri("http://localhost:9200/"))
       {
           AutoRegisterTemplate = true,
           IndexFormat= indexFormat

   })
   .CreateLogger();


builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Add services to the container.
builder.Services.AddScoped<IPatientService, PatientService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//# Click the Variables button, above, to create your own variables.
//GET / _cat / indices
//GET / patients
//GET / patients / _search
//{

//    "size": 100     // Number of results per page
//}
/*
-- Trigger definition
CREATE TRIGGER [dbo].[AfterAddPatients]
ON [dbo].[Patients]
AFTER INSERT
AS
BEGIN
    INSERT INTO UpdateEntities ([EntityId], [DateTime], [TypeEntities])
    SELECT Id, GETDATE(), 'Patients' FROM inserted;
END;

-- Inserting data into Patients table
INSERT INTO Patients (FirstName, LastName, DateOfBirth, Gender, Region, IsDeleted)
VALUES
    ('Maher', 'Jb', '1990-01-01', 1, 'Region 1', 0),
    ('Maher', 'Jb', '1985-05-15', 2, 'Region 2', 0),
    ('Maher', 'Jb', '1978-10-25', 1, 'Region 3', 0),
    ('Maher', 'Jb', '1992-03-08', 2, 'Region 4', 0),
    ('Maher', 'Jb', '1980-07-12', 1, 'Region 5', 0);


-- Selecting data from Patients table (limited to 1000 rows)
SELECT TOP (1000) [Id], [FirstName], [LastName], [Region], [Gender], [DateOfBirth], [IsDeleted]
FROM [PatientsDb].[dbo].[Patients]


-- Selecting data from UpdateEntities table (limited to 1000 rows)
SELECT TOP (1000) *
FROM [PatientsDb].[dbo].[UpdateEntities]


-- Trigger definition
CREATE TRIGGER [dbo].[AfterUpdatePatients]
ON [dbo].[Patients]
AFTER UPDATE
AS
BEGIN
    INSERT INTO UpdateEntities ([EntityId], [DateTime], [TypeEntities])
    SELECT Id, GETDATE(), 'Patients' FROM inserted;
END;


-- Update the FirstName of a patient with Id 1
UPDATE Patients
SET FirstName = 'Saad'
WHERE Id = 1;



*/