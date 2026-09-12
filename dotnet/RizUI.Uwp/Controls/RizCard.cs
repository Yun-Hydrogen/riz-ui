using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Path = Microsoft.UI.Xaml.Shapes.Path;

namespace RizUI.Uwp.Controls;

/// <summary>原生实现的 rizui_card；<see cref="Content"/> 对应 Vue 默认插槽。</summary>
public sealed class RizCard : ContentControl
{
    private Border? _shadow1;
    private Border? _shadow2;
    private Border? _root;
    private TextBlock? _icon;
    private TextBlock? _title;
    private TextBlock? _desc;
    private ContentPresenter? _contentHost;
    private Grid? _header;
    private Border? _foldBtn;
    private Path? _foldArrow;
    private ScrollViewer? _foldHost;
    private Storyboard? _foldStoryboard;
    private bool _isFolded;

    public RizCard()
    {
        DefaultStyleKey = typeof(RizCard);
        ActualThemeChanged += (_, _) => ApplyVisual();
        RegisterPropertyChangedCallback(ContentProperty, (_, _) => ApplyVisual());
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(RizCard), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

    public static readonly DependencyProperty TitleStyleProperty =
        DependencyProperty.Register(nameof(TitleStyle), typeof(Style), typeof(RizCard), new PropertyMetadata(null, OnVisualChanged));
    public Style? TitleStyle { get => (Style?)GetValue(TitleStyleProperty); set => SetValue(TitleStyleProperty, value); }

    public static readonly DependencyProperty DescProperty =
        DependencyProperty.Register(nameof(Desc), typeof(string), typeof(RizCard), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Desc { get => (string)GetValue(DescProperty); set => SetValue(DescProperty, value); }

    public static readonly DependencyProperty DescStyleProperty =
        DependencyProperty.Register(nameof(DescStyle), typeof(Style), typeof(RizCard), new PropertyMetadata(null, OnVisualChanged));
    public Style? DescStyle { get => (Style?)GetValue(DescStyleProperty); set => SetValue(DescStyleProperty, value); }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(RizCard), new PropertyMetadata(string.Empty, OnVisualChanged));
    public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }

    public static readonly DependencyProperty IconStyleProperty =
        DependencyProperty.Register(nameof(IconStyle), typeof(Style), typeof(RizCard), new PropertyMetadata(null, OnVisualChanged));
    public Style? IconStyle { get => (Style?)GetValue(IconStyleProperty); set => SetValue(IconStyleProperty, value); }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizCard), new PropertyMetadata("auto", OnVisualChanged));
    public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }

    public static readonly DependencyProperty FoldableProperty =
        DependencyProperty.Register(nameof(Foldable), typeof(bool), typeof(RizCard), new PropertyMetadata(false, OnFoldableChanged));

    /// <summary>是否可折叠：为 True 时标题栏出现折叠箭头，点击标题栏可收起/展开内容区（带平滑高度动画）。</summary>
    public bool Foldable { get => (bool)GetValue(FoldableProperty); set => SetValue(FoldableProperty, value); }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _shadow1 = GetTemplateChild("Shadow1") as Border;
        _shadow2 = GetTemplateChild("Shadow2") as Border;
        _root = GetTemplateChild("Root") as Border;
        _icon = GetTemplateChild("Icon") as TextBlock;
        _title = GetTemplateChild("Title") as TextBlock;
        _desc = GetTemplateChild("Desc") as TextBlock;
        _contentHost = GetTemplateChild("ContentHost") as ContentPresenter;
        _header = GetTemplateChild("Header") as Grid;
        _foldBtn = GetTemplateChild("FoldBtn") as Border;
        _foldArrow = GetTemplateChild("FoldArrow") as Path;
        _foldHost = GetTemplateChild("FoldHost") as ScrollViewer;

        if (_header != null)
        {
            _header.PointerReleased += (_, e) =>
            {
                if (!Foldable) return;
                SetFolded(!_isFolded, animate: true);
                e.Handled = true;
            };
        }

        ApplyVisual();
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizCard)d).ApplyVisual();

    private static void OnFoldableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var card = (RizCard)d;
        if (!(bool)e.NewValue && card._isFolded)
        {
            card.SetFolded(false, animate: false);
        }
        card.ApplyVisual();
    }

    private void ApplyVisual()
    {
        var dark = RizVisuals.IsDark(this, Theme);
        if (_shadow1 != null)
            _shadow1.Background = RizVisuals.Brush(dark ? Windows.UI.Color.FromArgb(40, 0, 0, 0) : Windows.UI.Color.FromArgb(15, 30, 41, 59));
        if (_shadow2 != null)
            _shadow2.Background = RizVisuals.Brush(dark ? Windows.UI.Color.FromArgb(25, 0, 0, 0) : Windows.UI.Color.FromArgb(8, 30, 41, 59));
        if (_root != null)
        {
            _root.Background = RizVisuals.Brush(RizVisuals.SurfaceSoft(dark));
            _root.BorderBrush = RizVisuals.Brush(RizVisuals.Border(dark));
        }
        if (_icon != null)
        {
            if (IconStyle != null) _icon.Style = IconStyle;
            RizVisuals.ApplyIcon(_icon, Icon, 20, RizVisuals.Icon(dark));
        }
        if (_title != null)
        {
            if (TitleStyle != null) _title.Style = TitleStyle;
            _title.Text = Title ?? string.Empty;
            _title.Visibility = string.IsNullOrWhiteSpace(Title) ? Visibility.Collapsed : Visibility.Visible;
            if (TitleStyle == null)
            {
                _title.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
                _title.Foreground = RizVisuals.Brush(RizVisuals.TitleText(dark));
            }
        }
        if (_desc != null)
        {
            if (DescStyle != null) _desc.Style = DescStyle;
            _desc.Text = Desc ?? string.Empty;
            _desc.Visibility = string.IsNullOrWhiteSpace(Desc) ? Visibility.Collapsed : Visibility.Visible;
            if (DescStyle == null) _desc.Foreground = RizVisuals.Brush(RizVisuals.Subtext(dark));
        }
        if (_contentHost != null)
            _contentHost.Visibility = Content is null ? Visibility.Collapsed : Visibility.Visible;

        if (_foldBtn != null)
        {
            _foldBtn.Visibility = Foldable ? Visibility.Visible : Visibility.Collapsed;
        }
        if (_foldArrow != null)
        {
            _foldArrow.Stroke = RizVisuals.Brush(RizVisuals.Subtext(dark));
        }
    }

    /// <summary>折叠/展开内容区。高度动画由 ScrollViewer 裁剪超出的内容保证不溢出，透明度同步淡入淡出。</summary>
    private void SetFolded(bool folded, bool animate)
    {
        if (_foldHost == null) return;
        _isFolded = folded;

        // 先停掉上一次折叠动画，避免相互覆盖
        if (_foldStoryboard != null)
        {
            _foldStoryboard.Stop();
            _foldStoryboard = null;
        }

        var host = _foldHost;

        // 箭头旋转：展开 180°（朝上）/ 折叠 0°（朝下）——纯渲染动画，不影响布局
        if (_foldArrow?.RenderTransform is RotateTransform rotate)
        {
            if (animate)
            {
                var rotateAnimation = new DoubleAnimation
                {
                    To = folded ? 0 : 180,
                    Duration = new Duration(TimeSpan.FromMilliseconds(240)),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(rotateAnimation, rotate);
                Storyboard.SetTargetProperty(rotateAnimation, "Angle");
                var rotateStoryboard = new Storyboard();
                rotateStoryboard.Children.Add(rotateAnimation);
                rotateStoryboard.Begin();
            }
            else
            {
                rotate.Angle = folded ? 0 : 180;
            }
        }

        if (!animate)
        {
            host.Height = double.NaN;
            host.Opacity = 1;
            host.Visibility = folded ? Visibility.Collapsed : Visibility.Visible;
            return;
        }

        double from;
        double to;
        if (folded)
        {
            from = host.ActualHeight;
            to = 0;
        }
        else
        {
            // 先恢复可见并测量自然高度，再回到 0 播放展开动画
            host.Visibility = Visibility.Visible;
            host.Height = double.NaN;
            host.UpdateLayout();
            from = 0;
            to = host.ActualHeight;
            host.Height = 0;
        }

        var heightAnimation = new DoubleAnimation
        {
            From = from,
            To = to,
            Duration = new Duration(TimeSpan.FromMilliseconds(260)),
            EnableDependentAnimation = true,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        var fadeAnimation = new DoubleAnimation
        {
            From = folded ? 1 : 0,
            To = folded ? 0 : 1,
            Duration = new Duration(TimeSpan.FromMilliseconds(folded ? 200 : 240)),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };

        var storyboard = new Storyboard();
        Storyboard.SetTarget(heightAnimation, host);
        Storyboard.SetTargetProperty(heightAnimation, "Height");
        Storyboard.SetTarget(fadeAnimation, host);
        Storyboard.SetTargetProperty(fadeAnimation, "Opacity");
        storyboard.Children.Add(heightAnimation);
        storyboard.Children.Add(fadeAnimation);
        storyboard.Completed += (_, _) =>
        {
            storyboard.Stop();
            host.Opacity = 1;
            host.Height = double.NaN;
            host.Visibility = folded ? Visibility.Collapsed : Visibility.Visible;
        };
        _foldStoryboard = storyboard;
        storyboard.Begin();
    }
}