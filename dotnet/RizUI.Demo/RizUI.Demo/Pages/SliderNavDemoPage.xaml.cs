using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using RizUI.Uwp.Controls;
namespace RizUI.Demo.Pages;
public sealed partial class SliderNavDemoPage : Page
{
    public SliderNavDemoPage()
    {
        InitializeComponent();
        FixedNav.Items = Items();
        FloatingNav.Items = Items();
    }
    private static IList<RizSliderNavItem> Items() => new List<RizSliderNavItem>
    {
        new() { Key = "home", Text = "首页", Icon = "fa-solid fa-house" },
        new() { Key = "github", Text = "GitHub", Icon = "fa-brands fa-github" },
        new() { Key = "help", Text = "帮助", Icon = "fa-regular fa-circle-question" },
        new() { Key = "more", Text = "更多", Icon = "fa-solid fa-folder", IsExpanded = true, Children = { new() { Key = "deep", Text = "三级条目", Icon = "fa-solid fa-folder-open", IsExpanded = true, Children = { new() { Key = "leaf", Text = "叶子条目", Icon = "fa-solid fa-leaf" } } } } }
    };
    private void OnSelected(object sender, RizSliderNavItem item) => FixedStatus.Text = $"当前：{item.Key} / {item.Text}";
}