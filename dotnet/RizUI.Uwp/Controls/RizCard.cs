using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Uwp.Controls;

/// <summary>原生实现的 rizui_card；<see cref="Content"/> 对应 Vue 默认插槽。</summary>
public sealed class RizCard : ContentControl
{
    private Border? _shadow1;
    private Border? _shadow2;
    private Border? _root;
    private TextBlock? _icon;
    private TextBlock? _title;
    private TextBlock? _desc;
    private ContentPresenter? _contentHost;

    public RizCard()
    {
        DefaultStyleKey = typeof(RizCard);
        ActualThemeChanged += (_, _) => ApplyVisual();
        RegisterPropertyChangedCallback(ContentProperty, (_, _) => ApplyVisual());
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(RizCard), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

    public static readonly DependencyProperty TitleStyleProperty =
        DependencyProperty.Register(nameof(TitleStyle), typeof(Style), typeof(RizCard), new PropertyMetadata(null, OnVisualChanged));
    public Style? TitleStyle { get => (Style?)GetValue(TitleStyleProperty); set => SetValue(TitleStyleProperty, value); }

    public static readonly DependencyProperty DescProperty =
        DependencyProperty.Register(nameof(Desc), typeof(string), typeof(RizCard), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Desc { get => (string)GetValue(DescProperty); set => SetValue(DescProperty, value); }

    public static readonly DependencyProperty DescStyleProperty =
        DependencyProperty.Register(nameof(DescStyle), typeof(Style), typeof(RizCard), new PropertyMetadata(null, OnVisualChanged));
    public Style? DescStyle { get => (Style?)GetValue(DescStyleProperty); set => SetValue(DescStyleProperty, value); }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(RizCard), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }

    public static readonly DependencyProperty IconStyleProperty =
        DependencyProperty.Register(nameof(IconStyle), typeof(Style), typeof(RizCard), new PropertyMetadata(null, OnVisualChanged));
    public Style? IconStyle { get => (Style?)GetValue(IconStyleProperty); set => SetValue(IconStyleProperty, value); }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizCard), new PropertyMetadata("auto", OnVisualChanged));
    public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _shadow1 = GetTemplateChild("Shadow1") as Border;
        _shadow2 = GetTemplateChild("Shadow2") as Border;
        _root = GetTemplateChild("Root") as Border;
        _icon = GetTemplateChild("Icon") as TextBlock;
        _title = GetTemplateChild("Title") as TextBlock;
        _desc = GetTemplateChild("Desc") as TextBlock;
        _contentHost = GetTemplateChild("ContentHost") as ContentPresenter;
        ApplyVisual();
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizCard)d).ApplyVisual();

    private void ApplyVisual()
    {
        var dark = RizVisuals.IsDark(this, Theme);
        if (_shadow1 != null)
            _shadow1.Background = RizVisuals.Brush(dark ? Windows.UI.Color.FromArgb(40, 0, 0, 0) : Windows.UI.Color.FromArgb(15, 30, 41, 59));
        if (_shadow2 != null)
            _shadow2.Background = RizVisuals.Brush(dark ? Windows.UI.Color.FromArgb(25, 0, 0, 0) : Windows.UI.Color.FromArgb(8, 30, 41, 59));
        if (_root != null)
        {
            _root.Background = RizVisuals.Brush(RizVisuals.SurfaceSoft(dark));
            _root.BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark));
        }
        if (_icon != null)
        {
            if (IconStyle != null) _icon.Style = IconStyle;
            RizVisuals.ApplyIcon(_icon, Icon, 20, RizVisuals.Icon(dark));
        }
        if (_title != null)
        {
            if (TitleStyle != null) _title.Style = TitleStyle;
            _title.Text = Title ?? string.Empty;
            _title.Visibility = string.IsNullOrWhiteSpace(Title) ? Visibility.Collapsed : Visibility.Visible;
            if (TitleStyle == null)
            {
                _title.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
                _title.Foreground = RizVisuals.Brush(RizVisuals.TitleText(dark));
            }
        }
        if (_desc != null)
        {
            if (DescStyle != null) _desc.Style = DescStyle;
            _desc.Text = Desc ?? string.Empty;
            _desc.Visibility = string.IsNullOrWhiteSpace(Desc) ? Visibility.Collapsed : Visibility.Visible;
            if (DescStyle == null) _desc.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
        }
        if (_contentHost != null)
            _contentHost.Visibility = Content is null ? Visibility.Collapsed : Visibility.Visible;
    }
}