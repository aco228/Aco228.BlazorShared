namespace Aco228.BlazorShared.Models;

public class UIModel<TEntry>
{
    public bool IsVisible { get; set; }
    public bool IsSelected { get; set; }
    public TEntry Entry { get; set; }
}

public static class UIModelExtensions
{
    public static List<UIModel<TEntry>> ToUIModels<TEntry>(
        this IEnumerable<TEntry> entries, 
        bool isVisibleByDefault = true)
    {
        var result = new List<UIModel<TEntry>>();
        foreach (var entry in entries)
            result.Add(new()
            {
                Entry = entry,
                IsVisible = isVisibleByDefault,
                IsSelected = false
            });

        return result;
    }
}