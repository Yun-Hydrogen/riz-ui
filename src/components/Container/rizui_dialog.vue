<template>
  <Teleport :to="teleport">
    <Transition name="cdlg-pop">
      <div
        v-if="show"
        class="cdlg-overlay"
        :class="{ 'cdlg-anchored': isAnchored }"
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
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup>
import { computed } from 'vue'

defineOptions({ inheritAttrs: false })

const props = defineProps({
  show:         { type: Boolean, default: false },
  title:        { type: String,  default: '' },
  titleStyle: { type: Object,  default: () => ({}) },
  icon:         { type: String,  default: '' },
  iconStyle:  { type: Object,  default: () => ({}) },
  color:        { type: String,  default: '' },
  teleport:     { type: String,  default: 'body' },
  width:        { type: String,  default: 'auto' },
  closable:     { type: Boolean, default: true },
  maskClose:    { type: Boolean, default: true }
})

const emit = defineEmits(['update:show'])

const isAnchored = computed(() => props.teleport !== 'body')

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const cssColor = computed(() => props.color || 'var(--rizui_themecolor)')

const boxStyle = computed(() => ({
  width: props.width,
  '--cdlg-color': cssColor.value
}))

function close() {
  emit('update:show', false)
}

function onMaskClick() {
  if (props.maskClose) close()
}
</script>

<style scoped>
.cdlg-overlay {
  position: fixed;
  inset: 0;
  z-index: 9990;
  background: rgba(255, 255, 255, 0.25);
  backdrop-filter: blur(2px);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40px;
}

.cdlg-anchored {
  position: absolute;
}

.cdlg-pop-enter-active,
.cdlg-pop-leave-active {
  transition: opacity 0.2s ease, transform 0.2s ease;
}
.cdlg-pop-enter-from,
.cdlg-pop-leave-to {
  opacity: 0;
  transform: scale(0.92);
}

.cdlg-box {
  background: #fff;
  border-radius: 16px;
  box-shadow: 0 12px 40px rgba(0, 0, 0, 0.18);
  min-width: 280px;
  max-width: 90vw;
  display: flex;
  flex-direction: column;
  overflow: hidden;
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
  color: #334;
}

.cdlg-close {
  width: 26px;
  height: 26px;
  border: none;
  border-radius: 50%;
  background: #f0f2f5;
  color: #888;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: all 0.2s;
}

.cdlg-close:hover {
  background: var(--cdlg-color);
  color: #fff;
}

/* ── 内容区 ── */
.cdlg-body {
  padding: 0 20px 8px;
  font-size: 13px;
  color: #667;
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
</style>
