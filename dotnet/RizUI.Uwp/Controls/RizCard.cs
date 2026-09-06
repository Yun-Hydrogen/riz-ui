using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Uwp.Controls;

/// <summary>原生实现的 rizui_card；<see cref="Content"/> 对应 Vue 默认插槽。</summary>
public sealed class RizCard : ContentControl
{
    private Border? _root;
    private TextBlock? _icon;
    private TextBlock? _title;
    private TextBlock? _desc;
    private ContentPresenter? _contentHost;

    public RizCard()
    {
        DefaultStyleKey = typeof(RizCard);
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
        if (_root != null)
        {
            _root.Background = RizVisuals.Brush(RizVisuals.SurfaceSoft(dark));
            _root.BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark));
        }
        if (_icon != null)
        {
            _icon.Text = RizVisuals.ResolveIcon(Icon, string.Empty);
            _icon.Visibility = string.IsNullOrWhiteSpace(Icon) ? Visibility.Collapsed : Visibility.Visible;
            _icon.Style = IconStyle;
            _icon.Foreground = RizVisuals.Brush(RizVisuals.Icon(dark));
        }
        if (_title != null)
        {
            _title.Text = Title ?? string.Empty;
            _title.Visibility = string.IsNullOrWhiteSpace(Title) ? Visibility.Collapsed : Visibility.Visible;
            _title.Style = TitleStyle;
            _title.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        }
        if (_desc != null)
        {
            _desc.Text = Desc ?? string.Empty;
            _desc.Visibility = string.IsNullOrWhiteSpace(Desc) ? Visibility.Collapsed : Visibility.Visible;
            _desc.Style = DescStyle;
            _desc.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
        }
        if (_contentHost != null)
            _contentHost.Visibility = Content is null ? Visibility.Collapsed : Visibility.Visible;
    }
}