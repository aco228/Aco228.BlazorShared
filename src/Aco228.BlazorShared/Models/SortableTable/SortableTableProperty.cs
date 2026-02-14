using System.Reflection;
using Aco228.Common.Extensions;
using Aco228.MongoDb.Models;
using Humanizer;

namespace Aco228.BlazorShared.Models.SortableTable;

public class SortableTableProperty<TEntry>
    where TEntry : SortableTableEntry
{
    public SortableTableNameAttribute Attribute { get; set; }
    public PropertyInfo PropertyInfo { get; set; }
    public bool IsIgnored { get; set; } = false;
    public string Url { get; set; }
    public string Name { get; set; }

    public void Initialize(TEntry input)
    {
        if (!string.IsNullOrEmpty(Attribute?.Url))
            Url = Attribute.Url;

        if (!string.IsNullOrEmpty(Attribute?.UrlParamName))
        {
            var propName = input.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(Attribute.UrlParamName));
            if (propName != null)
            {
                Url = propName.GetValue(input).ToString();
            }
        }
    }

    public string GetValue(TEntry input)
    {
        var val = PropertyInfo.GetValue(input);

        if (val != null)
        {
            if (val is double)
            {
                if (double.IsInfinity((double) val) || double.IsNaN((double) val))
                    return 0.ToString();

                return ((double) val).ToDoubleString();
            }

            if (val is long)
            {
                if (Attribute?.HumanizeDate == true)
                    return ((long) val).FromUnixTimestampMilliseconds().Humanize();
                if (Attribute?.HumanizeDateUtc == true)
                    return ((long) val).FromUnixTimestampMillisecondsUtc().Humanize(utcDate: true);   
            }

            if (val is IdDocument)
            {
                return ((IdDocument) val)?.Name ?? "";
            }

            if (val is DateTime)
            {
                if (Attribute?.HumanizeDate == true)
                    return ((DateTime) val).Humanize();
                if (Attribute?.HumanizeDateUtc == true)
                    return ((DateTime) val).Humanize(utcDate: true);
            }
        }

        return val?.ToString() ?? "";
    }
}