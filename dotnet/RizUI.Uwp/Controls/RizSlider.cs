using System;
using System.Globalization;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace RizUI.Uwp.Controls;

/// <summary>
/// 原生实现的 rizui_slider。
/// 胶囊步进滑条：左侧减号按钮、中间数值/单位展示、右侧加号按钮。
/// 支持左右拖拽连续调节、点击 ± 步进微调、长按连续增减以及主题色背景填充。
/// </summary>
public sealed class RizSlider : Control
{
    private Border? _root;
    private Border? _fill;
    private FrameworkElement? _btnMinus;
    private FrameworkElement? _btnPlus;
    private TextBlock? _valueText;
    private DispatcherQueueTimer? _repeatTimer;
    private DispatcherQueueTimer? _initialDelayTimer;
    private int _repeatDirection;
    private bool _isDragging;
    private bool _templateApplied;

    public RizSlider()
    {
        DefaultStyleKey = typeof(RizSlider);
        IsTabStop = true;
        RegisterPropertyChangedCallback(IsEnabledProperty, (_, _) => UpdateVisual());
        ActualThemeChanged += (_, _) => UpdateVisual();
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(double), typeof(RizSlider),
            new PropertyMetadata(0d, OnValueChanged));

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(RizSlider),
            new PropertyMetadata(0d, OnRangeChanged));

    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(RizSlider),
            new PropertyMetadata(100d, OnRangeChanged));

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly DependencyProperty StepProperty =
        DependencyProperty.Register(nameof(Step), typeof(double), typeof(RizSlider),
            new PropertyMetadata(1d, OnRangeChanged));

    public double Step
    {
        get => (double)GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    public static readonly DependencyProperty DisplayProperty =
        DependencyProperty.Register(nameof(Display), typeof(string), typeof(RizSlider),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    /// <summary>显示单位文案，如 "%"、"px"、"倍速"。</summary>
    public string Display
    {
        get => (string)GetValue(DisplayProperty);
        set => SetValue(DisplayProperty, value);
    }

    public static readonly DependencyProperty DisplayValueProperty =
        DependencyProperty.Register(nameof(DisplayValue), typeof(string), typeof(RizSlider),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    /// <summary>自定义显示文本覆盖。</summary>
    public string DisplayValue
    {
        get => (string)GetValue(DisplayValueProperty);
        set => SetValue(DisplayValueProperty, value);
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizSlider),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    public string Color
    {
        get => (string)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizSlider),
            new PropertyMetadata("auto", OnVisualChanged));

    public string Theme
    {
        get => (string)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public event EventHandler<double>? ValueChanged;

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var s = (RizSlider)d;
        s.UpdateVisual();
        s.ValueChanged?.Invoke(s, (double)e.NewValue);
    }

    private static void OnRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizSlider)d).UpdateVisual();
    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizSlider)d).UpdateVisual();

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _root = GetTemplateChild("Root") as Border;
        _fill = GetTemplateChild("Fill") as Border;
        _btnMinus = GetTemplateChild("BtnMinus") as FrameworkElement;
        _btnPlus = GetTemplateChild("BtnPlus") as FrameworkElement;
        _valueText = GetTemplateChild("ValueText") as TextBlock;

        if (_btnMinus != null)
        {
            _btnMinus.PointerPressed += (_, e) => StartRepeat(-1, e);
            _btnMinus.PointerReleased += (_, e) => StopRepeat(e);
            _btnMinus.PointerCanceled += (_, e) => StopRepeat(e);
            _btnMinus.PointerCaptureLost += (_, e) => StopRepeat(e);
        }

        if (_btnPlus != null)
        {
            _btnPlus.PointerPressed += (_, e) => StartRepeat(1, e);
            _btnPlus.PointerReleased += (_, e) => StopRepeat(e);
            _btnPlus.PointerCanceled += (_, e) => StopRepeat(e);
            _btnPlus.PointerCaptureLost += (_, e) => StopRepeat(e);
        }

        if (_root != null)
        {
            _root.PointerPressed += OnRootPointerPressed;
            _root.PointerMoved += OnRootPointerMoved;
            _root.PointerReleased += OnRootPointerReleased;
            _root.PointerCanceled += OnRootPointerReleased;
            _root.PointerCaptureLost += OnRootPointerReleased;
            _root.SizeChanged += (_, _) => UpdateVisual();
        }

        _templateApplied = true;
        UpdateVisual();
    }

    private void StartRepeat(int dir, PointerRoutedEventArgs e)
    {
        if (!IsEnabled) return;
        e.Handled = true;
        (e.OriginalSource as FrameworkElement)?.CapturePointer(e.Pointer);
        _repeatDirection = dir;
        StepChange(dir);

        StopRepeatTimers();
        _initialDelayTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();
        _initialDelayTimer.Interval = TimeSpan.FromMilliseconds(400);
        _initialDelayTimer.IsRepeating = false;
        _initialDelayTimer.Tick += (_, _) =>
        {
            _repeatTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();
            _repeatTimer.Interval = TimeSpan.FromMilliseconds(80);
            _repeatTimer.IsRepeating = true;
            _repeatTimer.Tick += (_, _) => StepChange(_repeatDirection);
            _repeatTimer.Start();
        };
        _initialDelayTimer.Start();
    }

    private void StopRepeat(PointerRoutedEventArgs? e = null)
    {
        if (e != null)
        {
            e.Handled = true;
            (e.OriginalSource as FrameworkElement)?.ReleasePointerCapture(e.Pointer);
        }
        StopRepeatTimers();
    }

    private void StopRepeatTimers()
    {
        if (_initialDelayTimer != null)
        {
            _initialDelayTimer.Stop();
            _initialDelayTimer = null;
        }
        if (_repeatTimer != null)
        {
            _repeatTimer.Stop();
            _repeatTimer = null;
        }
    }

    private void StepChange(int direction)
    {
        var step = Step > 0 ? Step : 1;
        var next = Value + direction * step;
        SetValueClamped(next);
    }

    private void SetValueClamped(double raw)
    {
        var clamped = Math.Clamp(raw, Minimum, Maximum);
        var step = Step > 0 ? Step : 1;
        var stepsCount = Math.Round((clamped - Minimum) / step);
        var rounded = Minimum + stepsCount * step;
        rounded = Math.Clamp(rounded, Minimum, Maximum);
        Value = Math.Round(rounded, 4);
    }

    private void OnRootPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (!IsEnabled || _root == null) return;
        _isDragging = true;
        _root.CapturePointer(e.Pointer);
        UpdateValueFromPoint(e.GetCurrentPoint(_root).Position.X);
        e.Handled = true;
    }

    private void OnRootPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_isDragging || _root == null) return;
        UpdateValueFromPoint(e.GetCurrentPoint(_root).Position.X);
        e.Handled = true;
    }

    private void OnRootPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (!_isDragging || _root == null) return;
        _isDragging = false;
        _root.ReleasePointerCapture(e.Pointer);
        e.Handled = true;
    }

    private void UpdateValueFromPoint(double x)
    {
        if (_root == null) return;
        var width = _root.ActualWidth;
        if (width <= 0) return;
        var ratio = Math.Clamp(x / width, 0, 1);
        var range = Maximum - Minimum;
        var rawVal = Minimum + ratio * range;
        SetValueClamped(rawVal);
    }

    private void UpdateVisual()
    {
        if (!_templateApplied || _root == null || _fill == null || _valueText == null) return;

        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);

        _root.Background = RizVisuals.Brush(RizVisuals.Surface(dark));
        _root.BorderBrush = RizVisuals.Brush(accent);

        // 填充进度条（半透明主题色渐变，对齐 Web 端 linear-gradient 20% accent）
        var range = Maximum - Minimum;
        var ratio = range > 0 ? Math.Clamp((Value - Minimum) / range, 0, 1) : 0;
        var width = _root.ActualWidth > 0 ? _root.ActualWidth : 160;
        // 拖动/步进时平滑跟随（80ms 缓动，对应 Web 的 0.08s 过渡）
        RizVisuals.AnimateDouble(_fill, FrameworkElement.WidthProperty, "Width", width * ratio, 80);
        _fill.Background = RizVisuals.Brush(RizVisuals.Mix(accent, RizVisuals.Surface(dark), 0.22));

        // 文案显示
        if (!string.IsNullOrEmpty(DisplayValue))
        {
            _valueText.Text = DisplayValue;
        }
        else
        {
            var strVal = Step < 1 ? Value.ToString("0.0", CultureInfo.InvariantCulture) : Value.ToString("0", CultureInfo.InvariantCulture);
            _valueText.Text = string.IsNullOrEmpty(Display) ? strVal : $"{strVal}{Display}";
        }
        _valueText.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));

        if (_btnMinus is Border bMinus && bMinus.Child is TextBlock tbMinus)
            tbMinus.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
        if (_btnPlus is Border bPlus && bPlus.Child is TextBlock tbPlus)
            tbPlus.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));

        Opacity = IsEnabled ? 1 : 0.45;
    }
}
