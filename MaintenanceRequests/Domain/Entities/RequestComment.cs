namespace MaintenanceRequests.Domain.Entities;

public class RequestComment
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}