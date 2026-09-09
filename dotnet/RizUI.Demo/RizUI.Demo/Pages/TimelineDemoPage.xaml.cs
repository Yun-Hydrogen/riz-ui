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
    }
}
