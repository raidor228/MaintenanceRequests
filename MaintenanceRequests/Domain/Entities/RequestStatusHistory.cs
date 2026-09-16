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

    private RequestStatusHistory()
    {
        ChangedByUserId = string.Empty;
    }

    public RequestStatusHistory(RequestStatus oldStatus, RequestStatus newStatus, 
        string changedByUserId, string? comment = null)
    {
        if (string.IsNullOrWhiteSpace(changedByUserId))
        {
            throw new ArgumentException("Необходимо указать пользователя.", nameof(changedByUserId));
        }

        OldStatus = oldStatus;
        NewStatus = newStatus;
        ChangedByUserId = changedByUserId;
        Comment = comment;
        ChangedAt = DateTime.UtcNow;
    }
}