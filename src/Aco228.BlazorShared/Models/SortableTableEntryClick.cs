namespace Aco228.BlazorShared.Models;

public class SortableTableEntryClick<TEntry>
{
    public TEntry Entry { get; set; }
    public string PropertyName { get; set; }
}