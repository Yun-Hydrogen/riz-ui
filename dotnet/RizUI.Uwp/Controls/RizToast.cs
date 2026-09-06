using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Text;
using System.Collections.Generic;
using System.Linq;

namespace RizUI.Uwp.Controls;

/// <summary>原生实现的 rizui_toast。所有实例共享右上角宿主并按 Vue 的规则排队。</summary>
public sealed class RizToast : ContentControl
{
    private sealed class ToastHost
    {
        public required XamlRoot XamlRoot;
        public required Popup Popup;
        public required Grid Root;
        public required Canvas Canvas;
        public readonly List<RizToast> Items = new();
    }

    private static readonly List<ToastHost> Hosts = new();
    private ToastHost? _host;
    private Grid? _visual;
    private Border? _card;
    private TextBlock? _icon;
    private TextBlock? _title;
    private TextBlock? _hint;
    private ContentPresenter? _contentHost;
    private Button? _close;
    private Border? _progress;
    private DispatcherQueueTimer? _timer;
    private DateTime _shownAt;
    private bool _closing;
    private bool _suppressClick;

    public RizToast()
    {
        DefaultStyleKey = typeof(RizToast);
        Loaded += (_, _) => OpenToast();
        Unloaded += (_, _) => Close(false);
        RegisterPropertyChangedCallback(ContentProperty, (_, _) => UpdateVisual());
    }

    public static readonly DependencyProperty TypeProperty =
        DependencyProperty.Register(nameof(Type), typeof(string), typeof(RizToast), new PropertyMetadata("info", OnVisualChanged));
    public string Type { get => (string)GetValue(TypeProperty); set => SetValue(TypeProperty, value); }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(RizToast), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

    public static readonly DependencyProperty HintProperty =
        DependencyProperty.Register(nameof(Hint), typeof(string), typeof(RizToast), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Hint { get => (string)GetValue(HintProperty); set => SetValue(HintProperty, value); }

    public static readonly DependencyProperty TitleStyleProperty =
        DependencyProperty.Register(nameof(TitleStyle), typeof(Style), typeof(RizToast), new PropertyMetadata(null, OnVisualChanged));
    public Style? TitleStyle { get => (Style?)GetValue(TitleStyleProperty); set => SetValue(TitleStyleProperty, value); }

    public static readonly DependencyProperty HintStyleProperty =
        DependencyProperty.Register(nameof(HintStyle), typeof(Style), typeof(RizToast), new PropertyMetadata(null, OnVisualChanged));
    public Style? HintStyle { get => (Style?)GetValue(HintStyleProperty); set => SetValue(HintStyleProperty, value); }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(RizToast), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }

    public static readonly DependencyProperty IconStyleProperty =
        DependencyProperty.Register(nameof(IconStyle), typeof(Style), typeof(RizToast), new PropertyMetadata(null, OnVisualChanged));
    public Style? IconStyle { get => (Style?)GetValue(IconStyleProperty); set => SetValue(IconStyleProperty, value); }

    public static readonly DependencyProperty CountdownProperty =
        DependencyProperty.Register(nameof(Countdown), typeof(double), typeof(RizToast), new PropertyMetadata(3d));
    public double Countdown { get => (double)GetValue(CountdownProperty); set => SetValue(CountdownProperty, value); }

    public static readonly DependencyProperty ClosableProperty =
        DependencyProperty.Register(nameof(Closable), typeof(bool), typeof(RizToast), new PropertyMetadata(true, OnVisualChanged));
    public bool Closable { get => (bool)GetValue(ClosableProperty); set => SetValue(ClosableProperty, value); }

    public static readonly DependencyProperty ShowBarProperty =
        DependencyProperty.Register(nameof(ShowBar), typeof(bool), typeof(RizToast), new PropertyMetadata(true, OnVisualChanged));
    public bool ShowBar { get => (bool)GetValue(ShowBarProperty); set => SetValue(ShowBarProperty, value); }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizToast), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Color { get => (string)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

    public static readonly DependencyProperty TeleportProperty =
        DependencyProperty.Register(nameof(Teleport), typeof(string), typeof(RizToast), new PropertyMetadata("body"));
    public string Teleport { get => (string)GetValue(TeleportProperty); set => SetValue(TeleportProperty, value); }

    public static readonly DependencyProperty WidthTextProperty =
        DependencyProperty.Register(nameof(WidthText), typeof(string), typeof(RizToast), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string WidthText { get => (string)GetValue(WidthTextProperty); set => SetValue(WidthTextProperty, value); }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizToast), new PropertyMetadata("auto", OnVisualChanged));
    public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }

    public event EventHandler<double>? Closed;
    /// <summary>对应 Vue 的 click emit。</summary>
    public event EventHandler? Click;
    /// <summary>Clicked 为兼容性别名。</summary>
    public event EventHandler? Clicked;

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizToast)d).UpdateVisual();

    private void OpenToast()
    {
        if (_host != null || XamlRoot is null) return;
        _host = GetHost(XamlRoot);
        _host.Items.Add(this);
        BuildVisual();
        _host.Canvas.Children.Add(_visual!);
        _shownAt = DateTime.UtcNow;
        _host.Popup.IsOpen = true;
        LayoutHost(_host, false);
        AnimateEnter();

        if (Countdown > 0)
        {
            _timer = DispatcherQueue.GetForCurrentThread().CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(Countdown);
            _timer.IsRepeating = false;
            _timer.Tick += (_, _) => Close();
            _timer.Start();
        }
    }

    private static ToastHost GetHost(XamlRoot root)
    {
        var existing = Hosts.FirstOrDefault(h => ReferenceEquals(h.XamlRoot, root));
        if (existing != null) return existing;

        var canvas = new Canvas { IsHitTestVisible = true };
        var grid = new Grid { IsHitTestVisible = true };
        grid.Children.Add(canvas);
        var popup = new Popup { Child = grid, IsLightDismissEnabled = false, XamlRoot = root };
        var host = new ToastHost { XamlRoot = root, Popup = popup, Root = grid, Canvas = canvas };
        if (root.Content is FrameworkElement content)
        {
            content.SizeChanged += (_, _) => ResizeHost(host);
        }
        Hosts.Add(host);
        ResizeHost(host);
        return host;
    }

    private static void ResizeHost(ToastHost host)
    {
        if (host.XamlRoot.Content is not FrameworkElement content) return;
        host.Root.Width = Math.Max(1, content.ActualWidth);
        host.Root.Height = Math.Max(1, content.ActualHeight);
        host.Canvas.Width = host.Root.Width;
        host.Canvas.Height = host.Root.Height;
        LayoutHost(host, false);
    }

    private void BuildVisual()
    {
        _visual = new Grid { IsHitTestVisible = true };
        _card = new Border
        {
            Padding = new Thickness(16, 14, 40, 14),
            CornerRadius = new CornerRadius(14),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };
        _visual.Children.Add(_card);
        var content = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 12 };
        _icon = new TextBlock { FontSize = 18, Margin = new Thickness(0, 2, 0, 0), VerticalAlignment = VerticalAlignment.Top };
        var text = new StackPanel { Spacing = 0, VerticalAlignment = VerticalAlignment.Top };
        _title = new TextBlock { FontSize = 14, FontWeight = FontWeights.SemiBold, LineHeight = 21 };
        _hint = new TextBlock { FontSize = 12.5, LineHeight = 20, Margin = new Thickness(0, 2, 0, 0), TextWrapping = TextWrapping.Wrap };
        _contentHost = new ContentPresenter { Margin = new Thickness(0, 8, 0, 0) };
        text.Children.Add(_title);
        text.Children.Add(_hint);
        text.Children.Add(_contentHost);
        content.Children.Add(_icon);
        content.Children.Add(text);
        _card.Child = content;

        _close = new Button
        {
            Content = "×",
            Width = 22,
            Height = 22,
            Padding = new Thickness(0),
            FontSize = 16,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 10, 10, 0)
        };
        _close.PointerPressed += (_, _) => _suppressClick = true;
        _close.Click += (_, _) => Close();
        _visual.Children.Add(_close);

        var progressTrack = new Border { Height = 3, VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Stretch };
        _progress = new Border { Height = 3, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom };
        var progressGrid = new Grid { VerticalAlignment = VerticalAlignment.Bottom };
        progressGrid.Children.Add(progressTrack);
        progressGrid.Children.Add(_progress);
        _visual.Children.Add(progressGrid);
        _visual.PointerPressed += OnVisualPressed;
        UpdateVisual();
    }

    private void OnVisualPressed(object sender, PointerRoutedEventArgs e)
    {
        if (_suppressClick) { _suppressClick = false; return; }
        Click?.Invoke(this, EventArgs.Empty);
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateVisual()
    {
        if (_card == null || _icon == null || _title == null || _hint == null || _contentHost == null || _close == null || _progress == null) return;
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.PresetColor(Type, Color);
        var fallback = string.Equals(Type, "warn", StringComparison.OrdinalIgnoreCase) ? "⚠"
            : string.Equals(Type, "error", StringComparison.OrdinalIgnoreCase) ? "!" : "ⓘ";
        _card.Background = RizVisuals.Brush(RizVisuals.Surface(dark));
        _icon.Text = RizVisuals.ResolveIcon(Icon, fallback);
        _icon.Foreground = RizVisuals.Brush(accent);
        _icon.Style = IconStyle;
        _title.Text = Title ?? string.Empty;
        _title.Visibility = string.IsNullOrWhiteSpace(Title) ? Visibility.Collapsed : Visibility.Visible;
        _title.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        _title.Style = TitleStyle;
        _hint.Text = Hint ?? string.Empty;
        _hint.Visibility = string.IsNullOrWhiteSpace(Hint) ? Visibility.Collapsed : Visibility.Visible;
        _hint.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
        _hint.Style = HintStyle;
        _contentHost.Content = Content;
        _contentHost.Visibility = Content is null ? Visibility.Collapsed : Visibility.Visible;
        _close.Visibility = Closable ? Visibility.Visible : Visibility.Collapsed;
        _close.Background = RizVisuals.Brush(RizVisuals.Mix(accent, RizVisuals.Surface(dark), 0.12));
        _close.Foreground = RizVisuals.Brush(accent);
        _progress.Background = RizVisuals.Brush(accent);
        _progress.Visibility = ShowBar ? Visibility.Visible : Visibility.Collapsed;
    }

    private void AnimateEnter()
    {
        if (_visual == null) return;
        _visual.Opacity = 0;
        _visual.RenderTransform = new CompositeTransform { TranslateX = 40 };
        var storyboard = new Storyboard();
        var opacity = new DoubleAnimation { To = 1, Duration = new Duration(TimeSpan.FromMilliseconds(250)) };
        var translate = new DoubleAnimation { To = 0, Duration = new Duration(TimeSpan.FromMilliseconds(250)) };
        Storyboard.SetTarget(opacity, _visual);
        Storyboard.SetTargetProperty(opacity, "Opacity");
        Storyboard.SetTarget(translate, _visual.RenderTransform);
        Storyboard.SetTargetProperty(translate, "TranslateX");
        storyboard.Children.Add(opacity);
        storyboard.Children.Add(translate);
        storyboard.Begin();

        if (Countdown > 0 && _progress != null)
        {
            var scale = new ScaleTransform { ScaleX = 1, ScaleY = 1, CenterX = 0, CenterY = 0 };
            _progress.RenderTransform = scale;
            var progressAnimation = new DoubleAnimation { To = 0, Duration = new Duration(TimeSpan.FromSeconds(Countdown)), EnableDependentAnimation = true };
            Storyboard.SetTarget(progressAnimation, scale);
            Storyboard.SetTargetProperty(progressAnimation, "ScaleX");
            var progressStoryboard = new Storyboard();
            progressStoryboard.Children.Add(progressAnimation);
            progressStoryboard.Begin();
        }
    }

    public void Close() => Close(true);

    private void Close(bool notify)
    {
        if (_host == null || _closing) return;
        _closing = true;
        _timer?.Stop();
        var elapsed = Math.Round((DateTime.UtcNow - _shownAt).TotalSeconds, 1);
        if (_visual == null) FinishClose(notify, elapsed);
        else
        {
            var storyboard = new Storyboard();
            var opacity = new DoubleAnimation { To = 0, Duration = new Duration(TimeSpan.FromMilliseconds(250)) };
            var transform = _visual.RenderTransform as CompositeTransform ?? new CompositeTransform();
            _visual.RenderTransform = transform;
            var translate = new DoubleAnimation { To = 40, Duration = new Duration(TimeSpan.FromMilliseconds(250)) };
            Storyboard.SetTarget(opacity, _visual);
            Storyboard.SetTargetProperty(opacity, "Opacity");
            Storyboard.SetTarget(translate, transform);
            Storyboard.SetTargetProperty(translate, "TranslateX");
            storyboard.Children.Add(opacity);
            storyboard.Children.Add(translate);
            storyboard.Completed += (_, _) => FinishClose(notify, elapsed);
            storyboard.Begin();
        }
    }

    private void FinishClose(bool notify, double elapsed)
    {
        if (_host == null) return;
        var host = _host;
        host.Items.Remove(this);
        if (_visual != null) host.Canvas.Children.Remove(_visual);
        _host = null;
        _visual = null;
        _card = null;
        _closing = false;
        LayoutHost(host, true);
        if (host.Items.Count == 0)
        {
            host.Popup.IsOpen = false;
            Hosts.Remove(host);
        }
        if (notify) Closed?.Invoke(this, elapsed);
    }

    private static void LayoutHost(ToastHost host, bool animate)
    {
        var old = host.Items.ToDictionary(item => item, item => Canvas.GetTop(item._visual));
        var y = 20d;
        foreach (var item in host.Items)
        {
            if (item._visual == null) continue;
            var width = RizVisuals.ParsePixels(item.WidthText, 320);
            width = Math.Min(width, Math.Max(1, host.Root.Width) * 0.86);
            item._visual.Width = width;
            Canvas.SetLeft(item._visual, Math.Max(0, host.Root.Width - width - 20));
            var next = y;
            Canvas.SetTop(item._visual, next);
            y += item._visual.ActualHeight > 0 ? item._visual.ActualHeight + 10 : 70;
            if (animate && old.TryGetValue(item, out var previous) && Math.Abs(previous - next) > 0.5)
            {
                var transform = new TranslateTransform { Y = previous - next };
                item._visual.RenderTransform = transform;
                var animation = new DoubleAnimation { To = 0, Duration = new Duration(TimeSpan.FromMilliseconds(300)) };
                Storyboard.SetTarget(animation, transform);
                Storyboard.SetTargetProperty(animation, "Y");
                var storyboard = new Storyboard();
                storyboard.Children.Add(animation);
                storyboard.Begin();
            }
        }
    }
}