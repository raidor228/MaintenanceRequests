namespace MaintenanceRequests.Application.DTOs.RepairRequest;

public class AssignRepairRequestDto
{
    public string WorkerId { get; set; } = string.Empty;
    public string? Comment { get; set; }
}