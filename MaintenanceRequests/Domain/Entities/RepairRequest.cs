using MaintenanceRequests.Domain.Enums;

namespace MaintenanceRequests.Domain.Entities;

public class RepairRequest
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public RequestStatus Status { get; private set; }
    public RequestPriority Priority { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public string ClientId { get; private set; }
    public string? WorkerId { get; private set; }
    public int CategoryId { get; private set; }
    public RepairCategory? Category { get; private set; }
    public ICollection<RequestComment> Comments { get; private set; } = new List<RequestComment>();
    public ICollection<RequestStatusHistory> StatusHistory { get; private set; } = new List<RequestStatusHistory>();
    
    private const int MaxTitleLength = 100;
    private const int MaxDescriptionLength = 2000;
    
    public RepairRequest(string title, string description, RequestPriority priority, string clientId, int categoryId)
    {
        ValidateTitle(title);
        ValidateDescription(description);

        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new ArgumentException("Необходимо указать клиента.", nameof(clientId));
        }

        Title = title;
        Description = description;
        Priority = priority;
        ClientId = clientId;
        CategoryId = categoryId;

        Status = RequestStatus.New;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void SubmitForApproval(string changedByUserId, string? comment = null)
    {
        if (Status != RequestStatus.New)
        {
            throw new InvalidOperationException("На рассмотрение можно отправить только новую заявку.");
        }

        ChangeStatus(RequestStatus.PendingApproval, changedByUserId, comment);
    }

    public void Approve(string changedByUserId, string? comment = null)
    {
        if (Status != RequestStatus.PendingApproval)
        {
            throw new InvalidOperationException("Одобрить можно только заявку на рассмотрении.");
        }

        ChangeStatus(RequestStatus.Approved, changedByUserId, comment);
    }

    public void Reject(string changedByUserId, string? comment = null)
    {
        if (Status != RequestStatus.PendingApproval)
        {
            throw new InvalidOperationException("Отклонить можно только заявку на рассмотрении.");
        }

        ChangeStatus(RequestStatus.Rejected, changedByUserId, comment);
    }

    public void Assign(string workerId, string changedByUserId, string? comment = null)
    {
        if (Status != RequestStatus.Approved)
        {
            throw new InvalidOperationException("Назначить работника можно только на одобренную заявку.");
        }

        if (string.IsNullOrWhiteSpace(workerId))
        {
            throw new ArgumentException("Необходимо указать работника.", nameof(workerId));
        }

        WorkerId = workerId;
        ChangeStatus(RequestStatus.Assigned, changedByUserId, comment);
    }

    public void Start(string changedByUserId, string? comment = null)
    {
        if (Status != RequestStatus.Assigned && Status != RequestStatus.Waiting)
        {
            throw new InvalidOperationException("Заявку нельзя начать выполнять в текущем состоянии.");
        }

        ChangeStatus(RequestStatus.InProgress, changedByUserId, comment);
    }

    public void SetWaiting(string changedByUserId, string? comment = null)
    {
        if (Status != RequestStatus.InProgress)
        {
            throw new InvalidOperationException("Приостановить можно только выполняемую заявку.");
        }

        ChangeStatus(RequestStatus.Waiting, changedByUserId, comment);
    }

    public void Complete(string changedByUserId, string? comment = null)
    {
        if (Status != RequestStatus.InProgress)
        {
            throw new InvalidOperationException("Завершить можно только выполняемую заявку.");
        }

        CompletedAt = DateTime.UtcNow;
        ChangeStatus(RequestStatus.Completed, changedByUserId, comment);
    }

    public void Close(string changedByUserId, string? comment = null)
    {
        if (Status != RequestStatus.Completed)
        {
            throw new InvalidOperationException("Закрыть можно только завершённую заявку.");
        }

        ChangeStatus(RequestStatus.Closed, changedByUserId, comment);
    }

    public void Cancel(string changedByUserId, string? comment = null)
    {
        if (Status != RequestStatus.New)
        {
            throw new InvalidOperationException("Отменить можно только новую заявку.");
        }

        ChangeStatus(RequestStatus.Cancelled, changedByUserId, comment);
    }
    
    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Название заявки не может быть пустым.", nameof(title));
        }

        if (title.Length > MaxTitleLength)
        {
            throw new ArgumentException(
                $"Название заявки не может быть длиннее {MaxTitleLength} символов.", nameof(title));
        }
    }
    
    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Описание заявки не может быть пустым.", nameof(description));
        }

        if (description.Length > MaxDescriptionLength)
        {
            throw new ArgumentException(
                $"Описание заявки не может быть длиннее {MaxDescriptionLength} символов.", nameof(description));
        }
    }
    
    public void UpdateDetails(string title, string description, RequestPriority priority)
    {
        if (Status == RequestStatus.Closed)
        {
            throw new InvalidOperationException("Закрытую заявку нельзя изменить.");
        }

        if (Status == RequestStatus.Cancelled)
        {
            throw new InvalidOperationException("Отменённую заявку нельзя изменить.");
        }

        ValidateTitle(title);
        ValidateDescription(description);

        Title = title;
        Description = description;
        Priority = priority;
    }
    
    public void AddComment(string userId, string text)
    {
        if (Status == RequestStatus.Closed)
        {
            throw new InvalidOperationException("В закрытую заявку нельзя добавлять комментарии.");
        }

        if (Status == RequestStatus.Cancelled)
        {
            throw new InvalidOperationException("В отменённую заявку нельзя добавлять комментарии.");
        }

        Comments.Add(new RequestComment(Id, userId, text));
    }
    
    private void ChangeStatus(RequestStatus newStatus, string changedByUserId, string? comment = null)
    {
        var oldStatus = Status;
        Status = newStatus;
        StatusHistory.Add(new RequestStatusHistory(Id, oldStatus, newStatus, changedByUserId, comment));
    }
}