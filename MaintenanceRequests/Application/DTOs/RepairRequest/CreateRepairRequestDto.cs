using MaintenanceRequests.Domain.Enums;

namespace MaintenanceRequests.Application.DTOs.RepairRequest;

public class CreateRepairRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RequestPriority Priority { get; set; }
    public int CategoryId { get; set; }
}