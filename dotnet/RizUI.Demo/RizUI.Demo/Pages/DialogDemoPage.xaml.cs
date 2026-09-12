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

    public string CodeUsage5 { get; } =
@"// ── Web: <Teleport to=""body""> 的 C# 等价物：Popup + XamlRoot ──
// Popup 的内容不在页面视觉树里，而是挂在 XamlRoot 的顶层弹层：
// 不受 ScrollViewer 裁剪，也不受父级 z-index / transform 层叠影响。
var popup = new Popup
{
    XamlRoot = XamlRoot,   // ← 顶层宿主（等价 Teleport 的目标节点）
    Child = overlay        // ← 遮罩 + 弹窗卡片（等价被 Teleport 的整棵子树）
};
popup.IsOpen = true;

// 遮罩铺满视口（等价 position: fixed; inset: 0），并跟随窗口尺寸变化
var root = XamlRoot.Content as FrameworkElement;
overlay.Width  = root.ActualWidth;
overlay.Height = root.ActualHeight;
root.SizeChanged += (_, e) =>
{
    overlay.Width  = e.NewSize.Width;
    overlay.Height = e.NewSize.Height;
};

// 点击遮罩关闭（等价 @click.self：只有点在遮罩本身、而不是卡片上时才算）
overlay.PointerPressed += (_, e) =>
{
    if (ReferenceEquals(e.OriginalSource, overlay)) popup.IsOpen = false;
};

// 退场动画播完再卸载（等价 <Transition> 的 @after-leave）
exitStoryboard.Completed += (_, _) => popup.IsOpen = false;";

    public DialogDemoPage()
    {
        InitializeComponent();
        PropsTable.Headers = new System.Collections.ObjectModel.ObservableCollection<string> { "属性", "类型", "默认值", "说明" };
        PropsTable.Rows = new System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<string>>
        {
            new() { "Show", "bool", "false", "控制对话框显示与隐藏" },
            new() { "Title", "string", "\"\"", "对话框标题文字" },
            new() { "TitleStyle", "Style", "null", "标题文本 XAML 样式覆盖" },
            new() { "Icon", "string", "\"\"", "标题左侧 Font Awesome 图标类名" },
            new() { "IconStyle", "Style", "null", "图标 XAML 样式覆盖" },
            new() { "Color", "string", "\"\"", "主题色，空则跟随全局 --rizui_themecolor" },
            new() { "WidthText", "string", "\"auto\"", "对话框宽度（如 360px、420px、auto）" },
            new() { "Closable", "bool", "true", "是否显示右上角胶囊关闭按钮" },
            new() { "MaskClose", "bool", "true", "点击背景遮罩是否关闭对话框" },
            new() { "Countdown", "double", "0", "自动关闭倒计时（秒），大于 0 时底部展示进度条" },
            new() { "ShowBar", "bool", "false", "" },
            new() { "Progress", "double", "-1", "手动进度值（0.0 ~ 1.0），小于 0 时由 Countdown 驱动" },
            new() { "Theme", "string", "\"auto\"", "Dark 强制暗色 / Light 强制亮色 / auto 跟随系统" }
        };
    }

    private void OpenBasic(object sender, EventArgs e) => BasicDialog.Show = true;
    private void OpenNoClose(object sender, EventArgs e) => NoCloseDialog.Show = true;
    private void OpenNoMask(object sender, EventArgs e) => NoMaskDialog.Show = true;
    private void OpenStrict(object sender, EventArgs e) => StrictDialog.Show = true;
    private void OpenCountdown(object sender, EventArgs e) => CountdownDialog.Show = true;
    private void OpenProgressDialog(object sender, EventArgs e) => ProgressManualDialog.Show = true;
    private void OpenCustom(object sender, EventArgs e) => CustomDialog.Show = true;
    private void OpenTeleport(object sender, EventArgs e) => TeleportDialog.Show = true;

    private void CloseBasic(object sender, EventArgs e) => BasicDialog.Show = false;
    private void CloseNoClose(object sender, EventArgs e) => NoCloseDialog.Show = false;
    private void CloseNoMask(object sender, EventArgs e) => NoMaskDialog.Show = false;
    private void CloseStrict(object sender, EventArgs e) => StrictDialog.Show = false;
    private void CloseCountdown(object sender, EventArgs e) => CountdownDialog.Show = false;
    private void CloseProgressDialog(object sender, EventArgs e) => ProgressManualDialog.Show = false;
    private void CloseCustom(object sender, EventArgs e) => CustomDialog.Show = false;
    private void CloseTeleport(object sender, EventArgs e) => TeleportDialog.Show = false;
}