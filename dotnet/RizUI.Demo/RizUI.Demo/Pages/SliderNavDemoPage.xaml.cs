using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using RizUI.Uwp.Controls;
namespace RizUI.Demo.Pages;
public sealed partial class SliderNavDemoPage : Page
{
    public string CodeUsageFixed { get; } =
@"<!-- 固定布局模式（StyleMode=""Fixed""） -->
<rizui:RizSliderNav Title=""RizUI Console""
                    Icon=""fa-solid fa-cube""
                    StyleMode=""Fixed""
                    DefaultSelectedItem=""home""
                    SelectionChanged=""OnSelected"">
  <rizui:RizSliderNav.Content>
    <!-- 右侧页面主内容 -->
    <Grid Padding=""24"">
      <TextBlock Text=""主界面内容区"" FontSize=""16"" />
    </Grid>
  </rizui:RizSliderNav.Content>
</rizui:RizSliderNav>";

    public string CodeUsageFloating { get; } =
@"<!-- 悬浮浮动布局（StyleMode=""Floating""） -->
<rizui:RizSliderNav Title=""浮动导航""
                    Icon=""fa-solid fa-compass""
                    StyleMode=""Floating""
                    DefaultSelectedItem=""home""
                    SelectionChanged=""OnSelected"">
  <rizui:RizSliderNav.Content>
    <!-- 通栏铺满背景的主内容 -->
    <Grid Background=""#F8FAFC"">
      <TextBlock Text=""悬浮通栏内容区域"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" />
    </Grid>
  </rizui:RizSliderNav.Content>
</rizui:RizSliderNav>";

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