using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;

namespace RizUI.Uwp.Controls;

/// <summary>
/// 原生实现的 rizui_dropdown。
/// 胶囊触发按钮 + 展开下拉列表，支持点击外部自动关闭、活动项高亮、平滑进出场动画与全暗亮模式。
/// </summary>
public sealed class RizDropdown : Control
{
    private Border? _trigger;
    private TextBlock? _label;
    private Microsoft.UI.Xaml.Shapes.Path? _arrow;
    private Popup? _popup;
    private Border? _dropBorder;
    private StackPanel? _listHost;
    private bool _isOpen;
    private bool _templateApplied;

    public RizDropdown()
    {
        DefaultStyleKey = typeof(RizDropdown);
        IsTabStop = true;
        ActualThemeChanged += (_, _) => UpdateVisual();
        RegisterPropertyChangedCallback(IsEnabledProperty, (_, _) => UpdateVisual());
    }

    public static readonly DependencyProperty SelectedValueProperty =
        DependencyProperty.Register(nameof(SelectedValue), typeof(string), typeof(RizDropdown),
            new PropertyMetadata(string.Empty, OnSelectedValueChanged));

    /// <summary>v-model 绑定值（选项 Value）。</summary>
    public string SelectedValue
    {
        get => (string)GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    public static readonly DependencyProperty OptionsProperty =
        DependencyProperty.Register(nameof(Options), typeof(IList<RizDropdownItem>), typeof(RizDropdown),
            new PropertyMetadata(null, OnOptionsChanged));

    /// <summary>下拉选项列表。</summary>
    public IList<RizDropdownItem>? Options
    {
        get => (IList<RizDropdownItem>?)GetValue(OptionsProperty);
        set => SetValue(OptionsProperty, value);
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizDropdown),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    /// <summary>自定义颜色，空则跟随主题色。</summary>
    public string Color
    {
        get => (string)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizDropdown),
            new PropertyMetadata("auto", OnVisualChanged));

    /// <summary>主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局。</summary>
    public string Theme
    {
        get => (string)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public event EventHandler<string>? SelectionChanged;

    private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var dd = (RizDropdown)d;
        dd.UpdateLabel();
        dd.RebuildOptions();
        dd.SelectionChanged?.Invoke(dd, (string)e.NewValue);
    }

    private static void OnOptionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var dd = (RizDropdown)d;
        dd.UpdateLabel();
        dd.RebuildOptions();
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((RizDropdown)d).UpdateVisual();
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _trigger = GetTemplateChild("Trigger") as Border;
        _label = GetTemplateChild("Label") as TextBlock;
        _arrow = GetTemplateChild("Arrow") as Microsoft.UI.Xaml.Shapes.Path;
        _popup = GetTemplateChild("DropPopup") as Popup;
        _dropBorder = GetTemplateChild("DropBorder") as Border;
        _listHost = GetTemplateChild("ListHost") as StackPanel;

        if (_trigger != null)
        {
            _trigger.PointerPressed += (_, e) =>
            {
                if (!IsEnabled) return;
                e.Handled = true;
                Toggle();
            };
        }

        if (_popup != null)
        {
            _popup.Closed += (_, _) =>
            {
                _isOpen = false;
                UpdateTriggerVisual();
            };
        }

        _templateApplied = true;
        UpdateVisual();
        UpdateLabel();
        RebuildOptions();
    }

    private bool _isDropUp;

    private void Toggle()
    {
        if (_isOpen) Close();
        else Open();
    }

    private void Open()
    {
        if (_popup == null || _isOpen) return;
        _isOpen = true;
        UpdateTriggerVisual();
        RebuildOptions();

        // 测量并判断是否超出屏幕下方，如超出则自动改为向上弹出
        _isDropUp = false;
        int optCount = Options?.Count ?? 0;
        double popupHeight = Math.Max(40, (optCount > 0 ? optCount : 1) * 36 + 12);
        if (_dropBorder != null && _dropBorder.ActualHeight > 0)
        {
            popupHeight = _dropBorder.ActualHeight;
        }

        try
        {
            var triggerTransform = _trigger?.TransformToVisual(null);
            if (triggerTransform != null && XamlRoot != null)
            {
                var point = triggerTransform.TransformPoint(new Windows.Foundation.Point(0, 0));
                double triggerHeight = _trigger?.ActualHeight ?? 30;
                double triggerBottom = point.Y + triggerHeight;
                double windowHeight = XamlRoot.Size.Height;

                // 留出 12px 屏幕边缘安全间距
                if (triggerBottom + popupHeight + 12 > windowHeight && point.Y - popupHeight - 6 >= 0)
                {
                    _isDropUp = true;
                }
            }
        }
        catch
        {
            _isDropUp = false;
        }

        // 设置垂直偏移量：向下弹出为 +36（高度30+间距6），向上弹出为 -(popupHeight + 6)
        _popup.VerticalOffset = _isDropUp ? -(popupHeight + 6) : 36;
        _popup.IsOpen = true;

        if (_dropBorder != null)
        {
            double startY = _isDropUp ? 6 : -6;
            var transform = new TranslateTransform { Y = startY };
            _dropBorder.RenderTransform = transform;
            _dropBorder.Opacity = 0;

            var sb = new Storyboard();
            var fade = new DoubleAnimation { To = 1, Duration = new Duration(TimeSpan.FromMilliseconds(150)) };
            Storyboard.SetTarget(fade, _dropBorder);
            Storyboard.SetTargetProperty(fade, "Opacity");

            var move = new DoubleAnimation { To = 0, Duration = new Duration(TimeSpan.FromMilliseconds(150)), EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
            Storyboard.SetTarget(move, transform);
            Storyboard.SetTargetProperty(move, "Y");

            sb.Children.Add(fade);
            sb.Children.Add(move);
            sb.Begin();
        }
    }

    private void Close()
    {
        if (_popup == null || !_isOpen) return;
        _isOpen = false;
        UpdateTriggerVisual();
        if (_dropBorder != null)
        {
            var transform = _dropBorder.RenderTransform as TranslateTransform ?? new TranslateTransform();
            _dropBorder.RenderTransform = transform;
            var sb = new Storyboard();
            var fade = new DoubleAnimation { To = 0, Duration = new Duration(TimeSpan.FromMilliseconds(120)) };
            Storyboard.SetTarget(fade, _dropBorder);
            Storyboard.SetTargetProperty(fade, "Opacity");

            double endY = _isDropUp ? 6 : -6;
            var move = new DoubleAnimation { To = endY, Duration = new Duration(TimeSpan.FromMilliseconds(120)), EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn } };
            Storyboard.SetTarget(move, transform);
            Storyboard.SetTargetProperty(move, "Y");

            sb.Children.Add(fade);
            sb.Children.Add(move);
            sb.Completed += (_, _) => _popup.IsOpen = false;
            sb.Begin();
        }
        else
        {
            _popup.IsOpen = false;
        }
    }

    private void UpdateLabel()
    {
        if (_label == null) return;
        var selected = Options?.FirstOrDefault(o => o.Value == SelectedValue);
        _label.Text = selected != null ? selected.Label : (SelectedValue ?? string.Empty);
    }

    private void UpdateVisual()
    {
        if (!_templateApplied) return;
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);

        UpdateTriggerVisual();

        if (_dropBorder != null)
        {
            _dropBorder.Background = RizVisuals.Brush(RizVisuals.Surface(dark));
            _dropBorder.BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark));
        }

        RebuildOptions();
    }

    private void UpdateTriggerVisual()
    {
        if (_trigger == null || _label == null || _arrow == null) return;
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);

        _trigger.Background = RizVisuals.Brush(RizVisuals.Surface(dark));
        _trigger.BorderBrush = RizVisuals.Brush(_isOpen ? accent : RizVisuals.Border(dark));
        _label.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        _arrow.Stroke = RizVisuals.Brush(RizVisuals.Subtext(dark));

        var arrowTransform = _arrow.RenderTransform as RotateTransform ?? new RotateTransform { CenterX = 5, CenterY = 4 };
        _arrow.RenderTransform = arrowTransform;
        arrowTransform.CenterX = 5;
        arrowTransform.CenterY = 4;
        arrowTransform.Angle = _isOpen ? 180 : 0;

        Opacity = IsEnabled ? 1 : 0.45;
    }

    private void RebuildOptions()
    {
        if (_listHost == null) return;
        _listHost.Children.Clear();
        if (Options == null) return;

        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);
        var surface = RizVisuals.Surface(dark);
        // 选中项背景：对齐 Web 端 color-mix(in srgb, accent 10%, surface)
        var activeBg = RizVisuals.Mix(accent, surface, 0.10);
        var hoverBg = RizVisuals.Mix(RizVisuals.Text(dark), surface, 0.06);

        foreach (var opt in Options)
        {
            var isSelected = opt.Value == SelectedValue;
            var itemBorder = new Border
            {
                Padding = new Thickness(14, 7, 14, 7),
                CornerRadius = new CornerRadius(7),
                Background = isSelected ? RizVisuals.Brush(activeBg) : new SolidColorBrush(Microsoft.UI.Colors.Transparent),
                IsHitTestVisible = true
            };

            var itemText = new TextBlock
            {
                Text = opt.Label,
                FontSize = 12,
                FontWeight = isSelected ? Microsoft.UI.Text.FontWeights.SemiBold : Microsoft.UI.Text.FontWeights.Normal,
                Foreground = RizVisuals.Brush(isSelected ? accent : RizVisuals.Text(dark))
            };
            itemBorder.Child = itemText;

            itemBorder.PointerEntered += (_, _) =>
            {
                if (opt.Value != SelectedValue)
                {
                    itemBorder.Background = RizVisuals.Brush(hoverBg);
                }
            };
            itemBorder.PointerExited += (_, _) =>
            {
                if (opt.Value != SelectedValue)
                {
                    itemBorder.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
                }
            };
            itemBorder.PointerPressed += (_, e) =>
            {
                e.Handled = true;
                SelectedValue = opt.Value;
                Close();
            };

            _listHost.Children.Add(itemBorder);
        }
    }
}
