<template>
  <div class="cfgr-row" :class="{ 'cfgr-stack': stack, 'cfgr-disabled': disabled }" :data-rizui-theme="themeAttr">
    <!-- 头部：图标 + label/hint（图标仅跨 label/hint 两行） -->
    <div class="cfgr-head">
      <div v-if="icon" class="cfgr-icon" :style="iconStyle">
        <i :class="icon"></i>
      </div>
      <div class="cfgr-head-text">
        <div v-if="label" class="cfgr-label" :style="labelStyle">{{ label }}</div>
        <div v-if="hint" class="cfgr-hint" :style="hintStyle">{{ hint }}</div>
      </div>
      <!-- 非 stack 模式控件在右侧 -->
      <div v-if="!stack" class="cfgr-slot-inner" :class="{ 'cfgr-slot-disabled': disabled }">
        <slot/>
      </div>
    </div>
    <!-- stack 模式控件在头部下方独立一行 -->
    <div v-if="stack" class="cfgr-slot-wrap">
      <div class="cfgr-slot-inner" :class="{ 'cfgr-slot-disabled': disabled }">
        <slot/>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  /** 左栏标签文字 Label text */
  label: {type: String, default: ''},
  /** 标签样式覆盖 Label style override */
  labelStyle: {type: Object, default: () => ({})},
  /** 标签下方小字提示 Hint text */
  hint: {type: String, default: ''},
  /** 提示样式覆盖 Hint style override */
  hintStyle: {type: Object, default: () => ({})},
  /** 左侧图标（完整 FA class）Left icon (full FA class) */
  icon: {type: String, default: ''},
  /** 图标样式覆盖 Icon style override */
  iconStyle: {type: Object, default: () => ({})},
  /** 控件换到标签下方独立一行 Stack content below label */
  stack: {type: Boolean, default: false},
  /** 禁用整行 Disable the whole row */
  disabled: {type: Boolean, default: false},
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme: {type: String, default: 'auto'}
})

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})
</script>

<style scoped>
.cfgr-row {
  padding: 12px 0;
  border-bottom: 1px solid var(--rz-border);
}

/* 组件级强制暗色（theme="Dark"） */
.cfgr-row[data-rizui-theme="dark"] {
  --rz-border: var(--rz-dark-border);
  --rz-text: var(--rz-dark-text);
  --rz-subtext: var(--rz-dark-subtext);
  --rz-icon: var(--rz-dark-icon);
}

/* 组件级强制亮色（theme="Light"） */
.cfgr-row[data-rizui-theme="light"] {
  --rz-border: #e8ecf2;
  --rz-text: #334;
  --rz-subtext: #99a;
  --rz-icon: #99a;
}

/* ── 头部行：图标 + label/hint（横向排列，图标跨 label/hint 两行） ── */
.cfgr-head {
  display: flex;
  align-items: center;
  gap: 14px;
}

.cfgr-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  width: 36px;
  font-size: 18px;
  color: var(--rz-icon);
}

.cfgr-head-text {
  flex: 1;
  min-width: 0;
}

.cfgr-label {
  font-size: 14px;
  color: var(--rz-text);
  line-height: 1.4;
  margin-bottom: 2px;
}

.cfgr-hint {
  font-size: 11px;
  color: var(--rz-subtext);
  overflow-wrap: break-word;
  line-height: 1.4;
}

/* ── stack 模式：控件在头部下方独立一行，不影响头部图标高度 ── */
.cfgr-slot-wrap {
  margin-top: 8px;
}

.cfgr-slot-inner {
  display: flex;
  align-items: center;
  flex: 1;
  min-width: 0;
  justify-content: flex-end;
}

.cfgr-stack .cfgr-slot-inner {
  display: block;
}

.cfgr-slot-disabled {
  opacity: 0.45;
  pointer-events: none;
  user-select: none;
}
</style>