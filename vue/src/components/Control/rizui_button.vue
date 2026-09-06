<template>
  <!-- 按钮：需要打开链接时请用 @click="openURL(url)"（组件库导出，见 /demo/open-url） -->
  <button
    class="cbtn-root"
    :class="{
      'cbtn-primary': primary,
      'cbtn-danger': danger,
      'cbtn-long': long,
      'cbtn-disabled': disabled
    }"
    :data-rizui-theme="themeAttr"
    :style="rootStyle"
    :type="type"
    :disabled="disabled || undefined"
    @click="$emit('click', $event)"
  >
    <i v-if="lIcon" class="cbtn-icon" :class="lIcon" :style="lIconStyle"></i>
    <span v-if="text" class="cbtn-text" :style="textStyle">{{ text }}</span>
    <i v-if="rIcon" class="cbtn-icon" :class="rIcon" :style="rIconStyle"></i>
  </button>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  /** 按钮文字 Button text */
  text:         { type: String,  default: '' },
  /** 文字样式覆盖 Text style override */
  textStyle:  { type: Object,  default: () => ({}) },
  /** 左侧图标（完整 FA class）Left icon (full FA class) */
  lIcon:        { type: String,  default: '' },
  /** 左图标样式覆盖 Left icon style override */
  lIconStyle: { type: Object,  default: () => ({}) },
  /** 右侧图标（完整 FA class）Right icon (full FA class) */
  rIcon:        { type: String,  default: '' },
  /** 右图标样式覆盖 Right icon style override */
  rIconStyle: { type: Object,  default: () => ({}) },
  /** 主题色填充 Primary filled style */
  primary:      { type: Boolean, default: false },
  /** 危险红色 Danger red style */
  danger:       { type: Boolean, default: false },
  /** 通栏（占满宽度）Full width */
  long:         { type: Boolean, default: false },
  /** 自定义颜色，空则跟随主题色 Custom color (defaults to theme) */
  color:        { type: String,  default: '' },
  /** 禁用 Disabled */
  disabled:     { type: Boolean, default: false },
  /** 原生按钮类型 Native button type */
  type:         { type: String,  default: 'button' },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:        { type: String,  default: 'auto' }
})

defineEmits(['click'])

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const rootStyle = computed(() => ({
  '--cbtn-color': props.color || 'var(--rizui_themecolor)'
}))

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})
</script>

<style scoped>
.cbtn-root {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 6px;
  padding: 6px 16px;
  border: 1.5px solid var(--rz-border);
  border-radius: 999px;
  background: var(--rz-bg);
  color: var(--rz-text);
  font-size: 12px;
  font-weight: 600;
  font-family: inherit;
  cursor: pointer;
  outline: none;
  white-space: nowrap;
  user-select: none;
  transition: filter 0.2s;
}

.cbtn-root:hover {
  border-color: var(--cbtn-color);
  color: var(--cbtn-color);
}

.cbtn-root:active {
  filter: brightness(0.92);
}

/* ── primary：背景填充跟随主题色 ── */
.cbtn-primary {
  background: var(--cbtn-color);
  border-color: var(--cbtn-color);
  color: #fff;
}

.cbtn-primary:hover {
  filter: brightness(1.08);
  color: #fff;
}

/* ── danger：危险 / 警告按钮（红色系） ── */
.cbtn-danger {
  background: #e05555;
  border-color: #e05555;
  color: #fff;
}

.cbtn-danger:hover {
  filter: brightness(1.08);
  color: #fff;
  border-color: #ff0000;
}

/* ── long：宽度撑满父容器 ── */
.cbtn-long {
  width: 100%;
}

/* ── 禁用态 ── */
.cbtn-disabled,
.cbtn-root:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  filter: none;
  pointer-events: none;
}

.cbtn-icon {
  font-size: 12px;
  line-height: 1;
}

.cbtn-text {
  line-height: 1;
}
</style>
