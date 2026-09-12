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
        PropsTable.Headers = new System.Collections.ObjectModel.ObservableCollection<string> { "属性", "类型", "默认值", "说明" };
        PropsTable.Rows = new System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<string>>
        {
            new() { "IsOn", "bool", "false", "开关当前状态（对应 Vue 的 modelValue）" },
            new() { "Color", "string", "\"\"", "开启时主题色，空则跟随 --rizui_themecolor" },
            new() { "Theme", "string", "\"auto\"", "Dark 强制暗色 / Light 强制亮色 / auto 跟随系统" }
        };
    }
}
