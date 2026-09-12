using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Path = Microsoft.UI.Xaml.Shapes.Path;

namespace RizUI.Uwp.Controls;

/// <summary>
/// 原生实现的 rizui_timeline。
/// 带刻度线的时间轴控件：填充进度条 + 倒三角指示器 + 气泡时间标签 + 底部时间刻度。
/// </summary>
public sealed class RizTimeline : Control
{
    private Canvas? _trackCanvas;
    private Border? _trackBar;
    private Border? _fillBar;
    private StackPanel? _indicator;
    private Border? _bubble;
    private TextBlock? _bubbleText;
    private Path? _arrow;
    private Canvas? _labelsCanvas;
    private bool _isDragging;
    private bool _templateApplied;

    public RizTimeline()
    {
        DefaultStyleKey = typeof(RizTimeline);
        IsTabStop = true;
        ActualThemeChanged += (_, _) => UpdateVisual();
        RegisterPropertyChangedCallback(IsEnabledProperty, (_, _) => UpdateVisual());
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(double), typeof(RizTimeline),
            new PropertyMetadata(0d, OnValueChanged));

    /// <summary>当前时间值（秒）。</summary>
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(RizTimeline),
            new PropertyMetadata(0d, OnRangeChanged));

    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(RizTimeline),
            new PropertyMetadata(120d, OnRangeChanged));

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly DependencyProperty StepProperty =
        DependencyProperty.Register(nameof(Step), typeof(double), typeof(RizTimeline),
            new PropertyMetadata(1d, OnRangeChanged));

    public double Step
    {
        get => (double)GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizTimeline),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    public string Color
    {
        get => (string)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizTimeline),
            new PropertyMetadata("auto", OnVisualChanged));

    public string Theme
    {
        get => (string)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public event EventHandler<double>? ValueChanged;

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var tl = (RizTimeline)d;
        tl.UpdatePositions();
        tl.ValueChanged?.Invoke(tl, (double)e.NewValue);
    }

    private static void OnRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizTimeline)d).UpdateVisual();
    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizTimeline)d).UpdateVisual();

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _trackCanvas = GetTemplateChild("TrackCanvas") as Canvas;
        _trackBar = GetTemplateChild("TrackBar") as Border;
        _fillBar = GetTemplateChild("FillBar") as Border;
        _indicator = GetTemplateChild("Indicator") as StackPanel;
        _bubble = GetTemplateChild("Bubble") as Border;
        _bubbleText = GetTemplateChild("BubbleText") as TextBlock;
        _arrow = GetTemplateChild("Arrow") as Path;
        _labelsCanvas = GetTemplateChild("LabelsCanvas") as Canvas;

        var hitArea = GetTemplateChild("HitArea") as FrameworkElement;
        if (hitArea != null)
        {
            hitArea.PointerPressed += OnTrackPointerPressed;
            hitArea.PointerMoved += OnTrackPointerMoved;
            hitArea.PointerReleased += OnTrackPointerReleased;
            hitArea.PointerCanceled += OnTrackPointerReleased;
            hitArea.PointerCaptureLost += OnTrackPointerReleased;
            hitArea.SizeChanged += (_, _) => UpdateVisual();
        }

        _templateApplied = true;
        UpdateVisual();
    }

    private void OnTrackPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (!IsEnabled) return;
        _isDragging = true;
        ((FrameworkElement)sender).CapturePointer(e.Pointer);
        UpdateValueFromPoint(e.GetCurrentPoint((UIElement)sender).Position.X, ((FrameworkElement)sender).ActualWidth);
        e.Handled = true;
    }

    private void OnTrackPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_isDragging) return;
        UpdateValueFromPoint(e.GetCurrentPoint((UIElement)sender).Position.X, ((FrameworkElement)sender).ActualWidth);
        e.Handled = true;
    }

    private void OnTrackPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (!_isDragging) return;
        _isDragging = false;
        ((FrameworkElement)sender).ReleasePointerCapture(e.Pointer);
        e.Handled = true;
    }

    private void UpdateValueFromPoint(double x, double totalWidth)
    {
        if (totalWidth <= 0) return;
        var ratio = Math.Clamp(x / totalWidth, 0, 1);
        var range = Maximum - Minimum;
        var rawVal = Minimum + ratio * range;
        var step = Step > 0 ? Step : 1;
        var stepsCount = Math.Round((rawVal - Minimum) / step);
        var rounded = Minimum + stepsCount * step;
        Value = Math.Round(Math.Clamp(rounded, Minimum, Maximum), 2);
    }

    private void UpdateVisual()
    {
        if (!_templateApplied || _trackCanvas == null || _trackBar == null || _fillBar == null) return;
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);
        var surface = RizVisuals.Surface(dark);

        _trackBar.Background = RizVisuals.Brush(RizVisuals.Border(dark));
        _fillBar.Background = RizVisuals.Brush(RizVisuals.Mix(accent, surface, 0.35));

        if (_bubble != null)
        {
            _bubble.Background = RizVisuals.Brush(surface);
            _bubble.BorderBrush = RizVisuals.Brush(accent);
        }
        if (_bubbleText != null)
        {
            _bubbleText.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        }
        if (_arrow != null)
        {
            _arrow.Fill = RizVisuals.Brush(accent);
        }

        RebuildTicksAndLabels(dark);
        UpdatePositions();
        Opacity = IsEnabled ? 1 : 0.45;
    }

    private void RebuildTicksAndLabels(bool dark)
    {
        if (_trackCanvas == null || _labelsCanvas == null) return;

        // 清除动态添加的刻度线（保留 TrackBar、FillBar 与 Indicator）
        var itemsToRemove = _trackCanvas.Children.Where(c => !ReferenceEquals(c, _trackBar) && !ReferenceEquals(c, _fillBar) && !ReferenceEquals(c, _indicator)).ToList();
        foreach (var item in itemsToRemove) _trackCanvas.Children.Remove(item);
        _labelsCanvas.Children.Clear();

        var totalWidth = _trackCanvas.ActualWidth > 0 ? _trackCanvas.ActualWidth : 300;
        var range = Maximum - Minimum;
        if (range <= 0) return;

        var interval = range <= 60 ? 5d : 10d;
        var labelInterval = range <= 60 ? 15d : 30d;

        for (var v = Minimum; v <= Maximum; v += interval)
        {
            var pct = (v - Minimum) / range;
            var isMinor = ((int)Math.Round(v) % (range <= 60 ? 10 : 20)) != 0;
            var tick = new Border
            {
                Width = 2,
                Height = isMinor ? 6 : 12,
                CornerRadius = new CornerRadius(1),
                Background = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), RizVisuals.Surface(dark), isMinor ? 0.14 : 0.22)),
                IsHitTestVisible = false
            };
            Canvas.SetLeft(tick, pct * totalWidth - 1);
            Canvas.SetTop(tick, isMinor ? 13 : 10);
            _trackCanvas.Children.Insert(2, tick);
        }

        for (var v = Minimum; v <= Maximum; v += labelInterval)
        {
            var pct = (v - Minimum) / range;
            var s = (int)Math.Round(v);
            var text = $"{s / 60}:{s % 60:D2}";
            var label = new TextBlock
            {
                Text = text,
                FontSize = 10,
                Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark)),
                IsHitTestVisible = false
            };
            Canvas.SetLeft(label, pct * totalWidth - 12);
            Canvas.SetTop(label, 0);
            _labelsCanvas.Children.Add(label);
        }
    }

    private void UpdatePositions()
    {
        if (_trackCanvas == null || _trackBar == null || _fillBar == null || _indicator == null || _bubbleText == null) return;
        var totalWidth = _trackCanvas.ActualWidth > 0 ? _trackCanvas.ActualWidth : 300;
        _trackBar.Width = totalWidth;

        var range = Maximum - Minimum;
        var pct = range > 0 ? Math.Clamp((Value - Minimum) / range, 0, 1) : 0;

        var posX = pct * totalWidth;
        // 拖动/点击时平滑跟随（80ms 缓动，对应 Web 的 transition: width/left 0.08s ease）
        RizVisuals.AnimateDouble(_fillBar, FrameworkElement.WidthProperty, "Width", posX, 80);

        var indicatorTransform = _indicator.RenderTransform as TranslateTransform;
        if (indicatorTransform == null)
        {
            // 必须显式初始化 X：Uno 中对从未赋值的依赖属性启动动画会拿 UnsetValue 转换而抛 InvalidCastException
            indicatorTransform = new TranslateTransform { X = 0 };
            _indicator.RenderTransform = indicatorTransform;
        }
        Canvas.SetLeft(_indicator, 0);
        // 指示器位移：从当前值出发的渲染层平滑追赶（对应 Web 的 transition: left 0.08s ease）
        var indicatorAnimation = new DoubleAnimation
        {
            From = indicatorTransform.X,
            To = posX - 24,
            Duration = new Duration(TimeSpan.FromMilliseconds(80)),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(indicatorAnimation, indicatorTransform);
        Storyboard.SetTargetProperty(indicatorAnimation, "X");
        var indicatorStoryboard = new Storyboard();
        indicatorStoryboard.Children.Add(indicatorAnimation);
        indicatorStoryboard.Begin();

        var s = (int)Math.Max(0, Math.Round(Value));
        _bubbleText.Text = $"{s / 60}:{s % 60:D2}";
    }
}
