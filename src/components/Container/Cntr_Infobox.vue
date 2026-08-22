<template>
  <div class="inbx-root" :class="`inbx-${type}`">
    <!-- 左侧图标：自定义 icon 优先，否则按 type 预设 -->
    <div class="inbx-icon" :style="iconInstyle">
      <i :class="icon || defaultIcon"></i>
    </div>
    <!-- 文本 + 默认插槽 -->
    <div class="inbx-content">
      <div v-if="text" class="inbx-text" :style="textInstyle">{{ text }}</div>
      <slot />
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  type:        { type: String,  default: 'info' },
  text:        { type: String,  default: '' },
  textInstyle: { type: Object,  default: () => ({}) },
  icon:        { type: String,  default: '' },
  iconInstyle: { type: Object,  default: () => ({}) }
})

/* 未自定义 icon 时，按 type 自动选择默认图标（完整 FA class） */
const DEFAULT_ICONS = {
  info:  'fa-solid fa-circle-info',
  warn:  'fa-solid fa-triangle-exclamation',
  error: 'fa-solid fa-circle-exclamation'
}

const defaultIcon = computed(() => DEFAULT_ICONS[props.type] || DEFAULT_ICONS.info)
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
  color: #445;
  line-height: 1.6;
}

/* ── 预设 type 配色 ── */
.inbx-info  { --inbx-color: #4a9eff; --inbx-bg: #eaf3ff; }
.inbx-warn  { --inbx-color: #e6a23c; --inbx-bg: #fff7e6; }
.inbx-error { --inbx-color: #e05555; --inbx-bg: #fdecec; }

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
  color: #445;
  line-height: 1.6;
}</style>
