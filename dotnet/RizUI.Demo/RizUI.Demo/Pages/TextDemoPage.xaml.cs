using System;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class TextDemoPage : Page
{
    public string SingleValue { get; set; } = "阿拜多斯废墟调查";
    public string MultiValue { get; set; } = "天童爱丽丝：邦邦卡邦！老师，周末要一起来游戏开发部打游戏吗？\n小鸟游星野：唔...大叔我还是想趴在沙勒的办公室沙发上睡觉呀～";

    public string CodeUsage1 { get; } =
@"<!-- 单行胶囊文本输入 -->
<rizui:RizCfgRow Label=""搜索关键词"" Hint=""聚焦时主题色边框从中轴平滑向两侧展开"">
  <rizui:RizText Text=""阿拜多斯废墟调查"" Placeholder=""请输入搜索内容..."" Width=""220"" />
</rizui:RizCfgRow>";

    public string CodeUsage2 { get; } =
@"<!-- 多行纯文本模式 (Plaintext=True) -->
<rizui:RizCfgRow Label=""任务备忘录"" Hint=""多行纯文本编辑模式"" Stack=""True"">
  <rizui:RizText Plaintext=""True"" Placeholder=""在此记录行动摘要..."" HeightText=""100px"" Margin=""0,6,0,0"" />
</rizui:RizCfgRow>";

    public string CodeUsage3 { get; } =
@"<!-- 大文本框限制高度 (HeightText=""120px"")，内部平滑滚动 -->
<rizui:RizText Plaintext=""True"" HeightText=""120px"" Text=""..."" />";

    public string CodeUsage4 { get; } =
@"<!-- 只读模式与自定义颜色 -->
<rizui:RizText Text=""终端只读日志：沙勒网络已安全连通"" IsReadOnly=""True"" Color=""#39C5BB"" />";

    public TextDemoPage()
    {
        InitializeComponent();
        PropsTable.Headers = new System.Collections.ObjectModel.ObservableCollection<string> { "属性", "类型", "默认值", "说明" };
        PropsTable.Rows = new System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<string>>
        {
            new() { "Text", "string", "\"\"", "当前输入文本内容（双向绑定，对应 Vue 的 modelValue）" },
            new() { "Placeholder", "string", "\"\"", "占位提示文字" },
            new() { "Plaintext", "bool", "false", "是否多行纯文本编辑模式（textarea）" },
            new() { "IsReadOnly", "bool", "false", "是否只读状态" },
            new() { "Color", "string", "\"\"", "聚焦边框高亮主题色，空则跟随 --rizui_themecolor" },
            new() { "Theme", "string", "\"auto\"", "Dark 强制暗色 / Light 强制亮色 / auto 跟随系统" }
        };
    }
}
