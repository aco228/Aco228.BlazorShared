using Microsoft.AspNetCore.Components;

namespace Aco228.BlazorShared.Models;

public class SectionButton
{
    public string Title { get; set; }
    public Action OnClick { get; set; }

    public SectionButton()
    {
        
    }

    public SectionButton(string title, Action onClick)
    {
        Title = title;
        OnClick = onClick;       
    }
}