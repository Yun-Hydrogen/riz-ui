using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using RizUI.Uwp.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class DropdownDemoPage : Page
{
    public IList<RizDropdownItem> GraphicOptions { get; } = new List<RizDropdownItem>
    {
        new("option1", "Vulkan"),
        new("option2", "OpenGL"),
        new("option3", "DirectX 12"),
        new("option4", "Software (关闭)")
    };

    public string CodeUsage1 { get; } =
@"<!-- 基础下拉选择器 -->
var options = new List<RizDropdownItem>
{
    new(""option1"", ""Vulkan""),
    new(""option2"", ""OpenGL""),
    new(""option3"", ""DirectX 12"")
};

<rizui:RizCfgRow Label=""选择图形加速后端"" Hint=""游戏画面异常时，可尝试更改此处设置"">
  <rizui:RizDropdown SelectedValue=""option2"" Options=""{x:Bind GraphicOptions}"" />
</rizui:RizCfgRow>";

    public string CodeUsage2 { get; } =
@"<!-- 自定义主题色 -->
<rizui:RizDropdown SelectedValue=""option1"" Options=""{x:Bind GraphicOptions}"" Color=""#39C5BB"" />";

    public DropdownDemoPage()
    {
        InitializeComponent();
    }
}
