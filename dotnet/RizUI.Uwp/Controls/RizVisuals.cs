using System.Globalization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

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
        => dark ? Windows.UI.Color.FromArgb(230, 0x1E, 0x26, 0x30) : Windows.UI.Color.FromArgb(153, 0xF8, 0xFA, 0xFC);

    public static Windows.UI.Color Border(bool dark)
        => dark ? Windows.UI.Color.FromArgb(255, 0x32, 0x3D, 0x4B) : Windows.UI.Color.FromArgb(255, 0xE8, 0xEC, 0xF2);

    public static Windows.UI.Color Text(bool dark)
        => dark ? Windows.UI.Color.FromArgb(255, 0xE6, 0xEB, 0xF2) : Windows.UI.Color.FromArgb(255, 0x33, 0x33, 0x44);

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

    /// <summary>
    /// 原生端没有随 Vue 包内置的 Font Awesome 字体；支持 Unicode glyph，
    /// 并把组件默认及常见的 FA class 降级为语义相同的系统符号。
    /// </summary>
    public static string ResolveIcon(string? icon, string fallback)
    {
        if (string.IsNullOrWhiteSpace(icon)) return fallback;
        if (!icon.Contains("fa-", StringComparison.OrdinalIgnoreCase)) return icon;

        if (icon.Contains("circle-info", StringComparison.OrdinalIgnoreCase)) return "ⓘ";
        if (icon.Contains("triangle-exclamation", StringComparison.OrdinalIgnoreCase)) return "⚠";
        if (icon.Contains("circle-exclamation", StringComparison.OrdinalIgnoreCase)) return "!";
        if (icon.Contains("xmark", StringComparison.OrdinalIgnoreCase)) return "×";
        if (icon.Contains("check", StringComparison.OrdinalIgnoreCase)) return "✓";
        if (icon.Contains("copy", StringComparison.OrdinalIgnoreCase)) return "▣";
        return fallback;
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