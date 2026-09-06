<template>
  <label class="uis-root" :class="{ 'uis-disabled': disabled }">
    <input
      type="checkbox"
      :checked="modelValue"
      :disabled="disabled"
      @change="$emit('update:modelValue', $event.target.checked)"
    />
    <span class="uis-track" :style="trackStyle"></span>
  </label>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  modelValue: { type: Boolean, default: false },
  color:      { type: String, default: '' },
  disabled:   { type: Boolean, default: false }
})

defineEmits(['update:modelValue'])

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const cssColor = computed(() => props.color || 'var(--rizui_themecolor)')

const trackStyle = computed(() => ({
  background: props.modelValue ? cssColor.value : undefined
}))
</script>

<style scoped>
.uis-root {
  position: relative;
  display: inline-block;
  width: 44px;
  height: 24px;
  cursor: pointer;
  flex-shrink: 0;
  user-select: none;
}

.uis-disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.uis-root input {
  opacity: 0;
  width: 0;
  height: 0;
  position: absolute;
}

.uis-track {
  position: absolute;
  inset: 0;
  border-radius: 12px;
  background: #ccc;
  transition: background 0.2s, transform 0.2s;
}

.uis-track::after {
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

.uis-root input:checked ~ .uis-track::after {
  transform: translateX(20px);
}
</style>
