using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace RizUI.Uwp.Controls;

/// <summary>
/// 原生实现的表格控件 RizTable。
/// 支持只读展示或实时交互编辑模式（单元格编辑、添加行、添加列、行/列数量限制、深色模式深度适配）。
/// </summary>
public sealed class RizTable : Control
{
    private Border? _root;
    private Grid? _tableGrid;
    private StackPanel? _actionsPanel;
    private Button? _btnAddRow;
    private Button? _btnAddCol;
    private bool _templateApplied;

    public RizTable()
    {
        DefaultStyleKey = typeof(RizTable);
        ActualThemeChanged += (_, _) => ApplyVisual();
        RegisterPropertyChangedCallback(IsEnabledProperty, (_, _) => ApplyVisual());

        var headers = new ObservableCollection<string>();
        headers.CollectionChanged += OnCollectionChanged;
        Headers = headers;

        var rows = new ObservableCollection<ObservableCollection<string>>();
        rows.CollectionChanged += OnRowsCollectionChanged;
        Rows = rows;
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => RebuildTable();

    private void OnRowsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (ObservableCollection<string> row in e.NewItems)
            {
                row.CollectionChanged += OnCollectionChanged;
            }
        }
        if (e.OldItems != null)
        {
            foreach (ObservableCollection<string> row in e.OldItems)
            {
                row.CollectionChanged -= OnCollectionChanged;
            }
        }
        RebuildTable();
    }

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(RizTable),
            new PropertyMetadata(false, OnStructureChanged));

    /// <summary>是否只读。默认 false（支持单元格编辑与加行加列），设为 true 时为静态展示表格。</summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly DependencyProperty HeadersProperty =
        DependencyProperty.Register(nameof(Headers), typeof(IList<string>), typeof(RizTable),
            new PropertyMetadata(null, OnHeadersPropertyChanged));

    /// <summary>表头列表集合。</summary>
    public IList<string> Headers
    {
        get => (IList<string>)GetValue(HeadersProperty);
        set => SetValue(HeadersProperty, value);
    }

    public static readonly DependencyProperty RowsProperty =
        DependencyProperty.Register(nameof(Rows), typeof(IList<ObservableCollection<string>>), typeof(RizTable),
            new PropertyMetadata(null, OnRowsPropertyChanged));

    /// <summary>表格行数据集合。</summary>
    public IList<ObservableCollection<string>> Rows
    {
        get => (IList<ObservableCollection<string>>)GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public static readonly DependencyProperty MaxRowsProperty =
        DependencyProperty.Register(nameof(MaxRows), typeof(int), typeof(RizTable),
            new PropertyMetadata(0, OnStructureChanged));

    /// <summary>最大行数限制。0 表示无限制。</summary>
    public int MaxRows
    {
        get => (int)GetValue(MaxRowsProperty);
        set => SetValue(MaxRowsProperty, value);
    }

    public static readonly DependencyProperty MaxColumnsProperty =
        DependencyProperty.Register(nameof(MaxColumns), typeof(int), typeof(RizTable),
            new PropertyMetadata(0, OnStructureChanged));

    /// <summary>最大列数限制。0 表示无限制。</summary>
    public int MaxColumns
    {
        get => (int)GetValue(MaxColumnsProperty);
        set => SetValue(MaxColumnsProperty, value);
    }

    public static readonly DependencyProperty RowCountProperty =
        DependencyProperty.Register(nameof(RowCount), typeof(int), typeof(RizTable),
            new PropertyMetadata(0, OnTargetSizeChanged));

    /// <summary>指定固定行数。大于 0 时自动扩展或裁切行。</summary>
    public int RowCount
    {
        get => (int)GetValue(RowCountProperty);
        set => SetValue(RowCountProperty, value);
    }

    public static readonly DependencyProperty ColumnCountProperty =
        DependencyProperty.Register(nameof(ColumnCount), typeof(int), typeof(RizTable),
            new PropertyMetadata(0, OnTargetSizeChanged));

    /// <summary>指定固定列数。大于 0 时自动扩展或裁切列。</summary>
    public int ColumnCount
    {
        get => (int)GetValue(ColumnCountProperty);
        set => SetValue(ColumnCountProperty, value);
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(RizTable),
            new PropertyMetadata(string.Empty, OnVisualChanged));

    /// <summary>自定义主题色，空则跟随 --rizui_themecolor。</summary>
    public string Color
    {
        get => (string)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RizTable),
            new PropertyMetadata("auto", OnVisualChanged));

    /// <summary>主题模式：Dark / Light / auto。</summary>
    public string Theme
    {
        get => (string)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public event EventHandler? DataChanged;

    private static void OnHeadersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var tb = (RizTable)d;
        tb.RebuildTable();
    }

    private static void OnRowsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var tb = (RizTable)d;
        tb.RebuildTable();
    }

    private static void OnStructureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var tb = (RizTable)d;
        tb.RebuildTable();
        tb.ApplyVisual();
    }

    private static void OnTargetSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var tb = (RizTable)d;
        tb.ApplySpecifiedDimensions();
        tb.RebuildTable();
    }

    private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((RizTable)d).ApplyVisual();

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _root = GetTemplateChild("Root") as Border;
        _tableGrid = GetTemplateChild("TableGrid") as Grid;
        _actionsPanel = GetTemplateChild("ActionsPanel") as StackPanel;
        _btnAddRow = GetTemplateChild("BtnAddRow") as Button;
        _btnAddCol = GetTemplateChild("BtnAddCol") as Button;

        if (_btnAddRow != null)
        {
            _btnAddRow.Click += (_, _) => AddRow();
        }

        if (_btnAddCol != null)
        {
            _btnAddCol.Click += (_, _) => AddColumn();
        }

        _templateApplied = true;
        ApplySpecifiedDimensions();
        RebuildTable();
        ApplyVisual();
    }

    /// <summary>添加新行。</summary>
    public void AddRow()
    {
        if (IsReadOnly || !IsEnabled) return;
        if (MaxRows > 0 && Rows.Count >= MaxRows) return;

        var colCount = Math.Max(1, Headers?.Count ?? (Rows.Count > 0 ? Rows[0].Count : 1));
        var newRow = new ObservableCollection<string>();
        for (int i = 0; i < colCount; i++)
        {
            newRow.Add(string.Empty);
        }
        newRow.CollectionChanged += OnCollectionChanged;
        Rows.Add(newRow);
        DataChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>添加新列。</summary>
    public void AddColumn(string? headerName = null)
    {
        if (IsReadOnly || !IsEnabled) return;
        var currentCols = Headers?.Count ?? 0;
        if (MaxColumns > 0 && currentCols >= MaxColumns) return;

        var name = headerName ?? $"列 {currentCols + 1}";
        Headers?.Add(name);

        foreach (var r in Rows)
        {
            r.Add(string.Empty);
        }
        DataChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ApplySpecifiedDimensions()
    {
        if (ColumnCount > 0 && Headers != null)
        {
            while (Headers.Count < ColumnCount)
            {
                Headers.Add($"列 {Headers.Count + 1}");
            }
            while (Headers.Count > ColumnCount)
            {
                Headers.RemoveAt(Headers.Count - 1);
            }

            foreach (var r in Rows)
            {
                while (r.Count < ColumnCount) r.Add(string.Empty);
                while (r.Count > ColumnCount) r.RemoveAt(r.Count - 1);
            }
        }

        if (RowCount > 0 && Rows != null)
        {
            var colCount = Math.Max(1, Headers?.Count ?? 1);
            while (Rows.Count < RowCount)
            {
                var row = new ObservableCollection<string>();
                for (int i = 0; i < colCount; i++) row.Add(string.Empty);
                row.CollectionChanged += OnCollectionChanged;
                Rows.Add(row);
            }
            while (Rows.Count > RowCount)
            {
                Rows.RemoveAt(Rows.Count - 1);
            }
        }
    }

    private void RebuildTable()
    {
        if (!_templateApplied || _tableGrid == null) return;
        _tableGrid.Children.Clear();
        _tableGrid.RowDefinitions.Clear();
        _tableGrid.ColumnDefinitions.Clear();

        var headers = Headers ?? new List<string>();
        var rows = Rows ?? new List<ObservableCollection<string>>();

        int colCount = Math.Max(headers.Count, rows.Count > 0 ? rows.Max(r => r.Count) : 0);
        if (colCount == 0) colCount = 1;

        for (int c = 0; c < colCount; c++)
        {
            _tableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        // Header Row
        _tableGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        for (int r = 0; r < rows.Count; r++)
        {
            _tableGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }

        var dark = RizVisuals.IsDark(this, Theme);
        var accent = RizVisuals.ThemeColor(Color);
        var borderBrush = RizVisuals.Brush(RizVisuals.Border(dark));

        // 1. 表头行背景
        var headerBg = new Border
        {
            Background = RizVisuals.Brush(RizVisuals.Mix(accent, RizVisuals.Surface(dark), 0.10)),
            BorderThickness = new Thickness(0, 0, 0, 1),
            BorderBrush = borderBrush,
            CornerRadius = new CornerRadius(10, 10, 0, 0)
        };
        Grid.SetRow(headerBg, 0);
        Grid.SetColumnSpan(headerBg, colCount);
        _tableGrid.Children.Add(headerBg);

        // 表头单元格
        for (int c = 0; c < colCount; c++)
        {
            var headerText = c < headers.Count ? headers[c] : $"列 {c + 1}";
            FrameworkElement headerElement;
            if (!IsReadOnly)
            {
                var input = new RizText
                {
                    Text = headerText,
                    HeightText = "28px",
                    Placeholder = "表头",
                    Margin = new Thickness(6, 4, 6, 4),
                    VerticalAlignment = VerticalAlignment.Center
                };
                int colIndex = c;
                input.TextChanged += (_, newVal) =>
                {
                    while (headers.Count <= colIndex) headers.Add(string.Empty);
                    headers[colIndex] = newVal;
                    DataChanged?.Invoke(this, EventArgs.Empty);
                };
                headerElement = input;
            }
            else
            {
                headerElement = new TextBlock
                {
                    Text = headerText,
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 12,
                    Foreground = RizVisuals.Brush(RizVisuals.TitleText(dark)),
                    Margin = new Thickness(14, 9, 14, 9),
                    VerticalAlignment = VerticalAlignment.Center
                };
            }
            Grid.SetRow(headerElement, 0);
            Grid.SetColumn(headerElement, c);
            _tableGrid.Children.Add(headerElement);
        }

        // 2. 数据行
        for (int r = 0; r < rows.Count; r++)
        {
            var rowData = rows[r];
            var isLast = r == rows.Count - 1;

            var rowBorder = new Border
            {
                BorderThickness = new Thickness(0, 0, 0, isLast ? 0 : 1),
                BorderBrush = RizVisuals.Brush(RizVisuals.Mix(RizVisuals.Border(dark), RizVisuals.Surface(dark), 0.65))
            };
            Grid.SetRow(rowBorder, r + 1);
            Grid.SetColumnSpan(rowBorder, colCount);
            _tableGrid.Children.Add(rowBorder);

            for (int c = 0; c < colCount; c++)
            {
                var cellVal = c < rowData.Count ? rowData[c] : string.Empty;
                FrameworkElement cellElement;
                if (!IsReadOnly)
                {
                    var cellInput = new RizText
                    {
                        Text = cellVal,
                        HeightText = "28px",
                        Placeholder = "...",
                        Margin = new Thickness(6, 4, 6, 4),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    int rowIdx = r;
                    int colIdx = c;
                    cellInput.TextChanged += (_, newVal) =>
                    {
                        while (rows[rowIdx].Count <= colIdx) rows[rowIdx].Add(string.Empty);
                        rows[rowIdx][colIdx] = newVal;
                        DataChanged?.Invoke(this, EventArgs.Empty);
                    };
                    cellElement = cellInput;
                }
                else
                {
                    cellElement = new TextBlock
                    {
                        Text = cellVal,
                        FontSize = 12,
                        Foreground = RizVisuals.Brush(RizVisuals.Text(dark)),
                        Margin = new Thickness(14, 8, 14, 8),
                        VerticalAlignment = VerticalAlignment.Center,
                        TextWrapping = TextWrapping.Wrap
                    };
                }
                Grid.SetRow(cellElement, r + 1);
                Grid.SetColumn(cellElement, c);
                _tableGrid.Children.Add(cellElement);
            }
        }

        UpdateActionButtons();
    }

    private void UpdateActionButtons()
    {
        if (_actionsPanel == null) return;
        _actionsPanel.Visibility = IsReadOnly ? Visibility.Collapsed : Visibility.Visible;

        if (_btnAddRow != null)
        {
            var canAddRow = MaxRows <= 0 || (Rows?.Count ?? 0) < MaxRows;
            _btnAddRow.IsEnabled = canAddRow;
        }

        if (_btnAddCol != null)
        {
            var currentCols = Headers?.Count ?? 0;
            var canAddCol = MaxColumns <= 0 || currentCols < MaxColumns;
            _btnAddCol.IsEnabled = canAddCol;
        }
    }

    private void ApplyVisual()
    {
        if (!_templateApplied) return;
        var dark = RizVisuals.IsDark(this, Theme);
        var surface = RizVisuals.Surface(dark);
        var border = RizVisuals.Border(dark);

        if (_root != null)
        {
            _root.Background = RizVisuals.Brush(surface);
            _root.BorderBrush = RizVisuals.Brush(border);
        }

        UpdateActionButtons();
    }
}
