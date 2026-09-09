using System;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class ThemeDemoPage : Page
{
    public string XamlUsage1 { get; } =
@"<!-- 1. XAML 资源字典引用 -->
<Border Background=""{StaticResource RZ_Theme}"" CornerRadius=""8"" Padding=""14,6"">
  <TextBlock Text=""沙勒特别事务局"" Foreground=""White"" />
</Border>
<Border BorderBrush=""{StaticResource RZ_Theme}"" BorderThickness=""1.5"" CornerRadius=""8"" Padding=""14,6"">
  <TextBlock Text=""无限制决战"" Foreground=""{StaticResource RZ_Theme}"" />
</Border>";

    public string XamlUsage2 { get; } =
@"<!-- 2. 组件层级主题切换 (Theme 属性) -->
<!-- 强制暗色 -->
<rizui:RizCard Title=""强制暗色卡片"" Theme=""Dark"">
  <rizui:RizCfgRow Label=""暗色模式"" Theme=""Dark"">
    <rizui:RizSwitch IsOn=""True"" Theme=""Dark"" />
  </rizui:RizCfgRow>
</rizui:RizCard>

<!-- 强制亮色 -->
<rizui:RizCard Title=""强制浅色卡片"" Theme=""Light"">
  <rizui:RizCfgRow Label=""亮色模式"" Theme=""Light"">
    <rizui:RizButton Text=""按钮"" Primary=""True"" Theme=""Light"" />
  </rizui:RizCfgRow>
</rizui:RizCard>";

    public string CSharpUsage { get; } =
@"// 3. C# 动态设置与读取全局主题色
// 读取当前默认主题色
var accent = RizVisuals.ThemeColor(null); // 返回 #66CCFF

// 将主题色传递给指定控件
myButton.Color = ""#A855F7""; // 覆盖为紫罗兰色
myCard.Theme = ""Dark"";      // 局部锁定为暗色";

    public ThemeDemoPage()
    {
        InitializeComponent();
    }
}
