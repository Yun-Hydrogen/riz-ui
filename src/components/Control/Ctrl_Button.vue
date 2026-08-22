<template>
  <button
    class="cbtn-root"
    :class="{
      'cbtn-primary': primary,
      'cbtn-danger': danger,
      'cbtn-long': long,
      'cbtn-disabled': disabled
    }"
    :style="rootStyle"
    :type="type"
    :disabled="disabled"
    @click="$emit('click', $event)"
  >
    <i v-if="lIcon" class="cbtn-icon" :class="lIcon" :style="lIconInstyle"></i>
    <span v-if="text" class="cbtn-text" :style="textInstyle">{{ text }}</span>
    <i v-if="rIcon" class="cbtn-icon" :class="rIcon" :style="rIconInstyle"></i>
  </button>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  text:         { type: String,  default: '' },
  textInstyle:  { type: Object,  default: () => ({}) },
  lIcon:        { type: String,  default: '' },
  lIconInstyle: { type: Object,  default: () => ({}) },
  rIcon:        { type: String,  default: '' },
  rIconInstyle: { type: Object,  default: () => ({}) },
  primary:      { type: Boolean, default: false },
  danger:       { type: Boolean, default: false },
  long:         { type: Boolean, default: false },
  color:        { type: String,  default: '' },
  disabled:     { type: Boolean, default: false },
  type:         { type: String,  default: 'button' }
})

defineEmits(['click'])

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const rootStyle = computed(() => ({
  '--cbtn-color': props.color || 'var(--rizui_themecolor)'
}))
</script>

<style scoped>
.cbtn-root {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 6px;
  padding: 6px 16px;
  border: 1.5px solid #d0d8e0;
  border-radius: 999px;
  background: #fff;
  color: #445;
  font-size: 12px;
  font-weight: 600;
  font-family: inherit;
  cursor: pointer;
  outline: none;
  white-space: nowrap;
  user-select: none;
  transition: filter 0.2s, background 0.2s, border-color 0.2s, color 0.2s;
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
