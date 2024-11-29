// See https://aka.ms/new-console-template for more information
using System;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Aggregations;
using Elastic.Clients.Elasticsearch.Nodes;
using Elastic.Clients.Elasticsearch.QueryDsl;

public class ElasticClientsManager
{
    private readonly ElasticsearchClient client;

    public ElasticClientsManager()
    {
        client = new ElasticsearchClient(new Uri("http://localhost:9200"));
    }


    public async Task<PagedList<SubmissionSummaryResponse>> ExecuteSearch()
    {
        int from = 0;
        var providerIds = new List<string> { "MF3668", "MF168SE234", "MF3048" };
        var fieldValues = providerIds.Select(value => FieldValue.String(value)).ToList();
        var termsQueryField = new TermsQueryField(fieldValues);
        var searchResponse = await client.SearchAsync<SubmissionSummaryResponse>(s => s
            .RequestConfiguration(r=>r.DisableDirectStreaming())
            .Index("report-submission-summary")
            .From(from)
            .Size(10000)
            .Query(q => q
                .Bool(b => b
                    .Must(mu => mu
                        .Terms(ts => ts
                            .Field(f => f.ProviderId.Suffix("keyword"))
                            .Terms(new Elastic.Clients.Elasticsearch.QueryDsl.TermsQueryField(providerIds.Select(id => FieldValue.String(id)).ToArray()))
                        )
                    )
        
                )
            )
        );

        if (!searchResponse.IsValidResponse)
            throw new Exception($"Error executing search: {searchResponse.ElasticsearchServerError?.Error?.Reason}");

        return new PagedList<SubmissionSummaryResponse>(
            searchResponse.Documents.ToList(),
            1,
            10,
            (int)searchResponse.Total
        );
    }


    public async Task ExecuteAggregation()
    {
        var response = await client.SearchAsync<object>(s => s
            .Index("report-submission-summary")
            .Size(0) // We only need aggregation results, not search hits
            .Aggregations(aggs => aggs
                .Add("ProviderId", new AggregationDescriptor<object>()
                    .Terms(t => t
                        .Field("EncounterStartDate.keyword")
                        .Size(5)
                    )
                )
            )
        );

        if (response.IsValidResponse)
        {
            var plansAgg = response.Aggregations.Values.ToList();
            foreach (var item in plansAgg)
            {
                var x = ((Elastic.Clients.Elasticsearch.Aggregations.StringTermsAggregate)item).Buckets;
                foreach (var item1 in x)
                {
                    Console.WriteLine(item1.Key  +"\n"+ item1.DocCount);;
                }
            }


        }
        else
        {
            Console.WriteLine("Error executing aggregation: " + response.DebugInformation);
        }
    }

    public async Task ExecuteAggregation(string indexName, string aggsBy)
    {
        if (string.IsNullOrEmpty(indexName) || string.IsNullOrEmpty(aggsBy))
        {
            return;
        }

        try
        {
            var response = await client.SearchAsync<object>(s => s
                .Index(indexName)
                .Size(0) // Focus on aggregations, not search hits
                .Aggregations(aggs => aggs
                .Add(aggsBy, new AggregationDescriptor<object>()
                    .Terms(t => t
                        .Field(aggsBy)
                        .Size(5))
                    )
                )
            );

            if (response.IsValidResponse)
            {
                var plansAgg = response.Aggregations.Values.ToList();
                foreach (var item in plansAgg)
                {
                    var x = ((Elastic.Clients.Elasticsearch.Aggregations.StringTermsAggregate)item).Buckets;
                    foreach (var item1 in x)
                    {
                        Console.WriteLine(item1.Key + "\n" + item1.DocCount); ;
                    }
                }

            }
            else
            {
                Console.WriteLine("Error executing aggregation: " + response.DebugInformation);
            }
        }
        catch (Exception ex)
        {
        }
    }




    async Task<long> CountDocumentsAsync(string indexName)
    {
        var countResponse = await client.CountAsync<object>(c => c.Indices(indexName));

        if (!countResponse.IsValidResponse)
        {
            throw new Exception($"Error executing count: {countResponse.ElasticsearchServerError?.Error?.Reason}");
        }

        return countResponse.Count;
    }
}











