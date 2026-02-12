using Aco228.BlazorShared.Models;

namespace Aco228.BlazorShared.Code;

public class NotificationMediator : INotificationMediator
{
    public event EventHandler<NotificationDto>? OnNotificationReceived;

    public void NotifySuccess(string title, string description)
    {
        OnNotificationReceived?.Invoke(this, new NotificationDto
        {
            Type = NotificationType.Success,
            Title = title,
            Description = description
        });
    }

    public void NotifyError(string title, string description)
    {
        OnNotificationReceived?.Invoke(this, new NotificationDto
        {
            Type = NotificationType.Error,
            Title = title,
            Description = description
        });
    }
}
