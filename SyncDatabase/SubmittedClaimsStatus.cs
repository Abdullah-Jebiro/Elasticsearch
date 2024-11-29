// See https://aka.ms/new-console-template for more information
using Nest;

[ElasticsearchType(IdProperty = "ClaimID")]
public class SubmittedClaimsStatus
{
    public DateTime? LastUpdate { get; set; }
    public string? ClaimID { get; set; }
    public string? ProviderID { get; set; }
    public string? ProviderName { get; set; }
    public string? ReceiverID { get; set; }
    public string? ReceiverName { get; set; }
    public string? PayerName { get; set; }
    public string? PayerID { get; set; }
    public DateTime? EncounterEndDate { get; set; }
    public decimal? InitialSubmissionAmount { get; set; }
    // Uncomment additional properties if needed
    // public string? LastSubmissionLevel { get; set; }
    // public decimal? LastResubmissionAmount { get; set; }
    // public decimal? TotalClaimedAmount { get; set; }
    // public decimal? RemittedAmount { get; set; }
    // public decimal? PaymentAmount { get; set; }
    // public decimal? RejectedAmount { get; set; }

    public override string ToString()
    {
        return $"ClaimID: {ClaimID}, ProviderID: {ProviderID}, ProviderName: {ProviderName}, " +
               $"ReceiverID: {ReceiverID}, ReceiverName: {ReceiverName}, PayerName: {PayerName}, " +
               $"PayerID: {PayerID}, EncounterEndDate: {EncounterEndDate?.ToString("o")}, " +
               $"InitialSubmissionAmount: {InitialSubmissionAmount}, LastUpdate: {LastUpdate?.ToString("o")+
               "\n\r________________________________________________________________________"}";
    }
}
