using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
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
    private const double ExpandedWidth = 236;
    private const double CollapsedWidth = 60;

    private Grid? _root;
    private Border? _nav;
    private TextBlock? _brandIcon;
    private TextBlock? _title;
    private Button? _collapse;
    private StackPanel? _itemsHost;
    private ContentPresenter? _contentHost;
    private ColumnDefinition? _navColumn;
    private bool _templateReady;
    private bool _animatingCollapse;

    private readonly Dictionary<string, (Border Row, TextBlock? Text, TextBlock? Icon, RizSliderNavItem Item)> _itemViews = new(StringComparer.Ordinal);

    public RizSliderNav()
    {
        DefaultStyleKey = typeof(RizSliderNav);
        RegisterPropertyChangedCallback(ContentProperty, (_, _) =>
        {
            if (_contentHost != null && !ReferenceEquals(_contentHost.Content, Content))
            {
                _contentHost.Content = Content;
                AnimateContentTransition();
            }
        });
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
        if (_collapse != null) _collapse.Click += (_, _) => { if (IsCollapsible) AnimateCollapse(); };
        _templateReady = true;
        if (string.IsNullOrWhiteSpace(SelectedItem) && !string.IsNullOrWhiteSpace(DefaultSelectedItem))
            SetValue(SelectedItemProperty, DefaultSelectedItem);
        ApplyLayout();
        RebuildItems();
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizSliderNav)d).ApplyLayout();
    private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizSliderNav)d).RebuildItems();
    private static void OnSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var nav = (RizSliderNav)d;
        nav.UpdateSelectionVisuals((string?)e.OldValue ?? string.Empty, (string?)e.NewValue ?? string.Empty);
    }
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
        var width = IsCollapsed && IsCollapsible ? CollapsedWidth : ExpandedWidth;
        var surface = RizVisuals.Surface(dark);

        // 视觉对齐 Vue Demo：白色圆角浮层卡片 + 1px 边框 + 四周留白；
        // Floating 时 Content 跨两列绘制，Nav 在其后声明故位于内容之上。
        _navColumn.Width = new GridLength(width + 16);
        _nav.Width = width;
        _nav.Margin = new Thickness(16, 16, 0, 16);
        _nav.CornerRadius = new CornerRadius(14);
        _nav.Background = RizVisuals.Brush(surface);
        _nav.BorderThickness = new Thickness(1);
        _nav.BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark));
        Grid.SetColumn(_nav, 0);
        Grid.SetColumn(_contentHost, floating ? 0 : 1);
        Grid.SetColumnSpan(_contentHost, floating ? 2 : 1);

        if (!ReferenceEquals(_contentHost.Content, Content))
        {
            _contentHost.Content = Content;
            AnimateContentTransition();
        }

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

        var headerGrid = GetTemplateChild("HeaderGrid") as Grid;
        if (headerGrid != null && _collapse != null && _brandIcon != null && _title != null)
        {
            if (IsCollapsed)
            {
                headerGrid.Padding = new Thickness(0, 14, 0, 6);
                _title.Visibility = Visibility.Collapsed;
                _brandIcon.Visibility = Visibility.Collapsed;
                _collapse.SetValue(Grid.ColumnSpanProperty, 3);
                _collapse.SetValue(Grid.ColumnProperty, 0);
                _collapse.HorizontalAlignment = HorizontalAlignment.Center;
                _collapse.Width = 36;
                _collapse.Height = 36;
                _collapse.Content = "›";
                _collapse.CornerRadius = new CornerRadius(8);
            }
            else
            {
                headerGrid.Padding = new Thickness(14, 16, 12, 12);
                _title.Visibility = Visibility.Visible;
                _brandIcon.Visibility = Visibility.Visible;
                _collapse.SetValue(Grid.ColumnSpanProperty, 1);
                _collapse.SetValue(Grid.ColumnProperty, 2);
                _collapse.HorizontalAlignment = HorizontalAlignment.Right;
                _collapse.Width = 28;
                _collapse.Height = 28;
                _collapse.Content = "‹";
                _collapse.CornerRadius = new CornerRadius(6);
            }
            _collapse.Visibility = IsCollapsible ? Visibility.Visible : Visibility.Collapsed;
            _collapse.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
            _collapse.Background = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), surface, 0.06));
        }
        RebuildItems();
    }

    private void RebuildItems()
    {
        if (!_templateReady || _itemsHost == null) return;
        _itemsHost.Children.Clear();
        _itemViews.Clear();
        if (Items is null) return;
        foreach (var item in Items) AddItem(item, 0, _itemsHost);
    }

    private static bool ContainsKey(RizSliderNavItem parent, string targetKey)
    {
        if (string.IsNullOrEmpty(targetKey)) return false;
        foreach (var child in parent.Children)
        {
            if (string.Equals(child.Key, targetKey, StringComparison.Ordinal)) return true;
            if (child.Children.Count > 0 && ContainsKey(child, targetKey)) return true;
        }
        return false;
    }

    private void UpdateSelectionVisuals(string oldKey, string newKey)
    {
        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(null);
        var surface = RizVisuals.Surface(dark);
        var activeBg = RizVisuals.Mix(accent, surface, 0.12);
        var defaultText = RizVisuals.Text(dark);
        var defaultIcon = RizVisuals.Icon(dark);

        foreach (var (key, (row, text, icon, item)) in _itemViews)
        {
            var hasChildren = item.Children.Count > 0;
            if (hasChildren)
            {
                // 父级栏目：只有当当前选中的子栏目位于其子树中时才变蓝，其他一律为灰色
                var isParentOfSelected = ContainsKey(item, newKey);
                if (text != null)
                {
                    text.Foreground = RizVisuals.Brush(isParentOfSelected ? accent : defaultText);
                    text.FontWeight = FontWeights.SemiBold;
                }
                if (icon != null)
                {
                    icon.Foreground = RizVisuals.Brush(isParentOfSelected ? accent : defaultIcon);
                }
                row.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            }
            else
            {
                // 叶子栏目：仅当自己被选中时才变蓝并带浅蓝背景块
                var isSelected = string.Equals(key, newKey, StringComparison.Ordinal);
                var wasSelected = string.Equals(key, oldKey, StringComparison.Ordinal);

                if (isSelected)
                {
                    AnimateRowBackground(row, Microsoft.UI.Colors.Transparent, activeBg, 150);
                    if (text != null)
                    {
                        text.Foreground = RizVisuals.Brush(accent);
                        text.FontWeight = FontWeights.SemiBold;
                    }
                    if (icon != null)
                    {
                        icon.Foreground = RizVisuals.Brush(accent);
                    }
                }
                else if (wasSelected)
                {
                    AnimateRowBackground(row, activeBg, Microsoft.UI.Colors.Transparent, 150);
                    if (text != null)
                    {
                        text.Foreground = RizVisuals.Brush(defaultText);
                        text.FontWeight = FontWeights.Normal;
                    }
                    if (icon != null)
                    {
                        icon.Foreground = RizVisuals.Brush(defaultIcon);
                    }
                }
                else
                {
                    row.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
                    if (text != null)
                    {
                        text.Foreground = RizVisuals.Brush(defaultText);
                        text.FontWeight = FontWeights.Normal;
                    }
                    if (icon != null)
                    {
                        icon.Foreground = RizVisuals.Brush(defaultIcon);
                    }
                }
            }
        }
    }

    private static void AnimateRowBackground(Border row, Windows.UI.Color from, Windows.UI.Color to, double durationMs)
    {
        var brush = new SolidColorBrush(from);
        row.Background = brush;
        var anim = new ColorAnimation
        {
            From = from,
            To = to,
            Duration = new Duration(TimeSpan.FromMilliseconds(durationMs)),
            EnableDependentAnimation = true,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(anim, brush);
        Storyboard.SetTargetProperty(anim, "Color");
        var sb = new Storyboard();
        sb.Children.Add(anim);
        sb.Completed += (_, _) =>
        {
            row.Background = new SolidColorBrush(to);
        };
        sb.Begin();
    }

    private void AddItem(RizSliderNavItem item, int depth, Panel parent)
    {
        var dark = RizVisuals.IsDark(this, Theme);
        var selected = !string.IsNullOrEmpty(item.Key) && string.Equals(item.Key, SelectedItem, StringComparison.Ordinal);
        var hasChildren = item.Children.Count > 0;
        var accent = RizVisuals.ThemeColor(null);
        var surface = RizVisuals.Surface(dark);

        // 高亮规则：
        // 1. 无子栏目时，自身被选中时才变蓝；
        // 2. 有子栏目时，旗下包含被选中的子栏目才变蓝，其他一律为灰色。
        var isParentActive = hasChildren && ContainsKey(item, SelectedItem);
        var isLeafSelected = !hasChildren && selected;
        var shouldHighlight = isParentActive || isLeafSelected;

        var row = new Border
        {
            Height = hasChildren ? 38 : 36,
            Margin = IsCollapsed ? new Thickness(6, 2, 6, 2) : new Thickness(8, hasChildren ? 6 : 2, 8, hasChildren ? 2 : 2),
            Padding = new Thickness(IsCollapsed ? 0 : 12 + depth * 12, 0, IsCollapsed ? 0 : 10, 0),
            CornerRadius = new CornerRadius(9),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = RizVisuals.Brush(isLeafSelected
                ? RizVisuals.Mix(accent, surface, 0.12)
                : Microsoft.UI.Colors.Transparent)
        };
        var grid = new Grid
        {
            ColumnSpacing = IsCollapsed ? 0 : 10,
            HorizontalAlignment = IsCollapsed ? HorizontalAlignment.Center : HorizontalAlignment.Stretch
        };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        // 图标（折叠模式居中对齐，展开模式空图标则不占位）
        var hasIcon = !string.IsNullOrWhiteSpace(item.Icon);
        TextBlock? icon = null;
        if (hasIcon)
        {
            icon = new TextBlock
            {
                Width = 24,
                FontSize = 15,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            ApplyIcon(icon, item.Icon, 15, shouldHighlight ? accent : RizVisuals.Icon(dark));
            Grid.SetColumn(icon, 0);
            grid.Children.Add(icon);
        }

        var text = new TextBlock
        {
            Text = item.Text ?? string.Empty,
            FontSize = hasChildren ? 14 : 13,
            FontWeight = (hasChildren || isLeafSelected) ? FontWeights.SemiBold : FontWeights.Normal,
            Foreground = RizVisuals.Brush(shouldHighlight ? accent : RizVisuals.Text(dark)),
            VerticalAlignment = VerticalAlignment.Center,
            Visibility = IsCollapsed ? Visibility.Collapsed : Visibility.Visible,
            TextTrimming = TextTrimming.CharacterEllipsis
        };
        Grid.SetColumn(text, hasIcon ? 1 : 0);
        Grid.SetColumnSpan(text, hasIcon ? 1 : 2);
        grid.Children.Add(text);

        // 旋转箭头：使用 16x16 正方形宿主并在正中央绘制标准 Chevron。
        // RenderTransformOrigin 设为严格正中心 (0.5, 0.5)，旋转时绝对原位自转，不会偏心位移。
        Grid? arrowHost = null;
        if (hasChildren)
        {
            arrowHost = new Grid
            {
                Width = 16,
                Height = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5),
                Visibility = !IsCollapsed ? Visibility.Visible : Visibility.Collapsed
            };
            var pg = new PathGeometry();
            var pf = new PathFigure { StartPoint = new Windows.Foundation.Point(0, 0), IsClosed = false };
            pf.Segments.Add(new LineSegment { Point = new Windows.Foundation.Point(4, 4) });
            pf.Segments.Add(new LineSegment { Point = new Windows.Foundation.Point(0, 8) });
            pg.Figures.Add(pf);

            var arrowPath = new Microsoft.UI.Xaml.Shapes.Path
            {
                Data = pg,
                Stroke = RizVisuals.Brush(isParentActive ? accent : RizVisuals.Subtext(dark)),
                StrokeThickness = 1.6,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            arrowHost.Children.Add(arrowPath);
            arrowHost.RenderTransform = new RotateTransform { Angle = item.IsExpanded ? 90 : 0 };
            Grid.SetColumn(arrowHost, 2);
            grid.Children.Add(arrowHost);
        }
        row.Child = grid;
        row.PointerEntered += (_, _) =>
        {
            if (!hasChildren && !string.Equals(item.Key, SelectedItem, StringComparison.Ordinal))
            {
                row.Background = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Text(dark), surface, 0.05));
            }
        };
        row.PointerExited += (_, _) =>
        {
            if (!hasChildren && !string.Equals(item.Key, SelectedItem, StringComparison.Ordinal))
            {
                row.Background = RizVisuals.Brush(Microsoft.UI.Colors.Transparent);
            }
        };
        parent.Children.Add(row);

        if (!string.IsNullOrEmpty(item.Key))
        {
            _itemViews[item.Key] = (row, text, icon, item);
        }

        if (!hasChildren)
        {
            row.PointerPressed += (_, e) => { SelectItem(item); e.Handled = true; };
            return;
        }

        // 外层剪裁容器：动画 Height 与 Opacity，平滑撑开/收缩空间，下方的分类平滑被推开/回升。
        var animHost = new Grid
        {
            Visibility = (item.IsExpanded && !IsCollapsed) ? Visibility.Visible : Visibility.Collapsed,
            Height = (item.IsExpanded && !IsCollapsed) ? double.NaN : 0,
            Opacity = (item.IsExpanded && !IsCollapsed) ? 1 : 0
        };
        var childHost = new StackPanel();
        animHost.Children.Add(childHost);
        parent.Children.Add(animHost);

        foreach (var child in item.Children) AddItem(child, depth + 1, childHost);

        row.PointerPressed += (_, e) =>
        {
            if (IsCollapsed)
            {
                SelectItem(item);
                e.Handled = true;
                return;
            }
            item.IsExpanded = !item.IsExpanded;
            AnimateChildren(animHost, childHost, item.IsExpanded);
            if (arrowHost != null) AnimateArrow(arrowHost, item.IsExpanded);
            e.Handled = true;
        };
    }

    private static void AnimateArrow(UIElement arrowHost, bool expand)
    {
        arrowHost.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
        if (arrowHost.RenderTransform is not RotateTransform transform)
        {
            transform = new RotateTransform();
            arrowHost.RenderTransform = transform;
        }
        var storyboard = new Storyboard();
        var animation = new DoubleAnimation
        {
            To = expand ? 90 : 0,
            Duration = new Duration(TimeSpan.FromMilliseconds(220)),
            EnableDependentAnimation = true,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(animation, transform);
        Storyboard.SetTargetProperty(animation, "Angle");
        storyboard.Children.Add(animation);
        storyboard.Begin();
    }

    private void AnimateChildren(Grid animHost, StackPanel childHost, bool expand)
    {
        var duration = new Duration(TimeSpan.FromMilliseconds(240));
        var storyboard = new Storyboard();

        if (expand)
        {
            animHost.Visibility = Visibility.Visible;
            childHost.Measure(new Windows.Foundation.Size(ExpandedWidth, double.PositiveInfinity));
            var targetHeight = childHost.DesiredSize.Height;
            if (targetHeight <= 0) targetHeight = Math.Max(38, childHost.Children.Count * 38);

            var fromHeight = animHost.ActualHeight > 0 && animHost.ActualHeight < targetHeight ? animHost.ActualHeight : 0;
            animHost.Height = fromHeight;

            var heightAnim = new DoubleAnimation
            {
                From = fromHeight,
                To = targetHeight,
                Duration = duration,
                EnableDependentAnimation = true,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTarget(heightAnim, animHost);
            Storyboard.SetTargetProperty(heightAnim, "Height");
            storyboard.Children.Add(heightAnim);

            var opacityAnim = new DoubleAnimation
            {
                From = animHost.Opacity,
                To = 1,
                Duration = duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTarget(opacityAnim, animHost);
            Storyboard.SetTargetProperty(opacityAnim, "Opacity");
            storyboard.Children.Add(opacityAnim);

            storyboard.Completed += (_, _) =>
            {
                // 动画完成恢复自适应高度，避免内部项变化时受限
                animHost.Height = double.NaN;
            };
        }
        else
        {
            var currentHeight = animHost.ActualHeight > 0 ? animHost.ActualHeight : childHost.ActualHeight;
            if (currentHeight <= 0) currentHeight = Math.Max(38, childHost.Children.Count * 38);

            var heightAnim = new DoubleAnimation
            {
                From = currentHeight,
                To = 0,
                Duration = duration,
                EnableDependentAnimation = true,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(heightAnim, animHost);
            Storyboard.SetTargetProperty(heightAnim, "Height");
            storyboard.Children.Add(heightAnim);

            var opacityAnim = new DoubleAnimation
            {
                From = animHost.Opacity,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(180)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            Storyboard.SetTarget(opacityAnim, animHost);
            Storyboard.SetTargetProperty(opacityAnim, "Opacity");
            storyboard.Children.Add(opacityAnim);

            storyboard.Completed += (_, _) =>
            {
                animHost.Visibility = Visibility.Collapsed;
                animHost.Height = 0;
            };
        }
        storyboard.Begin();
    }

    private void AnimateContentTransition()
    {
        if (_contentHost == null) return;
        var transform = _contentHost.RenderTransform as CompositeTransform ?? new CompositeTransform();
        _contentHost.RenderTransform = transform;

        var duration = new Duration(TimeSpan.FromMilliseconds(220));
        var opacityAnim = new DoubleAnimation
        {
            From = 0.4,
            To = 1.0,
            Duration = duration,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(opacityAnim, _contentHost);
        Storyboard.SetTargetProperty(opacityAnim, "Opacity");

        var translateAnim = new DoubleAnimation
        {
            From = 14,
            To = 0,
            Duration = duration,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(translateAnim, transform);
        Storyboard.SetTargetProperty(translateAnim, "TranslateY");

        var sb = new Storyboard();
        sb.Children.Add(opacityAnim);
        sb.Children.Add(translateAnim);
        sb.Begin();
    }

    private void AnimateCollapse()
    {
        if (_animatingCollapse || _nav == null) return;
        _animatingCollapse = true;
        var target = IsCollapsed ? ExpandedWidth : CollapsedWidth;
        var storyboard = new Storyboard();
        var animation = new DoubleAnimation
        {
            To = target,
            Duration = new Duration(TimeSpan.FromMilliseconds(220)),
            EnableDependentAnimation = true,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
        };
        Storyboard.SetTarget(animation, _nav);
        Storyboard.SetTargetProperty(animation, "Width");
        storyboard.Completed += (_, _) =>
        {
            SetValue(IsCollapsedProperty, !IsCollapsed);
            _animatingCollapse = false;
        };
        storyboard.Begin();
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