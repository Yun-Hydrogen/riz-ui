<template>
  <label class="cchk-root" :class="{ 'cchk-disabled': disabled }" :data-rizui-theme="themeAttr">
    <input
      type="checkbox"
      :checked="modelValue"
      :disabled="disabled"
      @change="$emit('update:modelValue', $event.target.checked)"
    />
    <span v-if="lLabel" class="cchk-label cchk-label-l" :style="lLabelStyle">{{ lLabel }}</span>
    <span class="cchk-box" :style="boxStyle">
      <svg class="cchk-tick" viewBox="0 0 12 9" width="11" height="8">
        <path
          d="M1 4.5L4.5 8L11 1"
          stroke="currentColor"
          stroke-width="2.2"
          fill="none"
          stroke-linecap="round"
          stroke-linejoin="round"
        />
      </svg>
    </span>
    <span v-if="rLabel" class="cchk-label cchk-label-r" :style="rLabelStyle">{{ rLabel }}</span>
  </label>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  /** v-model 绑定值 Checked state via v-model */
  modelValue:   { type: Boolean, default: false },
  /** 自定义颜色，空则跟随主题色 Custom color (defaults to theme) */
  color:        { type: String,  default: '' },
  /** 禁用 Disabled */
  disabled:     { type: Boolean, default: false },
  /** 左侧标签文字 Left label */
  lLabel:       { type: String,  default: '' },
  /** 左标签样式覆盖 Left label style override */
  lLabelStyle:  { type: Object,  default: () => ({}) },
  /** 右侧标签文字 Right label */
  rLabel:       { type: String,  default: '' },
  /** 右标签样式覆盖 Right label style override */
  rLabelStyle:  { type: Object,  default: () => ({}) },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:        { type: String,  default: 'auto' }
})

defineEmits(['update:modelValue'])

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const boxStyle = computed(() => ({
  '--cchk-color': props.modelValue
    ? (props.color || 'var(--rizui_themecolor)')
    : undefined
}))

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})
</script>

<style scoped>
.cchk-root {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
  user-select: none;
  flex-shrink: 0;
}

.cchk-disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.cchk-root input {
  opacity: 0;
  width: 0;
  height: 0;
  position: absolute;
}

.cchk-box {
  position: relative;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 18px;
  height: 18px;
  border: 1.5px solid var(--rz-border);
  border-radius: 5px;
  background: var(--rz-bg);
}

.cchk-tick {
  color: #fff;
  opacity: 0;
  transform: scale(0.4);
  transition: opacity 0.15s, transform 0.15s;
}

.cchk-root input:checked ~ .cchk-box {
  background: var(--cchk-color, #39c5bb);
  border-color: var(--cchk-color, #39c5bb);
}

.cchk-root input:checked ~ .cchk-box .cchk-tick {
  opacity: 1;
  transform: scale(1);
}

.cchk-label {
  font-size: 13px;
  color: var(--rz-text);
  line-height: 1.4;
}
</style>
