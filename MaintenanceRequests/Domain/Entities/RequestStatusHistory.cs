using MaintenanceRequests.Domain.Enums;

namespace MaintenanceRequests.Domain.Entities;

public class RequestStatusHistory
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public RequestStatus OldStatus { get; set; }
    public RequestStatus NewStatus { get; set; }
    public string ChangedByUserId { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
    public string? Comment { get; set; }
}