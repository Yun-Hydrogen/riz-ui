using System;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class InfoBoxDemoPage : Page
{
    public string GachaResult { get; } = "📘📘📘📘📘📘📘📘📘📙";

    public string UsageCode1 { get; } =
@"<!-- 1. 三种预设样式 (info / warn / error) -->
<rizui:RizInfoBox Type=""info"" Text=""emm...今天真是个好天气！"" />
<rizui:RizInfoBox Type=""warn"" Text=""看起来，好像要下雨......"" />
<rizui:RizInfoBox Type=""error"" Text=""老师，请立即下达撤退命令！"" />";

    public string UsageCode2 { get; } =
@"<!-- 2. 自定义样式与颜色 (支持 Icon、IconStyle、TextStyle、Color) -->
<rizui:RizInfoBox Type=""info"" Text=""抽取结果: 📘📘📘📘📘📘📘📘📘📙""
                  Icon=""fa-solid fa-bell"">
  <rizui:RizInfoBox.IconStyle>
    <Style TargetType=""TextBlock"">
      <Setter Property=""FontSize"" Value=""18"" />
      <Setter Property=""Foreground"" Value=""#E6A23C"" />
    </Style>
  </rizui:RizInfoBox.IconStyle>
</rizui:RizInfoBox>

<rizui:RizInfoBox Type=""warn"" Text=""青辉石 -1200"">
  <rizui:RizInfoBox.TextStyle>
    <Style TargetType=""TextBlock"">
      <Setter Property=""FontWeight"" Value=""Bold"" />
      <Setter Property=""FontSize"" Value=""14"" />
    </Style>
  </rizui:RizInfoBox.TextStyle>
</rizui:RizInfoBox>

<rizui:RizInfoBox Type=""info"" Text=""招募点数 Pt x 199"" Color=""#8B5CF6"" />";

    public string UsageCode3 { get; } =
@"<!-- 3. 文字 + 默认插槽 (自动在文字下方排布独立内容并带虚线分隔) -->
<rizui:RizInfoBox Type=""info"" Text=""这是标题文字"">
  <rizui:RizCfgRow Label=""这是一个配置行"" Hint=""可以放置更多交互控件"" />
</rizui:RizInfoBox>";

    public InfoBoxDemoPage()
    {
        InitializeComponent();
        PropsTable.Headers = new System.Collections.ObjectModel.ObservableCollection<string> { "属性", "类型", "默认值", "说明" };
        PropsTable.Rows = new System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<string>>
        {
            new() { "Type", "string", "\"info\"", "预设类型：info / warn / error" },
            new() { "Text", "string", "\"\"", "提示框主要展示文本" },
            new() { "TextStyle", "Style", "null", "主要文本的 XAML 样式覆盖" },
            new() { "Icon", "string", "\"\"", "自定义左侧图标（空则根据 Type 自动匹配）" },
            new() { "IconStyle", "Style", "null", "左侧图标的 XAML 样式覆盖" },
            new() { "Color", "string", "\"\"", "自定义主题色（仅在 Type='info' 时生效）" },
            new() { "Theme", "string", "\"auto\"", "Dark 强制暗色 / Light 强制亮色 / auto 跟随系统" }
        };
    }
}