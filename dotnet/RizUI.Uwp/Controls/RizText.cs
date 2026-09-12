using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;

namespace RizUI.Uwp.Controls;

/// <summary>
/// 原生实现的 rizui_text。
/// 支持单行输入与 plaintext 多行模式，带从中轴向两侧平滑展开的主题色聚焦光晕边框动画。
/// </summary>
public sealed class RizText : Control
{
    private Border? _wrap;
    private Border? _focusGlow;
    private TextBox? _singleInput;
    private TextBox? _multiInput;
    private ScaleTransform? _focusScale;
    private bool _templateApplied;
    private ContentControl? _blurSink;
    private bool _inputFocused;
    private bool _outsideHooked;
    private PointerEventHandler? _outsidePressedHandler;

    public RizText()
    {
        DefaultStyleKey = typeof(RizText);
        IsTabStop = false;
        ActualThemeChanged += (_, _) => UpdateVisual();
        RegisterPropertyChangedCallback(IsEnabledProperty, (_, _) => UpdateVisual());
        RegisterPropertyChangedCallback(HeightProperty, (_, _) => UpdateVisual());
        RegisterPropertyChangedCallback(MinHeightProperty, (_, _) => UpdateVisual());
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(RizText),
            new PropertyMetadata(string.Empty, OnTextChanged));

    /// <summary>输入文本内容（对应 Vue 的 modelValue）。</summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(RizText),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    /// <summary>占位提示文字。</summary>
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly DependencyProperty TypeProperty =
        DependencyProperty.Register(nameof(Type), typeof(string), typeof(RizText),
            new PropertyMetadata("text", OnVisualChanged));

    /// <summary>输入类型：text / number。</summary>
    public string Type
    {
        get => (string)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public static readonly DependencyProperty PlaintextProperty =
        DependencyProperty.Register(nameof(Plaintext), typeof(bool), typeof(RizText),
            new PropertyMetadata(false, OnModeChanged));

    /// <summary>多行纯文本模式（textarea）。</summary>
    public bool Plaintext
    {
        get => (bool)GetValue(PlaintextProperty);
        set => SetValue(PlaintextProperty, value);
    }

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(RizText),
            new PropertyMetadata(false, OnVisualChanged));

    /// <summary>只读模式。</summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizText),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    /// <summary>自定义聚焦边框主题色，空则跟随 --rizui_themecolor。</summary>
    public string Color
    {
        get => (string)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizText),
            new PropertyMetadata("auto", OnVisualChanged));

    public string Theme
    {
        get => (string)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public static readonly DependencyProperty HeightTextProperty =
        DependencyProperty.Register(nameof(HeightText), typeof(string), typeof(RizText),
            new PropertyMetadata("auto", OnVisualChanged));

    /// <summary>高度设置，如 "120px"、"160px"、"auto"。</summary>
    public string HeightText
    {
        get => (string)GetValue(HeightTextProperty);
        set => SetValue(HeightTextProperty, value);
    }

    public event EventHandler<string>? TextChanged;
    public event EventHandler<string>? Enter;

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var input = (RizText)d;
        input.SyncTextToInputs();
        input.TextChanged?.Invoke(input, (string)e.NewValue);
    }

    private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var input = (RizText)d;
        input.UpdateMode();
        input.UpdateVisual();
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizText)d).UpdateVisual();

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _wrap = GetTemplateChild("Wrap") as Border;
        _focusGlow = GetTemplateChild("FocusGlow") as Border;
        _singleInput = GetTemplateChild("SingleInput") as TextBox;
        _multiInput = GetTemplateChild("MultiInput") as TextBox;
        _blurSink = GetTemplateChild("BlurSink") as ContentControl;

        if (_focusGlow != null)
        {
            _focusScale = new ScaleTransform { ScaleX = 0, ScaleY = 1, CenterX = 0.5, CenterY = 0.5 };
            _focusGlow.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
            _focusGlow.RenderTransform = _focusScale;
            _focusGlow.Opacity = 0;
        }

        HookEvents(_singleInput);
        HookEvents(_multiInput);

        _templateApplied = true;
        UpdateMode();
        SyncTextToInputs();
        UpdateVisual();

        Loaded += (_, _) => HookOutsideClick();
        Unloaded += (_, _) => UnhookOutsideClick();
    }

    private void HookEvents(TextBox? tb)
    {
        if (tb == null) return;
        tb.GotFocus += (_, _) => { _inputFocused = true; AnimateFocusGlow(true); };
        tb.LostFocus += (_, _) => { _inputFocused = false; AnimateFocusGlow(false); };
        tb.TextChanged += (_, _) =>
        {
            if (!string.Equals(Text, tb.Text, StringComparison.Ordinal))
            {
                Text = tb.Text;
            }
        };
        tb.KeyDown += (_, e) =>
        {
            if (e.Key == Windows.System.VirtualKey.Enter && !Plaintext)
            {
                Enter?.Invoke(this, Text);
            }
        };
    }

    private void AnimateFocusGlow(bool isFocused)
    {
        if (_focusScale == null || _focusGlow == null) return;
        var duration = new Duration(TimeSpan.FromMilliseconds(350));
        var cubic = new CubicEase { EasingMode = isFocused ? EasingMode.EaseOut : EasingMode.EaseIn };

        var sb = new Storyboard();
        var scaleX = new DoubleAnimation
        {
            To = isFocused ? 1.0 : 0.0,
            Duration = duration,
            EnableDependentAnimation = true,
            EasingFunction = cubic
        };
        Storyboard.SetTarget(scaleX, _focusScale);
        Storyboard.SetTargetProperty(scaleX, "ScaleX");

        var fade = new DoubleAnimation
        {
            To = isFocused ? 1.0 : 0.0,
            Duration = new Duration(TimeSpan.FromMilliseconds(isFocused ? 200 : 180)),
            EnableDependentAnimation = true,
            EasingFunction = cubic
        };
        Storyboard.SetTarget(fade, _focusGlow);
        Storyboard.SetTargetProperty(fade, "Opacity");

        sb.Children.Add(scaleX);
        sb.Children.Add(fade);
        sb.Begin();
    }

    private void HookOutsideClick()
    {
        if (_outsideHooked) return;
        if (XamlRoot?.Content is not UIElement root) return;
        _outsidePressedHandler ??= OnOutsidePointerPressed;
        root.AddHandler(UIElement.PointerPressedEvent, _outsidePressedHandler, true);
        _outsideHooked = true;
    }

    private void UnhookOutsideClick()
    {
        if (!_outsideHooked) return;
        if (XamlRoot?.Content is UIElement root && _outsidePressedHandler != null)
        {
            root.RemoveHandler(UIElement.PointerPressedEvent, _outsidePressedHandler);
        }
        _outsideHooked = false;
    }

    /// <summary>焦点在输入框内时，点击控件区域以外的位置自动失焦（对齐 Web 的 blur 行为）。</summary>
    private void OnOutsidePointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (!_inputFocused || _blurSink == null) return;
        var point = e.GetCurrentPoint(this).Position;
        if (point.X >= 0 && point.Y >= 0 && point.X <= ActualWidth && point.Y <= ActualHeight) return;
        _blurSink.Focus(FocusState.Programmatic);
    }

    private void SyncTextToInputs()
    {
        var val = Text ?? string.Empty;
        if (_singleInput != null && !string.Equals(_singleInput.Text, val, StringComparison.Ordinal))
            _singleInput.Text = val;
        if (_multiInput != null && !string.Equals(_multiInput.Text, val, StringComparison.Ordinal))
            _multiInput.Text = val;
    }

    private void UpdateMode()
    {
        if (_singleInput != null) _singleInput.Visibility = Plaintext ? Visibility.Collapsed : Visibility.Visible;
        if (_multiInput != null) _multiInput.Visibility = Plaintext ? Visibility.Visible : Visibility.Collapsed;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (!_templateApplied) return;
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);
        var surface = RizVisuals.Surface(dark);
        var border = RizVisuals.Border(dark);

        var hasHeightText = !string.IsNullOrWhiteSpace(HeightText) && !string.Equals(HeightText, "auto", StringComparison.OrdinalIgnoreCase);
        double? explicitHeight = null;
        if (hasHeightText)
        {
            explicitHeight = RizVisuals.ParsePixels(HeightText, Plaintext ? 88 : 28);
        }
        else if (!double.IsNaN(Height) && Height > 0)
        {
            explicitHeight = Height;
        }

        if (Plaintext)
        {
            VerticalAlignment = VerticalAlignment.Stretch;
            var corner = new CornerRadius(10);
            if (_wrap != null) _wrap.CornerRadius = corner;
            if (_focusGlow != null) _focusGlow.CornerRadius = corner;
            if (_multiInput != null)
            {
                _multiInput.CornerRadius = corner;
                _multiInput.VerticalContentAlignment = VerticalAlignment.Top;
                _multiInput.TextAlignment = TextAlignment.Left;
                _multiInput.Padding = new Thickness(10, 8, 10, 8);
            }

            if (explicitHeight.HasValue)
            {
                var h = explicitHeight.Value;
                Height = h;
                if (_wrap != null) { _wrap.Height = h; _wrap.MinHeight = h; }
                if (_focusGlow != null) { _focusGlow.Height = h; _focusGlow.MinHeight = h; }
                if (_multiInput != null) { _multiInput.Height = h; _multiInput.MinHeight = h; }
            }
            else
            {
                ClearValue(HeightProperty);
                MinHeight = 84;
                if (_wrap != null) { _wrap.ClearValue(HeightProperty); _wrap.MinHeight = 84; }
                if (_focusGlow != null) { _focusGlow.ClearValue(HeightProperty); _focusGlow.MinHeight = 84; }
                if (_multiInput != null) { _multiInput.ClearValue(HeightProperty); _multiInput.MinHeight = 84; }
            }
        }
        else
        {
            VerticalAlignment = VerticalAlignment.Center;
            var h = explicitHeight ?? 28;
            Height = h;
            var corner = new CornerRadius(h / 2);
            if (_wrap != null)
            {
                _wrap.Height = h;
                _wrap.MinHeight = h;
                _wrap.CornerRadius = corner;
            }
            if (_focusGlow != null)
            {
                _focusGlow.Height = h;
                _focusGlow.MinHeight = h;
                _focusGlow.CornerRadius = corner;
            }
            if (_singleInput != null)
            {
                _singleInput.Height = h;
                _singleInput.MinHeight = h;
                _singleInput.CornerRadius = corner;
                _singleInput.VerticalContentAlignment = VerticalAlignment.Center;
                _singleInput.TextAlignment = TextAlignment.Center;
                _singleInput.Padding = new Thickness(10, 0, 10, 0);
            }
        }

        if (_wrap != null)
        {
            _wrap.Background = RizVisuals.Brush(surface);
            _wrap.BorderBrush = RizVisuals.Brush(border);
        }

        if (_focusGlow != null)
        {
            _focusGlow.BorderBrush = RizVisuals.Brush(accent);
        }

        var activeTb = Plaintext ? _multiInput : _singleInput;
        if (activeTb != null)
        {
            activeTb.PlaceholderText = Placeholder ?? string.Empty;
            activeTb.IsReadOnly = IsReadOnly;
            activeTb.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        }

        Opacity = IsEnabled ? 1.0 : 0.45;
    }
}
