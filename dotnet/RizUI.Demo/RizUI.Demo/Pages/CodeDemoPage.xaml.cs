using System;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class CodeDemoPage : Page
{
    public string AronaCs { get; } =
@"public class ShittimChest
{
    // 什亭之箱主控程序
    public string GreetSensei(string sensei)
    {
        var greeting = $""早安，{sensei}！今天也要元气满满地工作哦～"";
        Console.WriteLine(greeting);
        return greeting;
    }
}";

    public string SchaleHtml { get; } =
@"<!-- 什亭之箱 -->
<div class=""schale-window"">
  <h1>什亭之箱</h1>
  <p class=""status"">阿洛娜：睡眠中 普拉娜：工作中</p>
</div>";

    public string GameDevPy { get; } =
@"def deploy_cleaner_robot():
    # 游戏开发部
    print(""爱丽丝：光啊——！"")
    return ""已清除废墟区域障碍""";

    public string KivotosSql { get; } =
@"SELECT s.name, s.school, s.club, s.weapon_type
FROM students AS s
JOIN clubs AS c ON s.club_id = c.id
WHERE s.status = 'active'
ORDER BY s.power_level DESC;";

    public string Card1Usage { get; } =
@"<!-- 1. 单行内联胶囊 (默认) -->
<rizui:RizCode Code=""var answer = 42;"" Lang=""cs"" />
<StackPanel Orientation=""Horizontal"" Spacing=""4"">
  <TextBlock Text=""欢迎回到沙勒，请执行"" VerticalAlignment=""Center"" />
  <rizui:RizCode Code=""schale login"" Lang=""bash"" />
</StackPanel>";

    public string Card2Usage { get; } =
@"<!-- 2. 大代码块 (Block=True) + 语法高亮与 1.5s 复制动画 -->
<rizui:RizCode Block=""True"" Lang=""cs"" Code=""..."" />
<rizui:RizCode Block=""True"" Lang=""html"" Code=""..."" />";

    public string Card3Usage { get; } =
@"<!-- 3. 多语言支持 (py / sql / bash / json 等数十种语言) -->
<rizui:RizCode Block=""True"" Lang=""py"" Code=""..."" />
<rizui:RizCode Block=""True"" Lang=""sql"" Code=""..."" />";

    public string Card4Usage { get; } =
@"<!-- 4. 自定义主题色与最大高度限制 -->
<rizui:RizCode Block=""True"" Lang=""cs"" HeightText=""140px"" Color=""#A855F7"" Code=""..."" />";

    public CodeDemoPage()
    {
        InitializeComponent();
    }

    private void OnCopied(object sender, EventArgs e)
    {
        if (Status != null)
        {
            Status.Text = $"已复制代码到剪贴板 ({DateTime.Now:HH:mm:ss})";
        }
    }
}