using System;
using Microsoft.UI.Xaml.Controls;
namespace RizUI.Demo.Pages;
public sealed partial class ButtonDemoPage : Page
{
    public ButtonDemoPage() => InitializeComponent();
    private void OnClick(object sender, EventArgs e) => Status.Text = $"已点击 · {DateTime.Now:HH:mm:ss}";
}