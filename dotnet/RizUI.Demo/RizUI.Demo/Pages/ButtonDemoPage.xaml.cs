using System;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class ButtonDemoPage : Page
{
    public string CodeUsage1 { get; } =
@"<!-- 1. 基础形态：普通 / Primary / Danger / Long -->
<rizui:RizCfgRow Label=""日常确认"" Hint=""普通按钮形态，无额外填充"">
  <rizui:RizButton Text=""确认"" />
</rizui:RizCfgRow>
<rizui:RizCfgRow Label=""主指令 (Primary)"" Hint=""主题色填充背景，适合主要操作"">
  <rizui:RizButton Text=""动员"" Primary=""True"" RIcon=""fa-solid fa-rocket"" />
</rizui:RizCfgRow>
<rizui:RizCfgRow Label=""危险指令 (Danger)"" Hint=""红色警告，用于删除、撤退等操作"">
  <rizui:RizButton Text=""撤退"" Danger=""True"" />
</rizui:RizCfgRow>
<rizui:RizButton Text=""确认进入总力战"" Primary=""True"" Long=""True"" />";

    public string CodeUsage2 { get; } =
@"<!-- 2. 图标与进阶用法：LIcon / RIcon / 纯图标按钮 -->
<rizui:RizButton Text=""发送MomoTalk"" LIcon=""fa-solid fa-paper-plane"" Primary=""True"" />
<rizui:RizButton Text=""查看学员档案"" RIcon=""fa-solid fa-angle-right"" />
<rizui:RizButton LIcon=""fa-solid fa-plus"" Primary=""True"" />
<rizui:RizButton LIcon=""fa-solid fa-rotate-right"" />
<rizui:RizButton LIcon=""fa-solid fa-heart"" Danger=""True"" />";

    public string CodeUsage3 { get; } =
@"<!-- 3. 自定义主题色与禁用状态 -->
<rizui:RizButton Text=""青辉石招募"" Primary=""True"" Color=""#39C5BB"" />
<rizui:RizButton Text=""兑换信用点"" Primary=""True"" Color=""#F0A030"" />
<rizui:RizButton Text=""权限未解锁"" Primary=""True"" IsEnabled=""False"" />";

    public ButtonDemoPage()
    {
        InitializeComponent();
        PropsTable.Headers = new System.Collections.ObjectModel.ObservableCollection<string> { "属性", "类型", "默认值", "说明" };
        PropsTable.Rows = new System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<string>>
        {
            new() { "Text", "string", "\"\"", "按钮文本内容，可为空（纯图标模式）" },
            new() { "TextStyle", "Style", "null", "文本 XAML 样式覆盖" },
            new() { "LIcon", "string", "\"\"", "左侧图标（完整 Font Awesome 类名，如 fa-solid fa-paper-plane）" },
            new() { "LIconStyle", "Style", "null", "左侧图标 XAML 样式覆盖" },
            new() { "RIcon", "string", "\"\"", "右侧图标（完整 Font Awesome 类名，如 fa-solid fa-angle-right）" },
            new() { "RIconStyle", "Style", "null", "右侧图标 XAML 样式覆盖" },
            new() { "Primary", "bool", "false", "是否以主题色填充背景" },
            new() { "Danger", "bool", "false", "是否危险/警告按钮（珊瑚红背景）" },
            new() { "Long", "bool", "false", "是否宽度撑满父容器（通栏）" },
            new() { "Color", "string", "\"\"", "自定义填充色，空则跟随 --rizui_themecolor" },
            new() { "Theme", "string", "\"auto\"", "Dark 强制暗色 / Light 强制亮色 / auto 跟随系统" }
        };
    }

    private void OnClick(object sender, EventArgs e)
    {
        Status.Text = $"已触发点击：{DateTime.Now:HH:mm:ss}";
    }
}