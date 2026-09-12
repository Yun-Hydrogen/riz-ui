using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class CardDemoPage : Page
{
    public string CodeUsage1 { get; } =
@"// 1. 基础卡片：标题 + 描述 + 图标 + 内容
var card = new RizCard
{
    Title = ""MomoTalk 信箱"",
    Desc = ""老师，您有 3 条未读消息 · 最近来自游戏开发部"",
    Icon = ""fa-solid fa-envelope-open-text""
};
card.Content = new StackPanel
{
    Children =
    {
        new RizCfgRow { Label = ""天童爱丽丝"", Hint = ""邦邦卡邦！老师，周末来打游戏吧！"" },
        new RizCfgRow { Label = ""小鸟游星野"", Hint = ""老师要不要再来阿拜多斯体验大叔的枕头呀~"" }
    }
};

<!-- XAML 声明写法 -->
<rizui:RizCard Title=""MomoTalk 信箱""
               Desc=""老师，您有 3 条未读消息 · 最近来自游戏开发部""
               Icon=""fa-solid fa-envelope-open-text"">
    <rizui:RizCfgRow Label=""天童爱丽丝"" Hint=""邦邦卡邦！老师，周末来打游戏吧！"" />
</rizui:RizCard>";

    public string CodeUsage2 { get; } =
@"// 2. 空卡片：不设置任何属性，作为纯背景容器
var card = new RizCard();
card.Content = new RizCfgRow
{
    Label = ""干净的卡片"",
    Hint = ""GunMu""
};";

    public string CodeUsage3 { get; } =
@"// 3. 图标动效：支持全部 FA class，加个 fa-spin 就能一直转~
var card = new RizCard
{
    Title = ""总力战 · 备战中"",
    Desc = ""icon 支持全部 FA class，加个 fa-spin 就能一直旋转图标~"",
    Icon = ""fa-solid fa-gear fa-spin""
};";

    public string CodeUsage4 { get; } =
@"// 4. 样式定制：TitleStyle / DescStyle / IconStyle 完整覆盖
// C# 后台代码写法：
var iconStyle = new Style(typeof(TextBlock));
iconStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, new SolidColorBrush(ColorUtil.FromHex(""#E278B3"", Colors.Pink))));
iconStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, 24.0));

var titleStyle = new Style(typeof(TextBlock));
titleStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, new SolidColorBrush(ColorUtil.FromHex(""#E278B3"", Colors.Pink))));
titleStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, 17.0));
titleStyle.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));

var descStyle = new Style(typeof(TextBlock));
descStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, new SolidColorBrush(ColorUtil.FromHex(""#C084FC"", Colors.Purple))));
descStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, 12.0));

var card = new RizCard
{
    Title = ""沙勒公告"",
    Desc = ""来自什亭之箱的系统通知 · 今日基沃托斯天气：晴"",
    Icon = ""fa-solid fa-bullhorn"",
    IconStyle = iconStyle,
    TitleStyle = titleStyle,
    DescStyle = descStyle
};

<!-- XAML 声明写法： -->
<rizui:RizCard Title=""沙勒公告""
               Desc=""来自什亭之箱的系统通知 · 今日基沃托斯天气：晴""
               Icon=""fa-solid fa-bullhorn"">
  <rizui:RizCard.IconStyle>
    <Style TargetType=""TextBlock"">
      <Setter Property=""Foreground"" Value=""#E278B3"" />
      <Setter Property=""FontSize"" Value=""24"" />
    </Style>
  </rizui:RizCard.IconStyle>
  <rizui:RizCard.TitleStyle>
    <Style TargetType=""TextBlock"">
      <Setter Property=""Foreground"" Value=""#E278B3"" />
      <Setter Property=""FontSize"" Value=""17"" />
      <Setter Property=""FontWeight"" Value=""Bold"" />
    </Style>
  </rizui:RizCard.TitleStyle>
  <rizui:RizCard.DescStyle>
    <Style TargetType=""TextBlock"">
      <Setter Property=""Foreground"" Value=""#C084FC"" />
      <Setter Property=""FontSize"" Value=""12"" />
    </Style>
  </rizui:RizCard.DescStyle>
  <rizui:RizCfgRow Label=""阿洛娜"" Hint=""老师，今天也要元气满满地处理联邦学生会的事务哦～"" />
</rizui:RizCard>";

    public string CodeUsage5 { get; } =
@"<!-- 5. 可折叠卡片：Foldable=True，点击标题栏平滑折叠/展开 -->
<rizui:RizCard Title=""可折叠卡片"" Foldable=""True""
               Desc=""点击标题栏即可折叠 / 展开内容区""
               Icon=""fa-solid fa-folder-tree"">
  <StackPanel>
    <rizui:RizCfgRow Label=""阿洛娜的任务清单"" Hint=""折叠动画由高度 + 透明度双层缓动驱动"" />
  </StackPanel>
</rizui:RizCard>";

    public CardDemoPage()
    {
        InitializeComponent();
        PropsTable.Headers = new System.Collections.ObjectModel.ObservableCollection<string> { "属性", "类型", "默认值", "说明" };
        PropsTable.Rows = new System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<string>>
        {
            new() { "Title", "string", "\"\"", "卡片主标题文字" },
            new() { "TitleStyle", "Style", "null", "标题 XAML 样式覆盖" },
            new() { "Desc", "string", "\"\"", "卡片副标题/描述文字" },
            new() { "DescStyle", "Style", "null", "描述 XAML 样式覆盖" },
            new() { "Icon", "string", "\"\"", "Font Awesome 类名（如 fa-solid fa-envelope）" },
            new() { "IconStyle", "Style", "null", "图标 XAML 样式覆盖（颜色/字号等）" },
            new() { "Theme", "string", "\"auto\"", "Dark 强制暗色 / Light 强制亮色 / auto 跟随系统" },
            new() { "Foldable", "bool", "false", "是否可折叠：点击标题栏折叠/展开内容区，带平滑高度动画" }
        };
    }
}