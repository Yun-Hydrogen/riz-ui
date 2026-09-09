using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;

namespace RizUI.Uwp.Controls;

/// <summary>
/// 原生实现的 rizui_switch。
/// 44x24 胶囊跑道 + 20x20 白色圆形滚轮滑块，平滑弹性滑动动画与主题色填充。
/// </summary>
public sealed class RizSwitch : Control
{
    private Border? _track;
    private Border? _thumb;
    private TranslateTransform? _thumbTranslate;
    private bool _isPressed;
    private bool _templateApplied;

    public RizSwitch()
    {
        DefaultStyleKey = typeof(RizSwitch);
        IsTabStop = true;
        RegisterPropertyChangedCallback(IsEnabledProperty, (_, _) => UpdateVisual(false));
        ActualThemeChanged += (_, _) => UpdateVisual(false);
    }

    public static readonly DependencyProperty IsOnProperty =
        DependencyProperty.Register(nameof(IsOn), typeof(bool), typeof(RizSwitch),
            new PropertyMetadata(false, OnIsOnChanged));

    /// <summary>开关状态（对应 Vue 的 modelValue / v-model）。</summary>
    public bool IsOn
    {
        get => (bool)GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizSwitch),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    /// <summary>自定义开启时的主题色，空则跟随 --rizui_themecolor。</summary>
    public string Color
    {
        get => (string)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizSwitch),
            new PropertyMetadata("auto", OnVisualChanged));

    /// <summary>主题模式：Dark / Light / auto。</summary>
    public string Theme
    {
        get => (string)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public event EventHandler<bool>? Toggled;

    private static void OnIsOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var sw = (RizSwitch)d;
        sw.UpdateVisual(true);
        sw.Toggled?.Invoke(sw, (bool)e.NewValue);
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((RizSwitch)d).UpdateVisual(false);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _track = GetTemplateChild("Track") as Border;
        _thumb = GetTemplateChild("Thumb") as Border;
        if (_thumb != null)
        {
            _thumbTranslate = new TranslateTransform();
            _thumb.RenderTransform = _thumbTranslate;
        }

        PointerPressed += OnPointerPressed;
        PointerReleased += OnPointerReleased;
        PointerCanceled += OnPointerCanceled;
        PointerCaptureLost += OnPointerCanceled;

        _templateApplied = true;
        UpdateVisual(false);
    }

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (!IsEnabled) return;
        _isPressed = true;
        CapturePointer(e.Pointer);
    }

    private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (!IsEnabled || !_isPressed) return;
        _isPressed = false;
        ReleasePointerCapture(e.Pointer);
        IsOn = !IsOn;
    }

    private void OnPointerCanceled(object sender, PointerRoutedEventArgs e)
    {
        _isPressed = false;
        ReleasePointerCapture(e.Pointer);
    }

    private void UpdateVisual(bool animate)
    {
        if (!_templateApplied || _track == null || _thumb == null || _thumbTranslate == null) return;

        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);

        // 浅色未选中：color-mix(30% text, bg) 即灰调；选中：accent 主题色
        var inactiveBg = RizVisuals.Mix(RizVisuals.Text(dark), RizVisuals.Surface(dark), 0.30);
        var activeBg = accent;
        var targetBg = IsOn ? activeBg : inactiveBg;

        _track.Background = RizVisuals.Brush(targetBg);
        Opacity = IsEnabled ? 1 : 0.45;

        // 滑块位移：从 0 滑动到 20px
        var targetX = IsOn ? 20d : 0d;
        if (animate)
        {
            var anim = new DoubleAnimation
            {
                To = targetX,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTarget(anim, _thumbTranslate);
            Storyboard.SetTargetProperty(anim, "X");
            var sb = new Storyboard();
            sb.Children.Add(anim);
            sb.Begin();
        }
        else
        {
            _thumbTranslate.X = targetX;
        }
    }
}
