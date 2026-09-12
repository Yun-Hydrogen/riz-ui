using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Path = Microsoft.UI.Xaml.Shapes.Path;

namespace RizUI.Uwp.Controls;

/// <summary>
/// 原生实现的 rizui_toast。
/// 支持 info / warn / error / 自定义颜色、倒计时平滑收缩进度条、右上角纵向排队栈与移除后平滑上移动画（FLIP 布局）。
/// </summary>
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
    private Border? _closeBorder;
    private Path? _closePath;
    private Border? _progressTrack;
    private Border? _progress;
    private ScaleTransform? _progressScale;
    private DispatcherQueueTimer? _timer;
    private DateTime _shownAt;
    private bool _closing;
    private bool _suppressClick;

    public RizToast()
    {
        DefaultStyleKey = typeof(RizToast);
        Visibility = Visibility.Collapsed;
        Loaded += (_, _) => OpenToast();
        Unloaded += (_, _) => Close(false);
        ActualThemeChanged += (_, _) => UpdateVisual();
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
    public event EventHandler? Click;
    public event EventHandler? Clicked;

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizToast)d).UpdateVisual();

    /// <summary>快捷静态触发方式，方便在任何页面一行代码弹出 Toast。</summary>
    public static RizToast ShowToast(XamlRoot xamlRoot, string title, string hint = "", string type = "info", double countdown = 3, string color = "", string icon = "", object? content = null, FontFamily? fontFamily = null)
    {
        var toast = new RizToast
        {
            XamlRoot = xamlRoot,
            Title = title,
            Hint = hint,
            Type = type,
            Countdown = countdown,
            Color = color,
            Icon = icon,
            Content = content,
            FontFamily = fontFamily ?? (xamlRoot?.Content as Control)?.FontFamily
        };
        toast.OpenToast();
        return toast;
    }

    public void OpenToast()
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
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = GetAccentColor(dark);

        _visual = new Grid { IsHitTestVisible = true };

        // 主体卡片边框与内边距（padding: 14px 40px 14px 16px，已去除阴影）
        _card = new Border
        {
            CornerRadius = new CornerRadius(14),
            Background = RizVisuals.Brush(RizVisuals.Surface(dark)),
            BorderThickness = new Thickness(1),
            BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark)),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        // 内部剪裁/内容包裹层：通过 CornerRadius 保证底部进度条被严格约束在圆角内部
        var cardInnerGrid = new Grid
        {
            CornerRadius = new CornerRadius(14),
            Background = RizVisuals.Brush(RizVisuals.Surface(dark))
        };

        var mainLayout = new Grid
        {
            Padding = new Thickness(16, 14, 40, 14)
        };
        mainLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        mainLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        _icon = new TextBlock
        {
            FontSize = 18,
            Margin = new Thickness(0, 2, 12, 0),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        Grid.SetColumn(_icon, 0);
        mainLayout.Children.Add(_icon);

        var textPanel = new StackPanel { Spacing = 0, VerticalAlignment = VerticalAlignment.Top };
        _title = new TextBlock
        {
            FontSize = 14,
            FontWeight = FontWeights.SemiBold,
            LineHeight = 21,
            TextWrapping = TextWrapping.Wrap
        };
        _hint = new TextBlock
        {
            FontSize = 12.5,
            LineHeight = 20,
            Margin = new Thickness(0, 2, 0, 0),
            TextWrapping = TextWrapping.Wrap
        };
        _contentHost = new ContentPresenter
        {
            Margin = new Thickness(0, 8, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        var appFont = FontFamily ?? (XamlRoot?.Content as Control)?.FontFamily;
        if (appFont == null && Application.Current?.Resources?.TryGetValue("UiFont", out var uf) == true)
        {
            appFont = uf as FontFamily;
        }
        if (appFont != null)
        {
            _title.FontFamily = appFont;
            _hint.FontFamily = appFont;
            _contentHost.FontFamily = appFont;
        }

        textPanel.Children.Add(_title);
        textPanel.Children.Add(_hint);
        textPanel.Children.Add(_contentHost);
        Grid.SetColumn(textPanel, 1);
        mainLayout.Children.Add(textPanel);

        // 底部进度条层：槽底衬（浅色为主题色混入淡白/浅底色，暗色下为半透明淡色） + 动态递减进度条
        _progressTrack = new Border
        {
            Height = 3,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Bottom,
            CornerRadius = new CornerRadius(0, 0, 14, 14)
        };

        _progressScale = new ScaleTransform { ScaleX = 1, ScaleY = 1, CenterX = 0, CenterY = 0 };
        _progress = new Border
        {
            Height = 3,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Bottom,
            CornerRadius = new CornerRadius(0, 0, 14, 14),
            Background = RizVisuals.Brush(accent),
            RenderTransformOrigin = new Windows.Foundation.Point(0, 0.5),
            RenderTransform = _progressScale
        };

        var barContainer = new Grid
        {
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Height = 3,
            CornerRadius = new CornerRadius(0, 0, 14, 14)
        };
        barContainer.Children.Add(_progressTrack);
        barContainer.Children.Add(_progress);

        cardInnerGrid.Children.Add(mainLayout);
        cardInnerGrid.Children.Add(barContainer);

        _card.Child = cardInnerGrid;
        _visual.Children.Add(_card);

        // 右上角胶囊圆形关闭按钮（22x22）
        _closeBorder = CreateCloseButton(dark, accent);
        _closeBorder.HorizontalAlignment = HorizontalAlignment.Right;
        _closeBorder.VerticalAlignment = VerticalAlignment.Top;
        _closeBorder.Margin = new Thickness(0, 10, 10, 0);
        _visual.Children.Add(_closeBorder);

        _visual.PointerPressed += OnVisualPressed;
        ActualThemeChanged += (_, _) => UpdateVisual();
        UpdateVisual();
    }

    private Border CreateCloseButton(bool dark, Windows.UI.Color accent)
    {
        var border = new Border
        {
            Width = 22,
            Height = 22,
            CornerRadius = new CornerRadius(11),
            Background = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), RizVisuals.Surface(dark), 0.08)),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            IsHitTestVisible = true
        };

        _closePath = new Path
        {
            Data = new GeometryGroup
            {
                Children =
                {
                    new LineGeometry { StartPoint = new Windows.Foundation.Point(7, 7), EndPoint = new Windows.Foundation.Point(15, 15) },
                    new LineGeometry { StartPoint = new Windows.Foundation.Point(15, 7), EndPoint = new Windows.Foundation.Point(7, 15) }
                }
            },
            Stroke = RizVisuals.Brush(RizVisuals.Subtext(dark)),
            StrokeThickness = 1.8,
            StrokeStartLineCap = PenLineCap.Round,
            StrokeEndLineCap = PenLineCap.Round,
            Width = 22,
            Height = 22,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        border.Child = _closePath;

        border.PointerEntered += (_, _) =>
        {
            var isDk = RizVisuals.IsDark(this, Theme);
            var ac = GetAccentColor(isDk);
            border.Background = RizVisuals.Brush(RizVisuals.Mix(ac, RizVisuals.Surface(isDk), 0.30));
            _closePath.Stroke = RizVisuals.Brush(ac);
        };
        border.PointerExited += (_, _) =>
        {
            var isDk = RizVisuals.IsDark(this, Theme);
            border.Background = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(isDk), RizVisuals.Surface(isDk), 0.08));
            _closePath.Stroke = RizVisuals.Brush(RizVisuals.Subtext(isDk));
        };
        border.PointerPressed += (_, e) =>
        {
            _suppressClick = true;
            e.Handled = true;
        };
        border.PointerReleased += (_, e) =>
        {
            _suppressClick = true;
            e.Handled = true;
            Close();
        };

        return border;
    }

    private void OnVisualPressed(object sender, PointerRoutedEventArgs e)
    {
        if (_suppressClick) { _suppressClick = false; return; }
        Click?.Invoke(this, EventArgs.Empty);
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    private Windows.UI.Color GetAccentColor(bool dark)
    {
        if (string.Equals(Type, "warn", StringComparison.OrdinalIgnoreCase))
            return Windows.UI.Color.FromArgb(255, 0xE6, 0xA2, 0x3C);
        if (string.Equals(Type, "error", StringComparison.OrdinalIgnoreCase))
            return Windows.UI.Color.FromArgb(255, 0xE0, 0x55, 0x55);
        return RizVisuals.ThemeColor(Color);
    }

    private string GetDefaultIcon()
    {
        if (string.Equals(Type, "warn", StringComparison.OrdinalIgnoreCase))
            return "fa-solid fa-triangle-exclamation";
        if (string.Equals(Type, "error", StringComparison.OrdinalIgnoreCase))
            return "fa-solid fa-circle-exclamation";
        return "fa-solid fa-circle-info";
    }

    private void UpdateVisual()
    {
        if (_card == null || _icon == null || _title == null || _hint == null || _closeBorder == null || _progress == null || _progressTrack == null) return;
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = GetAccentColor(dark);

        _card.Background = RizVisuals.Brush(RizVisuals.Surface(dark));
        _card.BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark));
        if (_card.Child is Grid innerGrid)
        {
            innerGrid.Background = RizVisuals.Brush(RizVisuals.Surface(dark));
        }

        var iconToUse = !string.IsNullOrWhiteSpace(Icon) ? Icon : GetDefaultIcon();
        RizVisuals.ApplyIcon(_icon, iconToUse, 18, accent);
        if (IconStyle != null) _icon.Style = IconStyle;

        _title.Text = Title ?? string.Empty;
        _title.Visibility = string.IsNullOrWhiteSpace(Title) ? Visibility.Collapsed : Visibility.Visible;
        _title.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        _title.Style = TitleStyle;

        _hint.Text = Hint ?? string.Empty;
        _hint.Visibility = string.IsNullOrWhiteSpace(Hint) ? Visibility.Collapsed : Visibility.Visible;
        _hint.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
        _hint.Style = HintStyle;

        if (_contentHost != null)
        {
            _contentHost.Content = Content;
            _contentHost.Visibility = Content is null ? Visibility.Collapsed : Visibility.Visible;
        }

        _closeBorder.Visibility = Closable ? Visibility.Visible : Visibility.Collapsed;
        _closeBorder.Background = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), RizVisuals.Surface(dark), 0.08));
        if (_closePath != null)
        {
            _closePath.Stroke = RizVisuals.Brush(RizVisuals.Subtext(dark));
        }

        _progress.Background = RizVisuals.Brush(accent);
        // 槽底衬：浅色下为主题色混入淡白背景（透出柔和的淡色槽位），暗色下为 25% 半透明
        var trackColor = dark
            ? RizVisuals.Mix(accent, Windows.UI.Color.FromArgb(0, 0, 0, 0), 0.25)
            : RizVisuals.Mix(accent, Microsoft.UI.Colors.White, 0.25);
        _progressTrack.Background = RizVisuals.Brush(trackColor);

        var isBarVisible = ShowBar && Countdown > 0;
        _progress.Visibility = isBarVisible ? Visibility.Visible : Visibility.Collapsed;
        _progressTrack.Visibility = isBarVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    private void AnimateEnter()
    {
        if (_visual == null) return;
        _visual.Opacity = 0;
        var transform = new CompositeTransform { TranslateX = 40 };
        _visual.RenderTransform = transform;

        var cubicEase = new CubicEase { EasingMode = EasingMode.EaseOut };
        var storyboard = new Storyboard();
        var opacity = new DoubleAnimation { To = 1, Duration = new Duration(TimeSpan.FromMilliseconds(250)), EasingFunction = cubicEase };
        var translate = new DoubleAnimation { To = 0, Duration = new Duration(TimeSpan.FromMilliseconds(250)), EasingFunction = cubicEase };
        Storyboard.SetTarget(opacity, _visual);
        Storyboard.SetTargetProperty(opacity, "Opacity");
        Storyboard.SetTarget(translate, transform);
        Storyboard.SetTargetProperty(translate, "TranslateX");
        storyboard.Children.Add(opacity);
        storyboard.Children.Add(translate);
        storyboard.Begin();

        if (Countdown > 0 && _progressScale != null && ShowBar)
        {
            _progressScale.ScaleX = 1;
            var progressAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(Countdown)),
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(progressAnimation, _progressScale);
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
        if (_visual == null)
        {
            FinishClose(notify, elapsed);
        }
        else
        {
            var cubicEase = new CubicEase { EasingMode = EasingMode.EaseIn };
            var storyboard = new Storyboard();
            var opacity = new DoubleAnimation { To = 0, Duration = new Duration(TimeSpan.FromMilliseconds(220)), EasingFunction = cubicEase };
            var transform = _visual.RenderTransform as CompositeTransform ?? new CompositeTransform();
            _visual.RenderTransform = transform;
            var translate = new DoubleAnimation { To = 40, Duration = new Duration(TimeSpan.FromMilliseconds(220)), EasingFunction = cubicEase };
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

            item._visual.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
            var height = item._visual.DesiredSize.Height > 0 ? item._visual.DesiredSize.Height : 68d;
            y += height + 10;

            if (animate && old.TryGetValue(item, out var previous) && Math.Abs(previous - next) > 0.5)
            {
                var transform = new TranslateTransform { Y = previous - next };
                item._visual.RenderTransform = transform;
                var animation = new DoubleAnimation
                {
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(animation, transform);
                Storyboard.SetTargetProperty(animation, "Y");
                var storyboard = new Storyboard();
                storyboard.Children.Add(animation);
                storyboard.Begin();
            }
        }
    }
}