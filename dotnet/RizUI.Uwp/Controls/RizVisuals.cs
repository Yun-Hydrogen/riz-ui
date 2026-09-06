using System.Globalization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;

namespace RizUI.Uwp.Controls;

/// <summary>RizUI 控件内部共用的主题色、CSS color-mix 与图标降级工具。</summary>
internal static class RizVisuals
{
    public static bool IsDark(Control control, string? theme)
        => string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase)
           || (!string.Equals(theme, "Light", StringComparison.OrdinalIgnoreCase)
               && control.ActualTheme == ElementTheme.Dark);

    public static Windows.UI.Color ThemeColor(string? color)
        => ColorUtil.FromHex(color ?? string.Empty, Windows.UI.Color.FromArgb(255, 0x66, 0xCC, 0xFF));

    public static Windows.UI.Color Surface(bool dark)
        => dark ? Windows.UI.Color.FromArgb(255, 0x1E, 0x26, 0x30) : Microsoft.UI.Colors.White;

    public static Windows.UI.Color SurfaceSoft(bool dark)
        => dark ? Windows.UI.Color.FromArgb(230, 0x1E, 0x26, 0x30) : Windows.UI.Color.FromArgb(255, 0xFA, 0xFB, 0xFD);

    public static Windows.UI.Color Border(bool dark)
        => dark ? Windows.UI.Color.FromArgb(255, 0x32, 0x3D, 0x4B) : Windows.UI.Color.FromArgb(255, 0xE8, 0xEC, 0xF2);

    public static Windows.UI.Color Text(bool dark)
        => dark ? Windows.UI.Color.FromArgb(255, 0xE6, 0xEB, 0xF2) : Windows.UI.Color.FromArgb(255, 0x33, 0x33, 0x44);

    /// <summary>卡片/模态框等容器的主标题加黑加粗颜色。</summary>
    public static Windows.UI.Color TitleText(bool dark)
        => dark ? Windows.UI.Color.FromArgb(255, 0xF2, 0xF5, 0xF8) : Windows.UI.Color.FromArgb(255, 0x18, 0x20, 0x2C);

    public static Windows.UI.Color Subtext(bool dark)
        => dark ? Windows.UI.Color.FromArgb(255, 0x93, 0xA1, 0xB3) : Windows.UI.Color.FromArgb(255, 0x99, 0x99, 0xAA);

    public static Windows.UI.Color Icon(bool dark)
        => dark ? Windows.UI.Color.FromArgb(255, 0x7F, 0x8E, 0xA1) : Windows.UI.Color.FromArgb(255, 0x99, 0x99, 0xAA);

    /// <summary>等价于 color-mix(in srgb, foreground percentage, background)。</summary>
    public static Windows.UI.Color Mix(Windows.UI.Color foreground, Windows.UI.Color background, double foregroundRatio)
    {
        var t = Math.Clamp(foregroundRatio, 0, 1);
        return Windows.UI.Color.FromArgb(255,
            (byte)Math.Round(foreground.R * t + background.R * (1 - t)),
            (byte)Math.Round(foreground.G * t + background.G * (1 - t)),
            (byte)Math.Round(foreground.B * t + background.B * (1 - t)));
    }

    public static SolidColorBrush Brush(Windows.UI.Color color) => new(color);

    public static Windows.UI.Color PresetColor(string? type, string? infoColor = null)
        => string.Equals(type, "warn", StringComparison.OrdinalIgnoreCase)
            ? Windows.UI.Color.FromArgb(255, 0xE6, 0xA2, 0x3C)
            : string.Equals(type, "error", StringComparison.OrdinalIgnoreCase)
                ? Windows.UI.Color.FromArgb(255, 0xE0, 0x55, 0x55)
                : ThemeColor(infoColor);

    public static void ApplyIcon(TextBlock target, string? iconClass, double defaultSize, Windows.UI.Color defaultColor)
    {
        if (string.IsNullOrWhiteSpace(iconClass))
        {
            target.Text = string.Empty;
            target.Visibility = Visibility.Collapsed;
            return;
        }

        var resolved = RizFontAwesome.Resolve(iconClass);
        target.Text = resolved.Glyph;
        if (resolved.FontFamily != null)
        {
            target.FontFamily = resolved.FontFamily;
        }
        if (target.Style == null)
        {
            target.FontSize = defaultSize;
            target.Foreground = Brush(defaultColor);
        }
        target.Visibility = Visibility.Visible;

        // ── 动画修饰符与变换 ──
        if (!string.IsNullOrWhiteSpace(iconClass))
        {
            ApplyIconModifiers(target, iconClass);
        }
    }

    private static void ApplyIconModifiers(TextBlock target, string iconClass)
    {
        var tokens = iconClass.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var transform = new CompositeTransform();
        target.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
        target.RenderTransform = transform;

        // 翻转修饰符：fa-flip-horizontal (水平翻转), fa-flip-vertical (垂直翻转), fa-flip-both
        var flipH = tokens.Any(t => t.Equals("fa-flip-horizontal", StringComparison.OrdinalIgnoreCase));
        var flipV = tokens.Any(t => t.Equals("fa-flip-vertical", StringComparison.OrdinalIgnoreCase));
        var flipBoth = tokens.Any(t => t.Equals("fa-flip-both", StringComparison.OrdinalIgnoreCase));
        if (flipBoth) { flipH = true; flipV = true; }
        if (flipH) transform.ScaleX = -1;
        if (flipV) transform.ScaleY = -1;

        // 旋转角度修饰符：fa-rotate-90, fa-rotate-180, fa-rotate-270, fa-rotate-by (任意角度)
        var rotateToken = tokens.FirstOrDefault(t => t.StartsWith("fa-rotate-", StringComparison.OrdinalIgnoreCase));
        if (rotateToken != null)
        {
            var degStr = rotateToken.Substring("fa-rotate-".Length);
            if (double.TryParse(degStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var angle))
            {
                transform.Rotation = angle;
            }
        }

        // 动画修饰符
        var isSpin = tokens.Any(t => t.Equals("fa-spin", StringComparison.OrdinalIgnoreCase));
        var isSpinPulse = tokens.Any(t => t.Equals("fa-spin-pulse", StringComparison.OrdinalIgnoreCase));
        var isSpinReverse = tokens.Any(t => t.Equals("fa-spin-reverse", StringComparison.OrdinalIgnoreCase));
        var isPulse = tokens.Any(t => t.Equals("fa-pulse", StringComparison.OrdinalIgnoreCase));
        var isBeat = tokens.Any(t => t.Equals("fa-beat", StringComparison.OrdinalIgnoreCase));
        var isBounce = tokens.Any(t => t.Equals("fa-bounce", StringComparison.OrdinalIgnoreCase));
        var isFade = tokens.Any(t => t.Equals("fa-fade", StringComparison.OrdinalIgnoreCase));

        // 1. fa-spin: 匀速 360° 旋转 (reverse 支持反向旋转)
        if (isSpin)
        {
            var from = isSpinReverse ? 360 : 0;
            var to = isSpinReverse ? 0 : 360;
            var anim = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(2)),
                RepeatBehavior = RepeatBehavior.Forever,
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(anim, transform);
            Storyboard.SetTargetProperty(anim, "Rotation");
            var sb = new Storyboard();
            sb.Children.Add(anim);
            sb.Begin();
        }
        // 2. fa-spin-pulse / fa-pulse: 8 步跳跃脉冲旋转 (步进旋转)
        else if (isSpinPulse || isPulse)
        {
            var sb = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };
            var keyFrames = new DoubleAnimationUsingKeyFrames { EnableDependentAnimation = true };
            for (int i = 0; i <= 8; i++)
            {
                keyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(i * (1.0 / 8.0))),
                    Value = isSpinReverse ? (360 - i * 45) : (i * 45)
                });
            }
            Storyboard.SetTarget(keyFrames, transform);
            Storyboard.SetTargetProperty(keyFrames, "Rotation");
            sb.Children.Add(keyFrames);
            sb.Begin();
        }

        // 3. fa-beat: 心跳缩放动画 (1.0 -> 1.25 -> 1.0)
        if (isBeat)
        {
            var sb = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };
            var animX = new DoubleAnimation
            {
                From = 1.0,
                To = 1.25,
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                AutoReverse = true,
                EnableDependentAnimation = true,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            var animY = new DoubleAnimation
            {
                From = 1.0,
                To = 1.25,
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                AutoReverse = true,
                EnableDependentAnimation = true,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(animX, transform);
            Storyboard.SetTargetProperty(animX, "ScaleX");
            Storyboard.SetTarget(animY, transform);
            Storyboard.SetTargetProperty(animY, "ScaleY");
            sb.Children.Add(animX);
            sb.Children.Add(animY);
            sb.Begin();
        }

        // 4. fa-bounce: 上下跳动动画
        if (isBounce)
        {
            var sb = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };
            var animY = new DoubleAnimation
            {
                From = 0,
                To = -7,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                AutoReverse = true,
                EnableDependentAnimation = true,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTarget(animY, transform);
            Storyboard.SetTargetProperty(animY, "TranslateY");
            sb.Children.Add(animY);
            sb.Begin();
        }

        // 5. fa-fade: 呼吸渐隐渐显动画 (1.0 -> 0.4 -> 1.0)
        if (isFade)
        {
            var sb = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };
            var anim = new DoubleAnimation
            {
                From = 1.0,
                To = 0.4,
                Duration = new Duration(TimeSpan.FromMilliseconds(800)),
                AutoReverse = true,
                EnableDependentAnimation = true,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(anim, target);
            Storyboard.SetTargetProperty(anim, "Opacity");
            sb.Children.Add(anim);
            sb.Begin();
        }
    }

    public static double ParsePixels(string? value, double fallback)
    {
        if (string.IsNullOrWhiteSpace(value)) return fallback;
        var normalized = value.Trim().TrimEnd('p', 'x', 'P', 'X').Trim();
        return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)
            && result > 0
            ? result
            : fallback;
    }
}