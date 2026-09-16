using MaintenanceRequests.Domain.Enums;

namespace MaintenanceRequests.Domain.Entities;

public class RequestStatusHistory
{
    public int Id { get; private set; }
    public int RequestId { get; private set; }
    public RequestStatus OldStatus { get; private set; }
    public RequestStatus NewStatus { get; private set; }
    public string ChangedByUserId { get; private set; }
    public DateTime ChangedAt { get; private set; }
    public string? Comment { get; private set; }

    public RequestStatusHistory(int requestId, RequestStatus oldStatus, RequestStatus newStatus,
        string changedByUserId, string? comment)
    {
        RequestId = requestId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        ChangedByUserId = changedByUserId;
        Comment = comment;
        ChangedAt = DateTime.UtcNow;
    }

    private RequestStatusHistory()
    {
        
    }
}