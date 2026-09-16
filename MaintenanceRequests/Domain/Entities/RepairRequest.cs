using MaintenanceRequests.Domain.Enums;

namespace MaintenanceRequests.Domain.Entities;

public class RepairRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RequestStatus Status { get; private set; }
    public RequestPriority Priority { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public string ClientId { get; set; } = string.Empty;
    public string? WorkerId { get; private set; }
    public int CategoryId { get; set; }
    public RepairCategory? Category { get; set; }
    public ICollection<RequestComment> Comments { get; set; } = new List<RequestComment>();
    public ICollection<RequestStatusHistory> StatusHistory { get; set; } = new List<RequestStatusHistory>();
    
    public RepairRequest(string title, string description, RequestPriority priority, string clientId, int categoryId)
    {
        Title = title;
        Description = description;
        Priority = priority;
        ClientId = clientId;
        CategoryId = categoryId;

        Status = RequestStatus.New;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void Approve()
    {
        if (Status != RequestStatus.PendingApproval)
        {
            throw new InvalidOperationException("Заявку можно одобрить только после отправки на рассмотрение.");
        }

        Status = RequestStatus.Approved;
    }
    
    public void Reject()
    {
        if (Status != RequestStatus.PendingApproval)
        {
            throw new InvalidOperationException("Отклонить можно только заявку на рассмотрении.");
        }

        Status = RequestStatus.Rejected;
    }
    
    public void Assign(string workerId)
    {
        if (Status != RequestStatus.Approved)
        {
            throw new InvalidOperationException("Работника можно назначить только на одобренную заявку.");
        }

        if (string.IsNullOrWhiteSpace(workerId))
        {
            throw new ArgumentException("Необходимо указать работника.", nameof(workerId));
        }

        WorkerId = workerId;
        Status = RequestStatus.Assigned;
    }
    
    public void Start()
    {
        if (Status != RequestStatus.Assigned && Status != RequestStatus.Waiting)
        {
            throw new InvalidOperationException("Заявку нельзя начать выполнять в текущем состоянии.");
        }

        Status = RequestStatus.InProgress;
    }
    
    public void SetWaiting()
    {
        if (Status != RequestStatus.InProgress)
        {
            throw new InvalidOperationException("Приостановить можно только выполняемую заявку.");
        }

        Status = RequestStatus.Waiting;
    }
    
    public void Complete()
    {
        if (Status != RequestStatus.InProgress)
        {
            throw new InvalidOperationException("Завершить можно только выполняемую заявку.");
        }

        Status = RequestStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }
    
    public void Close()
    {
        if (Status != RequestStatus.Completed)
        {
            throw new InvalidOperationException("Закрыть можно только завершённую заявку.");
        }

        Status = RequestStatus.Closed;
    }
}