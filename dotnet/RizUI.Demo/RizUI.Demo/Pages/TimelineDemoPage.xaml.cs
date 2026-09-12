using System;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class TimelineDemoPage : Page
{
    public string CodeUsage1 { get; } =
@"<!-- 基础时间轴控件 -->
<rizui:RizCfgRow Label=""战斗回放进度"" Hint=""0~120 秒，拖拽或点击即可快速定位"" Stack=""True"">
  <rizui:RizTimeline Value=""45"" Minimum=""0"" Maximum=""120"" Step=""1"" Margin=""0,8,0,0"" />
</rizui:RizCfgRow>";

    public string CodeUsage2 { get; } =
@"<!-- 自定义主题色与短区间 (0~60 秒) -->
<rizui:RizTimeline Value=""25"" Minimum=""0"" Maximum=""60"" Step=""0.5"" Color=""#39C5BB"" />";

    public TimelineDemoPage()
    {
        InitializeComponent();
        PropsTable.Headers = new System.Collections.ObjectModel.ObservableCollection<string> { "属性", "类型", "默认值", "说明" };
        PropsTable.Rows = new System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<string>>
        {
            new() { "Value", "double", "0", "当前时间进度值（秒，对应 Vue 的 modelValue）" },
            new() { "Minimum", "double", "0", "起始时间刻度（秒）" },
            new() { "Maximum", "double", "120", "结束时间刻度（秒）" },
            new() { "Step", "double", "1", "步进数值（秒）" },
            new() { "Color", "string", "\"\"", "主题色，空则跟随 --rizui_themecolor" },
            new() { "Theme", "string", "\"auto\"", "Dark 强制暗色 / Light 强制亮色 / auto 跟随系统" }
        };
    }
}
