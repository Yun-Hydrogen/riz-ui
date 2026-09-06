using System;
using Windows.UI;

namespace RizUI.Uwp.Controls;

/// <summary>
/// 颜色转换工具（HEX / HSL / HSV）。
/// </summary>
public static class ColorUtil
{
    public static Color FromHex(string hex, Color fallback)
    {
        hex = (hex ?? string.Empty).Trim();
        if (!hex.StartsWith("#")) return fallback;
        hex = hex.Substring(1);
        if (hex.Length == 6)
        {
            try
            {
                return Color.FromArgb(255,
                    Convert.ToByte(hex.Substring(0, 2), 16),
                    Convert.ToByte(hex.Substring(2, 2), 16),
                    Convert.ToByte(hex.Substring(4, 2), 16));
            }
            catch { }
        }
        else if (hex.Length == 3)
        {
            try
            {
                var r = Convert.ToByte(hex.Substring(0, 1) + hex.Substring(0, 1), 16);
                var g = Convert.ToByte(hex.Substring(1, 1) + hex.Substring(1, 1), 16);
                var b = Convert.ToByte(hex.Substring(2, 1) + hex.Substring(2, 1), 16);
                return Color.FromArgb(255, r, g, b);
            }
            catch { }
        }
        return fallback;
    }

    public static string ToHex(Color c)
        => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

    /// <summary>H(0-360) S(0-100) V(0-100) → Color</summary>
    public static Color FromHsv(double h, double s, double v)
    {
        s = Math.Max(0, Math.Min(100, s));
        v = Math.Max(0, Math.Min(100, v));
        var sN = s / 100;
        var vN = v / 100;
        var c = vN * sN;
        var x = c * (1 - Math.Abs((h / 60 % 2) - 1));
        var m = vN - c;
        double r = 0, g = 0, b = 0;
        if (h < 60) { r = c; g = x; }
        else if (h < 120) { r = x; g = c; }
        else if (h < 180) { g = c; b = x; }
        else if (h < 240) { g = x; b = c; }
        else if (h < 300) { r = x; b = c; }
        else { r = c; b = x; }
        return Color.FromArgb(255,
            (byte)Math.Round((r + m) * 255),
            (byte)Math.Round((g + m) * 255),
            (byte)Math.Round((b + m) * 255));
    }

    /// <summary>Color → (H, S, V)</summary>
    public static (double h, double s, double v) ToHsv(Color c)
    {
        var r = c.R / 255.0;
        var g = c.G / 255.0;
        var b = c.B / 255.0;
        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));
        var d = max - min;
        double h = 0;
        if (d != 0)
        {
            if (max == r) h = (g - b) / d % 6;
            else if (max == g) h = (b - r) / d + 2;
            else h = (r - g) / d + 4;
            h *= 60;
            if (h < 0) h += 360;
        }
        var s = max == 0 ? 0 : (d / max) * 100;
        var v = max * 100;
        return (h, s, v);
    }
}
