using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Text;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.DataTransfer;

namespace RizUI.Uwp.Controls;

/// <summary>原生实现的 rizui_code；使用 TextBlock.Inlines 保留轻量 tokenizer 的逐 token 配色。</summary>
[ContentProperty(Name = nameof(Code))]
public sealed class RizCode : Control
{
    private TextBlock? _inline;
    private TextBlock? _blockCode;
    private TextBlock? _langText;
    private Button? _copy;
    private Border? _copyBar;
    private ScrollViewer? _scroll;
    private Grid? _blockRoot;
    private Storyboard? _copyStoryboard;

    private static readonly Dictionary<string, string> Aliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["javascript"] = "js", ["jsx"] = "js", ["typescript"] = "ts", ["tsx"] = "ts", ["py"] = "python",
        ["sh"] = "bash", ["shell"] = "bash", ["zsh"] = "bash", ["yml"] = "yaml", ["markdown"] = "md",
        ["c++"] = "cpp", ["csharp"] = "cs", ["c#"] = "cs", ["vue"] = "html", ["xml"] = "html", ["htm"] = "html",
        ["rs"] = "rust", ["rb"] = "ruby", ["kt"] = "kotlin", ["txt"] = "text", ["plain"] = "text"
    };

    private static readonly Dictionary<string, string[]> Keywords = new(StringComparer.OrdinalIgnoreCase)
    {
        ["js"] = "const let var function return if else for while new class extends this try catch throw async await import export from default delete static".Split(' '),
        ["ts"] = "const let var function return if else for while new class extends this try catch throw async await import export from default interface type enum implements readonly as string number boolean any unknown never".Split(' '),
        ["python"] = "def class return if elif else for while in not and or is pass break continue import from as with try except finally raise lambda yield True False None self print".Split(' '),
        ["bash"] = "if then else elif fi for while do done case esac function return local export echo read exit cd".Split(' '),
        ["cs"] = "public private protected internal class interface enum namespace using static void int string bool return if else for while new async await null true false".Split(' '),
        ["json"] = new[] { "true", "false", "null" },
        ["yaml"] = new[] { "true", "false", "null", "yes", "no", "on", "off" },
        ["sql"] = "SELECT FROM WHERE INSERT INTO VALUES UPDATE SET DELETE CREATE TABLE ALTER DROP JOIN LEFT RIGHT INNER ON GROUP BY ORDER HAVING LIMIT AND OR NOT NULL AS".Split(' ')
    };

    public RizCode()
    {
        DefaultStyleKey = typeof(RizCode);
        RegisterPropertyChangedCallback(CodeProperty, (_, _) => UpdateCode());
            RegisterPropertyChangedCallback(BlockProperty, (_, _) => { UpdateMode(); UpdateVisual(); });
        RegisterPropertyChangedCallback(LangProperty, (_, _) => UpdateCode());
        RegisterPropertyChangedCallback(ColorProperty, (_, _) => UpdateVisual());
        RegisterPropertyChangedCallback(HeightTextProperty, (_, _) => UpdateVisual());
        RegisterPropertyChangedCallback(ThemeProperty, (_, _) => UpdateVisual());
    }

    public static readonly DependencyProperty CodeProperty = DependencyProperty.Register(nameof(Code), typeof(string), typeof(RizCode), new PropertyMetadata(string.Empty, OnCodeChanged));
    public string Code { get => (string)GetValue(CodeProperty); set => SetValue(CodeProperty, value); }
    public static readonly DependencyProperty BlockProperty = DependencyProperty.Register(nameof(Block), typeof(bool), typeof(RizCode), new PropertyMetadata(false, OnCodeChanged));
    public bool Block { get => (bool)GetValue(BlockProperty); set => SetValue(BlockProperty, value); }
    public static readonly DependencyProperty LangProperty = DependencyProperty.Register(nameof(Lang), typeof(string), typeof(RizCode), new PropertyMetadata(string.Empty, OnCodeChanged));
    public string Lang { get => (string)GetValue(LangProperty); set => SetValue(LangProperty, value); }
    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizCode), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Color { get => (string)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }
    public static readonly DependencyProperty HeightTextProperty = DependencyProperty.Register(nameof(HeightText), typeof(string), typeof(RizCode), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string HeightText { get => (string)GetValue(HeightTextProperty); set => SetValue(HeightTextProperty, value); }
    public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizCode), new PropertyMetadata("auto", OnVisualChanged));
    public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }

    public event EventHandler? Copied;

    private static void OnCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizCode)d).UpdateCode();
    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizCode)d).UpdateVisual();

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _inline = GetTemplateChild("InlineCode") as TextBlock;
        _blockCode = GetTemplateChild("BlockCode") as TextBlock;
        _langText = GetTemplateChild("LangText") as TextBlock;
        _copy = GetTemplateChild("Copy") as Button;
        _copyBar = GetTemplateChild("CopyBar") as Border;
        _scroll = GetTemplateChild("CodeScroll") as ScrollViewer;
        if (_copy != null) _copy.Click += OnCopy;
        if (_inline != null) _inline.Visibility = Block ? Visibility.Collapsed : Visibility.Visible;
        _blockRoot = GetTemplateChild("BlockRoot") as Grid;
        UpdateMode();
        UpdateVisual();
        UpdateCode();
    }

    private void UpdateVisual()
    {
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);
        var surface = RizVisuals.Surface(dark);
        var border = string.IsNullOrWhiteSpace(Color) ? RizVisuals.Border(dark) : accent;
            var root = GetTemplateChild("Root") as Border;
            if (root != null)
            {
                root.BorderBrush = RizVisuals.Brush(border);
                root.Background = RizVisuals.Brush(Block
                    ? RizVisuals.Mix(RizVisuals.Text(dark), surface, 0.05)
                    : RizVisuals.Mix(RizVisuals.Text(dark), surface, 0.08));
                root.CornerRadius = new CornerRadius(Block ? 10 : 6);
            }
        if (_copy != null)
        {
            _copy.Foreground = RizVisuals.Brush(accent);
            _copy.Background = RizVisuals.Brush(RizVisuals.Mix(accent, surface, 0.12));
        }
        if (_langText != null) _langText.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
        if (_copyBar != null) _copyBar.Background = RizVisuals.Brush(accent);
        if (_scroll != null && !string.IsNullOrWhiteSpace(HeightText)) _scroll.MaxHeight = RizVisuals.ParsePixels(HeightText, double.PositiveInfinity);
        if (_inline != null) _inline.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        if (_blockCode != null) _blockCode.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
    }

    private void UpdateCode()
    {
        var text = Code ?? string.Empty;
        var canonical = Canonical(Lang);
        if (_langText != null) _langText.Text = LanguageName(canonical);
        if (_inline != null) Fill(_inline, text, canonical);
        if (_blockCode != null) Fill(_blockCode, text, canonical);
            UpdateMode();
    }

    private void UpdateMode()
    {
        if (_inline != null) _inline.Visibility = Block ? Visibility.Collapsed : Visibility.Visible;
        if (_blockRoot != null) _blockRoot.Visibility = Block ? Visibility.Visible : Visibility.Collapsed;
    }

    private void OnCopy(object sender, RoutedEventArgs e)
    {
        var package = new DataPackage();
        package.SetText(Code ?? string.Empty);
        Clipboard.SetContent(package);
        Copied?.Invoke(this, EventArgs.Empty);
        if (_copyBar == null) return;
        _copyStoryboard?.Stop();
        _copyBar.RenderTransform = new ScaleTransform { ScaleX = 0, ScaleY = 1, CenterX = 0, CenterY = 0 };
        var transform = (ScaleTransform)_copyBar.RenderTransform;
        var animation = new DoubleAnimation { To = 1, Duration = new Duration(TimeSpan.FromMilliseconds(1200)) };
        Storyboard.SetTarget(animation, transform);
        Storyboard.SetTargetProperty(animation, "ScaleX");
        _copyStoryboard = new Storyboard();
        _copyStoryboard.Children.Add(animation);
        _copyStoryboard.Begin();
        if (_copy != null) _copy.Content = "✓";
    }

    private static void Fill(TextBlock target, string text, string language)
    {
        target.Inlines.Clear();
        foreach (var token in Tokenize(text, language))
        {
            var run = new Run { Text = token.Text };
            if (token.Kind != TokenKind.Plain) run.Foreground = new SolidColorBrush(TokenColor(token.Kind));
            if (token.Kind == TokenKind.Keyword) run.FontWeight = FontWeights.SemiBold;
            if (token.Kind == TokenKind.Comment) run.FontStyle = Windows.UI.Text.FontStyle.Italic;
            target.Inlines.Add(run);
        }
    }

    private enum TokenKind { Plain, Keyword, String, Number, Comment, Function, Type, Operator }
    private readonly record struct Token(string Text, TokenKind Kind);

    private static IEnumerable<Token> Tokenize(string code, string language)
    {
        var kw = Keywords.TryGetValue(language, out var values) ? new HashSet<string>(values, StringComparer.OrdinalIgnoreCase) : new HashSet<string>();
        var pattern = language is "html" ? @"(<!--[\s\S]*?-->)|(""(?:[^""\\]|\\.)*""|'(?:[^'\\]|\\.)*')|(\b\d+(?:\.\d+)?\b)|(\b[A-Za-z_$][\w$]*\b)" : @"(//[^\n]*|#[^\n]*|/\*[\s\S]*?\*/)|(""(?:[^""\\]|\\.)*""|'(?:[^'\\]|\\.)*'|`(?:[^`\\]|\\.)*`)|(=>|===|!==|==|!=|<=|>=|&&|\|\||[-+*/%!&|^~=]+)|(\b\d[\d_]*(?:\.\d+)?\b)|(\b[A-Za-z_$][\w$]*\b)";
        var last = 0;
        foreach (Match match in Regex.Matches(code, pattern))
        {
            if (match.Index > last) yield return new Token(code[last..match.Index], TokenKind.Plain);
            var kind = match.Groups[1].Success ? TokenKind.Comment
                : match.Groups[2].Success ? TokenKind.String
                : match.Groups.Count > 3 && match.Groups[3].Success ? TokenKind.Operator
                : match.Groups.Count > 4 && match.Groups[4].Success ? TokenKind.Number
                : kw.Contains(match.Value) ? TokenKind.Keyword
                : match.Value is "string" or "number" or "bool" or "int" or "float" or "double" ? TokenKind.Type
                : code[(match.Index + match.Length)..].TrimStart().StartsWith("(") ? TokenKind.Function : TokenKind.Plain;
            yield return new Token(match.Value, kind);
            last = match.Index + match.Length;
        }
        if (last < code.Length) yield return new Token(code[last..], TokenKind.Plain);
    }

    private static Windows.UI.Color TokenColor(TokenKind kind) => kind switch
    {
        TokenKind.Keyword => Windows.UI.Color.FromArgb(255, 0xA6, 0x26, 0xA4),
        TokenKind.String => Windows.UI.Color.FromArgb(255, 0x50, 0xA1, 0x4F),
        TokenKind.Number => Windows.UI.Color.FromArgb(255, 0x98, 0x68, 0x01),
        TokenKind.Comment => Windows.UI.Color.FromArgb(255, 0x9A, 0xA0, 0xA6),
        TokenKind.Function => Windows.UI.Color.FromArgb(255, 0x40, 0x78, 0xF2),
        TokenKind.Type => Windows.UI.Color.FromArgb(255, 0x1D, 0x7F, 0xB6),
        TokenKind.Operator => Windows.UI.Color.FromArgb(255, 0x9B, 0x50, 0xE0),
        _ => Windows.UI.Color.FromArgb(255, 0x33, 0x33, 0x44)
    };

    private static string Canonical(string? language) => Aliases.TryGetValue(language?.Trim() ?? string.Empty, out var value) ? value : (language?.Trim().ToLowerInvariant() ?? "text");
    private static string LanguageName(string language) => language switch
    {
        "js" => "JavaScript", "ts" => "TypeScript", "html" => "HTML / Vue", "css" => "CSS", "python" => "Python",
        "bash" => "Bash", "cs" => "C#", "json" => "JSON", "yaml" => "YAML", "sql" => "SQL", "md" => "Markdown", _ => string.Empty
    };
}