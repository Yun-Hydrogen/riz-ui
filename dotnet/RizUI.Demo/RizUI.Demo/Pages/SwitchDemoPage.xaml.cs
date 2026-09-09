using System;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class SwitchDemoPage : Page
{
    public string CodeUsage1 { get; } =
@"<!-- 基础双向绑定 -->
<rizui:RizCfgRow Label=""主控连接"" Hint=""点击右侧滑动开关切换状态"">
  <rizui:RizSwitch IsOn=""True"" />
</rizui:RizCfgRow>";

    public string CodeUsage2 { get; } =
@"<!-- 禁用状态（不可交互，半透明展示） -->
<rizui:RizCfgRow Label=""安全保护场域"" Hint=""剧情模式下锁定无法关闭"">
  <rizui:RizSwitch IsOn=""True"" IsEnabled=""False"" />
</rizui:RizCfgRow>";

    public string CodeUsage3 { get; } =
@"<!-- 自定义主题色与事件 -->
<rizui:RizSwitch IsOn=""True"" Color=""#39C5BB"" Toggled=""OnSwitchToggled"" />";

    public SwitchDemoPage()
    {
        InitializeComponent();
    }
}
