using Microsoft.Data.SqlClient;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Service
{
    private readonly ElasticClient _client;
    private readonly string _connectionString;

    public Service()
    {
        _client = new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("submitted-claims-status"));
        _connectionString = "Data Source=localhost; Database=Clem.Test;UID=Clem.User;PWD=superclemittance;TrustServerCertificate=True";
        // Uncomment the following line if you wish to use a different connection string
        // _connectionString = "Data Source=M-DB;Initial Catalog=QA.Clemittance.ReadModel;User ID=devsa;Password=o*W9i23yfk!TD6JV;";
    }

    public void DeleteAllDocuments()
    {
        var response = _client.DeleteByQuery<SubmittedClaimsStatus>(d => d
            .Query(q => q.MatchAll())
        );

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

    public async Task IndexAllClaimInfoAsync()
    {
        // Create Elasticsearch index if it doesn't exist
        var createIndexResponse = await _client.Indices.CreateAsync("submitted-claims-status", e => e
            .Map<SubmittedClaimsStatus>(m => m.AutoMap<SubmittedClaimsStatus>())
        );

        var claims = GetClaims();
        int pageNumber = 1;
        int pageSize = 100;
        var totalClaims = claims.Count;
        var totalPages = (int)Math.Ceiling((double)totalClaims / pageSize);

        while (pageNumber <= totalPages)
        {
            var result = claims.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var indexManyResponse = await _client.IndexManyAsync(result);

            if (indexManyResponse.Errors)
            {
                foreach (var itemWithError in indexManyResponse.ItemsWithErrors)
                {
                    Console.WriteLine($"Failed to index document {itemWithError.Id}: {itemWithError.Error}");
                }
            }
            pageNumber++;
        }
    }

    public List<SubmittedClaimsStatus> GetClaims()
    {
        var claims = new List<SubmittedClaimsStatus>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = @"
                SELECT 
                    COALESCE([LastUpdateDate], [CreationDate]) AS LastUpdate,
                    c.ExternalId AS ClaimID,
                    c.ProviderID,
                    c.ProviderName,
                    c.ReceiverID,
                    c.ReceiverName,
                    c.PayerName,
                    c.PayerID,
                    c.Encounter_EndDate AS EncounterEndDate,
                    c.Net AS InitialSubmissionAmount
                FROM [Clem.Test].[dbo].[Claim] c";

            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var claim = new SubmittedClaimsStatus
                        {
                            LastUpdate = reader.IsDBNull(reader.GetOrdinal("LastUpdate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LastUpdate")),
                            ClaimID = reader.IsDBNull(reader.GetOrdinal("ClaimID")) ? null : reader.GetString(reader.GetOrdinal("ClaimID")),
                            ProviderID = reader.IsDBNull(reader.GetOrdinal("ProviderID")) ? null : reader.GetString(reader.GetOrdinal("ProviderID")),
                            ProviderName = reader.IsDBNull(reader.GetOrdinal("ProviderName")) ? null : reader.GetString(reader.GetOrdinal("ProviderName")),
                            ReceiverID = reader.IsDBNull(reader.GetOrdinal("ReceiverID")) ? null : reader.GetString(reader.GetOrdinal("ReceiverID")),
                            ReceiverName = reader.IsDBNull(reader.GetOrdinal("ReceiverName")) ? null : reader.GetString(reader.GetOrdinal("ReceiverName")),
                            PayerName = reader.IsDBNull(reader.GetOrdinal("PayerName")) ? null : reader.GetString(reader.GetOrdinal("PayerName")),
                            PayerID = reader.IsDBNull(reader.GetOrdinal("PayerID")) ? null : reader.GetString(reader.GetOrdinal("PayerID")),
                            EncounterEndDate = reader.IsDBNull(reader.GetOrdinal("EncounterEndDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EncounterEndDate")),
                            InitialSubmissionAmount = reader.IsDBNull(reader.GetOrdinal("InitialSubmissionAmount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("InitialSubmissionAmount"))
                        };

                        claims.Add(claim);
                    }
                }
            }
        }

        return claims;
    }


    //public async Task SearchFilteredDocuments(string prmTenant, DateTime? prmEncounterEndDateFrom, DateTime? prmEncounterEndDateTo, string? prmProviderId, string? prmPayerId, string? prmReceiverId)
    //{
    //    var searchResponse = await _client.SearchAsync<SubmittedClaimsStatus>(s => s
    //        .Query(q => q.Bool(b => b
    //            .Must(m => m
    //                .Bool(bq => bq
    //                    .MustNot(mn => mn
    //                        .Exists(e => e.Field(f => f.OriginalDuplicateId))
    //                    )
    //                    .Must(must => must
    //                        .Bool(bq => bq
    //                            .Must(must => must
    //                                .Term(t => t.ClaimStatus, 13) // ClaimStatus NOT IN (13)
    //                            )
    //                            .Filter(f => f
    //                                .Terms(t => t
    //                                    .Field(f => f.ProviderID)
    //                                    .Terms(prmTenant.Split(',')) // ProviderID IN (SELECT [Value] FROM fn_Split(@prmTenant, ','))
    //                                )
    //                            )
    //                        )
    //                    )
    //                )
    //            )
    //            .Filter(f =>
    //                (prmEncounterEndDateFrom == null || prmEncounterEndDateTo == null ||
    //                 f.DateRange(r => r
    //                     .Field(f => f.EncounterEndDate)
    //                     .GreaterThanOrEquals(prmEncounterEndDateFrom)
    //                     .LessThan(prmEncounterEndDateTo.Value.AddDays(1))
    //                 )) // Handle date range
    //            )
    //            .Filter(f =>
    //                (string.IsNullOrEmpty(prmProviderId) || f.Term(t => t.ProviderID, prmProviderId)) &&
    //                (string.IsNullOrEmpty(prmPayerId) || f.Term(t => t.PayerID, prmPayerId)) &&
    //                (string.IsNullOrEmpty(prmReceiverId) || f.Term(t => t.ReceiverID, prmReceiverId))
    //            )
    //        ));

    //    // Print or process the retrieved documents
    //    searchResponse.Documents.Print();
    //}

    public async Task SearchByProviderId(string providerId)
    {
        var searchResponse = await _client.SearchAsync<SubmittedClaimsStatus>(s => s
            .Query(q => q
                .Bool(b => b
                    .Must(mu => mu
                        .Match(m => m
                            .Field(f => f.ProviderID)
                            .Query(providerId)
                        )
                    )
                )
            )
        );

        searchResponse.Documents.Print();
    }
}