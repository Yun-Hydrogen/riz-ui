using System.Windows.Input;

namespace RizUI.Uwp.Controls;

/// <summary>RizSliderNav 的树状条目；Children 可继续嵌套，不限制层级。</summary>
public sealed class RizSliderNavItem
{
    /// <summary>稳定栏目键，用于 DefaultSelectedItem / SelectedItem。</summary>
    public string Key { get; set; } = string.Empty;
    /// <summary>显示文本。</summary>
    public string Text { get; set; } = string.Empty;
    /// <summary>完整 Font Awesome class 或 Unicode 文本，例如 fa-solid fa-house。</summary>
    public string Icon { get; set; } = string.Empty;
    /// <summary>任意深度的子栏目。</summary>
    public IList<RizSliderNavItem> Children { get; } = new List<RizSliderNavItem>();
    /// <summary>初始展开状态。</summary>
    public bool IsExpanded { get; set; }
    /// <summary>选择该条目时执行的 MVVM 命令。</summary>
    public ICommand? Command { get; set; }
    /// <summary>命令参数。</summary>
    public object? CommandParameter { get; set; }
}