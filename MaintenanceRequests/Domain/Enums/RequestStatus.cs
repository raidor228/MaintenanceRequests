namespace MaintenanceRequests.Domain.Enums;

public enum RequestStatus
{
    New,
    PendingApproval,
    Approved,
    Assigned,
    InProgress,
    Waiting,
    Completed,
    Closed,
    Rejected,
    Cancelled
}