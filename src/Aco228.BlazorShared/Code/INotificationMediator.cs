using Aco228.BlazorShared.Models;

namespace Aco228.BlazorShared.Code;

public interface INotificationMediator
{
    event EventHandler<NotificationDto> OnNotificationReceived;
    void NotifySuccess(string title, string description);
    void NotifyError(string title, string description);
}
