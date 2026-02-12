namespace Aco228.BlazorShared.Models;

public enum NotificationType
{
    Success,
    Error
}

public class NotificationDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
