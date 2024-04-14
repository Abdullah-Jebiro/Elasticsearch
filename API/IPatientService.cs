// See https://aka.ms/new-console-template for more information

public interface IPatientService
{
    Task DeleteAllPatients();
    Task IndexAllPatientsAsync();
    Task UpdatePatientsFromDatabaseAsync();
}