<template>
  <label class="csw-root" :class="{ 'csw-disabled': disabled }" :data-rizui-theme="themeAttr">
    <input
      type="checkbox"
      :checked="modelValue"
      :disabled="disabled"
      @change="$emit('update:modelValue', $event.target.checked)"
    />
    <span class="csw-track" :style="trackStyle"></span>
  </label>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  /** v-model 绑定开关状态 Switch state via v-model */
  modelValue: { type: Boolean, default: false },
  /** 自定义颜色，空则跟随主题色 Custom color (defaults to theme) */
  color:      { type: String,  default: '' },
  /** 禁用 Disabled */
  disabled:   { type: Boolean, default: false },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:      { type: String,  default: 'auto' }
})

defineEmits(['update:modelValue'])

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const cssColor = computed(() => props.color || 'var(--rizui_themecolor)')

const trackStyle = computed(() => ({
  background: props.modelValue ? cssColor.value : undefined
}))

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})
</script>

<style scoped>
.csw-root {
  position: relative;
  display: inline-block;
  width: 44px;
  height: 24px;
  cursor: pointer;
  flex-shrink: 0;
  user-select: none;
}

.csw-disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.csw-root input {
  opacity: 0;
  width: 0;
  height: 0;
  position: absolute;
}

.csw-track {
  position: absolute;
  inset: 0;
  border-radius: 12px;
  background: color-mix(in srgb, var(--rz-text) 30%, var(--rz-bg));
}

.csw-track::after {
  content: '';
  position: absolute;
  top: 2px;
  left: 2px;
  width: 20px;
  height: 20px;
  border-radius: 50%;
  background: #fff;
  transition: transform 0.2s;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.15);
}

.csw-root input:checked ~ .csw-track::after {
  transform: translateX(20px);
}
</style>
