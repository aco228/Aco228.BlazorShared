using Aco228.Common.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Aco228.BlazorShared.Code;

public abstract class PageImplementation : ComponentBase, IDisposable
{
    [Inject] public IJSRuntime JsRuntime { get; set; }
    public EventHandler EventStateHasChanged { get; set; }

    public abstract string Title { get; }
    public virtual string Subtitle { get; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public bool IsLoading { get; set; } = false;
    
    public bool IsActionLoading { get; set; } = false;
    public string ActionErrorMessage { get; set; } = string.Empty;

    public bool CombinedIsLoading => IsLoading || IsActionLoading;
    public string CombinedErrorMessage => !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : ActionErrorMessage;

    public string GetErrorMessage() => CombinedErrorMessage;
    public bool HasErrorMessage() => !string.IsNullOrEmpty(ErrorMessage);
    protected void SetErrorMessage(string? errorMessage = null)
    {
        ErrorMessage = errorMessage;
        InvokeStateHasChanged();
    }
    
    protected void OnLoadingCompleted()
    {
        IsLoading = false;
        InvokeStateHasChanged();
    }
    
    protected async Task OnActionExecuting(Func<Task> action, Func<Task>? final = null)
    {
        try
        {
            ActionErrorMessage = String.Empty;
            IsActionLoading = true;
            InvokeStateHasChanged();

            await action();
        }
        catch (Exception ex)
        {
            ActionErrorMessage = ex.ToString();
        }
        finally
        {
            if (final != null)
                await final();
            
            IsActionLoading = false;
            InvokeStateHasChanged();
        }
    }

    public void Dispose()
    {
        OnDispose();
    }

    protected virtual void OnDispose()
    {
        
    }

    public void InvokeStateHasChanged()
    {
        Console.WriteLine("State has changed");
        EventStateHasChanged?.Invoke(this, EventArgs.Empty);
        InvokeAsync(StateHasChanged);
    }

    public async Task InvokeTextArea(string className)
    {
        await InvokeAsync(StateHasChanged);
        TasksExtensions.RunWithDelay(500, async () =>
        {
            await JsRuntime.InvokeVoidAsync("resizeTextArea", className);
        }).ConfigureAwait(false);
    }
}