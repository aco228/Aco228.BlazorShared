using Aco228.BlazorShared.Models;
using Microsoft.AspNetCore.Components;

namespace Aco228.BlazorShared.Code;

public class ComponentImplementation : ComponentBase
{
    [Inject] public NavigationManager Navigation { get; set; }
    [Inject] public INotificationMediator Notifications { get; set; }
    [Parameter] public VisibilityModel? Visibility { get; set; }
    
    public bool IsInitialized { get; set; } = false;
    public EventHandler EventStateHasChanged { get; set; }
    public bool IsActionLoading { get; set; } = false;
    public string ErrorMessage { get; set; } = string.Empty;
    
    public void InvokeStateHasChanged()
    {
        EventStateHasChanged?.Invoke(this, EventArgs.Empty);
        InvokeAsync(StateHasChanged);
    }
    
    protected virtual Task OnFirstRender() => Task.FromResult(true);
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(!firstRender)
            return;

        await OnFirstRender();
        IsInitialized = true;
        InvokeStateHasChanged();
    }
    
    public string GetErrorMessage() => ErrorMessage;
    public bool HasErrorMessage() => !string.IsNullOrEmpty(ErrorMessage);
    public void SetErrorMessage(string? errorMessage = null)
    {
        ErrorMessage = errorMessage ?? string.Empty;
        InvokeStateHasChanged();
    }
    
    protected async Task OnActionExecuting(Func<Task> action, Func<Task>? final = null)
    {
        try
        {
            ErrorMessage = String.Empty;
            IsActionLoading = true;
            InvokeStateHasChanged();

            await action();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.ToString();
        }
        finally
        {
            if (final != null)
                await final();
            
            IsActionLoading = false;
            InvokeStateHasChanged();
        }
    }
    
}