using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.ApplicationModel.DataTransfer;

namespace RizUI.Uwp.Controls;

/// <summary>原生实现的 rizui_code；使用 TextBlock.Inlines 保留轻量 tokenizer 的逐 token 配色。</summary>
[ContentProperty(Name = nameof(Code))]
public sealed class RizCode : Control
{
    private Border? _root;
    private TextBlock? _inline;
    private TextBlock? _blockCode;
    private TextBlock? _langText;
    private Border? _copyBtn;
    private TextBlock? _copyIcon;
    private Border? _copyBar;
    private Border? _headerBorder;
    private ScrollViewer? _scroll;
    private Grid? _blockRoot;
    private Storyboard? _copyStoryboard;
    private DispatcherQueueTimer? _copiedResetTimer;
    private bool _isCopied;

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
        ActualThemeChanged += (_, _) => UpdateVisual();
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
        _root = GetTemplateChild("Root") as Border;
        _inline = GetTemplateChild("InlineCode") as TextBlock;
        _blockCode = GetTemplateChild("BlockCode") as TextBlock;
        _langText = GetTemplateChild("LangText") as TextBlock;
        _copyBtn = GetTemplateChild("CopyBtn") as Border;
        _copyIcon = GetTemplateChild("CopyIcon") as TextBlock;
        _copyBar = GetTemplateChild("CopyBar") as Border;
        _headerBorder = GetTemplateChild("HeaderBorder") as Border;
        _scroll = GetTemplateChild("CodeScroll") as ScrollViewer;

        if (_copyBtn != null)
        {
            _copyBtn.PointerPressed += (_, e) =>
            {
                e.Handled = true;
                OnCopy();
            };
            _copyBtn.PointerEntered += (_, _) =>
            {
                if (_copyBtn != null) _copyBtn.Opacity = 1.0;
            };
            _copyBtn.PointerExited += (_, _) =>
            {
                if (_copyBtn != null) _copyBtn.Opacity = 0.55;
            };
        }

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

        if (_root != null)
        {
            _root.BorderBrush = RizVisuals.Brush(border);
            _root.Background = RizVisuals.Brush(Block
                ? RizVisuals.Mix(RizVisuals.Text(dark), surface, 0.05)
                : RizVisuals.Mix(RizVisuals.Text(dark), surface, 0.08));
            _root.CornerRadius = new CornerRadius(Block ? 10 : 6);
        }

        if (_headerBorder != null)
        {
            _headerBorder.BorderBrush = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), surface, 0.08));
        }

        if (_copyBtn != null)
        {
            _copyBtn.Background = RizVisuals.Brush(RizVisuals.Mix(accent, surface, 0.12));
            _copyBtn.Opacity = 0.55;
        }

        if (_copyIcon != null)
        {
            UpdateCopyIconVisual(accent);
        }

        if (_langText != null)
        {
            _langText.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
        }

        if (_copyBar != null)
        {
            // 顶栏背景进度条：使用淡色主题色填充 (18%~25% accent)
            _copyBar.Background = RizVisuals.Brush(RizVisuals.Mix(accent, surface, 0.22));
        }

        if (_scroll != null && !string.IsNullOrWhiteSpace(HeightText))
        {
            _scroll.MaxHeight = RizVisuals.ParsePixels(HeightText, double.PositiveInfinity);
        }

        if (_inline != null) _inline.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        if (_blockCode != null) _blockCode.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));

        // 重新着色 tokens
        UpdateCode();
    }

    private void UpdateCopyIconVisual(Windows.UI.Color accent)
    {
        if (_copyIcon == null) return;
        var iconClass = _isCopied ? "fa-solid fa-check" : "fa-regular fa-copy";
        RizVisuals.ApplyIcon(_copyIcon, iconClass, 11, accent);
    }

    private void UpdateCode()
    {
        var text = Code ?? string.Empty;
        var canonical = Canonical(Lang);
        var dark = RizVisuals.IsDark(this, Theme);
        if (_langText != null) _langText.Text = LanguageName(canonical);
        if (_inline != null) Fill(_inline, text, canonical, dark);
        if (_blockCode != null) Fill(_blockCode, text, canonical, dark);
        UpdateMode();
    }

    private void UpdateMode()
    {
        if (_inline != null) _inline.Visibility = Block ? Visibility.Collapsed : Visibility.Visible;
        if (_blockRoot != null) _blockRoot.Visibility = Block ? Visibility.Visible : Visibility.Collapsed;
    }

    private void OnCopy()
    {
        var package = new DataPackage();
        package.SetText(Code ?? string.Empty);
        Clipboard.SetContent(package);
        Copied?.Invoke(this, EventArgs.Empty);

        _isCopied = true;
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);
        UpdateCopyIconVisual(accent);

        if (_copyBar != null)
        {
            _copyBar.Visibility = Visibility.Visible;
            _copyBar.Opacity = 1.0;
            _copyStoryboard?.Stop();

            // 测量 Header 宽度以进行像素级宽度动画
            var header = GetTemplateChild("Header") as FrameworkElement;
            var targetWidth = header?.ActualWidth > 0 ? header.ActualWidth : (_root?.ActualWidth > 0 ? _root.ActualWidth : 300);

            _copyBar.Width = 0;
            var animation = new DoubleAnimation
            {
                From = 0,
                To = targetWidth,
                Duration = new Duration(TimeSpan.FromMilliseconds(1300)),
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(animation, _copyBar);
            Storyboard.SetTargetProperty(animation, "Width");
            _copyStoryboard = new Storyboard();
            _copyStoryboard.Children.Add(animation);
            _copyStoryboard.Begin();
        }

        _copiedResetTimer?.Stop();
        _copiedResetTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();
        _copiedResetTimer.Interval = TimeSpan.FromMilliseconds(1300);
        _copiedResetTimer.IsRepeating = false;
        _copiedResetTimer.Tick += (_, _) =>
        {
            _isCopied = false;
            UpdateCopyIconVisual(accent);

            // 填充完成后平滑淡出消失 (200ms)
            if (_copyBar != null)
            {
                var fadeAnim = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EnableDependentAnimation = true
                };
                Storyboard.SetTarget(fadeAnim, _copyBar);
                Storyboard.SetTargetProperty(fadeAnim, "Opacity");
                var fadeSb = new Storyboard();
                fadeSb.Children.Add(fadeAnim);
                fadeSb.Completed += (_, _) =>
                {
                    if (_copyBar != null)
                    {
                        _copyBar.Visibility = Visibility.Collapsed;
                        _copyBar.Opacity = 1.0;
                    }
                };
                fadeSb.Begin();
            }

            _copiedResetTimer?.Stop();
            _copiedResetTimer = null;
        };
        _copiedResetTimer.Start();
    }

    private static void Fill(TextBlock target, string text, string language, bool dark)
    {
        target.Inlines.Clear();
        foreach (var token in Tokenize(text, language))
        {
            var run = new Run { Text = token.Text };
            if (token.Kind != TokenKind.Plain)
            {
                run.Foreground = new SolidColorBrush(TokenColor(token.Kind, dark));
            }
            if (token.Kind == TokenKind.Keyword) run.FontWeight = FontWeights.SemiBold;
            if (token.Kind == TokenKind.Comment) run.FontStyle = Windows.UI.Text.FontStyle.Italic;
            target.Inlines.Add(run);
        }
    }

    private enum TokenKind { Plain, Keyword, String, Number, Comment, Function, Type, Operator, Tag, Attr }
    private readonly record struct Token(string Text, TokenKind Kind);

    private static IEnumerable<Token> Tokenize(string code, string language)
    {
        var kw = Keywords.TryGetValue(language, out var values) ? new HashSet<string>(values, StringComparer.OrdinalIgnoreCase) : new HashSet<string>();
        var pattern = language is "html" or "xml" or "vue"
            ? @"(<!--[\s\S]*?-->)|(""(?:[^""\\]|\\.)*""|'(?:[^'\\]|\\.)*')|(=>|===|!==|==|!=|<=|>=|&&|\|\||[-+*/%!&|^~=]+)|(\b\d+(?:\.\d+)?\b)|(\b[A-Za-z_$][\w$]*\b)"
            : @"(//[^\n]*|#[^\n]*|/\*[\s\S]*?\*/)|(""(?:[^""\\]|\\.)*""|'(?:[^'\\]|\\.)*'|`(?:[^`\\]|\\.)*`)|(=>|===|!==|==|!=|<=|>=|&&|\|\||[-+*/%!&|^~=]+)|(\b\d[\d_]*(?:\.\d+)?\b)|(\b[A-Za-z_$][\w$]*\b)";
        var last = 0;
        foreach (Match match in Regex.Matches(code, pattern))
        {
            if (match.Index > last) yield return new Token(code[last..match.Index], TokenKind.Plain);
            var rest = code[(match.Index + match.Length)..];
            var prev = code[..match.Index];

            TokenKind kind;
            if (match.Groups[1].Success)
            {
                kind = TokenKind.Comment;
            }
            else if (match.Groups[2].Success)
            {
                kind = TokenKind.String;
            }
            else if (match.Groups[3].Success)
            {
                kind = TokenKind.Operator;
            }
            else if (match.Groups[4].Success)
            {
                kind = TokenKind.Number;
            }
            else if (language is "html" or "xml" or "vue" && Regex.IsMatch(prev, @"<\/?$"))
            {
                kind = TokenKind.Tag;
            }
            else if (Regex.IsMatch(rest, @"^\s*="))
            {
                kind = TokenKind.Attr;
            }
            else if (Regex.IsMatch(rest, @"^\s*\("))
            {
                kind = TokenKind.Function;
            }
            else if (kw.Contains(match.Value))
            {
                kind = TokenKind.Keyword;
            }
            else if (match.Value is "string" or "number" or "bool" or "boolean" or "int" or "float" or "double" or "void" or "object" or "var")
            {
                kind = TokenKind.Type;
            }
            else
            {
                kind = TokenKind.Plain;
            }

            yield return new Token(match.Value, kind);
            last = match.Index + match.Length;
        }
        if (last < code.Length) yield return new Token(code[last..], TokenKind.Plain);
    }

    private static Windows.UI.Color TokenColor(TokenKind kind, bool dark) => (kind, dark) switch
    {
        (TokenKind.Keyword, false) => Windows.UI.Color.FromArgb(255, 0xA6, 0x26, 0xA4),
        (TokenKind.Keyword, true)  => Windows.UI.Color.FromArgb(255, 0xC6, 0x78, 0xDD),
        (TokenKind.String, false)  => Windows.UI.Color.FromArgb(255, 0x50, 0xA1, 0x4F),
        (TokenKind.String, true)   => Windows.UI.Color.FromArgb(255, 0x98, 0xC3, 0x79),
        (TokenKind.Number, false)  => Windows.UI.Color.FromArgb(255, 0x98, 0x68, 0x01),
        (TokenKind.Number, true)   => Windows.UI.Color.FromArgb(255, 0xD1, 0x9A, 0x66),
        (TokenKind.Comment, false) => Windows.UI.Color.FromArgb(255, 0x9A, 0xA0, 0xA6),
        (TokenKind.Comment, true)  => Windows.UI.Color.FromArgb(255, 0x7F, 0x84, 0x8E),
        (TokenKind.Function, false)=> Windows.UI.Color.FromArgb(255, 0x40, 0x78, 0xF2),
        (TokenKind.Function, true) => Windows.UI.Color.FromArgb(255, 0x61, 0xAF, 0xEF),
        (TokenKind.Type, false)    => Windows.UI.Color.FromArgb(255, 0x1D, 0x7F, 0xB6),
        (TokenKind.Type, true)     => Windows.UI.Color.FromArgb(255, 0x56, 0xB6, 0xC2),
        (TokenKind.Operator, false)=> Windows.UI.Color.FromArgb(255, 0x9B, 0x50, 0xE0),
        (TokenKind.Operator, true) => Windows.UI.Color.FromArgb(255, 0xC5, 0x86, 0xC0),
        (TokenKind.Tag, false)     => Windows.UI.Color.FromArgb(255, 0xE4, 0x56, 0x49),
        (TokenKind.Tag, true)      => Windows.UI.Color.FromArgb(255, 0xE0, 0x6C, 0x75),
        (TokenKind.Attr, false)    => Windows.UI.Color.FromArgb(255, 0xB7, 0x6B, 0x01),
        (TokenKind.Attr, true)     => Windows.UI.Color.FromArgb(255, 0xE5, 0xC0, 0x7B),
        _ => dark ? Windows.UI.Color.FromArgb(255, 0xE6, 0xEB, 0xF2) : Windows.UI.Color.FromArgb(255, 0x33, 0x33, 0x44)
    };

    private static string Canonical(string? language) => Aliases.TryGetValue(language?.Trim() ?? string.Empty, out var value) ? value : (language?.Trim().ToLowerInvariant() ?? "text");
    private static string LanguageName(string language) => language switch
    {
        "js" => "JavaScript", "ts" => "TypeScript", "html" => "HTML / Vue", "css" => "CSS", "python" => "Python",
        "bash" => "Bash", "cs" => "C#", "json" => "JSON", "yaml" => "YAML", "sql" => "SQL", "md" => "Markdown", _ => string.Empty
    };
}