using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

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
    private Border? _hTrack;
    private Canvas? _hCanvas;
    private Border? _hThumb;
    private TranslateTransform? _hThumbTransform;
    private SolidColorBrush? _hThumbBrush;
    private bool _isDraggingThumb;
    private double _dragStartPointerX;
    private double _dragStartScrollOffset;
    private DispatcherQueueTimer? _copiedResetTimer;
    private Storyboard? _copyFadeStoryboard;
    private bool _isCopied;

    // ── 语言支持表（与 Web 端 rizui_code.vue 的 LANG_ALIAS / LANGS 保持一致）──

    /// <summary>支持的语言（简短名 + 显示名），顺序即清单展示顺序。</summary>
    private static readonly (string Lang, string Name)[] LanguageNames =
    {
        ("js", "JavaScript"), ("ts", "TypeScript"), ("html", "HTML / Vue"), ("css", "CSS"),
        ("md", "Markdown"), ("json", "JSON"), ("bash", "Bash"), ("python", "Python"),
        ("sql", "SQL"), ("yaml", "YAML"), ("ini", "INI / TOML"), ("java", "Java"),
        ("c", "C"), ("cpp", "C++"), ("cs", "C#"), ("go", "Go"), ("rust", "Rust"),
        ("ruby", "Ruby"), ("php", "PHP"), ("kotlin", "Kotlin"), ("swift", "Swift"), ("text", "Plain Text")
    };

    private static readonly Dictionary<string, string> Aliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["javascript"] = "js", ["jsx"] = "js", ["typescript"] = "ts", ["tsx"] = "ts", ["py"] = "python",
        ["sh"] = "bash", ["shell"] = "bash", ["zsh"] = "bash", ["yml"] = "yaml", ["markdown"] = "md",
        ["toml"] = "ini", ["c++"] = "cpp", ["csharp"] = "cs", ["c#"] = "cs",
        ["vue"] = "html", ["xml"] = "html", ["htm"] = "html",
        ["rs"] = "rust", ["rb"] = "ruby", ["kt"] = "kotlin", ["txt"] = "text", ["plain"] = "text"
    };

    private static readonly Dictionary<string, string[]> Keywords = new(StringComparer.OrdinalIgnoreCase)
    {
        ["js"] = Split("const let var function return if else for while do switch case break continue new class extends super this typeof instanceof in of try catch finally throw async await yield import export from default delete void static get set"),
        ["ts"] = Split("const let var function return if else for while do switch case break continue new class extends super this typeof instanceof in of try catch finally throw async await yield import export from default delete void static get set interface type enum implements declare namespace readonly abstract as satisfies keyof infer never unknown any string number boolean symbol object"),
        ["python"] = Split("def class return if elif else for while in not and or is pass break continue import from as with try except finally raise lambda yield global nonlocal del assert async await True False None self print"),
        ["bash"] = Split("if then else elif fi for while do done case esac function return local export echo read exit shift set source alias sudo cd ls cat grep sed awk"),
        ["cs"] = Split("public private protected internal class interface enum namespace using static void int string bool return if else for while new async await null true false var get set record struct"),
        ["json"] = new[] { "true", "false", "null" },
        ["yaml"] = new[] { "true", "false", "null", "yes", "no", "on", "off" },
        ["ini"] = new[] { "true", "false" },
        ["sql"] = Split("SELECT FROM WHERE INSERT INTO VALUES UPDATE SET DELETE CREATE TABLE ALTER DROP JOIN LEFT RIGHT INNER OUTER ON GROUP BY ORDER HAVING LIMIT AND OR NOT NULL AS DISTINCT UNION ALL PRIMARY KEY FOREIGN REFERENCES INDEX VIEW BEGIN COMMIT ROLLBACK TRANSACTION"),
        ["java"] = Split("public private protected class interface enum extends implements static final abstract void int long short byte char boolean float double return if else for while do switch case break continue new try catch finally throw throws this super import package null true false"),
        ["c"] = Split("int char float double long short unsigned signed void struct union enum typedef static const volatile register extern return if else for while do switch case break continue goto sizeof"),
        ["cpp"] = Split("int char float double long short unsigned signed void struct union enum typedef static const volatile register extern return if else for while do switch case break continue goto sizeof class public private protected template typename namespace using new delete try catch throw virtual override final constexpr auto nullptr this operator"),
        ["go"] = Split("package import func return if else for range switch case break continue go defer chan map struct interface type var const select default nil true false make new len cap append"),
        ["rust"] = Split("fn let mut const static struct enum impl trait pub use mod match if else for while loop return break continue in as where dyn async await move ref self Self true false Some None Ok Err"),
        ["ruby"] = Split("def end class module if elsif else unless while until for do begin rescue ensure return yield require include extend new puts print true false nil self"),
        ["php"] = Split("echo print if else elseif for foreach while do switch case break continue function class interface trait extends implements public private protected static return new try catch finally throw namespace use require include true false null"),
        ["kotlin"] = Split("fun val var class object interface when if else for while do return break continue try catch finally throw import package is in as null true false this super private public protected internal data sealed companion init"),
        ["swift"] = Split("func let var class struct enum protocol extension import return if else guard for while repeat switch case break continue in as is try catch throw defer nil true false self super init deinit private public internal open fileprivate static final override"),
    };

    /// <summary>逐语言的行注释标记（对齐 Web LANGS.line）。</summary>
    private static readonly Dictionary<string, string[]> LineComments = new(StringComparer.OrdinalIgnoreCase)
    {
        ["js"] = new[] { "//" }, ["ts"] = new[] { "//" }, ["cs"] = new[] { "//" }, ["java"] = new[] { "//" },
        ["c"] = new[] { "//" }, ["cpp"] = new[] { "//" }, ["go"] = new[] { "//" }, ["rust"] = new[] { "//" },
        ["kotlin"] = new[] { "//" }, ["swift"] = new[] { "//" }, ["php"] = new[] { "//", "#" },
        ["python"] = new[] { "#" }, ["bash"] = new[] { "#" }, ["ruby"] = new[] { "#" }, ["yaml"] = new[] { "#" },
        ["ini"] = new[] { "#", ";" }, ["sql"] = new[] { "--" }
    };

    /// <summary>逐语言的块注释标记对（对齐 Web LANGS.block）。</summary>
    private static readonly Dictionary<string, (string Open, string Close)[]> BlockComments = new(StringComparer.OrdinalIgnoreCase)
    {
        ["js"] = new[] { ("/*", "*/") }, ["ts"] = new[] { ("/*", "*/") }, ["cs"] = new[] { ("/*", "*/") },
        ["java"] = new[] { ("/*", "*/") }, ["c"] = new[] { ("/*", "*/") }, ["cpp"] = new[] { ("/*", "*/") },
        ["go"] = new[] { ("/*", "*/") }, ["rust"] = new[] { ("/*", "*/") }, ["kotlin"] = new[] { ("/*", "*/") },
        ["swift"] = new[] { ("/*", "*/") }, ["php"] = new[] { ("/*", "*/") }, ["css"] = new[] { ("/*", "*/") },
        ["sql"] = new[] { ("/*", "*/") }, ["python"] = new[] { ("\"\"\"", "\"\"\""), ("'''", "'''") },
        ["html"] = new[] { ("<!--", "-->") }
    };

    /// <summary>内置类型（单独配色，对齐 Web TYPES）。</summary>
    private static readonly HashSet<string> Types = new(StringComparer.Ordinal)
    {
        "string", "number", "boolean", "object", "array", "symbol", "bigint", "void", "any", "unknown", "never",
        "int", "long", "short", "float", "double", "char", "bool", "byte", "unsigned", "signed", "var",
        "String", "Number", "Boolean", "Object", "Array", "List", "Dict", "Set", "Tuple", "None", "NoneType"
    };

    private static readonly Dictionary<string, string> LanguageNameMap = BuildLanguageNameMap();

    /// <summary>组件支持的全部语言（简短名 / 显示名 / 可用别名），供 Demo 与文档枚举。</summary>
    public static IReadOnlyList<RizCodeLanguage> SupportedLanguages { get; } = BuildSupportedLanguages();

    private static string[] Split(string value) => value.Split(' ');

    private static Dictionary<string, string> BuildLanguageNameMap()
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (lang, name) in LanguageNames)
        {
            map[lang] = name;
        }
        return map;
    }

    private static IReadOnlyList<RizCodeLanguage> BuildSupportedLanguages()
    {
        var list = new List<RizCodeLanguage>();
        foreach (var (lang, name) in LanguageNames)
        {
            var aliases = new List<string>();
            foreach (var pair in Aliases)
            {
                if (string.Equals(pair.Value, lang, StringComparison.OrdinalIgnoreCase))
                {
                    aliases.Add(pair.Key);
                }
            }
            list.Add(new RizCodeLanguage { Lang = lang, Name = name, Aliases = string.Join(" / ", aliases) });
        }
        return list;
    }

    /// <summary>按语言组装注释匹配段（行注释 + 块注释；无注释配置时返回恒不匹配）。</summary>
    private static string BuildCommentPattern(string language)
    {
        var parts = new List<string>();
        if (LineComments.TryGetValue(language, out var lines))
        {
            foreach (var line in lines)
            {
                parts.Add(Regex.Escape(line) + "[^\\n]*");
            }
        }
        if (BlockComments.TryGetValue(language, out var blocks))
        {
            foreach (var (open, close) in blocks)
            {
                parts.Add(Regex.Escape(open) + "[\\s\\S]*?" + Regex.Escape(close));
            }
        }
        return parts.Count > 0 ? "(?:" + string.Join("|", parts) + ")" : "(?!)";
    }

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
        _hTrack = GetTemplateChild("HBarTrack") as Border;
        _hCanvas = GetTemplateChild("HBarCanvas") as Canvas;
        _hThumb = GetTemplateChild("HBarThumb") as Border;
        _hThumbTransform = _hThumb?.RenderTransform as TranslateTransform;

        if (_hThumb != null)
        {
            _hThumbBrush = new SolidColorBrush(GetThumbColor(0.35));
            _hThumb.Background = _hThumbBrush;
            _hThumb.PointerEntered += (_, _) => { if (_hThumbBrush != null) RizVisuals.AnimateColor(_hThumbBrush, GetThumbColor(0.60), 120); };
            _hThumb.PointerExited += (_, _) => { if (_hThumbBrush != null) RizVisuals.AnimateColor(_hThumbBrush, GetThumbColor(0.35), 180); };
            _hThumb.PointerPressed += OnThumbPointerPressed;
            _hThumb.PointerMoved += OnThumbPointerMoved;
            _hThumb.PointerReleased += OnThumbPointerReleased;
            _hThumb.PointerCanceled += OnThumbPointerReleased;
            _hThumb.PointerCaptureLost += OnThumbPointerReleased;
        }

        if (_hTrack != null)
        {
            _hTrack.PointerPressed += OnTrackPointerPressed;
        }

        if (_scroll != null)
        {
            _scroll.ViewChanged += (_, _) => UpdateHBar();
            _scroll.Loaded += (_, _) => UpdateHBar();
            _scroll.SizeChanged += (_, _) => UpdateHBar();
        }

        if (_blockCode != null)
        {
            _blockCode.SizeChanged += (_, _) => UpdateHBar();
        }

        SizeChanged += (_, _) => UpdateHBar();

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

    protected override Size MeasureOverride(Size availableSize)
    {
        // 关键：对内部测量传入可用宽度，但当 availableSize.Width 有限时，
        // 自身对外汇报的 DesiredSize.Width 坚决不超过 availableSize.Width，
        // 截断长行代码向上传播撑大外层卡片/页面；内部 ScrollViewer 则可以自由横向展开。
        var baseSize = base.MeasureOverride(availableSize);
        if (!double.IsInfinity(availableSize.Width) && availableSize.Width > 0)
        {
            return new Size(availableSize.Width, baseSize.Height);
        }
        return baseSize;
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
        if (_hThumbBrush != null) _hThumbBrush.Color = GetThumbColor(0.35);

        // 重新着色 tokens
        UpdateCode();
        UpdateHBar();
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
        UpdateHBar();
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
            // 关键：先释放上一轮动画的保持值——宽度动画停留的满宽、淡出动画停留的 Opacity=0。
            // 淡出动画若不停，动画值会持续压制基础值，第二次点击时进度条虽然照常动画但完全透明（看不到进度）。
            _copyStoryboard?.Stop();
            _copyFadeStoryboard?.Stop();
            _copyFadeStoryboard = null;
            _copyBar.Width = 0;
            _copyBar.Opacity = 1.0;
            _copyBar.Visibility = Visibility.Visible;

            // 测量 Header 宽度以进行像素级宽度动画
            var header = GetTemplateChild("Header") as FrameworkElement;
            var targetWidth = header?.ActualWidth > 0 ? header.ActualWidth : (_root?.ActualWidth > 0 ? _root.ActualWidth : 300);

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
                _copyFadeStoryboard = fadeSb;
                fadeSb.Completed += (_, _) =>
                {
                    if (_copyBar != null)
                    {
                        _copyBar.Visibility = Visibility.Collapsed;
                        // 先落基础值再 Stop，释放动画保持值（否则 Opacity 会一直被 0 保持，下次点击看不见进度条）
                        _copyBar.Opacity = 1.0;
                        fadeSb.Stop();
                    }
                    // 宽度动画的保持值也一并释放，回到基础值 0
                    _copyStoryboard?.Stop();
                    _copyFadeStoryboard = null;
                };
                fadeSb.Begin();
            }

            _copiedResetTimer?.Stop();
            _copiedResetTimer = null;
        };
        _copiedResetTimer.Start();
    }

    /// <summary>自绘水平滚动条的滑块颜色（对齐 Web：color-mix(accent 28%, transparent)，hover 48%）。</summary>
    private Windows.UI.Color ThumbColor(double ratio)
    {
        var accent = RizVisuals.ThemeColor(Color);
        return Windows.UI.Color.FromArgb((byte)Math.Round(255 * ratio), accent.R, accent.G, accent.B);
    }

    private Windows.UI.Color GetThumbColor(double opacity)
    {
        var accent = RizVisuals.ThemeColor(Color);
        return Windows.UI.Color.FromArgb((byte)Math.Round(255 * opacity), accent.R, accent.G, accent.B);
    }

    /// <summary>
    /// 自绘始终可见的水平滚动条（对齐 Web 端 .rzc-pre::-webkit-scrollbar 样式与体验）。
    /// 仅在单行代码超出视口时显示，滑块宽度根据视口比例自适应，位置与滚动偏移完全同步，支持拖拽和点击轨道跳转。
    /// </summary>
    private void UpdateHBar()
    {
        if (_scroll == null || _hTrack == null || _hThumb == null || _hThumbTransform == null || !Block) return;

        var scrollable = _scroll.ScrollableWidth;
        if (scrollable <= 1.0)
        {
            _hTrack.Visibility = Visibility.Collapsed;
            return;
        }

        _hTrack.Visibility = Visibility.Visible;

        // 轨道总宽度：使用当前可用实际宽度（扣除外层 Padding）
        var totalTrackWidth = _hTrack.ActualWidth > 1
            ? _hTrack.ActualWidth
            : (_scroll.ActualWidth > 24 ? _scroll.ActualWidth - 24 : 100);

        var extent = Math.Max(1, _scroll.ExtentWidth);
        var viewport = Math.Max(1, _scroll.ViewportWidth);
        var thumbWidth = Math.Clamp(viewport / extent * totalTrackWidth, 32, totalTrackWidth);
        _hThumb.Width = thumbWidth;

        var maxLeft = Math.Max(0, totalTrackWidth - thumbWidth);
        var ratio = Math.Clamp(_scroll.HorizontalOffset / scrollable, 0, 1);
        _hThumbTransform.X = ratio * maxLeft;
    }

    private void OnThumbPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (_scroll == null || _hTrack == null || _hThumb == null) return;
        _isDraggingThumb = true;
        _dragStartPointerX = e.GetCurrentPoint(_hTrack).Position.X;
        _dragStartScrollOffset = _scroll.HorizontalOffset;
        _hThumb.CapturePointer(e.Pointer);
        e.Handled = true;
    }

    private void OnThumbPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_isDraggingThumb || _scroll == null || _hTrack == null || _hThumb == null) return;
        var currentX = e.GetCurrentPoint(_hTrack).Position.X;
        var dx = currentX - _dragStartPointerX;
        var totalTrackWidth = _hTrack.ActualWidth;
        var maxLeft = Math.Max(1, totalTrackWidth - _hThumb.Width);
        var scrollable = Math.Max(1, _scroll.ScrollableWidth);
        var targetOffset = _dragStartScrollOffset + dx / maxLeft * scrollable;
        _scroll.ChangeView(Math.Clamp(targetOffset, 0, scrollable), null, null, true);
        e.Handled = true;
    }

    private void OnThumbPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (!_isDraggingThumb) return;
        _isDraggingThumb = false;
        _hThumb?.ReleasePointerCapture(e.Pointer);
        e.Handled = true;
    }

    private void OnTrackPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (_scroll == null || _hTrack == null || _hThumb == null) return;
        if (ReferenceEquals(e.OriginalSource, _hThumb)) return; // 点击滑块自身由拖拽处理
        var clickX = e.GetCurrentPoint(_hTrack).Position.X;
        var totalTrackWidth = _hTrack.ActualWidth;
        if (totalTrackWidth <= 1) return;
        var maxLeft = Math.Max(1, totalTrackWidth - _hThumb.Width);
        var scrollable = Math.Max(1, _scroll.ScrollableWidth);
        var targetOffset = (clickX - _hThumb.Width / 2) / maxLeft * scrollable;
        _scroll.ChangeView(Math.Clamp(targetOffset, 0, scrollable), null, null, true);
        e.Handled = true;
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
        // 注释标记按语言取（对齐 Web：python 的 """、sql 的 --、ini 的 # ; 等）
        var pattern =
            "(" + BuildCommentPattern(language) + ")" +
            @"|(""(?:[^""\\]|\\.)*""|'(?:[^'\\]|\\.)*'|`(?:[^`\\]|\\.)*`)" +
            @"|(=>|===|!==|==|!=|<=|>=|&&|\|\||[-+*/%!&|^~=]+)" +
            @"|(\b\d[\d_]*(?:\.\d+)?\b)" +
            @"|(\b[A-Za-z_$][\w$]*\b)";
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
            else if (Types.Contains(match.Value))
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
    private static string LanguageName(string language) => LanguageNameMap.TryGetValue(language, out var name) ? name : string.Empty;
}

/// <summary>RizCode 支持的语言描述（简短名 / 显示名 / 可用别名）。</summary>
public sealed class RizCodeLanguage
{
    /// <summary>传给 Lang 的简短名，如 cs / html / py 的规范名 python。</summary>
    public string Lang { get; init; } = string.Empty;

    /// <summary>展示名称，如 JavaScript / HTML / Vue。</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>可用别名（以 “ / ” 分隔），无别名时为空串。</summary>
    public string Aliases { get; init; } = string.Empty;
}