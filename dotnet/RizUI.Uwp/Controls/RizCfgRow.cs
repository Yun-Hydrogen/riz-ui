using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Uwp.Controls;

/// <summary>原生实现的 rizui_cfgrow；<see cref="Content"/> 是右侧或 stack 模式下方的控件。</summary>
public sealed class RizCfgRow : ContentControl
{
    private Border? _root;
    private TextBlock? _icon;
    private TextBlock? _label;
    private TextBlock? _hint;
    private ContentPresenter? _contentHost;

    public RizCfgRow()
    {
        DefaultStyleKey = typeof(RizCfgRow);
        RegisterPropertyChangedCallback(ContentProperty, (_, _) => ApplyVisual());
    }

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(RizCfgRow), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }

    public static readonly DependencyProperty LabelStyleProperty =
        DependencyProperty.Register(nameof(LabelStyle), typeof(Style), typeof(RizCfgRow), new PropertyMetadata(null, OnVisualChanged));
    public Style? LabelStyle { get => (Style?)GetValue(LabelStyleProperty); set => SetValue(LabelStyleProperty, value); }

    public static readonly DependencyProperty HintProperty =
        DependencyProperty.Register(nameof(Hint), typeof(string), typeof(RizCfgRow), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Hint { get => (string)GetValue(HintProperty); set => SetValue(HintProperty, value); }

    public static readonly DependencyProperty HintStyleProperty =
        DependencyProperty.Register(nameof(HintStyle), typeof(Style), typeof(RizCfgRow), new PropertyMetadata(null, OnVisualChanged));
    public Style? HintStyle { get => (Style?)GetValue(HintStyleProperty); set => SetValue(HintStyleProperty, value); }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(RizCfgRow), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }

    public static readonly DependencyProperty IconStyleProperty =
        DependencyProperty.Register(nameof(IconStyle), typeof(Style), typeof(RizCfgRow), new PropertyMetadata(null, OnVisualChanged));
    public Style? IconStyle { get => (Style?)GetValue(IconStyleProperty); set => SetValue(IconStyleProperty, value); }

    public static readonly DependencyProperty StackProperty =
        DependencyProperty.Register(nameof(Stack), typeof(bool), typeof(RizCfgRow), new PropertyMetadata(false, OnVisualChanged));
    public bool Stack { get => (bool)GetValue(StackProperty); set => SetValue(StackProperty, value); }

    public static readonly DependencyProperty DisabledProperty =
        DependencyProperty.Register(nameof(Disabled), typeof(bool), typeof(RizCfgRow), new PropertyMetadata(false, OnVisualChanged));
    public bool Disabled { get => (bool)GetValue(DisabledProperty); set => SetValue(DisabledProperty, value); }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizCfgRow), new PropertyMetadata("auto", OnVisualChanged));
    public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _root = GetTemplateChild("Root") as Border;
        _icon = GetTemplateChild("Icon") as TextBlock;
        _label = GetTemplateChild("Label") as TextBlock;
        _hint = GetTemplateChild("Hint") as TextBlock;
        _contentHost = GetTemplateChild("ContentHost") as ContentPresenter;
        ApplyVisual();
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizCfgRow)d).ApplyVisual();

    private void ApplyVisual()
    {
        var dark = RizVisuals.IsDark(this, Theme);
        if (_root != null) _root.BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark));
        if (_icon != null)
        {
            _icon.Text = RizVisuals.ResolveIcon(Icon, string.Empty);
            _icon.Visibility = string.IsNullOrWhiteSpace(Icon) ? Visibility.Collapsed : Visibility.Visible;
            _icon.Style = IconStyle;
            _icon.Foreground = RizVisuals.Brush(RizVisuals.Icon(dark));
        }
        if (_label != null)
        {
            _label.Text = Label ?? string.Empty;
            _label.Visibility = string.IsNullOrWhiteSpace(Label) ? Visibility.Collapsed : Visibility.Visible;
            _label.Style = LabelStyle;
            _label.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        }
        if (_hint != null)
        {
            _hint.Text = Hint ?? string.Empty;
            _hint.Visibility = string.IsNullOrWhiteSpace(Hint) ? Visibility.Collapsed : Visibility.Visible;
            _hint.Style = HintStyle;
            _hint.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
        }
        if (_contentHost == null) return;

        _contentHost.Visibility = Content is null ? Visibility.Collapsed : Visibility.Visible;
        _contentHost.Opacity = Disabled ? 0.45 : 1;
        _contentHost.IsHitTestVisible = !Disabled;
        Grid.SetRow(_contentHost, Stack ? 1 : 0);
        Grid.SetColumn(_contentHost, Stack ? 0 : 2);
        Grid.SetColumnSpan(_contentHost, Stack ? 3 : 1);
        _contentHost.HorizontalAlignment = Stack ? HorizontalAlignment.Stretch : HorizontalAlignment.Right;
        _contentHost.VerticalAlignment = VerticalAlignment.Center;
        _contentHost.Margin = Stack ? new Thickness(0, 8, 0, 0) : new Thickness(0);
    }
}