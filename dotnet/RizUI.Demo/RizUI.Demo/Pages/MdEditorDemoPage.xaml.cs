using System;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class MdEditorDemoPage : Page
{
    public string SampleMd { get; set; } =
"# 什亭之箱 · 作战记录\n\n" +
"这是 **RizUI** 的原生 Markdown 实时预览编辑器。支持 **粗体文本**、*斜体注解*、~~删除线标记~~ 与 `inline_code()` 行内代码胶囊。\n\n" +
"> 邦邦卡邦！老师，周末要一起来游戏开发部打游戏吗？点击右上角滑块可体验流畅过渡！\n\n" +
"### 战术人员部署表\n" +
"| 学员姓名 | 所属学园 | 作战职阶 | 装备等级 |\n" +
"| --- | --- | --- | --- |\n" +
"| 天童爱丽丝 | 千禧年游戏开发部 | 特攻突击 | T8 充能轨道炮 |\n" +
"| 小鸟游星野 | 阿拜多斯对策委员会 | 前线重装 | T8 防暴盾牌 |\n" +
"| 陆八魔阿露 | 便利屋 68 | 狙击输出 | T8 精确步枪 |\n\n" +
"### 任务待办清单\n" +
"- [x] 完成沙勒主控系统接入与双端架构对齐\n" +
"- [x] 优化全端 UI 视觉与深浅色模式实时热重绘\n" +
"- [ ] 准备阿拜多斯废墟特别调查行动方案\n\n" +
"```cs\n" +
"public static void Main()\n" +
"{\n" +
"    // 什亭之箱核心主控逻辑\n" +
"    Console.WriteLine(\"早安，老师！今天也要元气满满哦～\");\n" +
"}\n" +
"```\n\n" +
"---\n" +
"*联邦学生会 · 沙勒特别事务局 战略指挥终端*";

    public string CodeUsage1 { get; } =
@"<!-- 1. 基础 Markdown 编辑器 (固定高度滚动) -->
<rizui:RizMdEditor Value=""{x:Bind SampleMd, Mode=TwoWay}"" HeightText=""400px"" />";

    public string CodeUsage2 { get; } =
@"<!-- 2. 自适应高度模式 (HeightText=""auto"" 随内容撑开) -->
<rizui:RizMdEditor Value=""{x:Bind SampleMd, Mode=TwoWay}"" HeightText=""auto"" />";

    public string CodeUsage3 { get; } =
@"<!-- 3. 只读渲染模式 (IsReadOnly=""True"" 隐藏工具栏纯净预览) -->
<rizui:RizMdEditor Value=""{x:Bind SampleMd}"" IsReadOnly=""True"" HeightText=""300px"" />";

    public MdEditorDemoPage()
    {
        InitializeComponent();
        PropsTable.Headers = new System.Collections.ObjectModel.ObservableCollection<string> { "属性", "类型", "默认值", "说明" };
        PropsTable.Rows = new System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<string>>
        {
            new() { "Value", "string", "\"\"", "Markdown 源码文本（双向绑定，对应 Vue 的 modelValue）" },
            new() { "Placeholder", "string", "\"在此输入…\"", "编辑区域占位提示文字" },
            new() { "IsReadOnly", "bool", "false", "是否只读状态（隐藏工具栏与编辑框，仅呈现渲染预览）" },
            new() { "HeightText", "string", "\"320px\"", "编辑器整体高度，如 360px、400px" },
            new() { "Theme", "string", "\"auto\"", "Dark 强制暗色 / Light 强制亮色 / auto 跟随系统" }
        };
    }
}
