using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace RizUI.Uwp.Controls;

/// <summary>原生实现的 rizui_infobox；<see cref="Content"/> 对应 Vue 默认插槽。</summary>
public sealed class RizInfoBox : ContentControl
{
    private Border? _root;
    private Rectangle? _outline;
    private TextBlock? _icon;
    private TextBlock? _text;
    private Border? _separator;
    private ContentPresenter? _contentHost;

    public RizInfoBox()
    {
        DefaultStyleKey = typeof(RizInfoBox);
        ActualThemeChanged += (_, _) => ApplyVisual();
        RegisterPropertyChangedCallback(ContentProperty, (_, _) => ApplyVisual());
    }

    public static readonly DependencyProperty TypeProperty =
        DependencyProperty.Register(nameof(Type), typeof(string), typeof(RizInfoBox), new PropertyMetadata("info", OnVisualChanged));
    public string Type { get => (string)GetValue(TypeProperty); set => SetValue(TypeProperty, value); }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(RizInfoBox), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

    public static readonly DependencyProperty TextStyleProperty =
        DependencyProperty.Register(nameof(TextStyle), typeof(Style), typeof(RizInfoBox), new PropertyMetadata(null, OnVisualChanged));
    public Style? TextStyle { get => (Style?)GetValue(TextStyleProperty); set => SetValue(TextStyleProperty, value); }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(RizInfoBox), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }

    public static readonly DependencyProperty IconStyleProperty =
        DependencyProperty.Register(nameof(IconStyle), typeof(Style), typeof(RizInfoBox), new PropertyMetadata(null, OnVisualChanged));
    public Style? IconStyle { get => (Style?)GetValue(IconStyleProperty); set => SetValue(IconStyleProperty, value); }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizInfoBox), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Color { get => (string)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizInfoBox), new PropertyMetadata("auto", OnVisualChanged));
    public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _root = GetTemplateChild("Root") as Border;
        _outline = GetTemplateChild("Outline") as Rectangle;
        _icon = GetTemplateChild("Icon") as TextBlock;
        _text = GetTemplateChild("Text") as TextBlock;
        _separator = GetTemplateChild("Separator") as Border;
        _contentHost = GetTemplateChild("ContentHost") as ContentPresenter;
        ApplyVisual();
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizInfoBox)d).ApplyVisual();

    private void ApplyVisual()
    {
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.PresetColor(Type, Color);
        var hasText = !string.IsNullOrWhiteSpace(Text);
        var hasContent = Content is not null;
        var defaultIcon = string.Equals(Type, "warn", StringComparison.OrdinalIgnoreCase) ? "fa-solid fa-triangle-exclamation"
            : string.Equals(Type, "error", StringComparison.OrdinalIgnoreCase) ? "fa-solid fa-circle-exclamation" : "fa-solid fa-circle-info";

        if (_root != null) _root.Background = RizVisuals.Brush(RizVisuals.Mix(accent, RizVisuals.Surface(dark), 0.14));
        if (_outline != null) _outline.Stroke = RizVisuals.Brush(accent);
        if (_icon != null)
        {
            var iconToUse = string.IsNullOrWhiteSpace(Icon) ? defaultIcon : Icon;
            RizVisuals.ApplyIcon(_icon, iconToUse, 16, accent);
            if (IconStyle != null) _icon.Style = IconStyle;
        }
        if (_text != null)
        {
            _text.Text = Text ?? string.Empty;
            _text.Visibility = hasText ? Visibility.Visible : Visibility.Collapsed;
            _text.Style = TextStyle;
            if (TextStyle == null) _text.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        }
        if (_separator != null)
        {
            _separator.Visibility = hasText && hasContent ? Visibility.Visible : Visibility.Collapsed;
            _separator.BorderBrush = RizVisuals.Brush(RizVisuals.Mix(accent, RizVisuals.Surface(dark), 0.35));
        }
        if (_contentHost != null) _contentHost.Visibility = hasContent ? Visibility.Visible : Visibility.Collapsed;
    }
}