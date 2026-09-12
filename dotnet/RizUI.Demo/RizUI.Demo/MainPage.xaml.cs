using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using RizUI.Uwp.Controls;

namespace RizUI.Demo;

public sealed partial class MainPage : Page
{
    private readonly Dictionary<string, System.Type> _pages = new()
    {
        ["overview"] = typeof(Pages.OverviewPage),
        ["card"] = typeof(Pages.CardDemoPage),
        ["cfgrow"] = typeof(Pages.CfgRowDemoPage),
        ["infobox"] = typeof(Pages.InfoBoxDemoPage),
        ["dialog"] = typeof(Pages.DialogDemoPage),
        ["toast"] = typeof(Pages.ToastDemoPage),
        ["code"] = typeof(Pages.CodeDemoPage),
        ["button"] = typeof(Pages.ButtonDemoPage),
        ["switch"] = typeof(Pages.SwitchDemoPage),
        ["checkbox"] = typeof(Pages.CheckboxDemoPage),
        ["slider"] = typeof(Pages.SliderDemoPage),
        ["dropdown"] = typeof(Pages.DropdownDemoPage),
        ["timeline"] = typeof(Pages.TimelineDemoPage),
        ["colorpicker"] = typeof(Pages.ColorPickerDemoPage),
        ["table"] = typeof(Pages.TableDemoPage),
        ["text"] = typeof(Pages.TextDemoPage),
        ["mdeditor"] = typeof(Pages.MdEditorDemoPage),
        ["theme"] = typeof(Pages.ThemeDemoPage),
        ["slidernav"] = typeof(Pages.SliderNavDemoPage)
    };

    public MainPage()
    {
        InitializeComponent();
        ThemeModeDropdown.Options = new List<RizDropdownItem>
        {
            new("Default", "跟随系统"),
            new("Light", "浅色模式"),
            new("Dark", "深色模式")
        };
        DemoNav.Items = CreateNavigationItems();
        NavigateTo("overview");
    }

    private static IList<RizSliderNavItem> CreateNavigationItems() => new List<RizSliderNavItem>
    {
        new() { Key = "overview", Text = "组件总览", Icon = "fa-solid fa-house" },
        new()
        {
            Key = "containers", Text = "容器组件", Icon = "fa-solid fa-box-open", IsExpanded = true,
            Children =
            {
                new() { Key = "card", Text = "Card 卡片", Icon = "fa-solid fa-id-card" },
                new() { Key = "cfgrow", Text = "CfgRow 配置行", Icon = "fa-solid fa-list-check" },
                new() { Key = "infobox", Text = "InfoBox 提示框", Icon = "fa-solid fa-circle-info" },
                new() { Key = "dialog", Text = "Dialog 对话框", Icon = "fa-regular fa-window-maximize" },
                new() { Key = "toast", Text = "Toast 通知", Icon = "fa-solid fa-bell" },
                new() { Key = "code", Text = "Code 代码块", Icon = "fa-solid fa-code" }
            }
        },
        new()
        {
            Key = "controls", Text = "控制组件", Icon = "fa-solid fa-sliders", IsExpanded = true,
            Children =
            {
                new() { Key = "button", Text = "Button 按钮", Icon = "fa-solid fa-square" },
                new() { Key = "switch", Text = "Switch 开关", Icon = "fa-solid fa-toggle-on" },
                new() { Key = "checkbox", Text = "Checkbox 复选框", Icon = "fa-regular fa-square-check" },
                new() { Key = "slider", Text = "Slider 滑条", Icon = "fa-solid fa-sliders" },
                new() { Key = "dropdown", Text = "Dropdown 下拉", Icon = "fa-solid fa-square-caret-down" },
                new() { Key = "timeline", Text = "Timeline 时间轴", Icon = "fa-solid fa-timeline" },
                new() { Key = "colorpicker", Text = "ColorPicker 取色器", Icon = "fa-solid fa-eye-dropper" }
            }
        },
        new()
        {
            Key = "inputs", Text = "输入组件", Icon = "fa-solid fa-i-cursor", IsExpanded = true,
            Children =
            {
                new() { Key = "table", Text = "Table 表格", Icon = "fa-solid fa-table-cells" },
                new() { Key = "text", Text = "Text 文本框", Icon = "fa-solid fa-font" },
                new() { Key = "mdeditor", Text = "MdEditor 编辑器", Icon = "fa-solid fa-file-pen" }
            }
        },
        new()
        {
            Key = "theme", Text = "主题与模式", Icon = "fa-solid fa-palette"
        },
        new()
        {
            Key = "navigation", Text = "导航组件", Icon = "fa-solid fa-compass", IsExpanded = true,
            Children = { new() { Key = "slidernav", Text = "SliderNav 侧边栏", Icon = "fa-solid fa-bars" } }
        },
        new() { Key = "github", Text = "GitHub", Icon = "fa-brands fa-github" }
    };

    private void OnNavigationSelectionChanged(object sender, RizSliderNavItem item)
    {
        if (item.Key != "github") NavigateTo(item.Key);
    }

    private void OnThemeModeDropdownChanged(object sender, string tag)
    {
        var requestedTheme = tag switch
        {
            "Light" => ElementTheme.Light,
            "Dark" => ElementTheme.Dark,
            _ => ElementTheme.Default
        };

        // 1. 设置自身及当前子页面 Frame 的主题
        RequestedTheme = requestedTheme;
        if (PageHost != null)
        {
            PageHost.RequestedTheme = requestedTheme;
            if (PageHost.Content is FrameworkElement currentPage)
            {
                currentPage.RequestedTheme = requestedTheme;
            }
        }

        // 2. 将 Window.Content (根 Frame) 的 RequestedTheme 也同步更新，确保整棵可视化树即时生效
        if (XamlRoot?.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = requestedTheme;
        }
    }

    private void NavigateTo(string key)
    {
        if (_pages.TryGetValue(key, out var page) && PageHost.CurrentSourcePageType != page)
        {
            PageHost.Navigate(page);
            if (PageHost.Content is FrameworkElement newPage)
            {
                newPage.RequestedTheme = RequestedTheme;
            }
            AnimatePageTransition();
        }
    }

    private void AnimatePageTransition()
    {
        var transform = PageHost.RenderTransform as CompositeTransform ?? new CompositeTransform();
        PageHost.RenderTransform = transform;

        var duration = new Microsoft.UI.Xaml.Duration(System.TimeSpan.FromMilliseconds(220));
        var opacityAnim = new Microsoft.UI.Xaml.Media.Animation.DoubleAnimation
        {
            From = 0.3,
            To = 1.0,
            Duration = duration,
            EasingFunction = new Microsoft.UI.Xaml.Media.Animation.CubicEase { EasingMode = Microsoft.UI.Xaml.Media.Animation.EasingMode.EaseOut }
        };
        Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTarget(opacityAnim, PageHost);
        Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTargetProperty(opacityAnim, "Opacity");

        var translateAnim = new Microsoft.UI.Xaml.Media.Animation.DoubleAnimation
        {
            From = 14,
            To = 0,
            Duration = duration,
            EasingFunction = new Microsoft.UI.Xaml.Media.Animation.CubicEase { EasingMode = Microsoft.UI.Xaml.Media.Animation.EasingMode.EaseOut }
        };
        Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTarget(translateAnim, transform);
        Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTargetProperty(translateAnim, "TranslateY");

        var sb = new Microsoft.UI.Xaml.Media.Animation.Storyboard();
        sb.Children.Add(opacityAnim);
        sb.Children.Add(translateAnim);
        sb.Begin();
    }
}
