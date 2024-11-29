// See https://aka.ms/new-console-template for more information
using System;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

public class ElasticClientsManager
{
    private readonly ElasticsearchClient _client;

    public ElasticClientsManager()
    {
        _client = new ElasticsearchClient(new Uri("http://localhost:9200"));
    }


    public async Task<PagedList<SubmissionSummaryResponse>> ExecuteSearch()
    {
        int from = 0;
        var providerIds = new List<string> { "MF36!68SE", "MF168SE234", "M68SEF5678" };

        // Execute the search query
        var searchResponse = await _client.SearchAsync<SubmissionSummaryResponse>(s => s
            .Index("report-submission-summary")
            .From(from)
            .Size(10)
            .Query(q => q
                .Bool(b => b
                    .Must(mu =>
                    {
                    mu.Terms(ts => ts
                        .Field(f => f.ProviderId)
                        .Term(new Elastic.Clients.Elasticsearch.QueryDsl.TermsQueryField(providerIds.Select(id => FieldValue.String(id)).ToArray()))

                    );

                    mu.Term(t => t.Field(f => f.IsDeleted).Value(0));

        ); }

        if (!searchResponse.IsValidResponse)
            throw new Exception($"Error executing search: {searchResponse.ElasticsearchServerError?.Error?.Reason}");


        return new PagedList<SubmissionSummaryResponse>(
            searchResponse.Documents.ToList(),
            1,
            10,
            (int)searchResponse.Total
        );

    }
 


    // Method to index a single document
    public async Task IndexSingleDocumentAsync(string indexName)
        {
            Employee employee = new Employee
            {
                Name = "Ali Ali",
                Age = 30,
                Department = "HR"
            };

            var response = await _client.IndexAsync(employee, i => i.Index(indexName));

            if (response.IsValidResponse)
            {
                Console.WriteLine("Document indexed successfully.");
            }
            else
            {
                Console.WriteLine("Error indexing document: " + response.DebugInformation);
            }
        }

    public async Task<long> CountDocumentsAsync(string indexName)
    {
        var countResponse = await _client.CountAsync<object>(c => c.Indices(indexName));

        if (!countResponse.IsValidResponse)
        {
            throw new Exception($"Error executing count: {countResponse.ElasticsearchServerError?.Error?.Reason}");
        }

        return countResponse.Count;
    }
}











