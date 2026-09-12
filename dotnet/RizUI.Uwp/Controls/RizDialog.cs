using System;
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
/// 原生实现的 rizui_dialog。
/// 支持居中弹出、Scale &amp; Opacity 弹窗动画、遮罩毛玻璃渐变、右上角胶囊关闭按钮、底部倒计时进度条、图标与主题色定制。
/// Content 对应主体内容插槽，Footer 对应底部按钮区插槽。
/// </summary>
public sealed class RizDialog : ContentControl
{
    private Popup? _popup;
    private Grid? _overlay;
    private Grid? _boxContainer;
    private Border? _box;
    private TextBlock? _icon;
    private TextBlock? _title;
    private Border? _closeButton;
    private Path? _closePath;
    private SolidColorBrush? _closeBgBrush;
    private SolidColorBrush? _closeStrokeBrush;
    private ContentPresenter? _bodyPresenter;
    private ContentPresenter? _footerPresenter;
    private Border? _progressBar;
    private double _barFullWidth;
    private bool _barAnimPending;
    private double _manualProgress = -1;
    private ScaleTransform? _boxScale;
    private Storyboard? _enterStoryboard;
    private Storyboard? _exitStoryboard;
    private Storyboard? _progressStoryboard;
    private DispatcherQueueTimer? _countdownTimer;
    private FrameworkElement? _observedRoot;
    private bool _closing;
    private bool _notifyClose;

    public RizDialog()
    {
        DefaultStyleKey = typeof(RizDialog);
        Visibility = Visibility.Collapsed;
        Loaded += (_, _) => { if (Show) OpenDialog(); };
        Unloaded += (_, _) => CloseDialog(false);
        ActualThemeChanged += (_, _) => UpdateVisual();
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
    /// <summary>对话框宽度（如 360px、420px、auto）。</summary>
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

    public static readonly DependencyProperty CountdownProperty =
        DependencyProperty.Register(nameof(Countdown), typeof(double), typeof(RizDialog), new PropertyMetadata(0d, OnVisualChanged));
    /// <summary>自动关闭秒数；0 表示不自动关闭。大于 0 时自动在底部展示倒计时进度条并定时关闭。</summary>
    public double Countdown { get => (double)GetValue(CountdownProperty); set => SetValue(CountdownProperty, value); }

    public static readonly DependencyProperty ShowBarProperty =
        DependencyProperty.Register(nameof(ShowBar), typeof(bool), typeof(RizDialog), new PropertyMetadata(false, OnVisualChanged));
    /// <summary>是否显示底部进度条。当 Countdown > 0 时默认自动开启。</summary>
    public bool ShowBar { get => (bool)GetValue(ShowBarProperty); set => SetValue(ShowBarProperty, value); }

    public static readonly DependencyProperty ProgressProperty =
        DependencyProperty.Register(nameof(Progress), typeof(double), typeof(RizDialog), new PropertyMetadata(-1d, OnVisualChanged));
    /// <summary>手动设置进度值（0.0 ~ 1.0）。小于 0 时由 Countdown 倒计时自动驱动。</summary>
    public double Progress { get => (double)GetValue(ProgressProperty); set => SetValue(ProgressProperty, value); }

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
        AnimateEnter();

        if (Countdown > 0)
        {
            _countdownTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();
            _countdownTimer.Interval = TimeSpan.FromSeconds(Countdown);
            _countdownTimer.IsRepeating = false;
            _countdownTimer.Tick += (_, _) => Close();
            _countdownTimer.Start();
        }
    }

    private void BuildPopup()
    {
        _observedRoot = XamlRoot?.Content as FrameworkElement;
        var viewportWidth = Math.Max(1, _observedRoot?.ActualWidth ?? 800);
        var viewportHeight = Math.Max(1, _observedRoot?.ActualHeight ?? 600);
        if (_observedRoot != null)
        {
            _observedRoot.SizeChanged -= OnRootSizeChanged;
            _observedRoot.SizeChanged += OnRootSizeChanged;
        }

        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);

        // 遮罩层：对齐 Web 的 rgba(15, 23, 42, 0.45) 浅色与 rgba(0,0,0,0.60) 暗色
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

        // 弹窗外层容器：负责缩放与透明度动画以及多层柔和投影
        _boxScale = new ScaleTransform { ScaleX = 0.92, ScaleY = 0.92, CenterX = 0.5, CenterY = 0.5 };
        _boxContainer = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5),
            RenderTransform = _boxScale
        };

        var requestedWidth = RizVisuals.ParsePixels(WidthText, 0);
        if (requestedWidth > 0)
        {
            _boxContainer.Width = Math.Min(requestedWidth, viewportWidth * 0.9);
        }
        else
        {
            _boxContainer.MinWidth = 280;
            _boxContainer.MaxWidth = Math.Min(viewportWidth * 0.9, 640);
        }

        // 主体卡片：对齐 Web .cdlg-box（纯色背景 + 16px 圆角，无阴影、无描边），
        // 避免半透明假阴影的廉价感，以及浅色 1px 描边在深色遮罩上产生的抗锯齿细线
        _box = new Border
        {
            Background = RizVisuals.Brush(RizVisuals.Surface(dark)),
            CornerRadius = new CornerRadius(16),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        var layout = new Grid { CornerRadius = new CornerRadius(16) };
        layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Header
        layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Body
        layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Footer
        layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Progress Bar

        // 头部：图标 + 标题 + 胶囊关闭按钮（padding: 16px 20px 12px）
        var header = new Grid
        {
            ColumnSpacing = 10,
            Padding = new Thickness(20, 16, 16, 12)
        };
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        _icon = new TextBlock
        {
            FontSize = 16,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center
        };

        _title = new TextBlock
        {
            FontSize = 15,
            FontWeight = FontWeights.Bold,
            VerticalAlignment = VerticalAlignment.Center,
            TextTrimming = TextTrimming.CharacterEllipsis
        };

        var appFont = FontFamily ?? (_observedRoot as Control)?.FontFamily;
        if (appFont == null && Application.Current?.Resources?.TryGetValue("UiFont", out var uf) == true)
        {
            appFont = uf as FontFamily;
        }
        if (appFont != null)
        {
            _title.FontFamily = appFont;
            if (_bodyPresenter != null) _bodyPresenter.FontFamily = appFont;
            if (_footerPresenter != null) _footerPresenter.FontFamily = appFont;
        }

        _closeButton = CreateCloseButton(dark, accent);

        Grid.SetColumn(_icon, 0);
        Grid.SetColumn(_title, 1);
        Grid.SetColumn(_closeButton, 2);
        header.Children.Add(_icon);
        header.Children.Add(_title);
        header.Children.Add(_closeButton);
        Grid.SetRow(header, 0);
        layout.Children.Add(header);

        // 主体内容区（padding: 0 20px 8px）
        _bodyPresenter = new ContentPresenter
        {
            Content = Content,
            Margin = new Thickness(20, 0, 20, 8),
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        Grid.SetRow(_bodyPresenter, 1);
        layout.Children.Add(_bodyPresenter);

        // 底部按钮区（padding: 12px 20px 16px）
        _footerPresenter = new ContentPresenter
        {
            Content = Footer,
            Margin = new Thickness(20, 8, 20, 16),
            HorizontalAlignment = HorizontalAlignment.Right
        };
        Grid.SetRow(_footerPresenter, 2);
        layout.Children.Add(_footerPresenter);

        // 底部进度条：用“宽度动画”而不是缩放动画 —— 圆角不会被横向压扁，
        // 右端为对称半圆端帽（圆角 = 高度一半），任何进度下端点都是正圆
        _progressBar = new Border
        {
            Height = 4,
            Background = RizVisuals.Brush(accent),
            CornerRadius = new CornerRadius(0, 2, 2, 2),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom
        };
        Grid.SetRow(_progressBar, 2);
        Grid.SetRowSpan(_progressBar, 2);
        layout.Children.Add(_progressBar);
        layout.SizeChanged += OnBarLayoutSizeChanged;

        _box.Child = layout;
        _boxContainer.Children.Add(_box);
        _overlay.Children.Add(_boxContainer);

        _popup = new Popup
        {
            Child = _overlay,
            IsLightDismissEnabled = false,
            XamlRoot = XamlRoot
        };

        UpdateVisual();
    }

    private Border CreateCloseButton(bool dark, Windows.UI.Color accent)
    {
        // 专属画刷：hover 状态用颜色动画淡入淡出（对齐 Web 的 0.2s 过渡）
        _closeBgBrush = new SolidColorBrush(RizVisuals.Mix(RizVisuals.Text(dark), RizVisuals.Surface(dark), 0.08));
        _closeStrokeBrush = new SolidColorBrush(RizVisuals.Subtext(dark));

        var closeBorder = new Border
        {
            Width = 26,
            Height = 26,
            CornerRadius = new CornerRadius(13),
            Background = _closeBgBrush,
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
                    new LineGeometry { StartPoint = new Windows.Foundation.Point(8, 8), EndPoint = new Windows.Foundation.Point(18, 18) },
                    new LineGeometry { StartPoint = new Windows.Foundation.Point(18, 8), EndPoint = new Windows.Foundation.Point(8, 18) }
                }
            },
            Stroke = _closeStrokeBrush,
            StrokeThickness = 2,
            StrokeStartLineCap = PenLineCap.Round,
            StrokeEndLineCap = PenLineCap.Round,
            Width = 26,
            Height = 26,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        closeBorder.Child = _closePath;

        closeBorder.PointerEntered += (_, _) =>
        {
            if (_closeBgBrush == null || _closeStrokeBrush == null) return;
            RizVisuals.AnimateColor(_closeBgBrush, accent, 180);
            RizVisuals.AnimateColor(_closeStrokeBrush, Microsoft.UI.Colors.White, 180);
        };
        closeBorder.PointerExited += (_, _) =>
        {
            if (_closeBgBrush == null || _closeStrokeBrush == null) return;
            var isDk = RizVisuals.IsDark(this, Theme);
            RizVisuals.AnimateColor(_closeBgBrush, RizVisuals.Mix(RizVisuals.Text(isDk), RizVisuals.Surface(isDk), 0.08), 220);
            RizVisuals.AnimateColor(_closeStrokeBrush, RizVisuals.Subtext(isDk), 220);
        };
        closeBorder.PointerPressed += (_, e) =>
        {
            e.Handled = true;
            if (_closeBgBrush != null)
            {
                RizVisuals.AnimateColor(_closeBgBrush, RizVisuals.Mix(accent, Microsoft.UI.Colors.Black, 0.85), 100);
            }
        };
        closeBorder.PointerReleased += (_, e) =>
        {
            e.Handled = true;
            Close();
        };

        return closeBorder;
    }

    private void AnimateEnter()
    {
        if (_overlay == null || _boxContainer == null || _boxScale == null) return;

        _overlay.Opacity = 0;
        _boxContainer.Opacity = 0;
        _boxScale.ScaleX = 0.92;
        _boxScale.ScaleY = 0.92;

        var cubicEase = new CubicEase { EasingMode = EasingMode.EaseOut };
        var sb = new Storyboard();

        var overlayFade = new DoubleAnimation
        {
            To = 1,
            Duration = new Duration(TimeSpan.FromMilliseconds(220)),
            EasingFunction = cubicEase
        };
        Storyboard.SetTarget(overlayFade, _overlay);
        Storyboard.SetTargetProperty(overlayFade, "Opacity");

        var boxFade = new DoubleAnimation
        {
            To = 1,
            Duration = new Duration(TimeSpan.FromMilliseconds(220)),
            EasingFunction = cubicEase
        };
        Storyboard.SetTarget(boxFade, _boxContainer);
        Storyboard.SetTargetProperty(boxFade, "Opacity");

        var scaleXAnim = new DoubleAnimation
        {
            To = 1,
            Duration = new Duration(TimeSpan.FromMilliseconds(220)),
            EasingFunction = cubicEase
        };
        Storyboard.SetTarget(scaleXAnim, _boxScale);
        Storyboard.SetTargetProperty(scaleXAnim, "ScaleX");

        var scaleYAnim = new DoubleAnimation
        {
            To = 1,
            Duration = new Duration(TimeSpan.FromMilliseconds(220)),
            EasingFunction = cubicEase
        };
        Storyboard.SetTarget(scaleYAnim, _boxScale);
        Storyboard.SetTargetProperty(scaleYAnim, "ScaleY");

        sb.Children.Add(overlayFade);
        sb.Children.Add(boxFade);
        sb.Children.Add(scaleXAnim);
        sb.Children.Add(scaleYAnim);
        _enterStoryboard = sb;
        sb.Begin();

        // 倒计时进度条：countdown > 0 时从满宽线性收窄到 0（宽度动画，端帽圆角始终为正圆）
        if (Countdown > 0 && _progressBar != null && (ShowBar || Countdown > 0))
        {
            StartProgressAnimation();
        }
    }

    private void OnBarLayoutSizeChanged(object sender, SizeChangedEventArgs e)
    {
        _barFullWidth = e.NewSize.Width;
        if (_barAnimPending)
        {
            StartProgressAnimation();
        }
        else if (_progressBar != null && _manualProgress >= 0)
        {
            _progressBar.Width = _manualProgress * _barFullWidth;
        }
    }

    // 进度条宽度动画：不用 ScaleX 缩放，因此右端半圆端帽在任何进度下都不变形
    private void StartProgressAnimation()
    {
        if (_progressBar == null || Countdown <= 0) return;
        if (_barFullWidth < 1)
        {
            _barAnimPending = true; // 等首次布局拿到满宽后再启动
            return;
        }

        _barAnimPending = false;
        _progressBar.Width = _barFullWidth;
        var barAnim = new DoubleAnimation
        {
            From = _barFullWidth,
            To = 0,
            Duration = new Duration(TimeSpan.FromSeconds(Countdown)),
            EnableDependentAnimation = true
        };
        Storyboard.SetTarget(barAnim, _progressBar);
        Storyboard.SetTargetProperty(barAnim, "Width");
        var barSb = new Storyboard();
        barSb.Children.Add(barAnim);
        _progressStoryboard = barSb;
        barSb.Begin();
    }

    private void OnRootSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_overlay == null || _boxContainer == null) return;
        _overlay.Width = Math.Max(1, e.NewSize.Width);
        _overlay.Height = Math.Max(1, e.NewSize.Height);

        var requestedWidth = RizVisuals.ParsePixels(WidthText, 0);
        if (requestedWidth > 0)
        {
            _boxContainer.Width = Math.Min(requestedWidth, e.NewSize.Width * 0.9);
        }
        else
        {
            _boxContainer.MaxWidth = Math.Min(e.NewSize.Width * 0.9, 640);
        }
    }

    private void OnOverlayPressed(object sender, PointerRoutedEventArgs e)
    {
        if (MaskClose && ReferenceEquals(e.OriginalSource, _overlay))
        {
            Close();
        }
    }

    public void Close()
    {
        if (_popup?.IsOpen != true || _closing) return;
        CloseDialog(true);
    }

    private void CloseDialog(bool notify = true)
    {
        if (_countdownTimer != null)
        {
            _countdownTimer.Stop();
            _countdownTimer = null;
        }
        if (_progressStoryboard != null)
        {
            // 冻结当前宽度到基础值后再停止动画：直接 Stop 会让进度条瞬间回弹到 100% 而“闪一下”
            var frozen = _progressBar?.Width ?? 0;
            _progressStoryboard.Stop();
            if (_progressBar != null)
            {
                _progressBar.Width = frozen;
            }
            _progressStoryboard = null;
        }

        if (_popup?.IsOpen != true) return;
        if (_closing) return;
        _closing = true;
        _notifyClose = notify;

        if (_observedRoot != null)
        {
            _observedRoot.SizeChanged -= OnRootSizeChanged;
            _observedRoot = null;
        }

        if (_overlay == null || _boxContainer == null || _boxScale == null)
        {
            FinishClose();
            return;
        }

        var cubicEase = new CubicEase { EasingMode = EasingMode.EaseIn };
        var sb = new Storyboard();

        var overlayFade = new DoubleAnimation
        {
            To = 0,
            Duration = new Duration(TimeSpan.FromMilliseconds(200)),
            EasingFunction = cubicEase
        };
        Storyboard.SetTarget(overlayFade, _overlay);
        Storyboard.SetTargetProperty(overlayFade, "Opacity");

        var boxFade = new DoubleAnimation
        {
            To = 0,
            Duration = new Duration(TimeSpan.FromMilliseconds(200)),
            EasingFunction = cubicEase
        };
        Storyboard.SetTarget(boxFade, _boxContainer);
        Storyboard.SetTargetProperty(boxFade, "Opacity");

        var scaleXAnim = new DoubleAnimation
        {
            To = 0.92,
            Duration = new Duration(TimeSpan.FromMilliseconds(200)),
            EasingFunction = cubicEase
        };
        Storyboard.SetTarget(scaleXAnim, _boxScale);
        Storyboard.SetTargetProperty(scaleXAnim, "ScaleX");

        var scaleYAnim = new DoubleAnimation
        {
            To = 0.92,
            Duration = new Duration(TimeSpan.FromMilliseconds(200)),
            EasingFunction = cubicEase
        };
        Storyboard.SetTarget(scaleYAnim, _boxScale);
        Storyboard.SetTargetProperty(scaleYAnim, "ScaleY");

        sb.Children.Add(overlayFade);
        sb.Children.Add(boxFade);
        sb.Children.Add(scaleXAnim);
        sb.Children.Add(scaleYAnim);
        _exitStoryboard = sb;
        sb.Completed += (_, _) => FinishClose();
        sb.Begin();
    }

    private void FinishClose()
    {
        if (_popup != null)
        {
            _popup.IsOpen = false;
            _popup = null;
        }
        _overlay = null;
        _boxContainer = null;
        _box = null;
        _progressBar = null;
        _progressStoryboard = null;
        _barFullWidth = 0;
        _barAnimPending = false;
        _manualProgress = -1;
        _closing = false;

        if (GetValue(ShowProperty) is bool shown && shown)
        {
            SetValue(ShowProperty, false);
        }
        if (_notifyClose)
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        _notifyClose = false;
    }

    private void UpdateContent()
    {
        if (_bodyPresenter != null) _bodyPresenter.Content = Content;
    }

    private void UpdateVisual()
    {
        if (_box == null || _title == null || _icon == null || _closeButton == null) return;
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);

        _box.Background = RizVisuals.Brush(RizVisuals.Surface(dark));

        _title.Text = Title ?? string.Empty;
        _title.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        _title.Style = TitleStyle;

        var hasIcon = !string.IsNullOrWhiteSpace(Icon);
        _icon.Visibility = hasIcon ? Visibility.Visible : Visibility.Collapsed;
        if (hasIcon)
        {
            RizVisuals.ApplyIcon(_icon, Icon, 16, accent);
            if (IconStyle != null) _icon.Style = IconStyle;
        }

        _closeButton.Visibility = Closable ? Visibility.Visible : Visibility.Collapsed;
        if (_closeBgBrush != null)
        {
            _closeBgBrush.Color = RizVisuals.Mix(RizVisuals.Text(dark), RizVisuals.Surface(dark), 0.08);
        }
        if (_closeStrokeBrush != null)
        {
            _closeStrokeBrush.Color = RizVisuals.Subtext(dark);
        }

        if (_bodyPresenter != null)
        {
            _bodyPresenter.Content = Content;
            _bodyPresenter.Visibility = Content is null ? Visibility.Collapsed : Visibility.Visible;
        }

        if (_footerPresenter != null)
        {
            _footerPresenter.Content = Footer;
            _footerPresenter.Visibility = Footer is null ? Visibility.Collapsed : Visibility.Visible;
        }

        if (_progressBar != null)
        {
            var isBarVisible = ShowBar || Countdown > 0 || Progress >= 0;
            _progressBar.Visibility = isBarVisible ? Visibility.Visible : Visibility.Collapsed;
            _progressBar.Background = RizVisuals.Brush(accent);
            if (Progress >= 0)
            {
                _manualProgress = Math.Clamp(Progress, 0, 1);
                if (_barFullWidth >= 1)
                {
                    _progressBar.Width = _manualProgress * _barFullWidth;
                }
            }
        }
    }
}