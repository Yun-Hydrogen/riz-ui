using System;
using Microsoft.UI.Xaml.Controls;
using RizUI.Uwp.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class ToastDemoPage : Page
{
    public string CodeUsage1 { get; } =
@"<!-- 1. 预设样式与自定义颜色 -->
RizToast.ShowToast(XamlRoot, ""欢迎回来！"", ""老师，今天你想从哪件事开始做起呢...?"", type: ""info"", countdown: 3);
RizToast.ShowToast(XamlRoot, ""突击学员撤退！"", ""当有突击学员撤退时，技能蓄能值回复速率将减少。"", type: ""warn"", countdown: 3);
RizToast.ShowToast(XamlRoot, ""战败"", ""在下次尝试之前，先提升学员的属性值吧。"", type: ""error"", countdown: 3);
RizToast.ShowToast(XamlRoot, ""让世界变得更有趣吧！"", ""这是使用了自定义颜色的 Toast 哦！"", color: ""#E56161"", countdown: 3);";

    public string CodeUsage2 { get; } =
@"<!-- 2. 可配置功能：关进度条 / 关关闭按钮 / 常驻不自动关闭 -->
// 无进度条
RizToast.ShowToast(XamlRoot, ""任务开始！"", ""请击败敌方首领！"", countdown: 3); // ShowBar=False

// 无右上角关闭按钮
var noCloseToast = new RizToast
{
    XamlRoot = XamlRoot,
    Title = ""来自 MomoTalk 的信息"",
    Hint = ""老师，你能狡辩一下这笔沙勒的支出吗?!"",
    Color = ""#E278B3"",
    Closable = false,
    Countdown = 4
};
Host.Children.Add(noCloseToast);

// 常驻不自动关闭（Countdown = 0）
var persistToast = new RizToast
{
    XamlRoot = XamlRoot,
    Type = ""error"",
    Title = ""总力战已开启！"",
    Hint = ""快去准备部队迎战吧！点击通知本体即可关闭。"",
    Countdown = 0
};
persistToast.Click += (s, _) => ((RizToast)s).Close();
Host.Children.Add(persistToast);";

    public string CodeUsage3 { get; } =
@"<!-- 3. 自定义 Click 点击事件 -->
var rewardToast = new RizToast
{
    XamlRoot = XamlRoot,
    Title = ""点击领取 24000 青辉石"",
    Hint = ""为了庆祝三周年，特意为老师准备了 24000 青辉石！点击通知领取。"",
    Color = ""#39C5BB"",
    Icon = ""fa-solid fa-gift"",
    Countdown = 6
};
rewardToast.Click += (s, _) =>
{
    ((RizToast)s).Close();
    RizToast.ShowToast(XamlRoot, ""领取成功！"", ""24000 青辉石已发放至沙勒邮箱。"", color: ""#39C5BB"");
};
Host.Children.Add(rewardToast);";

    public ToastDemoPage()
    {
        InitializeComponent();
    }

    private void OnInfo(object s, EventArgs e)
    {
        if (XamlRoot is null) return;
        var toast = new RizToast
        {
            XamlRoot = XamlRoot,
            Type = "info",
            Title = "欢迎回来！",
            Hint = "老师，今天你想从哪件事开始做起呢...?",
            Countdown = 3
        };
        toast.Closed += (_, elapsed) => Status.Text = $"通知关闭，停留 {elapsed:0.0}s";
        Host.Children.Add(toast);
        Status.Text = "已弹出普通 Info 通知";
    }

    private void OnWarn(object s, EventArgs e)
    {
        if (XamlRoot is null) return;
        var toast = new RizToast
        {
            XamlRoot = XamlRoot,
            Type = "warn",
            Title = "突击学员撤退！",
            Hint = "当有突击学员撤退时，技能蓄能值回复速率将减少。",
            Countdown = 3.5
        };
        toast.Closed += (_, elapsed) => Status.Text = $"通知关闭，停留 {elapsed:0.0}s";
        Host.Children.Add(toast);
        Status.Text = "已弹出警告 Warn 通知";
    }

    private void OnError(object s, EventArgs e)
    {
        if (XamlRoot is null) return;
        var toast = new RizToast
        {
            XamlRoot = XamlRoot,
            Type = "error",
            Title = "战败",
            Hint = "在下次尝试之前，先提升学员的属性值吧。",
            Countdown = 4
        };
        toast.Closed += (_, elapsed) => Status.Text = $"通知关闭，停留 {elapsed:0.0}s";
        Host.Children.Add(toast);
        Status.Text = "已弹出错误 Error 通知";
    }

    private void OnColor(object s, EventArgs e)
    {
        if (XamlRoot is null) return;
        var toast = new RizToast
        {
            XamlRoot = XamlRoot,
            Type = "info",
            Color = "#E56161",
            Title = "让世界变得更有趣吧！",
            Hint = "这是使用了自定义颜色 #E56161 的 Toast 哦！",
            Countdown = 3.5
        };
        toast.Closed += (_, elapsed) => Status.Text = $"通知关闭，停留 {elapsed:0.0}s";
        Host.Children.Add(toast);
        Status.Text = "已弹出自定义颜色通知";
    }

    private void OnNoBar(object s, EventArgs e)
    {
        if (XamlRoot is null) return;
        var toast = new RizToast
        {
            XamlRoot = XamlRoot,
            Type = "info",
            Title = "任务开始！",
            Hint = "请击败敌方首领！",
            ShowBar = false,
            Countdown = 3
        };
        Host.Children.Add(toast);
        Status.Text = "已弹出无进度条通知";
    }

    private void OnNoClose(object s, EventArgs e)
    {
        if (XamlRoot is null) return;
        var toast = new RizToast
        {
            XamlRoot = XamlRoot,
            Type = "info",
            Title = "来自 MomoTalk 的信息",
            Hint = "老师，你能狡辩一下这笔沙勒的支出吗?!",
            Color = "#E278B3",
            Closable = false,
            Countdown = 4
        };
        Host.Children.Add(toast);
        Status.Text = "已弹出无关闭按钮通知";
    }

    private void OnPersistent(object s, EventArgs e)
    {
        if (XamlRoot is null) return;
        var toast = new RizToast
        {
            XamlRoot = XamlRoot,
            Type = "error",
            Title = "总力战已开启！",
            Hint = "快去准备部队迎战吧！（点击通知本体即可手动关闭）",
            Countdown = 0
        };
        toast.Click += (_, _) => toast.Close();
        Host.Children.Add(toast);
        Status.Text = "已弹出常驻通知（点击本体或右上角关闭）";
    }

    private void OnLongCountdown(object s, EventArgs e)
    {
        if (XamlRoot is null) return;
        var toast = new RizToast
        {
            XamlRoot = XamlRoot,
            Type = "info",
            Title = "作战剩余时间",
            Hint = "请在战斗结束前击败敌方首领！（30 秒长倒计时）",
            Countdown = 30
        };
        Host.Children.Add(toast);
        Status.Text = "已弹出 30 秒长倒计时通知";
    }

    private void OnSlotToast(object s, EventArgs e)
    {
        if (XamlRoot is null) return;
        var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Microsoft.UI.Xaml.Thickness(0, 4, 0, 0) };
        var input = new RizText { Placeholder = "在此快速回复...", Width = 220, HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Stretch };
        var btn = new RizButton { Primary = true, RIcon = "fa-solid fa-paper-plane" };
        panel.Children.Add(input);
        panel.Children.Add(btn);

        var toast = new RizToast
        {
            XamlRoot = XamlRoot,
            Type = "info",
            Title = "MomoTalk",
            Hint = "爱丽丝：邦邦卡邦！老师快来游戏开发部一趟！",
            Icon = "fa-solid fa-message",
            WidthText = "400px",
            Countdown = 0,
            Content = panel
        };
        btn.Click += (_, _) =>
        {
            toast.Close();
            var root = XamlRoot;
            if (root != null)
            {
                RizToast.ShowToast(root, "发送成功", "消息已回复给爱丽丝！", color: "#66CCFF");
            }
        };
        Host.Children.Add(toast);
        Status.Text = "已弹出带插槽控件的 Toast";
    }

    private void OnQueue(object s, EventArgs e)
    {
        OnInfo(s, e);
        OnWarn(s, e);
        OnError(s, e);
        Status.Text = "已一键触发 3 条通知加入排队栈";
    }

    private void OnClickToast(object s, EventArgs e)
    {
        if (XamlRoot is null) return;
        var toast = new RizToast
        {
            XamlRoot = XamlRoot,
            Title = "点击领取 24000 青辉石",
            Hint = "为了庆祝三周年，特意为老师准备了 24000 青辉石！点击本通知领取。",
            Color = "#39C5BB",
            Icon = "fa-solid fa-gift",
            Countdown = 6
        };
        toast.Click += (_, _) =>
        {
            toast.Close();
            var root = XamlRoot;
            if (root != null)
            {
                RizToast.ShowToast(root, "领取成功！", "24000 青辉石已发放至沙勒邮箱。", color: "#39C5BB");
            }
        };
        Host.Children.Add(toast);
        Status.Text = "已弹出带点击事件的 Toast";
    }
}