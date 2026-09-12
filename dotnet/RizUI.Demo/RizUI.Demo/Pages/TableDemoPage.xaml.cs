using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using RizUI.Uwp.Controls;

namespace RizUI.Demo.Pages;

public sealed partial class TableDemoPage : Page
{
    public string CodeUsage1 { get; } =
@"<!-- 1. 可编辑交互表格（默认） -->
<rizui:RizTable MaxRows=""10"" MaxColumns=""6"" />";

    public string CodeUsage2 { get; } =
@"<!-- 2. 只读表格展示模式 -->
<rizui:RizTable IsReadOnly=""True"" Headers=""{x:Bind TableHeaders}"" Rows=""{x:Bind TableRows}"" />";

    public TableDemoPage()
    {
        InitializeComponent();

        // 1. 初始化可编辑表格初始数据
        EditableTable.Headers = new ObservableCollection<string> { "学员姓名", "所属学园", "战术职阶", "初始星级" };
        EditableTable.Rows = new ObservableCollection<ObservableCollection<string>>
        {
            new() { "天童爱丽丝", "千禧年游戏开发部", "特攻突击", "3★" },
            new() { "小鸟游星野", "阿拜多斯对策委员会", "前线重装", "3★" },
            new() { "早濑优香", "千禧年研讨会", "前排防御", "2★" }
        };

        // 2. 初始化只读数据展示表格
        ReadOnlyTable.Headers = new ObservableCollection<string> { "服务器节点", "网络状态", "延迟 (ms)", "当前负载" };
        ReadOnlyTable.Rows = new ObservableCollection<ObservableCollection<string>>
        {
            new() { "沙勒主控核心", "运行正常", "12", "34%" },
            new() { "千禧年超算中心", "运行正常", "18", "52%" },
            new() { "联邦学生会终端", "繁忙", "85", "89%" }
        };

        // 3. 初始化属性表
        PropsTable.Headers = new ObservableCollection<string> { "属性", "类型", "默认值", "说明" };
        PropsTable.Rows = new ObservableCollection<ObservableCollection<string>>
        {
            new() { "IsReadOnly", "bool", "false", "是否只读。默认 false 支持单元格交互编辑与加行加列" },
            new() { "Headers", "IList<string>", "null", "表头文本集合" },
            new() { "Rows", "IList<ObservableCollection<string>>", "null", "表格二维行数据集合" },
            new() { "MaxRows", "int", "0", "允许添加的最大行数限制，0 表示无限制" },
            new() { "MaxColumns", "int", "0", "允许添加的最大列数限制，0 表示无限制" },
            new() { "RowCount", "int", "0", "指定生成或固定行数，大于 0 时自动补齐或裁剪" },
            new() { "ColumnCount", "int", "0", "指定生成或固定列数，大于 0 时自动补齐或裁剪" },
            new() { "Color", "string", "空", "主题色覆盖，空则跟随全局 --rizui_themecolor" },
            new() { "Theme", "string", "\"auto\"", "Dark 强制暗色 / Light 强制亮色 / auto 跟随系统" }
        };
    }
}
