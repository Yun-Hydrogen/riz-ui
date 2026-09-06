using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Text;

namespace RizUI.Uwp.Controls;

/// <summary>侧边栏展示样式。</summary>
public enum RizSliderNavStyle
{
    /// <summary>左侧占据布局列，默认内容显示在右侧。</summary>
    Fixed,
    /// <summary>左侧悬浮于内容之上，默认内容填满整个窗口。</summary>
    Floating
}

/// <summary>
/// 原生左侧多级侧边栏。Content 是页面内容；Items 是可无限嵌套的栏目树。
/// 支持 Font Awesome Free 的完整 class 组合、固定/悬浮布局及折叠状态。
/// </summary>
public sealed class RizSliderNav : ContentControl
{
    private Grid? _root;
    private Border? _nav;
    private TextBlock? _brandIcon;
    private TextBlock? _title;
    private Button? _collapse;
    private StackPanel? _itemsHost;
    private ContentPresenter? _contentHost;
    private ColumnDefinition? _navColumn;
    private bool _templateReady;

    public RizSliderNav()
    {
        DefaultStyleKey = typeof(RizSliderNav);
        RegisterPropertyChangedCallback(ContentProperty, (_, _) => ApplyLayout());
    }

    public static readonly DependencyProperty IsCollapsibleProperty =
        DependencyProperty.Register(nameof(IsCollapsible), typeof(bool), typeof(RizSliderNav), new PropertyMetadata(true, OnVisualChanged));
    /// <summary>是否显示折叠按钮并允许折叠。</summary>
    public bool IsCollapsible { get => (bool)GetValue(IsCollapsibleProperty); set => SetValue(IsCollapsibleProperty, value); }

    public static readonly DependencyProperty IsCollapsedProperty =
        DependencyProperty.Register(nameof(IsCollapsed), typeof(bool), typeof(RizSliderNav), new PropertyMetadata(false, OnVisualChanged));
    /// <summary>当前折叠状态；可双向绑定。</summary>
    public bool IsCollapsed { get => (bool)GetValue(IsCollapsedProperty); set => SetValue(IsCollapsedProperty, value); }

    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register(nameof(Items), typeof(IList<RizSliderNavItem>), typeof(RizSliderNav), new PropertyMetadata(null, OnItemsChanged));
    /// <summary>侧边栏树状条目，支持任意层 Children。</summary>
    public IList<RizSliderNavItem>? Items { get => (IList<RizSliderNavItem>?)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }

    public static readonly DependencyProperty DefaultSelectedItemProperty =
        DependencyProperty.Register(nameof(DefaultSelectedItem), typeof(string), typeof(RizSliderNav), new PropertyMetadata(string.Empty, OnDefaultSelectionChanged));
    /// <summary>首次加载时默认选中的条目 Key。</summary>
    public string DefaultSelectedItem { get => (string)GetValue(DefaultSelectedItemProperty); set => SetValue(DefaultSelectedItemProperty, value); }

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem), typeof(string), typeof(RizSliderNav), new PropertyMetadata(string.Empty, OnSelectedChanged));
    /// <summary>当前选中条目 Key。</summary>
    public string SelectedItem { get => (string)GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }

    public static readonly DependencyProperty StyleModeProperty =
        DependencyProperty.Register(nameof(StyleMode), typeof(RizSliderNavStyle), typeof(RizSliderNav), new PropertyMetadata(RizSliderNavStyle.Fixed, OnVisualChanged));
    /// <summary>固定布局或悬浮布局。</summary>
    public RizSliderNavStyle StyleMode { get => (RizSliderNavStyle)GetValue(StyleModeProperty); set => SetValue(StyleModeProperty, value); }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(RizSliderNav), new PropertyMetadata("RizUI", OnVisualChanged));
    /// <summary>侧边栏顶端标题。</summary>
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(RizSliderNav), new PropertyMetadata("fa-solid fa-layer-group", OnVisualChanged));
    /// <summary>顶端图标，接受完整 Font Awesome class。</summary>
    public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizSliderNav), new PropertyMetadata("auto", OnVisualChanged));
    public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }

    public event EventHandler<RizSliderNavItem>? SelectionChanged;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _root = GetTemplateChild("Root") as Grid;
        _nav = GetTemplateChild("Nav") as Border;
        _brandIcon = GetTemplateChild("BrandIcon") as TextBlock;
        _title = GetTemplateChild("Title") as TextBlock;
        _collapse = GetTemplateChild("Collapse") as Button;
        _itemsHost = GetTemplateChild("ItemsHost") as StackPanel;
        _contentHost = GetTemplateChild("ContentHost") as ContentPresenter;
        _navColumn = GetTemplateChild("NavColumn") as ColumnDefinition;
        if (_collapse != null) _collapse.Click += (_, _) => { if (IsCollapsible) IsCollapsed = !IsCollapsed; };
        _templateReady = true;
        if (string.IsNullOrWhiteSpace(SelectedItem) && !string.IsNullOrWhiteSpace(DefaultSelectedItem))
            SetValue(SelectedItemProperty, DefaultSelectedItem);
        ApplyLayout();
        RebuildItems();
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizSliderNav)d).ApplyLayout();
    private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizSliderNav)d).RebuildItems();
    private static void OnSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizSliderNav)d).RebuildItems();
    private static void OnDefaultSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var nav = (RizSliderNav)d;
        if (string.IsNullOrWhiteSpace(nav.SelectedItem)) nav.SetValue(SelectedItemProperty, e.NewValue);
    }

    private void ApplyLayout()
    {
        if (!_templateReady || _root == null || _nav == null || _contentHost == null || _navColumn == null) return;
        var dark = RizVisuals.IsDark(this, Theme);
        var floating = StyleMode == RizSliderNavStyle.Floating;
        var width = IsCollapsed && IsCollapsible ? 68 : 248;

        // Floating 仍保留导航自身宽度；Content 跨两列绘制以填满整个区域，
        // 而 Nav 在其后声明，因此显示在内容之上。
        _navColumn.Width = new GridLength(width);
        _nav.Width = width;
        _nav.Margin = floating ? new Thickness(16) : new Thickness(0);
        _nav.CornerRadius = new CornerRadius(floating ? 16 : 0);
        _nav.Background = RizVisuals.Brush(RizVisuals.Surface(dark));
        _nav.BorderThickness = new Thickness(floating ? 1 : 0);
        _nav.BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark));
        // Nav 在模板中声明于 ContentHost 之后，Grid 绘制顺序天然保证 Floating 模式位于内容之上。
        Grid.SetColumn(_nav, 0);
        Grid.SetColumn(_contentHost, floating ? 0 : 1);
        Grid.SetColumnSpan(_contentHost, floating ? 2 : 1);
        _contentHost.Content = Content;

        if (_title != null)
        {
            _title.Text = Title ?? string.Empty;
            _title.Visibility = IsCollapsed ? Visibility.Collapsed : Visibility.Visible;
            _title.Foreground = RizVisuals.Brush(RizVisuals.Text(dark));
        }
        if (_brandIcon != null)
        {
            ApplyIcon(_brandIcon, Icon, 18, RizVisuals.ThemeColor(null));
        }
        if (_collapse != null)
        {
            _collapse.Visibility = IsCollapsible ? Visibility.Visible : Visibility.Collapsed;
            _collapse.Content = IsCollapsed ? "›" : "‹";
            _collapse.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
            _collapse.Background = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), RizVisuals.Surface(dark), 0.06));
        }
        RebuildItems();
    }

    private void RebuildItems()
    {
        if (!_templateReady || _itemsHost == null) return;
        _itemsHost.Children.Clear();
        if (Items is null) return;
        foreach (var item in Items) AddItem(item, 0, _itemsHost);
    }

    private void AddItem(RizSliderNavItem item, int depth, Panel parent)
    {
        var dark = RizVisuals.IsDark(this, Theme);
        var selected = !string.IsNullOrEmpty(item.Key) && string.Equals(item.Key, SelectedItem, StringComparison.Ordinal);
        var hasChildren = item.Children.Count > 0;
        var row = new Border
        {
            Height = 38,
            Margin = new Thickness(8, 2, 8, 2),
            Padding = new Thickness(IsCollapsed ? 0 : 10 + depth * 14, 0, 8, 0),
            CornerRadius = new CornerRadius(8),
            Background = RizVisuals.Brush(selected
                ? RizVisuals.Mix(RizVisuals.ThemeColor(null), RizVisuals.Surface(dark), 0.14)
                : Microsoft.UI.Colors.Transparent)
        };
        var grid = new Grid { ColumnSpacing = 10 };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        var icon = new TextBlock { Width = 24, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        ApplyIcon(icon, item.Icon, 16, selected ? RizVisuals.ThemeColor(null) : RizVisuals.Icon(dark));
        var text = new TextBlock
        {
            Text = item.Text ?? string.Empty,
            FontSize = 13,
            FontWeight = selected ? FontWeights.SemiBold : FontWeights.Normal,
            Foreground = RizVisuals.Brush(selected ? RizVisuals.ThemeColor(null) : RizVisuals.Text(dark)),
            VerticalAlignment = VerticalAlignment.Center,
            Visibility = IsCollapsed ? Visibility.Collapsed : Visibility.Visible,
            TextTrimming = TextTrimming.CharacterEllipsis
        };
        var arrow = new TextBlock
        {
            Text = item.IsExpanded ? "⌄" : "›",
            FontSize = 14,
            Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark)),
            VerticalAlignment = VerticalAlignment.Center,
            Visibility = hasChildren && !IsCollapsed ? Visibility.Visible : Visibility.Collapsed
        };
        Grid.SetColumn(icon, 0);
        Grid.SetColumn(text, 1);
        Grid.SetColumn(arrow, 2);
        grid.Children.Add(icon);
        grid.Children.Add(text);
        grid.Children.Add(arrow);
        row.Child = grid;
        row.PointerEntered += (_, _) => { if (!selected) row.Background = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), RizVisuals.Surface(dark), 0.06)); };
        row.PointerExited += (_, _) => { if (!selected) row.Background = RizVisuals.Brush(Microsoft.UI.Colors.Transparent); };
        row.PointerPressed += (_, e) =>
        {
            if (hasChildren && !IsCollapsed) item.IsExpanded = !item.IsExpanded;
            SelectItem(item);
            e.Handled = true;
        };
        parent.Children.Add(row);

        if (!hasChildren || IsCollapsed || !item.IsExpanded) return;
        var childHost = new StackPanel();
        foreach (var child in item.Children) AddItem(child, depth + 1, childHost);
        parent.Children.Add(childHost);
    }

    private void SelectItem(RizSliderNavItem item)
    {
        if (!string.IsNullOrWhiteSpace(item.Key)) SetValue(SelectedItemProperty, item.Key);
        if (item.Command?.CanExecute(item.CommandParameter) == true) item.Command.Execute(item.CommandParameter);
        SelectionChanged?.Invoke(this, item);
    }

    private static void ApplyIcon(TextBlock target, string? value, double size, Windows.UI.Color color)
    {
        var resolved = RizFontAwesome.Resolve(value);
        target.Text = resolved.Glyph;
        target.FontFamily = resolved.FontFamily;
        target.FontSize = size;
        target.Foreground = RizVisuals.Brush(color);
    }
}