using System;
using Microsoft.UI.Xaml.Controls;
using RizUI.Uwp.Controls;
namespace RizUI.Demo.Pages;
public sealed partial class ToastDemoPage : Page
{
    public ToastDemoPage() => InitializeComponent();
    private void OnInfo(object s, EventArgs e) => Add("info", "操作成功", "Info Toast 将在 3 秒后关闭。", 3);
    private void OnWarn(object s, EventArgs e) => Add("warn", "注意", "Warn Toast 使用预设橙色。", 4);
    private void OnError(object s, EventArgs e) => Add("error", "发生错误", "Error Toast 使用预设红色。", 5);
    private void OnPersistent(object s, EventArgs e) => Add("info", "持久通知", "countdown=0，需要手动关闭。", 0);
    private void Add(string type, string title, string hint, double countdown)
    {
        var toast = new RizToast { XamlRoot = XamlRoot, Type = type, Title = title, Hint = hint, Countdown = countdown };
        toast.Closed += (_, elapsed) => Status.Text = $"通知关闭，停留 {elapsed:0.0}s";
        Host.Children.Add(toast);
        Status.Text = $"{title} 已加入队列";
    }
}