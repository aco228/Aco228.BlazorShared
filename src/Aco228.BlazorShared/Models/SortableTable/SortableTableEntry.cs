using LiteDB;

namespace Aco228.BlazorShared.Models.SortableTable;

public class SortableTableNameAttribute : Attribute
{
    public string Name { get; set; }
    public bool Ignore { get; set; } = false;
    public bool Clickable { get; set; } = false;
    public bool HumanizeDate { get; set; } = false;
    public bool HumanizeDateUtc { get; set; } = false;
    public string? Url { get; set; }
    public string? UrlParamName { get; set; }
}

public class SortableTableEntry
{
    [BsonIgnore]
    [SortableTableName(Ignore = true)]
    public bool IsVisible { get; set; } = true;
    
    [BsonIgnore]
    [SortableTableName(Ignore = true)]
    public bool IsSelected { get; set; } = false;
}