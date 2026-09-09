using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.UI.Text;

namespace RizUI.Uwp.Controls;

/// <summary>
/// 原生实现的 rizui_mdeditor。
/// 包含悬浮玻璃胶囊工具栏、带有图标与滑动过渡的编辑/预览选项卡、真实 Markdown 富文本实时排版渲染、主题聚焦光晕动画与自适应/固定高度。
/// </summary>
public sealed class RizMdEditor : Control
{
    private Border? _root;
    private Border? _focusGlow;
    private ScaleTransform? _focusScale;
    private ScrollViewer? _bodyScrollViewer;
    private Border? _toolbar;
    private StackPanel? _toolbarBtns;
    private Border? _tabsWrapper;
    private Border? _tabSlider;
    private TranslateTransform? _tabSliderTransform;
    private FrameworkElement? _tabEdit;
    private TextBlock? _tabEditIcon;
    private TextBlock? _tabEditText;
    private FrameworkElement? _tabPreview;
    private TextBlock? _tabPreviewIcon;
    private TextBlock? _tabPreviewText;
    private TextBox? _editor;
    private StackPanel? _previewPanel;
    private string _activeTab = "edit";
    private bool _templateApplied;

    public RizMdEditor()
    {
        DefaultStyleKey = typeof(RizMdEditor);
        IsTabStop = false;
        ActualThemeChanged += (_, _) => UpdateVisual();
        RegisterPropertyChangedCallback(IsEnabledProperty, (_, _) => UpdateVisual());
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(string), typeof(RizMdEditor),
            new PropertyMetadata(string.Empty, OnValueChanged));

    /// <summary>Markdown 源码文本（对应 Vue 的 modelValue）。</summary>
    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(RizMdEditor),
            new PropertyMetadata("在此输入 Markdown 内容…", OnVisualChanged));

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(RizMdEditor),
            new PropertyMetadata(false, OnReadOnlyChanged));

    /// <summary>是否只读（只读时隐藏工具栏与编辑框，仅展示渲染预览）。</summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizMdEditor),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    public string Color
    {
        get => (string)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DependencyProperty HeightTextProperty =
        DependencyProperty.Register(nameof(HeightText), typeof(string), typeof(RizMdEditor),
            new PropertyMetadata("320px", OnVisualChanged));

    /// <summary>编辑器高度：如 "320px"、"400px"，或 "auto" 根据内容自动展开。</summary>
    public string HeightText
    {
        get => (string)GetValue(HeightTextProperty);
        set => SetValue(HeightTextProperty, value);
    }

    public static readonly DependencyProperty DefaultProperty =
        DependencyProperty.Register(nameof(Default), typeof(string), typeof(RizMdEditor),
            new PropertyMetadata("edit", OnDefaultChanged));

    /// <summary>初始激活的选项卡："edit" 或 "preview"（对应 Vue 的 default 属性）。</summary>
    public string Default
    {
        get => (string)GetValue(DefaultProperty);
        set => SetValue(DefaultProperty, value);
    }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizMdEditor),
            new PropertyMetadata("auto", OnVisualChanged));

    public string Theme
    {
        get => (string)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public event EventHandler<string>? ValueChanged;

    private static void OnDefaultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ed = (RizMdEditor)d;
        var def = (string)e.NewValue;
        if (!ed.IsReadOnly)
        {
            ed._activeTab = string.Equals(def, "preview", StringComparison.OrdinalIgnoreCase) ? "preview" : "edit";
            ed.UpdateTabMode(false);
        }
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ed = (RizMdEditor)d;
        var val = (string)e.NewValue ?? string.Empty;
        if (ed._editor != null && !string.Equals(ed._editor.Text, val, StringComparison.Ordinal))
        {
            ed._editor.Text = val;
        }
        ed.RenderPreview();
        ed.ValueChanged?.Invoke(ed, val);
    }

    private static void OnReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ed = (RizMdEditor)d;
        if (ed.IsReadOnly)
        {
            ed._activeTab = "preview";
        }
        ed.UpdateTabMode(false);
        ed.UpdateVisual();
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizMdEditor)d).UpdateVisual();

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _root = GetTemplateChild("Root") as Border;
        _focusGlow = GetTemplateChild("FocusGlow") as Border;
        _bodyScrollViewer = GetTemplateChild("BodyScrollViewer") as ScrollViewer;
        _toolbar = GetTemplateChild("Toolbar") as Border;
        _toolbarBtns = GetTemplateChild("ToolbarBtns") as StackPanel;
        _tabsWrapper = GetTemplateChild("TabsWrapper") as Border;
        _tabSlider = GetTemplateChild("TabSlider") as Border;
        _tabEdit = GetTemplateChild("TabEdit") as FrameworkElement;
        _tabEditIcon = GetTemplateChild("TabEditIcon") as TextBlock;
        _tabEditText = GetTemplateChild("TabEditText") as TextBlock;
        _tabPreview = GetTemplateChild("TabPreview") as FrameworkElement;
        _tabPreviewIcon = GetTemplateChild("TabPreviewIcon") as TextBlock;
        _tabPreviewText = GetTemplateChild("TabPreviewText") as TextBlock;
        _editor = GetTemplateChild("Editor") as TextBox;
        _previewPanel = GetTemplateChild("PreviewPanel") as StackPanel;

        if (_focusGlow != null)
        {
            _focusScale = new ScaleTransform { ScaleX = 0, ScaleY = 1, CenterX = 0.5, CenterY = 0.5 };
            _focusGlow.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
            _focusGlow.RenderTransform = _focusScale;
            _focusGlow.Opacity = 0;
        }

        if (_tabSlider != null)
        {
            _tabSliderTransform = new TranslateTransform { X = 0 };
            _tabSlider.RenderTransform = _tabSliderTransform;
        }

        if (_tabEdit != null)
        {
            if (_tabEdit is Button btnEdit)
            {
                btnEdit.Click += (_, _) => SwitchTab("edit");
            }
            _tabEdit.Tapped += (_, e) =>
            {
                e.Handled = true;
                SwitchTab("edit");
            };
            _tabEdit.PointerPressed += (_, e) =>
            {
                e.Handled = true;
                SwitchTab("edit");
            };
        }

        if (_tabPreview != null)
        {
            if (_tabPreview is Button btnPreview)
            {
                btnPreview.Click += (_, _) => SwitchTab("preview");
            }
            _tabPreview.Tapped += (_, e) =>
            {
                e.Handled = true;
                SwitchTab("preview");
            };
            _tabPreview.PointerPressed += (_, e) =>
            {
                e.Handled = true;
                SwitchTab("preview");
            };
        }

        if (_editor != null)
        {
            _editor.Text = Value ?? string.Empty;
            _editor.GotFocus += (_, _) => AnimateFocusGlow(true);
            _editor.LostFocus += (_, _) => AnimateFocusGlow(false);
            _editor.TextChanged += (_, _) =>
            {
                if (!string.Equals(Value, _editor.Text, StringComparison.Ordinal))
                {
                    Value = _editor.Text;
                }
            };
        }

        BuildToolbarButtons();
        _templateApplied = true;
        if (IsReadOnly || string.Equals(Default, "preview", StringComparison.OrdinalIgnoreCase))
        {
            _activeTab = "preview";
        }
        UpdateTabMode(false);
        UpdateVisual();
        RenderPreview();
    }

    private void AnimateFocusGlow(bool isFocused)
    {
        if (_focusScale == null || _focusGlow == null) return;
        var duration = new Duration(TimeSpan.FromMilliseconds(350));
        var cubic = new CubicEase { EasingMode = isFocused ? EasingMode.EaseOut : EasingMode.EaseIn };

        var sb = new Storyboard();
        var scaleX = new DoubleAnimation
        {
            To = isFocused ? 1.0 : 0.0,
            Duration = duration,
            EnableDependentAnimation = true,
            EasingFunction = cubic
        };
        Storyboard.SetTarget(scaleX, _focusScale);
        Storyboard.SetTargetProperty(scaleX, "ScaleX");

        var fade = new DoubleAnimation
        {
            To = isFocused ? 1.0 : 0.0,
            Duration = new Duration(TimeSpan.FromMilliseconds(isFocused ? 200 : 180)),
            EnableDependentAnimation = true,
            EasingFunction = cubic
        };
        Storyboard.SetTarget(fade, _focusGlow);
        Storyboard.SetTargetProperty(fade, "Opacity");

        sb.Children.Add(scaleX);
        sb.Children.Add(fade);
        sb.Begin();
    }

    private void BuildToolbarButtons()
    {
        if (_toolbarBtns == null) return;
        _toolbarBtns.Children.Clear();

        var btns = new (string id, string tip, string icon)[]
        {
            ("bold", "加粗", "fa-solid fa-bold"),
            ("italic", "斜体", "fa-solid fa-italic"),
            ("strike", "删除线", "fa-solid fa-strikethrough"),
            ("h2", "标题", "fa-solid fa-heading"),
            ("quote", "引用", "fa-solid fa-quote-left"),
            ("ul", "无序列表", "fa-solid fa-list-ul"),
            ("ol", "有序列表", "fa-solid fa-list-ol"),
            ("task", "任务列表", "fa-solid fa-list-check"),
            ("code", "行内代码", "fa-solid fa-code"),
            ("codeblock", "代码块", "fa-solid fa-code-branch"),
            ("link", "链接", "fa-solid fa-link"),
            ("table", "表格", "fa-solid fa-table"),
            ("hr", "分割线", "fa-solid fa-minus")
        };

        foreach (var (id, tip, iconClass) in btns)
        {
            var btn = new Button
            {
                Width = 26,
                Height = 26,
                Padding = new Thickness(0),
                BorderThickness = new Thickness(0),
                CornerRadius = new CornerRadius(6),
                Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
                IsTabStop = false
            };
            ToolTipService.SetToolTip(btn, tip);

            var iconText = new TextBlock
            {
                FontSize = 11,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            RizVisuals.ApplyIcon(iconText, iconClass, 11, RizVisuals.Subtext(false));
            btn.Content = iconText;

            btn.PointerEntered += (_, _) =>
            {
                var isDk = RizVisuals.IsDark(this, Theme);
                var ac = RizVisuals.ThemeColor(Color);
                btn.Background = RizVisuals.Brush(RizVisuals.Mix(ac, RizVisuals.Surface(isDk), 0.12));
                iconText.Foreground = RizVisuals.Brush(ac);
            };
            btn.PointerExited += (_, _) =>
            {
                var isDk = RizVisuals.IsDark(this, Theme);
                btn.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
                iconText.Foreground = RizVisuals.Brush(RizVisuals.Subtext(isDk));
            };
            btn.Click += (_, _) => ExecuteToolbarAction(id);

            _toolbarBtns.Children.Add(btn);
        }
    }

    private void ExecuteToolbarAction(string id)
    {
        if (IsReadOnly || !IsEnabled) return;
        if (_activeTab != "edit") SwitchTab("edit");

        switch (id)
        {
            case "bold": InsertAround("**", "**", "加粗文本"); break;
            case "italic": InsertAround("*", "*", "斜体文本"); break;
            case "strike": InsertAround("~~", "~~", "删除文本"); break;
            case "h2": InsertPrefix("## "); break;
            case "quote": InsertPrefix("> "); break;
            case "ul": InsertPrefix("- "); break;
            case "ol": InsertPrefix("1. "); break;
            case "task": InsertPrefix("- [ ] "); break;
            case "code": InsertAround("`", "`", "code"); break;
            case "codeblock": InsertAround("\n```\n", "\n```\n", "const a = 1;"); break;
            case "link": InsertAround("[", "](https://)", "链接文字"); break;
            case "table": InsertAround("\n| 列1 | 列2 |\n| --- | --- |\n| 内容 | 内容 |\n", "", ""); break;
            case "hr": InsertAround("\n---\n", "", ""); break;
        }
    }

    private void InsertAround(string before, string after, string defaultSelect)
    {
        if (_editor == null) return;
        var src = _editor.Text ?? string.Empty;
        var start = _editor.SelectionStart;
        var len = _editor.SelectionLength;
        var selected = len > 0 ? src.Substring(start, len) : defaultSelect;
        var next = src.Substring(0, start) + before + selected + after + src.Substring(start + len);
        _editor.Text = next;
        _editor.SelectionStart = start + before.Length + selected.Length;
        _editor.SelectionLength = 0;
        _editor.Focus(FocusState.Programmatic);
    }

    private void InsertPrefix(string prefix)
    {
        if (_editor == null) return;
        var src = _editor.Text ?? string.Empty;
        var start = _editor.SelectionStart;
        var lineStart = src.LastIndexOf('\n', Math.Max(0, start - 1)) + 1;
        var next = src.Substring(0, lineStart) + prefix + src.Substring(lineStart);
        _editor.Text = next;
        _editor.SelectionStart = start + prefix.Length;
        _editor.SelectionLength = 0;
        _editor.Focus(FocusState.Programmatic);
    }

    private void SwitchTab(string tab)
    {
        if (_activeTab == tab) return;
        _activeTab = tab;
        UpdateTabMode(true);
    }

    private void UpdateTabMode(bool animate)
    {
        var isEdit = !IsReadOnly && _activeTab == "edit";
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);

        // 1. 滑块平滑位移动画 (0 -> 72px)
        if (_tabSliderTransform != null)
        {
            var targetX = _activeTab == "preview" ? 72d : 0d;
            if (animate)
            {
                var sb = new Storyboard();
                var anim = new DoubleAnimation
                {
                    To = targetX,
                    Duration = new Duration(TimeSpan.FromMilliseconds(280)),
                    EnableDependentAnimation = true,
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(anim, _tabSliderTransform);
                Storyboard.SetTargetProperty(anim, "X");
                sb.Children.Add(anim);
                sb.Begin();
            }
            else
            {
                _tabSliderTransform.X = targetX;
            }
        }

        // 2. 标签文本与图标主题色联动
        if (_tabEditIcon != null) RizVisuals.ApplyIcon(_tabEditIcon, "fa-solid fa-pen", 11, _activeTab == "edit" ? accent : RizVisuals.Subtext(dark));
        if (_tabPreviewIcon != null) RizVisuals.ApplyIcon(_tabPreviewIcon, "fa-solid fa-eye", 11, _activeTab == "preview" ? accent : RizVisuals.Subtext(dark));

        if (_tabEditText != null)
        {
            _tabEditText.Foreground = RizVisuals.Brush(_activeTab == "edit" ? accent : RizVisuals.Subtext(dark));
        }
        if (_tabPreviewText != null)
        {
            _tabPreviewText.Foreground = RizVisuals.Brush(_activeTab == "preview" ? accent : RizVisuals.Subtext(dark));
        }

        if (_toolbar != null)
        {
            _toolbar.Visibility = IsReadOnly ? Visibility.Collapsed : Visibility.Visible;
        }

        // 3. 画布平滑切换：edit→preview 向左滑入，preview→edit 向右滑入，配合渐变
        if (isEdit)
        {
            if (_previewPanel != null) _previewPanel.Visibility = Visibility.Collapsed;
            if (_editor != null)
            {
                _editor.Visibility = Visibility.Visible;
                if (animate)
                {
                    AnimateCanvasSlide(_editor, fromX: -24);
                }
                else
                {
                    _editor.Opacity = 1.0;
                    _editor.RenderTransform = null;
                }
            }
        }
        else
        {
            if (_editor != null) _editor.Visibility = Visibility.Collapsed;
            if (_previewPanel != null)
            {
                RenderPreview();
                _previewPanel.Visibility = Visibility.Visible;
                if (animate)
                {
                    AnimateCanvasSlide(_previewPanel, fromX: 24);
                }
                else
                {
                    _previewPanel.Opacity = 1.0;
                    _previewPanel.RenderTransform = null;
                }
            }
        }
    }

    private static void AnimateCanvasSlide(FrameworkElement target, double fromX)
    {
        var transform = new TranslateTransform { X = fromX };
        target.RenderTransform = transform;
        target.Opacity = 0.2;

        var sb = new Storyboard();
        var moveAnim = new DoubleAnimation
        {
            To = 0,
            Duration = new Duration(TimeSpan.FromMilliseconds(220)),
            EnableDependentAnimation = true,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(moveAnim, transform);
        Storyboard.SetTargetProperty(moveAnim, "X");

        var fadeAnim = new DoubleAnimation
        {
            To = 1.0,
            Duration = new Duration(TimeSpan.FromMilliseconds(200)),
            EnableDependentAnimation = true,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(fadeAnim, target);
        Storyboard.SetTargetProperty(fadeAnim, "Opacity");

        sb.Children.Add(moveAnim);
        sb.Children.Add(fadeAnim);
        sb.Completed += (_, _) =>
        {
            target.Opacity = 1.0;
            if (target.RenderTransform is TranslateTransform t)
            {
                t.X = 0;
            }
        };
        sb.Begin();
    }

    private void RenderPreview()
    {
        if (_previewPanel == null) return;
        _previewPanel.Children.Clear();

        var md = _editor != null ? _editor.Text : (Value ?? string.Empty);
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);
        var surface = RizVisuals.Surface(dark);
        var borderBrush = RizVisuals.Brush(RizVisuals.Border(dark));

        if (string.IsNullOrWhiteSpace(md))
        {
            _previewPanel.Children.Add(new TextBlock
            {
                Text = Placeholder ?? "（无内容）",
                FontSize = 13,
                Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark)),
                FontStyle = FontStyle.Italic
            });
            return;
        }

        // 统一规范换行符并保留空行判断
        var lines = (md.Replace("\r\n", "\n").Replace('\r', '\n') + "\n").Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            var rawLine = lines[i];
            var trimmed = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(trimmed)) continue;

            // 1. 代码块 ```
            if (trimmed.StartsWith("```"))
            {
                var lang = trimmed.Length > 3 ? trimmed[3..].Trim() : "text";
                var codeLines = new List<string>();
                i++;
                while (i < lines.Length && !lines[i].TrimStart().StartsWith("```"))
                {
                    codeLines.Add(lines[i]);
                    i++;
                }
                var codeBlock = new RizCode
                {
                    Block = true,
                    Code = string.Join("\n", codeLines),
                    Lang = lang,
                    Margin = new Thickness(0, 4, 0, 8),
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                _previewPanel.Children.Add(codeBlock);
                continue;
            }

            // 2. 表格：含 | 且下一行或下两行包含表格分隔行 | --- |
            if (trimmed.Contains('|'))
            {
                // 检查下一行是否是分隔行
                if (i + 1 < lines.Length && Regex.IsMatch(lines[i + 1].Trim(), @"^\|?\s*[-:]+[-| :]*$"))
                {
                    var tableLines = new List<string> { trimmed };
                    i++; // 跳过当前行
                    while (i < lines.Length && lines[i].Trim().Contains('|'))
                    {
                        tableLines.Add(lines[i].Trim());
                        i++;
                    }
                    i--; // 回退一行以抵消循环 i++

                    var tableUi = BuildTableUi(tableLines, dark, accent);
                    if (tableUi != null)
                    {
                        _previewPanel.Children.Add(tableUi);
                        continue;
                    }
                }
            }

            // 3. 标题 (# 到 ######)
            var hMatch = Regex.Match(trimmed, @"^(#{1,6})\s+(.*)$");
            if (hMatch.Success)
            {
                int level = hMatch.Groups[1].Length;
                string titleText = hMatch.Groups[2].Value;

                var titlePanel = new StackPanel { Spacing = 4, Margin = new Thickness(0, level == 1 ? 16 : level == 2 ? 12 : 8, 0, 4) };
                double fontSize = level switch
                {
                    1 => 22,
                    2 => 18,
                    3 => 16,
                    4 => 14,
                    _ => 13
                };
                var tb = new TextBlock
                {
                    FontSize = fontSize,
                    FontWeight = FontWeights.Bold,
                    Foreground = RizVisuals.Brush(RizVisuals.TitleText(dark)),
                    TextWrapping = TextWrapping.Wrap
                };
                ParseInlines(tb, titleText, dark, accent);
                titlePanel.Children.Add(tb);

                // H1 与 H2 底部附带浅色分割线
                if (level <= 2)
                {
                    titlePanel.Children.Add(new Border
                    {
                        Height = 1,
                        Background = borderBrush,
                        Margin = new Thickness(0, 4, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    });
                }

                _previewPanel.Children.Add(titlePanel);
                continue;
            }

            // 4. 引用块 (聚合连续以 > 开头的行)
            if (trimmed.StartsWith(">"))
            {
                var quoteLines = new List<string>();
                while (i < lines.Length && lines[i].Trim().StartsWith(">"))
                {
                    quoteLines.Add(Regex.Replace(lines[i].Trim(), @"^>\s?", ""));
                    i++;
                }
                i--; // 回退一行以抵消外部循环自增

                var quoteBorder = new Border
                {
                    BorderThickness = new Thickness(3.5, 0, 0, 0),
                    BorderBrush = RizVisuals.Brush(accent),
                    CornerRadius = new CornerRadius(0, 8, 8, 0),
                    Padding = new Thickness(14, 8, 14, 8),
                    Margin = new Thickness(0, 4, 0, 8),
                    Background = RizVisuals.Brush(RizVisuals.Mix(accent, surface, 0.08)),
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                var quotePanel = new StackPanel { Spacing = 4 };
                foreach (var ql in quoteLines)
                {
                    var tb = new TextBlock
                    {
                        FontSize = 13,
                        LineHeight = 22,
                        Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark)),
                        TextWrapping = TextWrapping.Wrap
                    };
                    ParseInlines(tb, ql, dark, accent);
                    quotePanel.Children.Add(tb);
                }
                quoteBorder.Child = quotePanel;
                _previewPanel.Children.Add(quoteBorder);
                continue;
            }

            // 5. 分割线 ---, ***, ___
            if (Regex.IsMatch(trimmed, @"^(-{3,}|\*{3,}|_{3,})$"))
            {
                var hr = new Border
                {
                    Height = 1,
                    Margin = new Thickness(0, 10, 0, 10),
                    Background = borderBrush,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                _previewPanel.Children.Add(hr);
                continue;
            }

            // 6. 任务清单 - [ ] / - [x]
            var taskMatch = Regex.Match(trimmed, @"^[-*+]\s+\[([ xX])\]\s+(.*)$");
            if (taskMatch.Success)
            {
                var isChecked = taskMatch.Groups[1].Value.Equals("x", StringComparison.OrdinalIgnoreCase);
                var textContent = taskMatch.Groups[2].Value;

                var itemPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 3, 0, 3) };
                var cb = new RizCheckbox { IsChecked = isChecked, IsEnabled = false, VerticalAlignment = VerticalAlignment.Center };
                var tb = new TextBlock
                {
                    FontSize = 13,
                    LineHeight = 20,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = RizVisuals.Brush(isChecked ? RizVisuals.Subtext(dark) : RizVisuals.Text(dark)),
                    TextWrapping = TextWrapping.Wrap
                };
                ParseInlines(tb, textContent, dark, accent);

                itemPanel.Children.Add(cb);
                itemPanel.Children.Add(tb);
                _previewPanel.Children.Add(itemPanel);
                continue;
            }

            // 7. 无序列表 - / * / +
            var ulMatch = Regex.Match(trimmed, @"^[-*+]\s+(.*)$");
            if (ulMatch.Success)
            {
                var itemPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(8, 2, 0, 2) };
                var dot = new Border
                {
                    Width = 5,
                    Height = 5,
                    CornerRadius = new CornerRadius(2.5),
                    Background = RizVisuals.Brush(accent),
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 8, 0, 0)
                };
                var tb = new TextBlock
                {
                    FontSize = 13,
                    LineHeight = 20,
                    Foreground = RizVisuals.Brush(RizVisuals.Text(dark)),
                    TextWrapping = TextWrapping.Wrap
                };
                ParseInlines(tb, ulMatch.Groups[1].Value, dark, accent);

                itemPanel.Children.Add(dot);
                itemPanel.Children.Add(tb);
                _previewPanel.Children.Add(itemPanel);
                continue;
            }

            // 8. 有序列表 1. 2.
            var olMatch = Regex.Match(trimmed, @"^(\d+)\.\s+(.*)$");
            if (olMatch.Success)
            {
                var num = olMatch.Groups[1].Value;
                var textContent = olMatch.Groups[2].Value;

                var itemPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6, Margin = new Thickness(8, 2, 0, 2) };
                var numTb = new TextBlock
                {
                    Text = $"{num}.",
                    FontSize = 13,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = RizVisuals.Brush(accent),
                    VerticalAlignment = VerticalAlignment.Top
                };
                var tb = new TextBlock
                {
                    FontSize = 13,
                    LineHeight = 20,
                    Foreground = RizVisuals.Brush(RizVisuals.Text(dark)),
                    TextWrapping = TextWrapping.Wrap
                };
                ParseInlines(tb, textContent, dark, accent);

                itemPanel.Children.Add(numTb);
                itemPanel.Children.Add(tb);
                _previewPanel.Children.Add(itemPanel);
                continue;
            }

            // 9. 普通段落
            var pTb = new TextBlock
            {
                FontSize = 13,
                LineHeight = 22,
                Foreground = RizVisuals.Brush(RizVisuals.Text(dark)),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 2, 0, 4)
            };
            ParseInlines(pTb, trimmed, dark, accent);
            _previewPanel.Children.Add(pTb);
        }
    }

    private static UIElement? BuildTableUi(List<string> lines, bool dark, Windows.UI.Color accent)
    {
        if (lines.Count < 1) return null;

        static string[] SplitCols(string row)
        {
            var clean = row.Trim();
            if (clean.StartsWith('|')) clean = clean[1..];
            if (clean.EndsWith('|')) clean = clean[..^1];
            return clean.Split('|').Select(s => s.Trim()).ToArray();
        }

        var headerCols = SplitCols(lines[0]);
        if (headerCols.Length == 0) return null;

        var rowLines = lines.Skip(1).ToList();

        var outerBorder = new Border
        {
            Margin = new Thickness(0, 6, 0, 10),
            CornerRadius = new CornerRadius(8),
            BorderThickness = new Thickness(1),
            BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark)),
            Background = RizVisuals.Brush(RizVisuals.Surface(dark)),
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        var tableGrid = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        foreach (var _ in headerCols)
        {
            tableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        tableGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Header
        for (int r = 0; r < rowLines.Count; r++)
        {
            tableGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }

        // 表头底色
        var headerBg = new Border
        {
            Background = RizVisuals.Brush(RizVisuals.Mix(accent, RizVisuals.Surface(dark), 0.10)),
            BorderThickness = new Thickness(0, 0, 0, 1),
            BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark)),
            CornerRadius = new CornerRadius(8, 8, 0, 0)
        };
        Grid.SetRow(headerBg, 0);
        Grid.SetColumnSpan(headerBg, headerCols.Length);
        tableGrid.Children.Add(headerBg);

        for (int c = 0; c < headerCols.Length; c++)
        {
            var th = new TextBlock
            {
                Text = headerCols[c],
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                Margin = new Thickness(12, 8, 12, 8),
                Foreground = RizVisuals.Brush(RizVisuals.TitleText(dark)),
                TextWrapping = TextWrapping.Wrap
            };
            Grid.SetRow(th, 0);
            Grid.SetColumn(th, c);
            tableGrid.Children.Add(th);
        }

        for (int r = 0; r < rowLines.Count; r++)
        {
            var cols = SplitCols(rowLines[r]);
            var rowBorder = new Border
            {
                BorderThickness = new Thickness(0, 0, 0, r == rowLines.Count - 1 ? 0 : 1),
                BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark))
            };
            Grid.SetRow(rowBorder, r + 1);
            Grid.SetColumnSpan(rowBorder, headerCols.Length);
            tableGrid.Children.Add(rowBorder);

            for (int c = 0; c < Math.Min(cols.Length, headerCols.Length); c++)
            {
                var td = new TextBlock
                {
                    FontSize = 12,
                    Margin = new Thickness(12, 7, 12, 7),
                    Foreground = RizVisuals.Brush(RizVisuals.Text(dark)),
                    TextWrapping = TextWrapping.Wrap
                };
                ParseInlines(td, cols[c], dark, accent);
                Grid.SetRow(td, r + 1);
                Grid.SetColumn(td, c);
                tableGrid.Children.Add(td);
            }
        }

        outerBorder.Child = tableGrid;
        return outerBorder;
    }

    private static void ParseInlines(TextBlock target, string text, bool dark, Windows.UI.Color accent)
    {
        target.Inlines.Clear();
        if (string.IsNullOrEmpty(text)) return;

        // 正则捕获：行内代码 `code`、粗体 **bold**、斜体 *italic*、删除线 ~~strike~~、超链接 [text](url)
        var regex = new Regex(@"(`[^`]+`)|(\*\*[^*]+\*\*)|(\*[^*\s][^*]*\*)|(~~[^~]+~~)|(\[[^\]]+\]\([^)]+\))");
        int lastIndex = 0;

        foreach (Match m in regex.Matches(text))
        {
            if (m.Index > lastIndex)
            {
                target.Inlines.Add(new Run { Text = text[lastIndex..m.Index] });
            }

            var val = m.Value;
            if (val.StartsWith('`') && val.EndsWith('`') && val.Length >= 2)
            {
                // 行内代码：洋红/深粉色 Cascadia Code，与 Vue 端的 #c7254e 风格一致
                target.Inlines.Add(new Run
                {
                    Text = " " + val[1..^1] + " ",
                    FontFamily = new FontFamily("Cascadia Code, Consolas, monospace"),
                    Foreground = RizVisuals.Brush(dark ? Windows.UI.Color.FromArgb(255, 244, 114, 182) : Windows.UI.Color.FromArgb(255, 199, 37, 78))
                });
            }
            else if (val.StartsWith("**") && val.EndsWith("**") && val.Length >= 4)
            {
                target.Inlines.Add(new Run
                {
                    Text = val[2..^2],
                    FontWeight = FontWeights.Bold
                });
            }
            else if (val.StartsWith("~~") && val.EndsWith("~~") && val.Length >= 4)
            {
                target.Inlines.Add(new Run
                {
                    Text = val[2..^2],
                    TextDecorations = TextDecorations.Strikethrough
                });
            }
            else if (val.StartsWith('*') && val.EndsWith('*') && val.Length >= 2)
            {
                target.Inlines.Add(new Run
                {
                    Text = val[1..^1],
                    FontStyle = FontStyle.Italic
                });
            }
            else if (val.StartsWith('[') && val.Contains("](") && val.EndsWith(')'))
            {
                var split = val.IndexOf("](");
                var linkText = val[1..split];
                target.Inlines.Add(new Run
                {
                    Text = linkText,
                    Foreground = RizVisuals.Brush(accent),
                    TextDecorations = TextDecorations.Underline
                });
            }
            else
            {
                target.Inlines.Add(new Run { Text = val });
            }

            lastIndex = m.Index + m.Length;
        }

        if (lastIndex < text.Length)
        {
            target.Inlines.Add(new Run { Text = text[lastIndex..] });
        }
    }

    private void UpdateVisual()
    {
        if (!_templateApplied) return;
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);
        var surface = RizVisuals.Surface(dark);
        var border = RizVisuals.Border(dark);

        if (_root != null)
        {
            _root.Background = RizVisuals.Brush(surface);
            _root.BorderBrush = RizVisuals.Brush(border);
        }

        // 高度自适应：如果为 "auto" 或空，则撑开高度并在外部滚动；如果为固定高度则内部滚动
        var isAutoHeight = string.IsNullOrWhiteSpace(HeightText) || string.Equals(HeightText, "auto", StringComparison.OrdinalIgnoreCase);
        if (isAutoHeight)
        {
            ClearValue(HeightProperty);
            MinHeight = 220;
            if (_root != null)
            {
                _root.ClearValue(HeightProperty);
                _root.MinHeight = 220;
            }
            if (_bodyScrollViewer != null)
            {
                _bodyScrollViewer.ClearValue(HeightProperty);
                _bodyScrollViewer.MinHeight = 220;
                _bodyScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
        }
        else
        {
            var h = RizVisuals.ParsePixels(HeightText, 340);
            Height = h;
            MinHeight = h;
            if (_root != null)
            {
                _root.Height = h;
                _root.MinHeight = h;
            }
            if (_bodyScrollViewer != null)
            {
                _bodyScrollViewer.Height = h;
                _bodyScrollViewer.MinHeight = h;
                _bodyScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
        }

        if (_focusGlow != null)
        {
            _focusGlow.BorderBrush = RizVisuals.Brush(accent);
        }

        // 浮动悬浮顶栏：半透明高斯玻璃胶囊 (固定圆角 18px，保证任何宽度下均为标准胶囊形态，绝非压扁椭圆)
        if (_toolbar != null)
        {
            var glassBg = dark ? Windows.UI.Color.FromArgb(220, 26, 32, 42) : Windows.UI.Color.FromArgb(230, 255, 255, 255);
            _toolbar.Background = RizVisuals.Brush(glassBg);
            _toolbar.BorderBrush = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), surface, 0.14));
        }

        if (_tabsWrapper != null)
        {
            _tabsWrapper.Background = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), surface, 0.08));
        }

        if (_tabSlider != null)
        {
            _tabSlider.Background = RizVisuals.Brush(surface);
        }

        if (_editor != null)
        {
            _editor.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
            _editor.PlaceholderText = Placeholder ?? string.Empty;
        }

        UpdateTabMode(false);
        Opacity = IsEnabled ? 1.0 : 0.45;
    }
}

