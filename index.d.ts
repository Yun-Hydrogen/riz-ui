/* ============================================================================
   RizUI — 类型声明
   让 VSCode / Volar 在模板中补全各组件 props / events / slots
   ========================================================================== */

import type { DefineComponent } from 'vue'

type Instyle = Record<string, string | number>

/* ═══════════════════════════ 工具函数 ═══════════════════════════ */

/** 打开链接（新标签页），内部合成 <a> 点击，避免被弹窗拦截 */
export declare function openURL(url: string, target?: string): void

/* ═══════════════════════════ Container ═══════════════════════════ */

/** 卡片容器：标题 + 描述 + 内容插槽 */
export declare const rizui_card: DefineComponent<{
  title?: string
  titleStyle?: Instyle
  desc?: string
  descStyle?: Instyle
  icon?: string
  iconStyle?: Instyle
  /** 主题模式 Dark/Light/auto */
  theme?: string
}>

/** 配置行：左侧标签 + 右侧控件，支持 stack 布局 */
export declare const rizui_cfgrow: DefineComponent<{
  label?: string
  labelStyle?: Instyle
  hint?: string
  hintStyle?: Instyle
  icon?: string
  iconStyle?: Instyle
  stack?: boolean
  disabled?: boolean
  /** 主题模式 Dark/Light/auto */
  theme?: string
}>

/** 提示框：虚线边框 + info/warn/error 预设配色 */
export declare const rizui_infobox: DefineComponent<{
  type?: string
  text?: string
  textStyle?: Instyle
  color?: string
  /** 主题模式 Dark/Light/auto */
  theme?: string
  icon?: string
  iconStyle?: Instyle
}>

/** 对话框：Teleport + 遮罩关闭 + 图标 + 底部倒计时进度条 */
export declare const rizui_dialog: DefineComponent<
  {
    show?: boolean
    title?: string
    titleStyle?: Instyle
    icon?: string
    iconStyle?: Instyle
    color?: string
    teleport?: string
    /** 主题模式 Dark/Light/auto */
    theme?: string
    width?: string
    closable?: boolean
    maskClose?: boolean
    /** 是否显示底部倒计时/进度条，默认 false（不启用） */
    showBar?: boolean
    /** 自动关闭倒计时秒数，0 为不自动关 */
    countdown?: number
    /** 自定义进度百分比 (0~100) */
    progress?: number
    /** 不定进度模式（未知进度/加载中） */
    indeterminate?: boolean
  },
  { 'update:show': (val: boolean) => void, close: () => void, timeout: () => void },
  { default: () => any, footer: () => any }
>

/** 代码块：单行代码或大代码块（block），支持语法高亮与复制 */
export declare const rizui_code: DefineComponent<{
  code?: string
  block?: boolean
  lang?: string
  color?: string
  /** 最大高度（如 320px），默认内容全高展开 */
  height?: string
  /** 主题模式 Dark/Light/auto */
  theme?: string
}>

export interface NavItem {
  id?: string | number
  label?: string
  name?: string
  zhName?: string
  icon?: string
  iconStyle?: Instyle
  labelStyle?: Instyle
  route?: string
  href?: string
  target?: string
  disabled?: boolean
  badge?: string | number
  badgeColor?: string
  tag?: string
  deprecated?: boolean
  children?: NavItem[]
  comps?: NavItem[]
}

/** 导航栏 / 侧边栏：支持树状多级子目录、折叠/展开、文字旁图标与 WinUI 3 风格胶囊指示动效 */
export declare const rizui_nav: DefineComponent<
  {
    items?: NavItem[]
    modelValue?: string | number
    collapsed?: boolean
    collapsible?: boolean
    title?: string
    icon?: string
    width?: number | string
    collapsedWidth?: number | string
    /** 导航栏高度，默认为页面高度 '100vh' */
    height?: number | string
    /** 是否固定定位在页面左侧（position: fixed） */
    fixed?: boolean
    /** 是否浮动在页面上方（Overlay 模式：不占用页面排版空间；为 false 时占用固定页面空间） */
    floating?: boolean
    /** 悬浮岛边距（与视口四周边缘的间距，默认 12px） */
    inset?: number | string
    /** 固定定位时的顶部偏移量（默认 0） */
    top?: number | string
    /** 固定定位时的底部偏移量（默认 0） */
    bottom?: number | string
    color?: string
    defaultExpandedKeys?: Array<string | number>
    accordion?: boolean
    bordered?: boolean
    /** 主题模式 Dark/Light/auto */
    theme?: string
  },
  {
    'update:modelValue': (val: string | number) => void
    'update:collapsed': (val: boolean) => void
    select: (item: NavItem) => void
    toggle: (payload: { item: NavItem, expanded: boolean }) => void
    'brand-click': () => void
  },
  {
    header: (scope: { collapsed: boolean, toggleCollapse: () => void }) => any
    footer: (scope: { collapsed: boolean, toggleCollapse: () => void }) => any
    icon: (scope: { item: NavItem, active: boolean }) => any
    label: (scope: { item: NavItem, active: boolean }) => any
    default: (scope: { items: NavItem[], activeId: string | number, collapsed: boolean }) => any
  }
>

export declare const rizui_sidebar: typeof rizui_nav

/** 通知：info/warn/error + 倒计时进度条，Teleport 右上角排队 */
export declare const rizui_toast: DefineComponent<
  {
    type?: string
    title?: string
    hint?: string
    titleStyle?: Instyle
    hintStyle?: Instyle
    icon?: string
    iconStyle?: Instyle
    /** 宽度（如 420px） */
    width?: string
    /** 主题模式 Dark/Light/auto */
    theme?: string
    countdown?: number
    closable?: boolean
    showBar?: boolean
    color?: string
    teleport?: string
  },
  {
    close: (duration: number) => void
    click: () => void
  },
  {}
>

/* ═══════════════════════════ Control ═══════════════════════════ */

/** 开关：纯 CSS 开关，v-model + 主题色 */
export declare const rizui_switch: DefineComponent<{
  modelValue?: boolean
  color?: string
  disabled?: boolean
  /** 主题模式 Dark/Light/auto */
  theme?: string
}>

/** 复选框：方框勾选，v-model + 主题色，label 可左右 */
export declare const rizui_checkbox: DefineComponent<{
  modelValue?: boolean
  color?: string
  disabled?: boolean
  lLabel?: string
  lLabelStyle?: Instyle
  rLabel?: string
  rLabelStyle?: Instyle
  /** 主题模式 Dark/Light/auto */
  theme?: string
}>

/** 滑条：步进 + 拖拽 / 长按 */
export declare const rizui_slider: DefineComponent<{
  modelValue?: number
  min?: number
  max?: number
  step?: number
  color?: string
  display?: string
  displayValue?: string
  /** 主题模式 Dark/Light/auto */
  theme?: string
}>

/** 时间轴：拖拽定位 + 刻度线 */
export declare const rizui_timeline: DefineComponent<{
  modelValue?: number
  min?: number
  max?: number
  step?: number
  color?: string
  /** 主题模式 Dark/Light/auto */
  theme?: string
}>

/** 下拉选择器：点击外部自动关闭 */
export declare const rizui_dropdown: DefineComponent<{
  modelValue?: string | number
  options?: Array<{ value: string | number, label?: string, icon?: string } | string | number>
  color?: string
  /** 主题模式 Dark/Light/auto */
  theme?: string
}>

/** 颜色选择器：HSL 调色盘 + hex 输入 */
export declare const rizui_colorpicker: DefineComponent<{
  modelValue?: string
  color?: string
  teleport?: string
  /** 主题模式 Dark/Light/auto */
  theme?: string
}>

/** 按钮：主题色填充 / 通栏 / 左右图标 / 危险 */
export declare const rizui_button: DefineComponent<
  {
    text?: string
    textStyle?: Instyle
    lIcon?: string
    lIconStyle?: Instyle
    rIcon?: string
    rIconStyle?: Instyle
    primary?: boolean
    danger?: boolean
    long?: boolean
    color?: string
    disabled?: boolean
    type?: string
    /** 主题模式 Dark/Light/auto */
    theme?: string
  },
  { click: (e: MouseEvent) => void },
  {}
>

/** 长条进度条：支持普通进度与不定进度（indeterminate）模式 */
export declare const rizui_progress: DefineComponent<{
  modelValue?: number
  value?: number
  min?: number
  max?: number
  /** 是否为不定进度模式（未知进度/加载中） */
  indeterminate?: boolean
  /** 自定义颜色，支持 CSS 颜色值或渐变；为空跟随全局主题色 */
  color?: string
  /** 自定义轨道底色 */
  trackColor?: string
  /** 预设状态配色：'info' | 'success' | 'warn' | 'error' */
  status?: 'info' | 'success' | 'warn' | 'error' | string
  /** 进度条高度（默认 10） */
  height?: number | string
  /** 胶囊圆角 */
  rounded?: boolean
  /** 是否显示进度文本 */
  showText?: boolean
  /** 文本是否内嵌在进度条内 */
  textInside?: boolean
  /** 自定义文案覆盖 */
  text?: string
  /** 外部文本样式覆盖 */
  textStyle?: Instyle
  /** 文本格式化函数 */
  format?: (value: number, percentage: number) => string
  /** 主题模式 Dark/Light/auto */
  theme?: string
}>

/** 环形进度条：支持常规进度、不定进度（indeterminate）模式、渐变色与自定义中心内容 */
export declare const rizui_circle_progress: DefineComponent<
  {
    modelValue?: number
    value?: number
    min?: number
    max?: number
    /** 是否为不定进度模式（未知进度/加载中） */
    indeterminate?: boolean
    /** 环形直径（默认 120） */
    size?: number | string
    /** 线条宽度（默认 8） */
    strokeWidth?: number
    /** 环端样式：'round' | 'butt' | 'square' */
    strokeLinecap?: 'round' | 'butt' | 'square' | string
    /** 自定义颜色，支持 CSS 颜色值、数组或渐变；为空跟随全局主题色 */
    color?: string | string[]
    /** 自定义底环轨道颜色 */
    trackColor?: string
    /** 预设状态配色：'info' | 'success' | 'warn' | 'error' */
    status?: 'info' | 'success' | 'warn' | 'error' | string
    /** 是否顺时针绘制（默认 true） */
    clockwise?: boolean
    /** 是否显示中心文案 */
    showText?: boolean
    /** 自定义中心文案覆盖 */
    text?: string
    /** 中心文字样式覆盖 */
    textStyle?: Instyle
    /** 文本格式化函数 */
    format?: (value: number, percentage: number) => string
    /** 主题模式 Dark/Light/auto */
    theme?: string
  },
  {},
  { default: (scope: { value: number, percentage: number }) => any }
>

export declare const rizui_progress_circle: typeof rizui_circle_progress

/* ═══════════════════════════ Input ═══════════════════════════ */

/** 文本 / 数字输入框 */
export declare const rizui_text: DefineComponent<{
  modelValue?: string | number | null
  type?: string
  placeholder?: string
  color?: string
  plaintext?: boolean
  disabled?: boolean
  readonly?: boolean
  /** 主题模式 Dark/Light/auto */
  theme?: string
}>

/** Markdown 编辑器：顶栏工具栏 + 预览选项卡 */
export declare const rizui_mdeditor: DefineComponent<{
  modelValue?: string
  placeholder?: string
  color?: string
  disabled?: boolean
  readonly?: boolean
  height?: string
  default?: 'edit' | 'preview'
  /** 主题模式 Dark/Light/auto */
  theme?: string
}>

/* ═══════════════════════════ old（已弃用，保留兼容） ═══════════════════════════ */

export declare const Ct_Card: DefineComponent<Record<string, any>>
export declare const UiCard: DefineComponent<Record<string, any>>
export declare const UiConfigRow: DefineComponent<Record<string, any>>
export declare const UiSwitch: DefineComponent<Record<string, any>>
export declare const UiSlider: DefineComponent<Record<string, any>>
export declare const UiTimeline: DefineComponent<Record<string, any>>
export declare const UiDropdown: DefineComponent<Record<string, any>>
export declare const UiColorPicker: DefineComponent<Record<string, any>>
export declare const UiDialog: DefineComponent<Record<string, any>>
export declare const NotReadyOverlay: DefineComponent<Record<string, any>>
