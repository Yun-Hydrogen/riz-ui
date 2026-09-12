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
        PropsTable.Headers = new System.Collections.ObjectModel.ObservableCollection<string> { "属性", "类型", "默认值", "说明" };
        PropsTable.Rows = new System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<string>>
        {
            new() { "IsChecked", "bool", "false", "选中状态（双向绑定首选属性，对应 Vue 的 modelValue）" },
            new() { "IsOn", "bool", "false", "兼容旧版 API 的开关状态别名属性" },
            new() { "LLabel", "string", "\"\"", "左侧标签文字" },
            new() { "RLabel", "string", "\"\"", "右侧标签文字" },
            new() { "Color", "string", "\"\"", "选中激活主题色，空则跟随全局 --rizui_themecolor" },
            new() { "Command", "ICommand", "null", "切换状态时执行的 MVVM 命令" },
            new() { "Theme", "string", "\"auto\"", "Dark 强制暗色 / Light 强制亮色 / auto 跟随系统" }
        };
    }

    private void OnToggled(object sender, EventArgs e)
    {
        if (Status != null)
        {
            Status.Text = ((RizCheckbox)sender).IsChecked ? "状态：已勾选" : "状态：未勾选";
        }
    }
}