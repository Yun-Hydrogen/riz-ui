using System;
using Microsoft.UI.Xaml.Controls;
namespace RizUI.Demo.Pages;
public sealed partial class DialogDemoPage : Page
{
    public DialogDemoPage() => InitializeComponent();
    private void OnOpen(object sender, EventArgs e) { Dialog.Show = true; Status.Text = "Dialog 已打开"; }
    private void OnCancel(object sender, EventArgs e) => Dialog.Show = false;
    private void OnSave(object sender, EventArgs e) { Dialog.Show = false; Status.Text = "已保存"; }
    private void OnClosed(object sender, EventArgs e) => Status.Text = "Dialog 已关闭";
}