namespace Aco228.BlazorShared.Code;

public interface ISidebarMediator
{
    public bool IsVisible { get; set; }
    event EventHandler<bool> OnSidebarVisibilityChanged;
    void ChangeSidebarVisibility(bool? isVisible = null);
}

public class SidebarMediator : ISidebarMediator
{
    public bool IsVisible { get; set; } = true;
    public event EventHandler<bool>? OnSidebarVisibilityChanged;
    
    public void ChangeSidebarVisibility(bool? isVisible = null)
    {
        IsVisible = isVisible ?? !IsVisible;
        OnSidebarVisibilityChanged?.Invoke(this, IsVisible);
    }
}
