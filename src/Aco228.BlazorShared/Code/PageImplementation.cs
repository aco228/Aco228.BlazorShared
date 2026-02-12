using Aco228.Common.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Aco228.BlazorShared.Code;

public abstract class PageImplementation : ComponentBase, IDisposable
{
    [Inject] public NavigationManager Navigation { get; set; }
    [Inject] public INotificationMediator Notifications { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    public EventHandler EventStateHasChanged { get; set; }

    public bool IsInitialized { get; set; } = false;
    public abstract string Title { get; }
    public virtual string Subtitle { get; } = string.Empty;
    public string FatalErrorMessage { get; set; } = string.Empty;
    public bool IsLoading { get; set; } = false;
    
    public bool IsActionLoading { get; set; } = false;
    public string ErrorMessage { get; set; } = string.Empty;

    public bool CombinedIsLoading => IsLoading || IsActionLoading;

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
    protected void SetErrorMessage(string? errorMessage = null)
    {
        ErrorMessage = errorMessage ?? string.Empty;
        InvokeStateHasChanged();
    }

    public void SetFatalErrorMessage(string errorMessage)
    {
        FatalErrorMessage = errorMessage;
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