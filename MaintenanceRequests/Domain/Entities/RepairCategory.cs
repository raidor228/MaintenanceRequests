namespace MaintenanceRequests.Domain.Entities;

public class RepairCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    private RepairCategory()
    {
        
    }
}