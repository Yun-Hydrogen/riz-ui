using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using RizUI.Uwp.Controls;
using Windows.UI;

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
        BuildLanguageChips();
        ActualThemeChanged += (_, _) => BuildLanguageChips();
        PropsTable.Headers = new System.Collections.ObjectModel.ObservableCollection<string> { "属性", "类型", "默认值", "说明" };
        PropsTable.Rows = new System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<string>>
        {
            new() { "Code", "string", "\"\"", "代码文本内容（也是 ContentProperty 默认内容）" },
            new() { "Block", "bool", "false", "大代码块模式（多行展示 + 标头语言 + 复制按钮）" },
            new() { "Lang", "string", "\"\"", "代码语言（22 种：cs / html / js / ts / python / sql / bash / json / yaml / java / go / rust 等，支持别名）" },
            new() { "Color", "string", "\"\"", "边框与复制按钮颜色，空则跟随 --rizui_themecolor" },
            new() { "HeightText", "string", "\"\"", "最大高度限制（如 140px、200px），超出则滚动" },
            new() { "Theme", "string", "\"auto\"", "Dark 强制暗色 / Light 强制亮色 / auto 跟随系统" }
        };
    }

    /// <summary>生成「支持语言清单」芯片（源自 RizCode.SupportedLanguages，随主题重建）。</summary>
    private void BuildLanguageChips()
    {
        if (LangList == null)
        {
            return;
        }

        LangList.Children.Clear();
        var dark = ActualTheme == ElementTheme.Dark;
        var accent = dark ? Color.FromArgb(0xFF, 0x66, 0xCC, 0xFF) : Color.FromArgb(0xFF, 0x00, 0x80, 0xFF);
        var chipBg = dark ? Color.FromArgb(0x1A, 0xFF, 0xFF, 0xFF) : Color.FromArgb(0x0F, 0x00, 0x00, 0x00);
        var chipBorder = dark ? Color.FromArgb(0x29, 0xFF, 0xFF, 0xFF) : Color.FromArgb(0x1A, 0x00, 0x00, 0x00);
        var nameColor = dark ? Color.FromArgb(0xFF, 0x9A, 0xA0, 0xA6) : Color.FromArgb(0xFF, 0x6B, 0x72, 0x80);

        const int perRow = 4;
        var languages = RizCode.SupportedLanguages;
        Grid? row = null;
        for (var i = 0; i < languages.Count; i++)
        {
            if (i % perRow == 0)
            {
                row = new Grid();
                for (var c = 0; c < perRow; c++)
                {
                    row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }
                LangList.Children.Add(row);
            }

            var info = languages[i];
            var caption = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
            caption.Children.Add(new TextBlock
            {
                Text = info.Lang,
                FontSize = 12.5,
                FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                Foreground = new SolidColorBrush(accent),
                VerticalAlignment = VerticalAlignment.Center
            });
            caption.Children.Add(new TextBlock
            {
                Text = info.Name,
                FontSize = 12.5,
                Foreground = new SolidColorBrush(nameColor),
                VerticalAlignment = VerticalAlignment.Center
            });

            var chip = new Border
            {
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10, 6, 10, 6),
                Margin = new Thickness(0, 0, 8, 0),
                Background = new SolidColorBrush(chipBg),
                BorderBrush = new SolidColorBrush(chipBorder),
                BorderThickness = new Thickness(1),
                Child = caption
            };
            if (!string.IsNullOrEmpty(info.Aliases))
            {
                ToolTipService.SetToolTip(chip, $"别名：{info.Aliases}");
            }

            Grid.SetColumn(chip, i % perRow);
            row!.Children.Add(chip);
        }
    }

    private void OnCopied(object sender, EventArgs e)
    {
        if (Status != null)
        {
            Status.Text = $"已复制代码到剪贴板 ({DateTime.Now:HH:mm:ss})";
        }
    }
}