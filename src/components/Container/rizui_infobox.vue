<template>
  <div class="inbx-root" :class="`inbx-${type}`" :style="inbxStyle" :data-rizui-theme="themeAttr">
    <!-- 左侧图标：自定义 icon 优先，否则按 type 预设 -->
    <div class="inbx-icon" :style="iconStyle">
      <i :class="icon || defaultIcon"></i>
    </div>
    <!-- 文本 + 默认插槽（stack=auto：有 text 且带 slot 时，slot 排在 text 下方） -->
    <div class="inbx-content">
      <div v-if="text" class="inbx-text" :style="textStyle">{{ text }}</div>
      <div v-if="hasSlot" class="inbx-slot" :class="{ 'inbx-slot-stack': isStack }">
        <slot />
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, useSlots } from 'vue'

const props = defineProps({
  /** 预设类型：info / warn / error Preset type */
  type:        { type: String,  default: 'info' },
  /** 提示文字 Text content */
  text:        { type: String,  default: '' },
  /** 文字样式覆盖 Text style override */
  textStyle: { type: Object, default: () => ({}) },
  /** 左侧图标（完整 FA class）Icon (full FA class) */
  icon:        { type: String,  default: '' },
  /** 图标样式覆盖 Icon style override */
  iconStyle: { type: Object, default: () => ({}) },
  /** 自定义颜色（仅 info 生效，空则跟随主题色）Custom color (info only) */
  color:       { type: String,  default: '' },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:       { type: String,  default: 'auto' }
})

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})

const slots = useSlots()
const hasSlot = computed(() => !!slots.default)
/* 有 text 且带插槽时，插槽排到 text 下方（类似 Cfgrow stack） */
const isStack = computed(() => !!props.text && hasSlot.value)

/* 未自定义 icon 时，按 type 自动选择默认图标（完整 FA class） */
const DEFAULT_ICONS = {
  info:  'fa-solid fa-circle-info',
  warn:  'fa-solid fa-triangle-exclamation',
  error: 'fa-solid fa-circle-exclamation'
}

const defaultIcon = computed(() => DEFAULT_ICONS[props.type] || DEFAULT_ICONS.info)

/* 颜色：info 跟随主题色，color prop 仅对 info 生效（自定义）；warn/error 用预设色 */
const TYPE_COLORS = { warn: '#e6a23c', error: '#e05555' }
const cssColor = computed(() => {
  if (props.type === 'info') return props.color || 'var(--rizui_themecolor)'
  return TYPE_COLORS[props.type] || 'var(--rizui_themecolor)'
})
/* 背景：主题色 14% + 全局表面色（浅色=白 / 暗色=深底），自动适配明暗 */
const cssBg = computed(() => `color-mix(in srgb, ${cssColor.value} 14%, var(--rz-bg))`)
const inbxStyle = computed(() => ({ '--inbx-color': cssColor.value, '--inbx-bg': cssBg.value }))
</script>

<style scoped>
.inbx-root {
  display: flex;
  align-items: flex-start;
  gap: 10px;
  width: 100%;
  padding: 12px 14px;
  border-radius: 10px;
  border: 1.5px dashed var(--inbx-color);
  background: var(--inbx-bg);
  box-sizing: border-box;
  margin: 12px 0;
  font-size: 13px;
  color: var(--rz-text);
  line-height: 1.6;
}

/* 组件级强制暗色（theme="Dark"） */
.inbx-root[data-rizui-theme="dark"] {
  --rz-bg: var(--rz-dark-bg);
  --rz-text: var(--rz-dark-text);
}

/* 组件级强制亮色（theme="Light"） */
.inbx-root[data-rizui-theme="light"] {
  --rz-bg: #fff;
  --rz-text: #334;
}

.inbx-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  font-size: 16px;
  color: var(--inbx-color);
  line-height: 1;
  margin-top: 2px;
}

.inbx-content {
  flex: 1;
  min-width: 0;
}

.inbx-text {
  font-size: 13px;
  color: var(--rz-text);
  line-height: 1.6;
}

/* ── stack 模式：slot 在 text 下方独立一行 ── */
.inbx-slot-stack {
  margin-top: 8px;
  padding-top: 8px;
  border-top: 1px dashed color-mix(in srgb, var(--inbx-color) 35%, transparent);
}</style>
