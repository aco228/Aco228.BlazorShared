namespace Aco228.BlazorShared.Models;

public class TabViewLink
{
    public string Title { get; set; }
    public string Href { get; set; }

    public TabViewLink() { }

    public TabViewLink(string title, string href)
    {
        Title = title;
        Href = href;
    }
}
