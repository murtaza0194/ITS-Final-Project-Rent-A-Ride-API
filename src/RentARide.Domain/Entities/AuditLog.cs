namespace RentARide.Domain.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty; // Created, Updated, Deleted
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Changes { get; set; } = string.Empty; // JSON of changes
}
