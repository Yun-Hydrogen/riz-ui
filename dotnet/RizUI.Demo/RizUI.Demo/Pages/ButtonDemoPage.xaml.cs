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
    }

    private void OnClick(object sender, EventArgs e)
    {
        Status.Text = $"已触发点击：{DateTime.Now:HH:mm:ss}";
    }
}