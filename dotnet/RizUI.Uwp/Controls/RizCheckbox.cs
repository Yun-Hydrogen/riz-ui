using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.UI;
using System.Windows.Input;

namespace RizUI.Uwp.Controls;

/// <summary>
/// 原生实现的 rizui_checkbox。
/// <see cref="IsChecked"/> 是对应 modelValue 的首选双向绑定属性；<see cref="IsOn"/> 保留旧 API。
/// </summary>
public partial class RizCheckbox : Control
{
    private StackPanel? _root;
    private Border? _box;
    private Microsoft.UI.Xaml.Shapes.Path? _tick;
    private ScaleTransform? _tickScale;
    private TextBlock? _labelL;
    private TextBlock? _labelR;
    private bool _isSynchronizing;
    private bool _templateApplied;

    public RizCheckbox()
    {
        DefaultStyleKey = typeof(RizCheckbox);
        ActualThemeChanged += (_, _) => UpdateVisualState(false);
        RegisterPropertyChangedCallback(IsEnabledProperty, (_, _) => UpdateVisualState(false));
    }

    public static readonly DependencyProperty IsCheckedProperty =
        DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(RizCheckbox),
            new PropertyMetadata(false, OnIsCheckedChanged));

    /// <summary>勾选状态（对应 Vue 的 modelValue）。</summary>
    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public static readonly DependencyProperty IsOnProperty =
        DependencyProperty.Register(nameof(IsOn), typeof(bool), typeof(RizCheckbox),
            new PropertyMetadata(false, OnIsOnChanged));

    /// <summary>勾选状态（对应 v-model / modelValue） IsOn</summary>
    public bool IsOn
    {
        get => (bool)GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizCheckbox),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    /// <summary>自定义颜色，空则跟随主题色 Color</summary>
    public string Color
    {
        get => (string)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DependencyProperty LLabelProperty =
        DependencyProperty.Register(nameof(LLabel), typeof(string), typeof(RizCheckbox),
            new PropertyMetadata(string.Empty, OnLabelChanged));

    /// <summary>左侧标签文字 LLabel</summary>
    public string LLabel
    {
        get => (string)GetValue(LLabelProperty);
        set => SetValue(LLabelProperty, value);
    }

    public static readonly DependencyProperty RLabelProperty =
        DependencyProperty.Register(nameof(RLabel), typeof(string), typeof(RizCheckbox),
            new PropertyMetadata(string.Empty, OnLabelChanged));

    /// <summary>右侧标签文字 RLabel</summary>
    public string RLabel
    {
        get => (string)GetValue(RLabelProperty);
        set => SetValue(RLabelProperty, value);
    }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizCheckbox),
            new PropertyMetadata("auto", OnVisualChanged));

    /// <summary>主题模式：Dark / Light / auto Theme</summary>
    public string Theme
    {
        get => (string)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>左侧标签的 XAML 样式覆盖，对应 lLabelStyle。</summary>
    public static readonly DependencyProperty LLabelStyleProperty =
        DependencyProperty.Register(nameof(LLabelStyle), typeof(Style), typeof(RizCheckbox),
            new PropertyMetadata(null, OnLabelChanged));

    public Style? LLabelStyle
    {
        get => (Style?)GetValue(LLabelStyleProperty);
        set => SetValue(LLabelStyleProperty, value);
    }

    /// <summary>右侧标签的 XAML 样式覆盖，对应 rLabelStyle。</summary>
    public static readonly DependencyProperty RLabelStyleProperty =
        DependencyProperty.Register(nameof(RLabelStyle), typeof(Style), typeof(RizCheckbox),
            new PropertyMetadata(null, OnLabelChanged));

    public Style? RLabelStyle
    {
        get => (Style?)GetValue(RLabelStyleProperty);
        set => SetValue(RLabelStyleProperty, value);
    }

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(RizCheckbox),
            new PropertyMetadata(null));

    /// <summary>切换完成后执行的 MVVM 命令。</summary>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(RizCheckbox),
            new PropertyMetadata(null));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    /// <summary>对应 checked 状态变更的原生事件。</summary>
    public event EventHandler? Checked;
    /// <summary>对应 unchecked 状态变更的原生事件。</summary>
    public event EventHandler? Unchecked;
    /// <summary>每次由用户操作切换后触发。</summary>
    public event EventHandler? Toggled;

    private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((RizCheckbox)d).SynchronizeCheckedState((bool)e.NewValue, isCheckedChange: true);

    private static void OnIsOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((RizCheckbox)d).SynchronizeCheckedState((bool)e.NewValue, isCheckedChange: false);

    private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((RizCheckbox)d).ApplyLabels();

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((RizCheckbox)d).UpdateVisualState();

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_root != null)
        {
            _root.PointerPressed -= OnPointerPressed;
            _root.PointerReleased -= OnPointerReleased;
        }

        _root = GetTemplateChild("Root") as StackPanel;
        _box = GetTemplateChild("Box") as Border;
        _tick = GetTemplateChild("Tick") as Microsoft.UI.Xaml.Shapes.Path;
        _labelL = GetTemplateChild("LabelL") as TextBlock;
        _labelR = GetTemplateChild("LabelR") as TextBlock;

        if (_root != null)
        {
            _root.PointerPressed += OnPointerPressed;
            _root.PointerReleased += OnPointerReleased;
        }
        _tickScale = _tick?.RenderTransform as ScaleTransform;

        ApplyLabels();
        UpdateVisualState(false);
        _templateApplied = true;
    }

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (!IsEnabled) return;
        _root?.CapturePointer(e.Pointer);
        e.Handled = true;
    }

    private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (!IsEnabled) return;
        _root?.ReleasePointerCapture(e.Pointer);
        IsChecked = !IsChecked;

        var command = Command;
        if (command?.CanExecute(CommandParameter) == true)
            command.Execute(CommandParameter);
        Toggled?.Invoke(this, EventArgs.Empty);
        e.Handled = true;
    }

    private void ApplyLabels()
    {
        if (_labelL != null)
        {
            _labelL.Text = LLabel ?? string.Empty;
            _labelL.Visibility = string.IsNullOrEmpty(LLabel) ? Visibility.Collapsed : Visibility.Visible;
            _labelL.Style = LLabelStyle;
        }
        if (_labelR != null)
        {
            _labelR.Text = RLabel ?? string.Empty;
            _labelR.Visibility = string.IsNullOrEmpty(RLabel) ? Visibility.Collapsed : Visibility.Visible;
            _labelR.Style = RLabelStyle;
        }
    }

    private void SynchronizeCheckedState(bool value, bool isCheckedChange)
    {
        if (!_isSynchronizing)
        {
            _isSynchronizing = true;
            SetValue(isCheckedChange ? IsOnProperty : IsCheckedProperty, value);
            _isSynchronizing = false;
        }

        UpdateVisualState();
        if (isCheckedChange)
        {
            if (value) Checked?.Invoke(this, EventArgs.Empty);
            else Unchecked?.Invoke(this, EventArgs.Empty);
        }
    }

    private void UpdateVisualState(bool animate = true)
    {
        if (_box == null) return;

        var color = GetColor();
        if (IsChecked)
        {
            SetBrushColor(_box.Background, value => _box.Background = value, color);
            SetBrushColor(_box.BorderBrush, value => _box.BorderBrush = value, color);
            // 同色边框在 Skia 的两层抗锯齿绘制中会显出浅色内缝；选中时与 CSS 的视觉
            // 效果相同，直接绘制一层填充，并额外获得完整的 18px 勾线可用区域。
            _box.BorderThickness = new Thickness(0);
            AnimateTick(1, 1, animate);
        }
        else
        {
            SetBrushColor(_box.Background, value => _box.Background = value, GetSurfaceColor());
            SetBrushColor(_box.BorderBrush, value => _box.BorderBrush = value, GetBorderColor());
            _box.BorderThickness = new Thickness(1);
            AnimateTick(0, 0.4, animate);
        }

        if (_root != null) _root.Opacity = IsEnabled ? 1 : 0.45;

        var textBrush = RizVisuals.Brush(IsDarkTheme() ? Windows.UI.Color.FromArgb(255, 0xE6, 0xEB, 0xF2) : Windows.UI.Color.FromArgb(255, 0x33, 0x33, 0x44));
        if (_labelL != null && LLabelStyle == null) _labelL.Foreground = textBrush;
        if (_labelR != null && RLabelStyle == null) _labelR.Foreground = textBrush;
    }

    private void AnimateTick(double opacity, double scale, bool animate)
    {
        if (_tick == null || _tickScale == null) return;
        if (!animate)
        {
            _tick.Opacity = opacity;
            _tickScale.ScaleX = scale;
            _tickScale.ScaleY = scale;
            return;
        }

        var storyboard = new Storyboard();
        var duration = new Duration(TimeSpan.FromMilliseconds(150));
        var opacityAnimation = new DoubleAnimation
        {
            To = opacity,
            Duration = duration,
            EnableDependentAnimation = true
        };
        Storyboard.SetTarget(opacityAnimation, _tick);
        Storyboard.SetTargetProperty(opacityAnimation, "Opacity");
        storyboard.Children.Add(opacityAnimation);

        foreach (var property in new[] { "ScaleX", "ScaleY" })
        {
            var scaleAnimation = new DoubleAnimation
            {
                To = scale,
                Duration = duration,
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(scaleAnimation, _tickScale);
            Storyboard.SetTargetProperty(scaleAnimation, property);
            storyboard.Children.Add(scaleAnimation);
        }
        storyboard.Begin();
    }

    private void SetBrushColor(Brush? current, Action<Brush> assign, Color color)
    {
        if (!_templateApplied || current is not SolidColorBrush brush)
        {
            assign(new SolidColorBrush(color));
            return;
        }

        var animation = new ColorAnimation
        {
            To = color,
            Duration = new Duration(TimeSpan.FromMilliseconds(150)),
            EnableDependentAnimation = true
        };
        Storyboard.SetTarget(animation, brush);
        Storyboard.SetTargetProperty(animation, "Color");
        var storyboard = new Storyboard();
        storyboard.Children.Add(animation);
        storyboard.Begin();
    }

    private Windows.UI.Color GetColor() => ColorUtil.FromHex(Color, Windows.UI.Color.FromArgb(255, 0x66, 0xCC, 0xFF));
    private Windows.UI.Color GetSurfaceColor() => IsDarkTheme() ? Windows.UI.Color.FromArgb(255, 0x1E, 0x26, 0x30) : Microsoft.UI.Colors.White;
    private Windows.UI.Color GetBorderColor() => IsDarkTheme() ? Windows.UI.Color.FromArgb(255, 0x32, 0x3D, 0x4B) : Windows.UI.Color.FromArgb(255, 0xE8, 0xEC, 0xF2);

    private bool IsDarkTheme()
        => string.Equals(Theme, "Dark", StringComparison.OrdinalIgnoreCase)
           || (!string.Equals(Theme, "Light", StringComparison.OrdinalIgnoreCase)
               && ActualTheme == ElementTheme.Dark);
}
