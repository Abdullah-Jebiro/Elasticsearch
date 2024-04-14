using Microsoft.EntityFrameworkCore;
using Nest;
using Microsoft.Extensions.Logging; // Add this namespace for ILogger

public class PatientService : IPatientService
{
    private readonly ILogger<PatientService> _logger;
    private readonly ElasticClient _client = new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("patients"));
    private readonly DataContext _context;

    public PatientService(ILogger<PatientService> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task UpdatePatientsFromDatabaseAsync()
    {
        try
        {
            var entitiesToUpdate = await _context.UpdateEntities.ToListAsync();

            foreach (var entity in entitiesToUpdate)
            {
                var patients = await _context.Patients
                    .FirstOrDefaultAsync(r => r.Id == entity.EntityId);

                if (patients != null)
                {
                    var response = await _client.IndexDocumentAsync(patients);

                    if (response.IsValid)
                    {
                        _context.UpdateEntities.Remove(entity);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogError($"Failed to index patients with ID {entity.EntityId}: {response.ServerError}");
                    }
                }
                else
                {
                    _logger.LogWarning($"Reservation with ID {entity.EntityId} does not exist in the database.");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred: {ex.Message}");
        }
    }

    public async Task IndexAllPatientsAsync()
    {
        try
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
                        _logger.LogError("Failed to index document {0}: {1}", itemWithError.Id, itemWithError.Error);
                    }
                }
                pageNumber++;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred: {ex.Message}");
        }
    }

    public async Task DeleteAllPatients()
    {
        try
        {
            var response = _client.DeleteByQuery<Patient>(d => d.Query(q => q.MatchAll()));

            if (response.IsValid)
            {
                _logger.LogInformation("All documents deleted successfully.");
            }
            else
            {
                _logger.LogError("An error occurred while deleting documents.");
                _logger.LogError($"Error: {response.ServerError?.Error?.Reason}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred: {ex.Message}");
        }
    }
}
