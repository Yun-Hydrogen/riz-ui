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
/// 原生实现的 rizui_button。
/// 属性名保留 Vue 组件的语义；<see cref="Command"/> 为原生 MVVM 补充入口。
/// </summary>
public partial class RizButton : Control
{
    private Border? _root;
    private TextBlock? _lIcon;
    private TextBlock? _label;
    private TextBlock? _rIcon;
    private bool _hover;
    private bool _pressed;
    private bool _templateApplied;

    public RizButton()
    {
        DefaultStyleKey = typeof(RizButton);
        ActualThemeChanged += (_, _) => ApplyVisual(false);
        RegisterPropertyChangedCallback(IsEnabledProperty, (_, _) =>
        {
            _pressed = false;
            ApplyVisual(false);
        });
    }

    /// <summary>原生点击契约（对应 @click）</summary>
    public event EventHandler? Click;

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(RizButton),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    /// <summary>按钮文字 Text</summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty PrimaryProperty =
        DependencyProperty.Register(nameof(Primary), typeof(bool), typeof(RizButton),
            new PropertyMetadata(false, OnVisualChanged));

    /// <summary>主题色填充主按钮 Primary</summary>
    public bool Primary
    {
        get => (bool)GetValue(PrimaryProperty);
        set => SetValue(PrimaryProperty, value);
    }

    public static readonly DependencyProperty DangerProperty =
        DependencyProperty.Register(nameof(Danger), typeof(bool), typeof(RizButton),
            new PropertyMetadata(false, OnVisualChanged));

    /// <summary>危险红色按钮 Danger</summary>
    public bool Danger
    {
        get => (bool)GetValue(DangerProperty);
        set => SetValue(DangerProperty, value);
    }

    public static readonly DependencyProperty LongProperty =
        DependencyProperty.Register(nameof(Long), typeof(bool), typeof(RizButton),
            new PropertyMetadata(false, OnVisualChanged));

    /// <summary>通栏（占满宽度） Long</summary>
    public bool Long
    {
        get => (bool)GetValue(LongProperty);
        set => SetValue(LongProperty, value);
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizButton),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    /// <summary>自定义颜色，空则跟随主题色 Color</summary>
    public string Color
    {
        get => (string)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DependencyProperty LIconProperty =
        DependencyProperty.Register(nameof(LIcon), typeof(string), typeof(RizButton),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    /// <summary>左侧图标（文本/符号） l-icon</summary>
    public string LIcon
    {
        get => (string)GetValue(LIconProperty);
        set => SetValue(LIconProperty, value);
    }

    public static readonly DependencyProperty RIconProperty =
        DependencyProperty.Register(nameof(RIcon), typeof(string), typeof(RizButton),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    /// <summary>右侧图标（文本/符号） r-icon</summary>
    public string RIcon
    {
        get => (string)GetValue(RIconProperty);
        set => SetValue(RIconProperty, value);
    }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizButton),
            new PropertyMetadata("auto", OnVisualChanged));

    /// <summary>主题模式：Dark / Light / auto Theme</summary>
    public string Theme
    {
        get => (string)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>与 Vue 的 type prop 对应；原生控件中保留为元数据。</summary>
    public static readonly DependencyProperty TypeProperty =
        DependencyProperty.Register(nameof(Type), typeof(string), typeof(RizButton),
            new PropertyMetadata("button"));

    public string Type
    {
        get => (string)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    /// <summary>文字的 XAML 样式覆盖，对应 textStyle。</summary>
    public static readonly DependencyProperty TextStyleProperty =
        DependencyProperty.Register(nameof(TextStyle), typeof(Style), typeof(RizButton),
            new PropertyMetadata(null, OnVisualChanged));

    public Style? TextStyle
    {
        get => (Style?)GetValue(TextStyleProperty);
        set => SetValue(TextStyleProperty, value);
    }

    /// <summary>左图标的 XAML 样式覆盖，对应 lIconStyle。</summary>
    public static readonly DependencyProperty LIconStyleProperty =
        DependencyProperty.Register(nameof(LIconStyle), typeof(Style), typeof(RizButton),
            new PropertyMetadata(null, OnVisualChanged));

    public Style? LIconStyle
    {
        get => (Style?)GetValue(LIconStyleProperty);
        set => SetValue(LIconStyleProperty, value);
    }

    /// <summary>右图标的 XAML 样式覆盖，对应 rIconStyle。</summary>
    public static readonly DependencyProperty RIconStyleProperty =
        DependencyProperty.Register(nameof(RIconStyle), typeof(Style), typeof(RizButton),
            new PropertyMetadata(null, OnVisualChanged));

    public Style? RIconStyle
    {
        get => (Style?)GetValue(RIconStyleProperty);
        set => SetValue(RIconStyleProperty, value);
    }

    /// <summary>MVVM 点击命令；与 <see cref="Click"/> 同时触发。</summary>
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(RizButton),
            new PropertyMetadata(null));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary><see cref="Command"/> 的参数。</summary>
    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(RizButton),
            new PropertyMetadata(null));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((RizButton)d).ApplyVisual();

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_root != null)
        {
            _root.PointerEntered -= OnPointerEntered;
            _root.PointerExited -= OnPointerExited;
            _root.PointerPressed -= OnPointerPressed;
            _root.PointerReleased -= OnPointerReleased;
        }

        _root = GetTemplateChild("Root") as Border;
        _lIcon = GetTemplateChild("LIcon") as TextBlock;
        _label = GetTemplateChild("Label") as TextBlock;
        _rIcon = GetTemplateChild("RIcon") as TextBlock;

        if (_root != null)
        {
            _root.PointerEntered += OnPointerEntered;
            _root.PointerExited += OnPointerExited;
            _root.PointerPressed += OnPointerPressed;
            _root.PointerReleased += OnPointerReleased;
        }

        _templateApplied = true;
        ApplyVisual(false);
    }

    private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        _hover = true;
        ApplyVisual();
    }

    private void OnPointerExited(object sender, PointerRoutedEventArgs e)
    {
        _hover = false;
        _pressed = false;
        ApplyVisual();
    }

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (!IsEnabled) return;
        _pressed = true;
        ApplyVisual();
        _root?.CapturePointer(e.Pointer);
        e.Handled = true;
    }

    private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (!IsEnabled || !_pressed) return;

        _pressed = false;
        _root?.ReleasePointerCapture(e.Pointer);
        ApplyVisual();

        var command = Command;
        if (command?.CanExecute(CommandParameter) == true)
            command.Execute(CommandParameter);
        Click?.Invoke(this, EventArgs.Empty);
        e.Handled = true;
    }

    private void ApplyVisual(bool animate = true)
    {
        if (_root == null) return;

        var c = Danger
            ? Windows.UI.Color.FromArgb(255, 0xE0, 0x55, 0x55)
            : GetColor();

        var filled = Primary || Danger;
        var bg = filled ? c : GetSurfaceColor();
        var fg = filled ? Microsoft.UI.Colors.White : GetTextColor();
        var bd = filled ? c : GetBorderColor();

        // Vue: :hover 变主题色；filled 使用 brightness(1.08)。
        if (_hover)
        {
            if (filled)
            {
                fg = Microsoft.UI.Colors.White;
                bg = MultiplyBrightness(c, 1.08);
                if (Danger) bd = Windows.UI.Color.FromArgb(255, 0xFF, 0x00, 0x00);
            }
            else
            {
                bd = c;
                fg = c;
            }
        }

        // Vue: :active { filter: brightness(.92) }。
        if (_pressed)
        {
            bg = MultiplyBrightness(bg, 0.92);
            bd = MultiplyBrightness(bd, 0.92);
            fg = MultiplyBrightness(fg, 0.92);
        }

        SetBrushColor(_root.Background, value => _root.Background = value, bg, animate);
        SetBrushColor(_root.BorderBrush, value => _root.BorderBrush = value, bd, animate);
        // 填充态的 border 与 background 在 CSS 中同色而不可见。Uno/Skia 若分别绘制
        // 两层会在抗锯齿边缘产生浅色细缝，因此直接使用单层填充。
        _root.BorderThickness = filled ? new Thickness(0) : new Thickness(1);
        // CSS 的 999px 圆角会被浏览器钳制到元素半高；Uno/Skia 不会，
        // 因此对 30px 高的按钮显式使用 15px，避免 long 按钮变成椭圆。
        _root.CornerRadius = new CornerRadius(15);
        _root.MinHeight = 30;
        _root.Opacity = IsEnabled ? 1 : 0.5;

        var hasText = !string.IsNullOrWhiteSpace(Text);
        var hasLIcon = !string.IsNullOrWhiteSpace(LIcon);
        var hasRIcon = !string.IsNullOrWhiteSpace(RIcon);
        var isIconOnly = !hasText && (hasLIcon ^ hasRIcon);

        if (_root != null)
        {
            // 纯图标按钮采用正方形 30x30 紧凑布局；有文本时采用水平 padding 16px
            if (isIconOnly)
            {
                _root.Width = 30;
                _root.Height = 30;
                _root.MinHeight = 30;
                _root.Padding = new Thickness(0);
            }
            else
            {
                _root.ClearValue(WidthProperty);
                _root.ClearValue(HeightProperty);
                _root.MinHeight = 30;
                _root.Padding = new Thickness(16, 6, 16, 6);
            }
        }

        if (_label != null)
        {
            _label.Text = Text ?? string.Empty;
            _label.Visibility = hasText ? Visibility.Visible : Visibility.Collapsed;
            _label.Style = TextStyle;
            SetBrushColor(_label.Foreground, value => _label.Foreground = value, fg, animate);
        }
        if (_lIcon != null)
        {
            _lIcon.Visibility = hasLIcon ? Visibility.Visible : Visibility.Collapsed;
            if (hasLIcon)
            {
                RizVisuals.ApplyIcon(_lIcon, LIcon, 13, fg);
                if (LIconStyle != null) _lIcon.Style = LIconStyle;
            }
            SetBrushColor(_lIcon.Foreground, value => _lIcon.Foreground = value, fg, animate);
        }
        if (_rIcon != null)
        {
            _rIcon.Visibility = hasRIcon ? Visibility.Visible : Visibility.Collapsed;
            if (hasRIcon)
            {
                RizVisuals.ApplyIcon(_rIcon, RIcon, 13, fg);
                if (RIconStyle != null) _rIcon.Style = RIconStyle;
            }
            SetBrushColor(_rIcon.Foreground, value => _rIcon.Foreground = value, fg, animate);
        }

        HorizontalAlignment = Long ? HorizontalAlignment.Stretch : HorizontalAlignment.Left;
    }

    private void SetBrushColor(Brush? current, Action<Brush> assign, Windows.UI.Color color, bool animate)
    {
        if (!_templateApplied || !animate || current is not SolidColorBrush brush)
        {
            assign(new SolidColorBrush(color));
            return;
        }

        var animation = new ColorAnimation
        {
            To = color,
            Duration = new Duration(TimeSpan.FromMilliseconds(200)),
            EnableDependentAnimation = true
        };
        Storyboard.SetTarget(animation, brush);
        Storyboard.SetTargetProperty(animation, "Color");
        var storyboard = new Storyboard();
        storyboard.Children.Add(animation);
        storyboard.Begin();
    }

    private static Windows.UI.Color MultiplyBrightness(Windows.UI.Color color, double factor)
        => Windows.UI.Color.FromArgb(color.A,
            (byte)Math.Min(255, color.R * factor),
            (byte)Math.Min(255, color.G * factor),
            (byte)Math.Min(255, color.B * factor));

    private Windows.UI.Color GetColor()
    {
        return ColorUtil.FromHex(Color, Windows.UI.Color.FromArgb(255, 0x66, 0xCC, 0xFF));
    }

    private Windows.UI.Color GetSurfaceColor() => IsDarkTheme() ? Windows.UI.Color.FromArgb(255, 0x1E, 0x26, 0x30) : Microsoft.UI.Colors.White;
    private Windows.UI.Color GetBorderColor() => IsDarkTheme() ? Windows.UI.Color.FromArgb(255, 0x32, 0x3D, 0x4B) : Windows.UI.Color.FromArgb(255, 0xE8, 0xEC, 0xF2);
    private Windows.UI.Color GetTextColor() => IsDarkTheme() ? Windows.UI.Color.FromArgb(255, 0xE6, 0xEB, 0xF2) : Windows.UI.Color.FromArgb(255, 0x33, 0x33, 0x44);

    private bool IsDarkTheme()
        => string.Equals(Theme, "Dark", StringComparison.OrdinalIgnoreCase)
           || (!string.Equals(Theme, "Light", StringComparison.OrdinalIgnoreCase)
               && ActualTheme == ElementTheme.Dark);
}

