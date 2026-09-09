using System;

namespace RizUI.Uwp.Controls;

/// <summary>RizDropdown 下拉项。</summary>
public class RizDropdownItem
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;

    public RizDropdownItem() { }
    public RizDropdownItem(string value, string label)
    {
        Value = value;
        Label = label;
    }

    public override string ToString() => Label;
}
