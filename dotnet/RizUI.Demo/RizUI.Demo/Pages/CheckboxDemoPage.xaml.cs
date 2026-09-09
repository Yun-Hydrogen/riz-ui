using System;
using Microsoft.UI.Xaml.Controls;
using RizUI.Uwp.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class CheckboxDemoPage : Page
{
    public string CodeUsage1 { get; } =
@"<!-- 1. 基础用法：IsChecked 双向绑定 + 标签文字 -->
<rizui:RizCheckbox IsChecked=""True"" RLabel=""接收沙勒的战略简报"" />
<rizui:RizCheckbox IsChecked=""False"" RLabel=""自动下载关卡资源包"" />";

    public string CodeUsage2 { get; } =
@"<!-- 2. 标签位置：支持 RLabel（右侧）与 LLabel（左侧） -->
<rizui:RizCheckbox IsChecked=""True"" LLabel=""启用战术指挥终端"" />
<rizui:RizCheckbox IsChecked=""True"" RLabel=""允许推送战斗回放"" />";

    public string CodeUsage3 { get; } =
@"<!-- 3. 自定义颜色与禁用态 -->
<rizui:RizCheckbox IsChecked=""True"" Color=""#39C5BB"" RLabel=""初音葱绿配色"" />
<rizui:RizCheckbox IsChecked=""True"" Color=""#A855F7"" RLabel=""普拉娜紫罗兰配色"" />
<rizui:RizCheckbox IsChecked=""True"" IsEnabled=""False"" RLabel=""安全协议限制（禁用态）"" />";

    public CheckboxDemoPage()
    {
        InitializeComponent();
    }

    private void OnToggled(object sender, EventArgs e)
    {
        if (Status != null)
        {
            Status.Text = ((RizCheckbox)sender).IsChecked ? "状态：已勾选" : "状态：未勾选";
        }
    }
}