using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.UI.Xaml.Media;

namespace RizUI.Uwp.Controls;

/// <summary>
/// RizUI 内置的 Font Awesome Free 7 图标解析器。
/// 接受完整 CSS class 组合（如 <c>fa-solid fa-house</c>、<c>fa-brands fa-github</c>），
/// 从随程序集嵌入的官方 all.css 读取所有 Free class→glyph 映射，不要求应用另装 Font Awesome。
/// </summary>
public static class RizFontAwesome
{
    /// <summary>Solid / Regular 字体族名称。</summary>
    public const string FreeFamily = "Font Awesome 7 Free";
    /// <summary>Brands 字体族名称。</summary>
    public const string BrandsFamily = "Font Awesome 7 Brands";

    private const string ResourceName = "RizUI.Uwp.Assets.FontAwesome.all.css";
    private static readonly Lazy<IReadOnlyDictionary<string, string>> Glyphs = new(LoadGlyphs);

    /// <summary>已从官方 CSS 加载的全部 FA Free class 数量。</summary>
    public static int SupportedClassCount => Glyphs.Value.Count;

    /// <summary>解析一个完整 FA class 组合。非 FA 文本会原样作为 glyph 返回。</summary>
    public static RizFontAwesomeIcon Resolve(string? classes)
    {
        if (string.IsNullOrWhiteSpace(classes)) return new RizFontAwesomeIcon(string.Empty, null);
        var tokens = classes.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var iconClass = tokens.FirstOrDefault(token => token.StartsWith("fa-", StringComparison.OrdinalIgnoreCase)
            && !IsStyleClass(token));
        if (iconClass is null || !Glyphs.Value.TryGetValue(iconClass, out var glyph))
            return new RizFontAwesomeIcon(classes, null);

        var isBrands = tokens.Any(token => token.Equals("fa-brands", StringComparison.OrdinalIgnoreCase)
            || token.Equals("fab", StringComparison.OrdinalIgnoreCase));
        var isRegular = tokens.Any(token => token.Equals("fa-regular", StringComparison.OrdinalIgnoreCase)
            || token.Equals("far", StringComparison.OrdinalIgnoreCase));
        var source = isBrands ? "fa-brands-400.woff2" : isRegular ? "fa-regular-400.woff2" : "fa-solid-900.woff2";
        var family = isBrands ? BrandsFamily : FreeFamily;
        return new RizFontAwesomeIcon(glyph, new FontFamily($"ms-appx:///RizUI.Uwp/Assets/FontAwesome/{source}#{family}"));
    }

    /// <summary>判断类名是否是已内置的 Font Awesome Free 图标类。</summary>
    public static bool Supports(string? iconClass)
        => !string.IsNullOrWhiteSpace(iconClass) && Glyphs.Value.ContainsKey(iconClass);

    private static bool IsStyleClass(string token)
        => token.Equals("fa-solid", StringComparison.OrdinalIgnoreCase)
           || token.Equals("fa-regular", StringComparison.OrdinalIgnoreCase)
           || token.Equals("fa-brands", StringComparison.OrdinalIgnoreCase)
           || token.Equals("fa-classic", StringComparison.OrdinalIgnoreCase)
           || token.Equals("fa", StringComparison.OrdinalIgnoreCase)
           || token.StartsWith("fa-rotate", StringComparison.OrdinalIgnoreCase)
           || token.StartsWith("fa-flip", StringComparison.OrdinalIgnoreCase)
           || token.StartsWith("fa-spin", StringComparison.OrdinalIgnoreCase)
           || token.StartsWith("fa-pulse", StringComparison.OrdinalIgnoreCase)
           || Regex.IsMatch(token, "^fa-(?:[2-9]|10)x$", RegexOptions.IgnoreCase);

    private static IReadOnlyDictionary<string, string> LoadGlyphs()
    {
        using var stream = typeof(RizFontAwesome).GetTypeInfo().Assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"Missing embedded Font Awesome resource: {ResourceName}");
        using var reader = new StreamReader(stream);
        var css = reader.ReadToEnd();
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // 官方规则的形式为 .fa-house { --fa: "\f015"; }，一个规则可能带多个 alias selector。
        var blocks = Regex.Matches(css, """(?ms)(?<selectors>(?:\.[\w-]+\s*,?\s*)+)\{[^{}]*?--fa:\s*"(?<escape>\\[^"]+)";[^{}]*?\}""");
        foreach (Match block in blocks)
        {
            var glyph = DecodeCssEscape(block.Groups["escape"].Value);
            if (string.IsNullOrEmpty(glyph)) continue;
            foreach (Match selector in Regex.Matches(block.Groups["selectors"].Value, @"\.((?:fa)-[\w-]+)"))
                result[selector.Groups[1].Value] = glyph;
        }
        return result;
    }

    private static string DecodeCssEscape(string value)
    {
        if (string.IsNullOrEmpty(value) || value[0] != '\\') return value;
        var escaped = value[1..];
        var hex = new string(escaped.TakeWhile(Uri.IsHexDigit).Take(6).ToArray());
        if (hex.Length > 0 && int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out var codePoint))
            return char.ConvertFromUtf32(codePoint);
        return escaped.Length > 0 ? escaped[..1] : string.Empty;
    }
}

/// <summary>已解析的 Font Awesome glyph 与其字体。</summary>
public readonly record struct RizFontAwesomeIcon(string Glyph, FontFamily? FontFamily);