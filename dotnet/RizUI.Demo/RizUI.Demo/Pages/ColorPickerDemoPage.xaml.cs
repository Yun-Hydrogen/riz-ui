using System;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class ColorPickerDemoPage : Page
{
    public string CodeUsage1 { get; } =
@"<!-- 基础颜色选择器 -->
<rizui:RizCfgRow Label=""选择自定义色"" Hint=""点击展开拾色板并输入十六进制颜色"">
  <rizui:RizColorPicker ColorValue=""#66CCFF"" />
</rizui:RizCfgRow>";

    public ColorPickerDemoPage()
    {
        InitializeComponent();
    }
}
