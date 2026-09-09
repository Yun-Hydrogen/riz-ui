using System;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class DialogDemoPage : Page
{
    public string CodeUsage1 { get; } =
@"<!-- 基础对话框用法 -->
<rizui:RizDialog x:Name=""BasicDialog""
                 Title=""确认操作""
                 Icon=""fa-solid fa-circle-question""
                 WidthText=""380px"">
  <TextBlock Text=""确定要同步当前终端的全部运行状态吗？"" />
  <rizui:RizDialog.Footer>
    <StackPanel Orientation=""Horizontal"" Spacing=""10"">
      <rizui:RizButton Text=""取消"" Click=""OnCloseDialog"" />
      <rizui:RizButton Text=""确认"" Primary=""True"" Click=""OnCloseDialog"" />
    </StackPanel>
  </rizui:RizDialog.Footer>
</rizui:RizDialog>";

    public string CodeUsage2 { get; } =
@"<!-- 变体：禁用右上角关闭按钮或禁用遮罩点击关闭 -->
<rizui:RizDialog Title=""重要决策""
                 Closable=""False""
                 MaskClose=""False""
                 WidthText=""380px"">
  <TextBlock Text=""此操作不可逆，请慎重点击！"" />
  <rizui:RizDialog.Footer>
    <StackPanel Orientation=""Horizontal"" Spacing=""10"">
      <rizui:RizButton Text=""我已知晓"" Primary=""True"" Click=""OnCloseDialog"" />
    </StackPanel>
  </rizui:RizDialog.Footer>
</rizui:RizDialog>";

    public string CodeUsage3 { get; } =
@"<!-- 倒计时与底部进度条（Countdown > 0 时进度条从 100% 递减至 0% 并自动关闭） -->
<rizui:RizDialog Title=""自动同步中""
                 Icon=""fa-solid fa-cloud-arrow-up""
                 Countdown=""5""
                 ShowBar=""True""
                 Color=""#39C5BB""
                 WidthText=""400px"">
  <TextBlock Text=""数据正在上传沙勒云端，5 秒后将自动关闭对话框..."" />
  <rizui:RizDialog.Footer>
    <rizui:RizButton Text=""立即完成"" Primary=""True"" Color=""#39C5BB"" Click=""OnCloseDialog"" />
  </rizui:RizDialog.Footer>
</rizui:RizDialog>";

    public string CodeUsage4 { get; } =
@"<!-- 样式与主题色定制 -->
<rizui:RizDialog Title=""沙勒助手普拉娜""
                 Icon=""fa-solid fa-wand-magic-sparkles""
                 Color=""#A855F7""
                 WidthText=""440px"">
  <StackPanel Spacing=""8"">
    <TextBlock Text=""老师，今天需要普拉娜帮您整理行动日程表吗？"" />
    <rizui:RizInfoBox Type=""info"" Color=""#A855F7"" Text=""主题色覆盖将同时作用于标题图标、关闭按钮 Hover 态与底部进度条。"" />
  </StackPanel>
  <rizui:RizDialog.Footer>
    <StackPanel Orientation=""Horizontal"" Spacing=""10"">
      <rizui:RizButton Text=""暂不需要"" Click=""OnCloseDialog"" />
      <rizui:RizButton Text=""立即整理"" Primary=""True"" Color=""#A855F7"" Click=""OnCloseDialog"" />
    </StackPanel>
  </rizui:RizDialog.Footer>
</rizui:RizDialog>";

    public DialogDemoPage()
    {
        InitializeComponent();
    }

    private void OpenBasic(object sender, EventArgs e) => BasicDialog.Show = true;
    private void OpenNoClose(object sender, EventArgs e) => NoCloseDialog.Show = true;
    private void OpenNoMask(object sender, EventArgs e) => NoMaskDialog.Show = true;
    private void OpenStrict(object sender, EventArgs e) => StrictDialog.Show = true;
    private void OpenCountdown(object sender, EventArgs e) => CountdownDialog.Show = true;
    private void OpenProgressDialog(object sender, EventArgs e) => ProgressManualDialog.Show = true;
    private void OpenCustom(object sender, EventArgs e) => CustomDialog.Show = true;

    private void CloseBasic(object sender, EventArgs e) => BasicDialog.Show = false;
    private void CloseNoClose(object sender, EventArgs e) => NoCloseDialog.Show = false;
    private void CloseNoMask(object sender, EventArgs e) => NoMaskDialog.Show = false;
    private void CloseStrict(object sender, EventArgs e) => StrictDialog.Show = false;
    private void CloseCountdown(object sender, EventArgs e) => CountdownDialog.Show = false;
    private void CloseProgressDialog(object sender, EventArgs e) => ProgressManualDialog.Show = false;
    private void CloseCustom(object sender, EventArgs e) => CustomDialog.Show = false;
}