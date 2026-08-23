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
export declare const Cntr_Card: DefineComponent<{
  title?: string
  titleInstyle?: Instyle
  desc?: string
  descInstyle?: Instyle
  icon?: string
  iconInstyle?: Instyle
}>

/** 配置行：左侧标签 + 右侧控件，支持 stack 布局 */
export declare const Cntr_Cfgrow: DefineComponent<{
  label?: string
  labelInstyle?: Instyle
  hint?: string
  hintInstyle?: Instyle
  icon?: string
  iconInstyle?: Instyle
  stack?: boolean
  disabled?: boolean
}>

/** 提示框：虚线边框 + info/warn/error 预设配色 */
export declare const Cntr_Infobox: DefineComponent<{
  type?: string
  text?: string
  textInstyle?: Instyle
  icon?: string
  iconInstyle?: Instyle
}>

/** 对话框：Teleport + 遮罩关闭 + 图标 */
export declare const Ctnr_Dialog: DefineComponent<
  {
    show?: boolean
    title?: string
    titleInstyle?: Instyle
    icon?: string
    iconInstyle?: Instyle
    color?: string
    teleport?: string
    width?: string
    closable?: boolean
    maskClose?: boolean
  },
  {},
  { default: () => any, footer: () => any }
>

/** 通知：info/warn/error + 倒计时进度条，Teleport 右上角排队 */
export declare const Ctnr_Toast: DefineComponent<
  {
    type?: string
    title?: string
    hint?: string
    titleInstyle?: Instyle
    hintInstyle?: Instyle
    icon?: string
    iconInstyle?: Instyle
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
export declare const Ctrl_Switch: DefineComponent<{
  modelValue?: boolean
  color?: string
  disabled?: boolean
}>

/** 滑条：步进 + 拖拽 / 长按 */
export declare const Ctrl_Slider: DefineComponent<{
  modelValue?: number
  min?: number
  max?: number
  step?: number
  color?: string
  display?: string
  displayValue?: string
}>

/** 时间轴：拖拽定位 + 刻度线 */
export declare const Ctrl_Timeline: DefineComponent<{
  modelValue?: number
  min?: number
  max?: number
  step?: number
  color?: string
}>

/** 下拉选择器：点击外部自动关闭 */
export declare const Ctrl_Dropdown: DefineComponent<{
  modelValue?: string | number
  options?: Array<{ value: string | number, label?: string, icon?: string } | string | number>
  color?: string
}>

/** 颜色选择器：HSL 调色盘 + hex 输入 */
export declare const Ctrl_ColorPicker: DefineComponent<{
  modelValue?: string
  color?: string
  teleport?: string
}>

/** 按钮：主题色填充 / 通栏 / 左右图标 / 危险 */
export declare const Ctrl_Button: DefineComponent<
  {
    text?: string
    textInstyle?: Instyle
    lIcon?: string
    lIconInstyle?: Instyle
    rIcon?: string
    rIconInstyle?: Instyle
    primary?: boolean
    danger?: boolean
    long?: boolean
    color?: string
    disabled?: boolean
    type?: string
  },
  { click: (e: MouseEvent) => void },
  {}
>

/* ═══════════════════════════ Input ═══════════════════════════ */

/** 文本 / 数字输入框 */
export declare const Inpt_Text: DefineComponent<{
  modelValue?: string | number | null
  type?: string
  placeholder?: string
  color?: string
  plaintext?: boolean
  disabled?: boolean
  readonly?: boolean
}>

/** Markdown 编辑器：顶栏工具栏 + 预览选项卡 */
export declare const Inpt_mdeditor: DefineComponent<{
  modelValue?: string
  placeholder?: string
  color?: string
  disabled?: boolean
  readonly?: boolean
  height?: string
  default?: 'edit' | 'preview'
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
