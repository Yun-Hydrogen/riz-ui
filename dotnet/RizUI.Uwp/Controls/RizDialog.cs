using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Text;

namespace RizUI.Uwp.Controls;

/// <summary>
/// 原生实现的 rizui_dialog。Content 对应默认插槽，Footer 对应 footer 插槽。
/// </summary>
public sealed class RizDialog : ContentControl
{
    private Popup? _popup;
    private Grid? _overlay;
    private Border? _box;
    private TextBlock? _icon;
    private TextBlock? _title;
    private Button? _close;
    private ContentPresenter? _body;
    private ContentPresenter? _footer;
    private bool _closing;
    private bool _notifyClose;

    public RizDialog()
    {
        DefaultStyleKey = typeof(RizDialog);
        Loaded += (_, _) => { if (Show) OpenDialog(); };
        Unloaded += (_, _) => CloseDialog(false);
        RegisterPropertyChangedCallback(ContentProperty, (_, _) => UpdateContent());
    }

    public static readonly DependencyProperty ShowProperty =
        DependencyProperty.Register(nameof(Show), typeof(bool), typeof(RizDialog), new PropertyMetadata(false, OnShowChanged));
    public bool Show { get => (bool)GetValue(ShowProperty); set => SetValue(ShowProperty, value); }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(RizDialog), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

    public static readonly DependencyProperty TitleStyleProperty =
        DependencyProperty.Register(nameof(TitleStyle), typeof(Style), typeof(RizDialog), new PropertyMetadata(null, OnVisualChanged));
    public Style? TitleStyle { get => (Style?)GetValue(TitleStyleProperty); set => SetValue(TitleStyleProperty, value); }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(RizDialog), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }

    public static readonly DependencyProperty IconStyleProperty =
        DependencyProperty.Register(nameof(IconStyle), typeof(Style), typeof(RizDialog), new PropertyMetadata(null, OnVisualChanged));
    public Style? IconStyle { get => (Style?)GetValue(IconStyleProperty); set => SetValue(IconStyleProperty, value); }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizDialog), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Color { get => (string)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

    public static readonly DependencyProperty WidthTextProperty =
        DependencyProperty.Register(nameof(WidthText), typeof(string), typeof(RizDialog), new PropertyMetadata("auto", OnVisualChanged));
    /// <summary>对应 Vue 的 width prop；使用 WidthText 避免遮蔽 FrameworkElement.Width。</summary>
    public string WidthText { get => (string)GetValue(WidthTextProperty); set => SetValue(WidthTextProperty, value); }

    public static readonly DependencyProperty ClosableProperty =
        DependencyProperty.Register(nameof(Closable), typeof(bool), typeof(RizDialog), new PropertyMetadata(true, OnVisualChanged));
    public bool Closable { get => (bool)GetValue(ClosableProperty); set => SetValue(ClosableProperty, value); }

    public static readonly DependencyProperty MaskCloseProperty =
        DependencyProperty.Register(nameof(MaskClose), typeof(bool), typeof(RizDialog), new PropertyMetadata(true));
    public bool MaskClose { get => (bool)GetValue(MaskCloseProperty); set => SetValue(MaskCloseProperty, value); }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizDialog), new PropertyMetadata("auto", OnVisualChanged));
    public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }

    public static readonly DependencyProperty FooterProperty =
        DependencyProperty.Register(nameof(Footer), typeof(object), typeof(RizDialog), new PropertyMetadata(null, OnVisualChanged));
    /// <summary>底部按钮区内容，对应 Vue 的 footer 插槽。</summary>
    public object? Footer { get => GetValue(FooterProperty); set => SetValue(FooterProperty, value); }

    public event EventHandler? Closed;

    private static void OnShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var dialog = (RizDialog)d;
        if ((bool)e.NewValue) dialog.OpenDialog();
        else dialog.CloseDialog();
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizDialog)d).UpdateVisual();

    private void OpenDialog()
    {
        if (_popup?.IsOpen == true || XamlRoot is null) return;
        _closing = false;
        BuildPopup();
        _popup!.IsOpen = true;

        if (_overlay != null)
        {
            _overlay.Opacity = 0;
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation { To = 1, Duration = new Duration(TimeSpan.FromMilliseconds(220)) };
            Storyboard.SetTarget(animation, _overlay);
            Storyboard.SetTargetProperty(animation, "Opacity");
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }
    }

    private void BuildPopup()
    {
        var contentRoot = XamlRoot?.Content as FrameworkElement;
        var viewportWidth = Math.Max(1, contentRoot?.ActualWidth ?? 800);
        var viewportHeight = Math.Max(1, contentRoot?.ActualHeight ?? 600);
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);

        _overlay = new Grid
        {
            Width = viewportWidth,
            Height = viewportHeight,
            Background = RizVisuals.Brush(dark
                ? Windows.UI.Color.FromArgb(153, 0, 0, 0)
                : Windows.UI.Color.FromArgb(115, 15, 23, 42)),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };
        _overlay.PointerPressed += OnOverlayPressed;

        _box = new Border
        {
            MinWidth = 280,
            MaxWidth = viewportWidth * 0.9,
            Background = RizVisuals.Brush(RizVisuals.Surface(dark)),
            CornerRadius = new CornerRadius(16),
            Padding = new Thickness(20, 16, 20, 16),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        var requestedWidth = RizVisuals.ParsePixels(WidthText, 0);
        if (requestedWidth > 0) _box.Width = Math.Min(requestedWidth, viewportWidth * 0.9);

        var layout = new Grid { RowSpacing = 0 };
        layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var header = new Grid { ColumnSpacing = 10 };
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        _icon = new TextBlock { FontSize = 16, VerticalAlignment = VerticalAlignment.Center };
        _title = new TextBlock { FontSize = 15, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
        _close = new Button
        {
            Content = "×",
            Width = 26,
            Height = 26,
            Padding = new Thickness(0),
            FontSize = 18,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        _close.Click += (_, _) => Close();
        Grid.SetColumn(_icon, 0);
        Grid.SetColumn(_title, 1);
        Grid.SetColumn(_close, 2);
        header.Children.Add(_icon);
        header.Children.Add(_title);
        header.Children.Add(_close);
        layout.Children.Add(header);

        _body = new ContentPresenter { Content = Content, Margin = new Thickness(0, 12, 0, 8) };
        Grid.SetRow(_body, 1);
        layout.Children.Add(_body);
        _footer = new ContentPresenter { Content = Footer, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 12, 0, 0) };
        Grid.SetRow(_footer, 2);
        layout.Children.Add(_footer);
        _box.Child = layout;
        Grid.SetColumn(_box, 0);
        _overlay.Children.Add(_box);

        _popup = new Popup { Child = _overlay, IsLightDismissEnabled = false, XamlRoot = XamlRoot };
        UpdateVisual();
    }

    private void OnOverlayPressed(object sender, PointerRoutedEventArgs e)
    {
        if (MaskClose && ReferenceEquals(e.OriginalSource, _overlay)) Close();
    }

    public void Close()
    {
        if (_popup?.IsOpen != true || _closing) return;
        CloseDialog();
    }

    private void CloseDialog(bool notify = true)
    {
        if (_popup?.IsOpen != true)
            return;
        
        if (_closing) return;
        _closing = true;
        _notifyClose = notify;

        var storyboard = new Storyboard();
        var animation = new DoubleAnimation { To = 0, Duration = new Duration(TimeSpan.FromMilliseconds(220)) };
        Storyboard.SetTarget(animation, _overlay);
        Storyboard.SetTargetProperty(animation, "Opacity");
        storyboard.Children.Add(animation);
        storyboard.Completed += (_, _) =>
        {
            _popup!.IsOpen = false;
            _popup = null;
            _overlay = null;
            _closing = false;
            if (GetValue(ShowProperty) is bool shown && shown) SetValue(ShowProperty, false);
            if (_notifyClose) Closed?.Invoke(this, EventArgs.Empty);
            _notifyClose = false;
        };
        storyboard.Begin();
    }

    private void UpdateContent()
    {
        if (_body != null) _body.Content = Content;
    }

    private void UpdateVisual()
    {
        if (_box == null || _title == null || _icon == null || _close == null || _body == null || _footer == null) return;
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);
        _box.Background = RizVisuals.Brush(RizVisuals.Surface(dark));
        _title.Text = Title ?? string.Empty;
        _title.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        _title.Style = TitleStyle;
        RizVisuals.ApplyIcon(_icon, Icon, 16, accent);
        if (IconStyle != null) _icon.Style = IconStyle;
        _close.Visibility = Closable ? Visibility.Visible : Visibility.Collapsed;
        _close.Background = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), RizVisuals.Surface(dark), 0.08));
        _close.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
        _footer.Content = Footer;
        _footer.Visibility = Footer is null ? Visibility.Collapsed : Visibility.Visible;
    }
}