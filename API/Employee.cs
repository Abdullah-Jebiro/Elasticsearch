// See https://aka.ms/new-console-template for more information

using Mapster;
using System.Text.Json.Serialization;






public interface IPagedList<T>
    where T : class
{
    int TotalPages { get; }
    bool HasPrevious { get; }
    bool HasNext { get; }
    IReadOnlyList<T> Items { get; init; }
    int TotalCount { get; init; }
    int PageNumber { get; init; }
    int PageSize { get; init; }

    IPagedList<TR> MapTo<TR>(Func<T, TR> map)
        where TR : class;
    IPagedList<TR> MapTo<TR>()
       where TR : class;
}

public record PagedList<T>(IReadOnlyList<T> Items, int PageNumber, int PageSize, int TotalCount) : IPagedList<T>
    where T : class
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
    public IPagedList<TR> MapTo<TR>(Func<T, TR> map)
        where TR : class
    {
        return new PagedList<TR>(Items.Select(map).ToList(), PageNumber, PageSize, TotalCount);
    }
    public IPagedList<TR> MapTo<TR>()
        where TR : class
    {
        return new PagedList<TR>(Items.Adapt<IReadOnlyList<TR>>(), PageNumber, PageSize, TotalCount);
    }
}

public class SubmissionSummaryResponse 
{

    [JsonPropertyName("SubmissionAmount")]
    public decimal? SubmissionAmount { get; set; }

    [JsonPropertyName("MemberId")]
    public string? MemberId { get; set; }

    [JsonPropertyName("ClaimId")]
    public string ClaimId { get; set; } = string.Empty;

    [JsonPropertyName("EncounterEndDate")]
    public DateTime? EncounterEndDate { get; set; }

    [JsonPropertyName("@timestamp")]
    public DateTime? Timestamp { get; set; }

    [JsonPropertyName("@version")]
    public string? Version { get; set; }

    [JsonPropertyName("ProviderName")]
    public string? ProviderName { get; set; }

    [JsonPropertyName("ReceiverName")]
    public string? ReceiverName { get; set; }

    [JsonPropertyName("PatientShare")]
    public decimal? PatientShare { get; set; }

    [JsonPropertyName("EncounterPatientId")]
    public string? EncounterPatientId { get; set; }

    [JsonPropertyName("FileName")]
    public string? FileName { get; set; }

    [JsonPropertyName("ExternalId")]
    public string ExternalId { get; set; }

    [JsonPropertyName("ReceiverId")]
    public string? ReceiverId { get; set; }

    [JsonPropertyName("EncounterType")]
    public string? EncounterType { get; set; }

    [JsonPropertyName("PayerId")]
    public string? PayerId { get; set; }

    [JsonPropertyName("ClaimType")]
    public string? ClaimType { get; set; }

    [JsonPropertyName("PayerName")]
    public string? PayerName { get; set; }

    [JsonPropertyName("SubmissionDate")]
    public DateTime? SubmissionDate { get; set; }

    [JsonPropertyName("BillingMonth")]
    public string? BillingMonth { get; set; }

    [JsonPropertyName("PlanName")]
    public string? PlanName { get; set; }

    [JsonPropertyName("LastUpdateDate")]
    public DateTime LastUpdateDate { get; set; }

    [JsonPropertyName("EncounterStartDate")]
    public DateTime? EncounterStartDate { get; set; }

    [JsonPropertyName("ProviderId")]
    public string? ProviderId { get; set; }

    [JsonPropertyName("IsDeleted")]
    public int IsDeleted { get; set; }
}