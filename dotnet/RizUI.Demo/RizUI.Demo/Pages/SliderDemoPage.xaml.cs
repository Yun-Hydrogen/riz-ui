using System;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class SliderDemoPage : Page
{
    public string CodeUsage1 { get; } =
@"<!-- 基础音量百分比滑条 -->
<rizui:RizCfgRow Label=""全局音量"" Hint=""0~100，单位 %"">
  <rizui:RizSlider Value=""50"" Minimum=""0"" Maximum=""100"" Step=""1"" Display=""%"" />
</rizui:RizCfgRow>";

    public string CodeUsage2 { get; } =
@"<!-- 作战倍速调节 -->
<rizui:RizCfgRow Label=""作战倍速"" Hint=""1~3 倍速，步进 0.5"">
  <rizui:RizSlider Value=""2"" Minimum=""1"" Maximum=""3"" Step=""0.5"" Display=""倍速"" />
</rizui:RizCfgRow>";

    public string CodeUsage3 { get; } =
@"<!-- 自定义配色与全宽展示 -->
<rizui:RizSlider Value=""360"" Minimum=""100"" Maximum=""999"" Step=""10"" Display="" AP"" Color=""#39C5BB"" />";

    public SliderDemoPage()
    {
        InitializeComponent();
    }
}
