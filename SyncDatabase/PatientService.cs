// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Nest;

public class PatientService
{


    ElasticClient _client = new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("patients"));
    DataContext _context = new DataContext();
    public async Task UpdatePatientFromDatabaseAsync()
    {
        try
        {
           
            // Retrieve entities to update
            var entitiesToUpdate = await _context.UpdateEntities.ToListAsync();

            foreach (var entity in entitiesToUpdate)
            {
                // Retrieve patients from the database
                var patients = await _context.Patients
                    .FirstOrDefaultAsync(r => r.Id == entity.EntityId);

                if (patients != null)
                {
                    // Create ReservationDto from Reservation
                   
                    // Index the patients data into Elasticsearch
                    var response = await _client.IndexDocumentAsync(patients);

                    // Check if indexing was successful
                    if (response.IsValid)
                    {
                        // If successful, delete the item from updateEntities table
                        _context.UpdateEntities.Remove(entity);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Handle indexing failure
                        Console.WriteLine($"Failed to index patients with ID {entity.EntityId}: {response.ServerError}");
                    }
                }
                else
                {
                    // Handle case where patients does not exist
                    Console.WriteLine($"Reservation with ID {entity.EntityId} does not exist in the database.");
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    public async Task IndexAllPatientAsync()
    {


        var createIndexResponse = _client.Indices.Create("patients", e => e
            .Map<Patient>(m => m.AutoMap<Patient>())
        );
        var patients = await _context.Patients.ToListAsync();

        int pageNumber = 1;
        int pageSize = 1000;
        var totalReservations = patients.Count;
        var paginationMetaData = new PaginationMetaData(totalReservations, pageSize, pageNumber);

        while (pageNumber <= paginationMetaData.TotalPageCount)
        {
            var result = patients.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var indexManyResponse = await _client.IndexManyAsync(result);

            if (indexManyResponse.Errors)
            {
                foreach (var itemWithError in indexManyResponse.ItemsWithErrors)
                {
                    Console.WriteLine("Failed to index document {0}: {1}", itemWithError.Id, itemWithError.Error);
                }
            }
            pageNumber++;
        }
    }

    public async Task DeleteAllReservations()
    {
        var response = _client.DeleteByQuery<Patient>(d => d.Query(q => q.MatchAll()));

        if (response.IsValid)
        {
           Console.WriteLine("All documents deleted successfully.");
        }
        else
        {
            Console.WriteLine("An error occurred while deleting documents.");
            Console.WriteLine($"Error: {response.ServerError?.Error?.Reason}");
        }
    }
}
