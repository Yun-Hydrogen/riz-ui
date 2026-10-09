<template>
  <Teleport :to="teleport">
    <Transition name="cdlg-pop" @after-leave="emit('update:show', false)">
      <div
        v-if="show"
        class="cdlg-overlay"
        :class="{ 'cdlg-anchored': isAnchored }"
        :data-rizui-theme="themeAttr"
        @click.self="onMaskClick"
      >
        <div class="cdlg-box" :style="boxStyle" v-bind="$attrs">
          <!-- 头部：图标 + 标题 + 关闭按钮 -->
          <div class="cdlg-header">
            <div v-if="icon" class="cdlg-icon" :style="iconStyle">
              <i :class="icon"></i>
            </div>
            <span class="cdlg-title" :style="titleStyle">{{ title }}</span>
            <button v-if="closable" class="cdlg-close" @click="close" type="button">
              <svg viewBox="0 0 24 24" width="12" height="12" stroke="currentColor" stroke-width="2.5" fill="none"><path d="M18 6L6 18M6 6l12 12"/></svg>
            </button>
          </div>

          <!-- 内容区 -->
          <div v-if="$slots.default" class="cdlg-body">
            <slot />
          </div>

          <!-- 底部按钮区 -->
          <div v-if="$slots.footer" class="cdlg-footer">
            <slot name="footer" />
          </div>

          <!-- 底部倒计时进度条：默认不启用 (showBar=false) -->
          <div v-if="showBar" class="cdlg-bar">
            <div
              class="cdlg-bar-inner"
              :class="{
                'cdlg-bar-full': !(countdown > 0) && progress === undefined && !indeterminate,
                'cdlg-bar-indeterminate': indeterminate
              }"
              :style="barInnerStyle"
            ></div>
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup>
import { computed, watch, onBeforeUnmount } from 'vue'

defineOptions({ inheritAttrs: false })

const props = defineProps({
  /** v-model:show 控制显隐 Show state via v-model:show */
  show:         { type: Boolean, default: false },
  /** 标题文字 Title text */
  title:        { type: String,  default: '' },
  /** 标题样式覆盖 Title style override */
  titleStyle: { type: Object,  default: () => ({}) },
  /** 标题左侧图标（完整 FA class）Title icon (full FA class) */
  icon:         { type: String,  default: '' },
  /** 图标样式覆盖 Icon style override */
  iconStyle:  { type: Object,  default: () => ({}) },
  /** 主题色，空则跟随 --rizui_themecolor Theme color */
  color:        { type: String,  default: '' },
  /** 挂载目标（选择器），默认 body Teleport target */
  teleport:     { type: String,  default: 'body' },
  /** 对话框宽度 Dialog width */
  width:        { type: String,  default: 'auto' },
  /** 显示关闭按钮 Show close button */
  closable:     { type: Boolean, default: true },
  /** 点击遮罩关闭 Close on mask click */
  maskClose:    { type: Boolean, default: true },
  /** 显示底部倒计时/进度条，默认 false（不启用）Show bottom countdown/progress bar */
  showBar:      { type: Boolean, default: false },
  /** 自动关闭倒计时秒数，0 为不自动关 Countdown seconds for auto-close (0 = never) */
  countdown:    { type: Number,  default: 0 },
  /** 自定义进度百分比 (0~100) Custom progress percentage (0~100) */
  progress:     { type: Number,  default: undefined },
  /** 不定进度模式（未知进度/加载中）Indeterminate progress mode */
  indeterminate:{ type: Boolean, default: false },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:       { type: String,  default: 'auto' }
})

const emit = defineEmits(['update:show', 'close', 'timeout'])

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})

const isAnchored = computed(() => props.teleport !== 'body')

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const cssColor = computed(() => props.color || 'var(--rizui_themecolor)')

const boxStyle = computed(() => ({
  width: props.width,
  '--cdlg-color': cssColor.value,
  '--cdlg-countdown': `${props.countdown}s`
}))

const barInnerStyle = computed(() => {
  if (props.progress !== undefined) {
    return {
      width: `${Math.max(0, Math.min(100, props.progress))}%`,
      animation: 'none',
      transition: 'width 0.3s cubic-bezier(0.4, 0, 0.2, 1)'
    }
  }
  return {}
})

let timer = null

function clearTimer() {
  if (timer) {
    clearTimeout(timer)
    timer = null
  }
}

function startTimer() {
  clearTimer()
  if (props.show && props.countdown > 0) {
    timer = setTimeout(() => {
      emit('timeout')
      close()
    }, props.countdown * 1000)
  }
}

watch(
  () => [props.show, props.countdown],
  ([newShow]) => {
    if (newShow) {
      startTimer()
    } else {
      clearTimer()
    }
  },
  { immediate: true }
)

onBeforeUnmount(() => {
  clearTimer()
})

function close() {
  clearTimer()
  emit('update:show', false)
  emit('close')
}

function onMaskClick() {
  if (props.maskClose) close()
}
</script>

<style scoped>
/* ── 进场 / 退场 ── */
.cdlg-pop-enter-active,
.cdlg-pop-leave-active {
  transition: opacity 0.22s ease;
}
.cdlg-pop-enter-from,
.cdlg-pop-leave-to {
  opacity: 0;
}

.cdlg-overlay {
  position: fixed;
  inset: 0;
  z-index: 9990;
  background: var(--rz-overlay);
  backdrop-filter: blur(2px);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40px;
}

.cdlg-anchored {
  position: absolute;
}

/* 组件级强制暗色（theme="Dark"） */
.cdlg-overlay[data-rizui-theme="dark"] {
  --rz-overlay: var(--rz-dark-overlay);
  --rz-bg: var(--rz-dark-bg);
  --rz-text: var(--rz-dark-text);
}

/* 组件级强制亮色（theme="Light"） */
.cdlg-overlay[data-rizui-theme="light"] {
  --rz-overlay: rgba(15, 23, 42, 0.45);
  --rz-bg: #fff;
  --rz-text: #334;
}

.cdlg-box {
  background: var(--rz-bg);
  border-radius: 16px;
  box-shadow: 0 12px 40px var(--rz-shadow);
  min-width: 280px;
  max-width: 90vw;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  position: relative;
}

/* ── 头部：图标 + 标题 + 关闭按钮 ── */
.cdlg-header {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 16px 20px 12px;
}

.cdlg-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  font-size: 16px;
  color: var(--cdlg-color);
}

.cdlg-title {
  flex: 1;
  min-width: 0;
  font-size: 15px;
  font-weight: 700;
  color: var(--rz-text);
}

.cdlg-close {
  width: 26px;
  height: 26px;
  border: none;
  border-radius: 50%;
  background: color-mix(in srgb, var(--rz-text) 8%, transparent);
  color: var(--rz-subtext);
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: filter 0.2s;
}

.cdlg-close:hover {
  background: var(--cdlg-color);
  color: #fff;
}

/* ── 内容区 ── */
.cdlg-body {
  padding: 0 20px 8px;
  font-size: 13px;
  color: var(--rz-text);
  line-height: 1.6;
  overflow-y: auto;
}

/* ── 底部按钮区 ── */
.cdlg-footer {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  padding: 12px 20px 16px;
}

/* ── 底部倒计时 / 进度条 ── */
.cdlg-bar {
  position: absolute;
  left: 0;
  bottom: 0;
  width: 100%;
  height: 3px;
  background: color-mix(in srgb, var(--cdlg-color) 35%, transparent);
  pointer-events: none;
}

.cdlg-bar-inner {
  height: 100%;
  width: 100%;
  background: var(--cdlg-color);
}

/* 倒计时自动收缩动画（仅在未指定固定进度、非全满、非不定进度时执行） */
.cdlg-bar-inner:not(.cdlg-bar-full):not(.cdlg-bar-indeterminate) {
  animation: cdlg-shrink var(--cdlg-countdown) linear forwards;
}

@keyframes cdlg-shrink {
  from { width: 100%; }
  to   { width: 0%; }
}

/* 不定进度模式（类似 toast / progress 流动加载） */
.cdlg-bar-inner.cdlg-bar-indeterminate {
  position: absolute;
  top: 0;
  bottom: 0;
  width: 35%;
  animation: cdlg-indeterminate 1.8s cubic-bezier(0.65, 0, 0.35, 1) infinite;
}

@keyframes cdlg-indeterminate {
  0% {
    left: -40%;
    width: 35%;
  }
  45% {
    left: 20%;
    width: 55%;
  }
  100% {
    left: 100%;
    width: 35%;
  }
}
</style>
