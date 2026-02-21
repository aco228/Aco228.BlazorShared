namespace Aco228.BlazorShared.Models;

public class VisibilityModel
{
    public bool IsVisible { get; set; } = false;

    public VisibilityModel(bool isVisible = false)
    {
        IsVisible = isVisible;
    }

    public void OnChange()
    {
        IsVisible = !IsVisible;
    }
}