using Aco228.BlazorShared.Models.SortableTable;
using Aco228.Common.Extensions;

namespace Aco228.BlazorShared.Helpers;

public static class SortableTableEntryHelper
{

    public static List<TRes> ConvertToSortableTableEntries<TEntry, TRes>(this List<TEntry> input) where TRes : SortableTableEntry
    {
        var result = new List<TRes>();
        var resultProperties = typeof(TRes).GetPropertyWithAttribute<SortableTableNameAttribute>();
        if (!resultProperties.Any())
            return result;
        
        var entryProperties = typeof(TEntry).GetProperties();
        foreach (var tEntry in input)
        {
            var entry = Activator.CreateInstance<TRes>();
            foreach (var resultProperty in resultProperties)
            {
                var tEntryProperty = entryProperties.FirstOrDefault(x => x.Name.Equals(resultProperty.Attribute?.Name));
                if (tEntryProperty == null)
                    continue;
                
                resultProperty.Info.SetValue(entry, tEntryProperty.GetValue(tEntry));
            }

            result.Add(entry);
        }

        return result;
    }
    
}