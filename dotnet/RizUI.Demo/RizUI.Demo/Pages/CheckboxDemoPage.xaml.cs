using System;
using Microsoft.UI.Xaml.Controls;
using RizUI.Uwp.Controls;
namespace RizUI.Demo.Pages;
public sealed partial class CheckboxDemoPage : Page
{
    public CheckboxDemoPage() => InitializeComponent();
    private void OnToggled(object sender, EventArgs e) => Status.Text = ((RizCheckbox)sender).IsChecked ? "已勾选" : "已取消勾选";
}