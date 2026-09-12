using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class CfgRowDemoPage : Page
{
    public string CodeUsage1 { get; } =
@"// 1. 横向模式：左侧标题与提示文字，右侧收纳各类轻量操作控件
var row = new RizCfgRow
{
    Label = ""主控系统接入"",
    Hint = ""连接沙勒的中央控制系统"",
    Icon = ""fa-solid fa-satellite-dish"",
    Content = new RizCheckbox { IsChecked = true }
};

<!-- XAML 声明写法 -->
<rizui:RizCfgRow Label=""主控系统接入""
                 Hint=""连接沙勒的中央控制系统""
                 Icon=""fa-solid fa-satellite-dish"">
    <rizui:RizCheckbox IsChecked=""True"" />
</rizui:RizCfgRow>";

    public string CodeUsage2 { get; } =
@"// 2. Stack 竖向模式：控件换行置于下方，适合较长的输入框、复合控件或完整宽度的滑块
var row = new RizCfgRow
{
    Label = ""工作备忘录"",
    Hint = ""老师可以在这里记录需要注意的事项哦"",
    Icon = ""fa-solid fa-clipboard-list"",
    Stack = true,
    Content = new RizText { Placeholder = ""点击输入文本"" }
};

<!-- XAML 声明写法 -->
<rizui:RizCfgRow Label=""工作备忘录""
                 Hint=""老师可以在这里记录需要注意的事项哦""
                 Icon=""fa-solid fa-clipboard-list""
                 Stack=""True"">
    <TextBox PlaceholderText=""点击输入文本"" />
</rizui:RizCfgRow>";

    public string CodeUsage3 { get; } =
@"// 3. 禁用态：Disabled 为 True 时，半透明展示且阻止右侧/下方控件的所有交互
var row = new RizCfgRow
{
    Label = ""关闭什亭之箱的保护场域"",
    Hint = ""老师...请不要关闭保护场域，否则将无法抵御外部攻击！"",
    Icon = ""fa-solid fa-lock"",
    Disabled = true,
    Content = new RizCheckbox { IsChecked = true }
};

<!-- XAML 声明写法 -->
<rizui:RizCfgRow Label=""关闭什亭之箱的保护场域""
                 Hint=""老师...请不要关闭保护场域，否则将无法抵御外部攻击！""
                 Icon=""fa-solid fa-lock""
                 Disabled=""True"">
    <rizui:RizCheckbox IsChecked=""True"" />
</rizui:RizCfgRow>";

    public string CodeUsage4 { get; } =
@"// 4. 样式定制：LabelStyle / HintStyle / IconStyle 完整样式覆盖
var iconStyle = new Style(typeof(TextBlock));
iconStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, new SolidColorBrush(ColorUtil.FromHex(""#A855F7"", Colors.Purple))));

var labelStyle = new Style(typeof(TextBlock));
labelStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, new SolidColorBrush(ColorUtil.FromHex(""#A855F7"", Colors.Purple))));
labelStyle.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));

var hintStyle = new Style(typeof(TextBlock));
hintStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, new SolidColorBrush(ColorUtil.FromHex(""#C084FC"", Colors.Purple))));

var row = new RizCfgRow
{
    Label = ""由普拉娜担任助手"",
    Hint = ""我猜阿罗娜前辈又在睡懒觉了..."",
    Icon = ""fa-solid fa-wand-magic-sparkles"",
    IconStyle = iconStyle,
    LabelStyle = labelStyle,
    HintStyle = hintStyle
};

<!-- XAML 声明写法 -->
<rizui:RizCfgRow Label=""由普拉娜担任助手""
                 Hint=""我猜阿罗娜前辈又在睡懒觉了...""
                 Icon=""fa-solid fa-wand-magic-sparkles"">
    <rizui:RizCfgRow.IconStyle>
        <Style TargetType=""TextBlock"">
            <Setter Property=""Foreground"" Value=""#A855F7"" />
        </Style>
    </rizui:RizCfgRow.IconStyle>
    <rizui:RizCfgRow.LabelStyle>
        <Style TargetType=""TextBlock"">
            <Setter Property=""Foreground"" Value=""#A855F7"" />
            <Setter Property=""FontWeight"" Value=""Bold"" />
        </Style>
    </rizui:RizCfgRow.LabelStyle>
    <rizui:RizCfgRow.HintStyle>
        <Style TargetType=""TextBlock"">
            <Setter Property=""Foreground"" Value=""#C084FC"" />
        </Style>
    </rizui:RizCfgRow.HintStyle>
    <rizui:RizCheckbox IsChecked=""True"" Color=""#A855F7"" />
</rizui:RizCfgRow>";

    public CfgRowDemoPage()
    {
        InitializeComponent();
        PropsTable.Headers = new System.Collections.ObjectModel.ObservableCollection<string> { "属性", "类型", "默认值", "说明" };
        PropsTable.Rows = new System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<string>>
        {
            new() { "Label", "string", "\"\"", "左栏标签标题文字" },
            new() { "LabelStyle", "Style", "null", "标签文本 XAML 样式覆盖" },
            new() { "Hint", "string", "\"\"", "标签下方提示小字说明" },
            new() { "HintStyle", "Style", "null", "提示小字 XAML 样式覆盖" },
            new() { "Icon", "string", "\"\"", "左侧图标（完整 Font Awesome 类名，如 fa-solid fa-bolt）" },
            new() { "IconStyle", "Style", "null", "图标 XAML 样式覆盖（颜色、字号等）" },
            new() { "Stack", "bool", "false", "是否启用竖向堆叠布局（将控件置于标题下方）" },
            new() { "Disabled", "bool", "false", "是否禁用整行（半透明展示并阻止子控件交互）" },
            new() { "Theme", "string", "\"auto\"", "Dark 强制暗色 / Light 强制亮色 / auto 跟随系统" }
        };
    }
}
