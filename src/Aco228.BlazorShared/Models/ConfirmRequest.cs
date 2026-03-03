namespace Aco228.BlazorShared.Models;

public class ConfirmRequest
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string OkText { get; set; } = "Confirm";
    public Action? ConfirmAction { get; set; }
}