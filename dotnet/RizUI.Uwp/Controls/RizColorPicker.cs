using System;
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
/// 原生实现的 rizui_colorpicker。
/// 包含彩色圆点胶囊预览、模态拾色弹窗、色相 Hue 滑块、SV 调色板、Hex 输入与即时确定。
/// </summary>
public sealed class RizColorPicker : Control
{
    private Border? _swatch;
    private Border? _swatchDot;
    private TextBlock? _swatchHex;

    private Popup? _popup;
    private Grid? _overlay;
    private Border? _dialogBox;
    private Canvas? _svCanvas;
    private Border? _svThumb;
    private Canvas? _hueCanvas;
    private Border? _hueThumb;
    private Border? _previewDot;
    private TextBox? _hexInput;
    private Border? _btnConfirm;
    private Border? _btnClose;

    private double _hue; // 0 ~ 360
    private double _sat = 1.0; // 0 ~ 1
    private double _val = 1.0; // 0 ~ 1
    private bool _isDraggingHue;
    private bool _isDraggingSV;
    private bool _templateApplied;

    public RizColorPicker()
    {
        DefaultStyleKey = typeof(RizColorPicker);
        IsTabStop = true;
        ActualThemeChanged += (_, _) => UpdateVisual();
        RegisterPropertyChangedCallback(IsEnabledProperty, (_, _) => UpdateVisual());
    }

    public static readonly DependencyProperty ColorValueProperty =
        DependencyProperty.Register(nameof(ColorValue), typeof(string), typeof(RizColorPicker),
            new PropertyMetadata("#66CCFF", OnColorValueChanged));

    public string ColorValue
    {
        get => (string)GetValue(ColorValueProperty);
        set => SetValue(ColorValueProperty, value);
    }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizColorPicker),
            new PropertyMetadata("auto", OnVisualChanged));

    public string Theme
    {
        get => (string)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public event EventHandler<string>? ColorChanged;

    private static void OnColorValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var cp = (RizColorPicker)d;
        cp.UpdateSwatch();
        cp.ColorChanged?.Invoke(cp, (string)e.NewValue);
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizColorPicker)d).UpdateVisual();

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _swatch = GetTemplateChild("Swatch") as Border;
        _swatchDot = GetTemplateChild("SwatchDot") as Border;
        _swatchHex = GetTemplateChild("SwatchHex") as TextBlock;

        if (_swatch != null)
        {
            _swatch.PointerPressed += (_, e) =>
            {
                if (!IsEnabled) return;
                e.Handled = true;
                Open();
            };
        }

        _templateApplied = true;
        ParseHexToHsv(ColorValue);
        UpdateVisual();
    }

    private void Open()
    {
        if (XamlRoot is null) return;
        BuildPopup();
        ParseHexToHsv(ColorValue);
        UpdatePreview();
        UpdateSvThumbPosition();
        UpdateHueThumbPosition();
        _popup!.IsOpen = true;

        if (_dialogBox != null)
        {
            _dialogBox.Opacity = 0;
            var transform = new TranslateTransform { Y = 20 };
            _dialogBox.RenderTransform = transform;

            var sb = new Storyboard();
            var fade = new DoubleAnimation { To = 1, Duration = new Duration(TimeSpan.FromMilliseconds(220)) };
            Storyboard.SetTarget(fade, _dialogBox);
            Storyboard.SetTargetProperty(fade, "Opacity");

            var move = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(220)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTarget(move, transform);
            Storyboard.SetTargetProperty(move, "Y");

            sb.Children.Add(fade);
            sb.Children.Add(move);
            sb.Begin();
        }
    }

    private void BuildPopup()
    {
        if (_popup != null) return;

        var contentRoot = XamlRoot?.Content as FrameworkElement;
        var viewportWidth = Math.Max(1, contentRoot?.ActualWidth ?? 800);
        var viewportHeight = Math.Max(1, contentRoot?.ActualHeight ?? 600);
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(ColorValue);

        _overlay = new Grid
        {
            Width = viewportWidth,
            Height = viewportHeight,
            Background = RizVisuals.Brush(dark ? Windows.UI.Color.FromArgb(140, 0, 0, 0) : Windows.UI.Color.FromArgb(90, 15, 23, 42)),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };
        _overlay.PointerPressed += (_, e) =>
        {
            if (ReferenceEquals(e.OriginalSource, _overlay)) Close();
        };

        if (contentRoot != null)
        {
            contentRoot.SizeChanged += (_, e) =>
            {
                if (_overlay != null)
                {
                    _overlay.Width = Math.Max(1, e.NewSize.Width);
                    _overlay.Height = Math.Max(1, e.NewSize.Height);
                }
            };
        }

        _dialogBox = new Border
        {
            Width = 280,
            Padding = new Thickness(18, 16, 18, 16),
            CornerRadius = new CornerRadius(16),
            Background = RizVisuals.Brush(RizVisuals.Surface(dark)),
            BorderThickness = new Thickness(1),
            BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark)),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        var dialogLayout = new StackPanel { Spacing = 12 };

        // 1. 标头：标题 + 关闭按钮
        var header = new Grid();
        var title = new TextBlock
        {
            Text = "选取颜色",
            FontSize = 14,
            FontWeight = FontWeights.Bold,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = RizVisuals.Brush(RizVisuals.Text(dark))
        };
        _btnClose = CreateCircleButton("✕", dark, () => Close());
        _btnClose.HorizontalAlignment = HorizontalAlignment.Right;
        header.Children.Add(title);
        header.Children.Add(_btnClose);
        dialogLayout.Children.Add(header);

        // 2. SV 饱和度/明度拾取板 (140px 高度，双层渐变：水平白->纯色 + 垂直透明->黑)
        var svBorder = new Border
        {
            Height = 140,
            CornerRadius = new CornerRadius(8),
            BorderThickness = new Thickness(1),
            BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark))
        };

        var svGrid = new Grid
        {
            Height = 140,
            CornerRadius = new CornerRadius(8),
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        _svCanvas = new Canvas
        {
            Height = 140,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            IsHitTestVisible = true
        };

        // 垂直黑色渐变遮罩层 (对应 Web 端 .ccp-sv::after: linear-gradient(to top, #000, transparent))
        var blackMask = new Border
        {
            Height = 140,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            IsHitTestVisible = false,
            Background = new LinearGradientBrush
            {
                StartPoint = new Windows.Foundation.Point(0, 0),
                EndPoint = new Windows.Foundation.Point(0, 1),
                GradientStops =
                {
                    new GradientStop { Color = Windows.UI.Color.FromArgb(0, 0, 0, 0), Offset = 0 },
                    new GradientStop { Color = Windows.UI.Color.FromArgb(255, 0, 0, 0), Offset = 1 }
                }
            }
        };

        UpdateSvBackground();

        _svThumb = new Border
        {
            Width = 14,
            Height = 14,
            CornerRadius = new CornerRadius(7),
            BorderThickness = new Thickness(2),
            BorderBrush = RizVisuals.Brush(Microsoft.UI.Colors.White),
            Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
            IsHitTestVisible = false
        };
        _svCanvas.Children.Add(_svThumb);

        svGrid.Children.Add(_svCanvas);
        svGrid.Children.Add(blackMask);
        svBorder.Child = svGrid;
        dialogLayout.Children.Add(svBorder);

        _svCanvas.PointerPressed += OnSvPressed;
        _svCanvas.PointerMoved += OnSvMoved;
        _svCanvas.PointerReleased += OnSvReleased;

        // 3. Hue 色相滑块条 (14px 高度)
        var hueBorder = new Border
        {
            Height = 14,
            CornerRadius = new CornerRadius(7),
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        _hueCanvas = new Canvas
        {
            Height = 14,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            IsHitTestVisible = true
        };
        _hueCanvas.Background = new LinearGradientBrush
        {
            StartPoint = new Windows.Foundation.Point(0, 0.5),
            EndPoint = new Windows.Foundation.Point(1, 0.5),
            GradientStops =
            {
                new GradientStop { Color = Windows.UI.Color.FromArgb(255, 255, 0, 0), Offset = 0 },
                new GradientStop { Color = Windows.UI.Color.FromArgb(255, 255, 255, 0), Offset = 0.17 },
                new GradientStop { Color = Windows.UI.Color.FromArgb(255, 0, 255, 0), Offset = 0.33 },
                new GradientStop { Color = Windows.UI.Color.FromArgb(255, 0, 255, 255), Offset = 0.5 },
                new GradientStop { Color = Windows.UI.Color.FromArgb(255, 0, 0, 255), Offset = 0.67 },
                new GradientStop { Color = Windows.UI.Color.FromArgb(255, 255, 0, 255), Offset = 0.83 },
                new GradientStop { Color = Windows.UI.Color.FromArgb(255, 255, 0, 0), Offset = 1.0 }
            }
        };

        _hueThumb = new Border
        {
            Width = 14,
            Height = 18,
            CornerRadius = new CornerRadius(4),
            BorderThickness = new Thickness(2),
            BorderBrush = RizVisuals.Brush(Microsoft.UI.Colors.White),
            Background = RizVisuals.Brush(Windows.UI.Color.FromArgb(120, 0, 0, 0)),
            IsHitTestVisible = false
        };
        Canvas.SetTop(_hueThumb, -2);
        _hueCanvas.Children.Add(_hueThumb);
        hueBorder.Child = _hueCanvas;
        dialogLayout.Children.Add(hueBorder);

        _hueCanvas.PointerPressed += OnHuePressed;
        _hueCanvas.PointerMoved += OnHueMoved;
        _hueCanvas.PointerReleased += OnHueReleased;

        // 4. 底部操作区：预览色块 + Hex 文本框 + 确定按钮
        var footer = new Grid { ColumnSpacing = 10 };
        footer.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        footer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        footer.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        _previewDot = new Border
        {
            Width = 28,
            Height = 28,
            CornerRadius = new CornerRadius(6),
            BorderThickness = new Thickness(1),
            BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark)),
            VerticalAlignment = VerticalAlignment.Center
        };
        Grid.SetColumn(_previewDot, 0);

        _hexInput = new TextBox
        {
            Height = 28,
            FontSize = 12,
            FontFamily = new FontFamily("Cascadia Code, Consolas, monospace"),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        _hexInput.TextChanged += (_, _) =>
        {
            var text = _hexInput.Text?.Trim() ?? string.Empty;
            if (!text.StartsWith("#")) text = "#" + text;
            if (ColorUtil.TryParseHex(text, out var c))
            {
                var (h, s, v) = RgbToHsv(c.R, c.G, c.B);
                _hue = h; _sat = s; _val = v;
                UpdateSvBackground();
                UpdateSvThumbPosition();
                UpdateHueThumbPosition();
                if (_previewDot != null) _previewDot.Background = RizVisuals.Brush(c);
            }
        };
        Grid.SetColumn(_hexInput, 1);

        _btnConfirm = CreateConfirmButton(accent, () => Confirm());
        Grid.SetColumn(_btnConfirm, 2);

        footer.Children.Add(_previewDot);
        footer.Children.Add(_hexInput);
        footer.Children.Add(_btnConfirm);
        dialogLayout.Children.Add(footer);

        _dialogBox.Child = dialogLayout;
        _overlay.Children.Add(_dialogBox);

        _popup = new Popup
        {
            Child = _overlay,
            IsLightDismissEnabled = false,
            XamlRoot = XamlRoot
        };
    }

    private Border CreateCircleButton(string text, bool dark, Action onClick)
    {
        var btn = new Border
        {
            Width = 24,
            Height = 24,
            CornerRadius = new CornerRadius(12),
            Background = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), RizVisuals.Surface(dark), 0.08)),
            IsHitTestVisible = true
        };
        var tb = new TextBlock
        {
            Text = text,
            FontSize = 11,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark))
        };
        btn.Child = tb;
        btn.PointerEntered += (_, _) =>
        {
            btn.Background = RizVisuals.Brush(Windows.UI.Color.FromArgb(255, 224, 85, 85));
            tb.Foreground = RizVisuals.Brush(Microsoft.UI.Colors.White);
        };
        btn.PointerExited += (_, _) =>
        {
            btn.Background = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), RizVisuals.Surface(dark), 0.08));
            tb.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
        };
        btn.PointerPressed += (_, e) =>
        {
            e.Handled = true;
            onClick();
        };
        return btn;
    }

    private Border CreateConfirmButton(Windows.UI.Color accent, Action onConfirm)
    {
        var btn = new Border
        {
            Width = 28,
            Height = 28,
            CornerRadius = new CornerRadius(14),
            Background = RizVisuals.Brush(accent),
            IsHitTestVisible = true
        };
        var tb = new TextBlock
        {
            Text = "✓",
            FontSize = 14,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = RizVisuals.Brush(Microsoft.UI.Colors.White)
        };
        btn.Child = tb;
        btn.PointerEntered += (_, _) => btn.Opacity = 0.85;
        btn.PointerExited += (_, _) => btn.Opacity = 1.0;
        btn.PointerPressed += (_, e) =>
        {
            e.Handled = true;
            onConfirm();
        };
        return btn;
    }

    private void Close()
    {
        if (_popup == null) return;
        _popup.IsOpen = false;
        _popup = null;
        _overlay = null;
        _dialogBox = null;
    }

    private void Confirm()
    {
        var currentHex = GetCurrentHex();
        ColorValue = currentHex;
        Close();
    }

    private void OnHuePressed(object sender, PointerRoutedEventArgs e)
    {
        _isDraggingHue = true;
        _hueCanvas?.CapturePointer(e.Pointer);
        UpdateHueFromPoint(e.GetCurrentPoint(_hueCanvas).Position.X);
        e.Handled = true;
    }

    private void OnHueMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_isDraggingHue) return;
        UpdateHueFromPoint(e.GetCurrentPoint(_hueCanvas).Position.X);
        e.Handled = true;
    }

    private void OnHueReleased(object sender, PointerRoutedEventArgs e)
    {
        if (!_isDraggingHue) return;
        _isDraggingHue = false;
        _hueCanvas?.ReleasePointerCapture(e.Pointer);
        e.Handled = true;
    }

    private void UpdateHueFromPoint(double x)
    {
        if (_hueCanvas == null || _hueCanvas.ActualWidth <= 0) return;
        var ratio = Math.Clamp(x / _hueCanvas.ActualWidth, 0, 1);
        _hue = ratio * 360;
        UpdateHueThumbPosition();
        UpdateSvBackground();
        UpdatePreview();
    }

    private void OnSvPressed(object sender, PointerRoutedEventArgs e)
    {
        _isDraggingSV = true;
        _svCanvas?.CapturePointer(e.Pointer);
        var pt = e.GetCurrentPoint(_svCanvas).Position;
        UpdateSvFromPoint(pt.X, pt.Y);
        e.Handled = true;
    }

    private void OnSvMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_isDraggingSV) return;
        var pt = e.GetCurrentPoint(_svCanvas).Position;
        UpdateSvFromPoint(pt.X, pt.Y);
        e.Handled = true;
    }

    private void OnSvReleased(object sender, PointerRoutedEventArgs e)
    {
        if (!_isDraggingSV) return;
        _isDraggingSV = false;
        _svCanvas?.ReleasePointerCapture(e.Pointer);
        e.Handled = true;
    }

    private void UpdateSvFromPoint(double x, double y)
    {
        if (_svCanvas == null || _svCanvas.ActualWidth <= 0 || _svCanvas.ActualHeight <= 0) return;
        _sat = Math.Clamp(x / _svCanvas.ActualWidth, 0, 1);
        _val = Math.Clamp(1.0 - (y / _svCanvas.ActualHeight), 0, 1);
        UpdateSvThumbPosition();
        UpdatePreview();
    }

    private void UpdateSvBackground()
    {
        if (_svCanvas == null) return;
        var pureColor = HsvToColor(_hue, 1.0, 1.0);
        _svCanvas.Background = new LinearGradientBrush
        {
            StartPoint = new Windows.Foundation.Point(0, 0),
            EndPoint = new Windows.Foundation.Point(1, 0),
            GradientStops =
            {
                new GradientStop { Color = Microsoft.UI.Colors.White, Offset = 0 },
                new GradientStop { Color = pureColor, Offset = 1 }
            }
        };
    }

    private void UpdateHueThumbPosition()
    {
        if (_hueCanvas == null || _hueThumb == null) return;
        var w = _hueCanvas.ActualWidth > 0 ? _hueCanvas.ActualWidth : 240;
        Canvas.SetLeft(_hueThumb, Math.Clamp((_hue / 360.0) * w - 7, -7, w - 7));
    }

    private void UpdateSvThumbPosition()
    {
        if (_svCanvas == null || _svThumb == null) return;
        var w = _svCanvas.ActualWidth > 0 ? _svCanvas.ActualWidth : 240;
        var h = _svCanvas.ActualHeight > 0 ? _svCanvas.ActualHeight : 140;
        Canvas.SetLeft(_svThumb, Math.Clamp(_sat * w - 7, -7, w - 7));
        Canvas.SetTop(_svThumb, Math.Clamp((1.0 - _val) * h - 7, -7, h - 7));
    }

    private void UpdatePreview()
    {
        var hex = GetCurrentHex();
        var color = ColorUtil.FromHex(hex, Windows.UI.Color.FromArgb(255, 102, 204, 255));
        if (_previewDot != null) _previewDot.Background = RizVisuals.Brush(color);
        if (_hexInput != null && !string.Equals(_hexInput.Text, hex, StringComparison.OrdinalIgnoreCase))
        {
            _hexInput.Text = hex;
        }
    }

    private string GetCurrentHex()
    {
        var (r, g, b) = HsvToRgb(_hue, _sat, _val);
        return $"#{r:X2}{g:X2}{b:X2}";
    }

    private void ParseHexToHsv(string hex)
    {
        var c = ColorUtil.FromHex(hex, Windows.UI.Color.FromArgb(255, 102, 204, 255));
        var (h, s, v) = RgbToHsv(c.R, c.G, c.B);
        _hue = h;
        _sat = s;
        _val = v;
    }

    private void UpdateVisual()
    {
        if (!_templateApplied) return;
        UpdateSwatch();
        Opacity = IsEnabled ? 1 : 0.45;
    }

    private void UpdateSwatch()
    {
        if (_swatch == null || _swatchDot == null || _swatchHex == null) return;
        var dark = RizVisuals.IsDark(this, Theme);
        var hex = string.IsNullOrWhiteSpace(ColorValue) ? "#66CCFF" : ColorValue;
        var c = ColorUtil.FromHex(hex, Windows.UI.Color.FromArgb(255, 102, 204, 255));

        _swatch.Background = RizVisuals.Brush(RizVisuals.Surface(dark));
        _swatch.BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark));
        _swatchDot.Background = RizVisuals.Brush(c);
        _swatchHex.Text = hex;
        _swatchHex.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
    }

    private static (double H, double S, double V) RgbToHsv(byte r, byte g, byte b)
    {
        double rd = r / 255.0, gd = g / 255.0, bd = b / 255.0;
        double max = Math.Max(rd, Math.Max(gd, bd)), min = Math.Min(rd, Math.Min(gd, bd));
        double delta = max - min;
        double h = 0;
        if (delta > 0)
        {
            if (max == rd) h = (gd - bd) / delta % 6;
            else if (max == gd) h = (bd - rd) / delta + 2;
            else h = (rd - gd) / delta + 4;
            h *= 60;
            if (h < 0) h += 360;
        }
        double s = max == 0 ? 0 : delta / max;
        return (h, s, max);
    }

    private static Windows.UI.Color HsvToColor(double h, double s, double v)
    {
        var (r, g, b) = HsvToRgb(h, s, v);
        return Windows.UI.Color.FromArgb(255, r, g, b);
    }

    private static (byte R, byte G, byte B) HsvToRgb(double h, double s, double v)
    {
        int hi = (int)Math.Floor(h / 60.0) % 6;
        double f = (h / 60.0) - Math.Floor(h / 60.0);
        double p = v * (1 - s);
        double q = v * (1 - f * s);
        double t = v * (1 - (1 - f) * s);
        double r = 0, g = 0, b = 0;
        switch (hi)
        {
            case 0: r = v; g = t; b = p; break;
            case 1: r = q; g = v; b = p; break;
            case 2: r = p; g = v; b = t; break;
            case 3: r = p; g = q; b = v; break;
            case 4: r = t; g = p; b = v; break;
            case 5: r = v; g = p; b = q; break;
        }
        return ((byte)Math.Round(r * 255), (byte)Math.Round(g * 255), (byte)Math.Round(b * 255));
    }
}

