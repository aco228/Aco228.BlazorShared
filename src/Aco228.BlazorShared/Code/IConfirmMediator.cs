using Aco228.BlazorShared.Models;

namespace Aco228.BlazorShared.Code;

public interface IConfirmMediator
{
    event EventHandler<ConfirmRequest> OnConfirmReceived;
    void AskForConfirmation(string title, Action onConfirmAction, string? description = null, string? okText = null);
}

public class ConfirmMediator : IConfirmMediator
{
    public event EventHandler<ConfirmRequest>? OnConfirmReceived;
    
    public void AskForConfirmation(string title, Action onConfirmAction, string? description = null, string? okText = null)
    {
        OnConfirmReceived?.Invoke(this, new()
        {
            Title = title,
            ConfirmAction = onConfirmAction,
            Description = description,
            OkText = okText ?? "Confirm"
        });
    }
}