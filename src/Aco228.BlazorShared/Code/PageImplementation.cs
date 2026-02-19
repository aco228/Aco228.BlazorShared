using Aco228.Common.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Aco228.BlazorShared.Code;

public abstract class PageImplementation : ComponentImplementation
{
    [Inject] public IJSRuntime JsRuntime { get; set; }

    public abstract string Title { get; }
    public virtual string Subtitle { get; } = string.Empty;
    public string FatalErrorMessage { get; set; } = string.Empty;
    
    public bool IsLoading { get; set; } = false;

    public bool CombinedIsLoading => IsLoading || IsActionLoading;


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

    public async Task InvokeTextArea(string className)
    {
        await InvokeAsync(StateHasChanged);
        TasksExtensions.RunWithDelay(500, async () =>
        {
            await JsRuntime.InvokeVoidAsync("resizeTextArea", className);
        }).ConfigureAwait(false);
    }
}