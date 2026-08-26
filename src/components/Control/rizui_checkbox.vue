<template>
  <label class="cchk-root" :class="{ 'cchk-disabled': disabled }">
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
  modelValue:   { type: Boolean, default: false },
  color:        { type: String,  default: '' },
  disabled:     { type: Boolean, default: false },
  lLabel:       { type: String,  default: '' },
  lLabelStyle:  { type: Object,  default: () => ({}) },
  rLabel:       { type: String,  default: '' },
  rLabelStyle:  { type: Object,  default: () => ({}) }
})

defineEmits(['update:modelValue'])

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const boxStyle = computed(() => ({
  '--cchk-color': props.modelValue
    ? (props.color || 'var(--rizui_themecolor)')
    : undefined
}))
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
  border: 1.5px solid #d0d8e0;
  border-radius: 5px;
  background: #fff;
  transition: background 0.2s, border-color 0.2s;
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
  color: #445;
  line-height: 1.4;
}
</style>
