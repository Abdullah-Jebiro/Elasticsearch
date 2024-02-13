// See https://aka.ms/new-console-template for more information
using Nest;

public class ElasticsearchManager
{
    private ElasticClient _client;
    public ElasticsearchManager()
    {
        var uri = new Uri("http://localhost:9200");
        var settings = new ConnectionSettings(uri).DefaultIndex("employees");
        _client = new ElasticClient(settings);
    }

    public void CreateIndex()
    {
        var createIndexResponse = _client.Indices.Create("employees", e => e
            .Map<Employee>(m => m.AutoMap<Employee>())
        );
    }

    public void IndexSingleDocument()
    {
        Employee employee = new Employee()
        {
            Name = "Ali Ali",
            Age = 30,
            Department = "HR"
        };
        var indexResponse = _client.IndexDocument(employee);
    }

    public void IndexMultipleDocuments()
    {
        var employees = new List<Employee>
    {
        new Employee { Name = "Ahmed Ali", Age = 30, Department = "Human Resources" },
        new Employee { Name = "Omar Ali", Age = 35, Department = "HR" },
    };

        var indexManyResponse = _client.IndexMany(employees);

        if (indexManyResponse.Errors)
        {
            foreach (var itemWithError in indexManyResponse.ItemsWithErrors)
            {
                Console.WriteLine("Failed to index document {0}: {1}", itemWithError.Id, itemWithError.Error);
            }
        }
    }

    public async Task SearchAllDocuments()
    {
        var searchResponse = await _client.SearchAsync<Employee>(s => s
            .Query(q => q.MatchAll())
        );

        searchResponse.Documents.Print();
    }

    public async Task StructuredSearchWithDateRangeFilter()
    {
        var searchResponse = await _client.SearchAsync<Employee>(s => s
            .Query(q => q
                .Bool(b => b
                    .Filter(bf => bf
                        .DateRange(r => r
                            .Field(f => f.Created)
                            .GreaterThanOrEquals(new DateTime(2023, 01, 01))
                            .LessThan(new DateTime(2027, 01, 01))
                        )
                    )
                )
            )
        );

        searchResponse.Documents.Print();
    }

    public async Task UnstructuredSearchWithMatchQuery()
    {
        var searchResponse = await _client.SearchAsync<Employee>(s => s
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Department)
                    .Query("Resources")
                )
            )
        );

        searchResponse.Documents.Print();
    }

    public async Task PaginationWithParameters(int from, int size)
    {
        var searchResponse = await _client.SearchAsync<Employee>(s => s
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Name)
                    .Query("Omar")
                )
            )
            .From(from)
            .Size(size)
        );

        searchResponse.Documents.Print();
    }

    public async Task SearchWithSourceFiltering()
    {
        var searchResponse = await _client.SearchAsync<Employee>(s => s
            .Source(sf => sf
                .Includes(i => i
                    .Fields(
                        f => f.Name
                    )
                )
                .Excludes(e => e
                    .Fields("C*")
                )
            )
            .Query(q => q
                .MatchAll()
            )
        );

        searchResponse.Documents.Print();
    }

    public async Task ComplexSearch()
    {
        var searchResponse = await _client.SearchAsync<Employee>(s => s
            .Query(q => q
                .Bool(b => b
                    .Must(mu => mu
                        .Match(m => m
                            .Field(f => f.Name)
                            .Query("Ahmed")
                        ),
                        mu => mu
                        .Match(m => m
                            .Field(f => f.Name)
                            .Query("ALi")
                        )
                    )
                    .Filter(fi => fi
                        .DateRange(r => r
                            .Field(f => f.Created)
                            .GreaterThanOrEquals(new DateTime(2017, 01, 01))
                            .LessThan(new DateTime(2038, 01, 01))
                        )
                    )
                )
            )
        );

        searchResponse.Documents.Print();
    }

    public void DeleteAllDocuments()
    {
        var response = _client.DeleteByQuery<Employee>(d => d
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

    public async Task DelaySeconds(int seconds)
    {
        Console.WriteLine($"Waiting for {seconds} seconds...");
        await Task.Delay(seconds * 1000);
        Console.WriteLine($"{seconds} seconds have passed.\n\n");

    }
}

