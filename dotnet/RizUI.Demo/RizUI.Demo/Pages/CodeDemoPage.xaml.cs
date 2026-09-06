using System;
using Microsoft.UI.Xaml.Controls;
namespace RizUI.Demo.Pages;
public sealed partial class CodeDemoPage : Page
{
    public CodeDemoPage() => InitializeComponent();
    private void OnCopied(object sender, EventArgs e) => Status.Text = "代码已复制到剪贴板";
}