namespace Aco228.BlazorShared.Models;

public class VisibilityModel
{
    public bool IsVisible { get; set; } = true;

    public VisibilityModel(bool isVisible = true)
    {
        IsVisible = isVisible;
    }

    public void OnChange()
    {
        IsVisible = !IsVisible;
    }
}