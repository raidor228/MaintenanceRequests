namespace MaintenanceRequests.Domain.Entities;

public class RequestComment
{
    public int Id { get; private set; }
    public int RequestId { get; private set; }
    public string UserId { get; private set; }
    public string Text { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public RequestComment(int requestId, string userId, string text)
    {
        if (requestId <= 0)
        {
            throw new ArgumentException("Некорректный идентификатор заявки.", nameof(requestId));
        }

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("Необходимо указать пользователя.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Комментарий не может быть пустым.", nameof(text));
        }

        if (text.Length > 2000)
        {
            throw new ArgumentException("Комментарий не может быть длиннее 2000 символов.", nameof(text));
        }

        RequestId = requestId;
        UserId = userId;
        Text = text;
        CreatedAt = DateTime.UtcNow;
    }

    private RequestComment()
    {
        
    }
}