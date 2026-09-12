using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;

namespace RizUI.Uwp.Controls;

/// <summary>
/// 快捷静态工厂帮助类，用于极速生成只读的属性说明表。
/// </summary>
public static class RizTableHelper
{
    public static RizTable CreatePropsTable(IEnumerable<(string Prop, string Type, string DefaultVal, string Desc)> properties)
    {
        var table = new RizTable
        {
            IsReadOnly = true,
            Headers = new ObservableCollection<string> { "属性", "类型", "默认值", "说明" }
        };

        var rows = new ObservableCollection<ObservableCollection<string>>();
        foreach (var (prop, type, defaultVal, desc) in properties)
        {
            rows.Add(new ObservableCollection<string> { prop, type, defaultVal, desc });
        }
        table.Rows = rows;
        return table;
    }
}
