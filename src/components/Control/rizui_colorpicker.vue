<template>
  <div class="ccp-root" :data-rizui-theme="themeAttr">
    <div class="ccp-swatch" @click="open">
      <span class="ccp-swatch-dot" :style="{ background: modelValue || '#ffffff' }"></span>
      <span class="ccp-swatch-hex">{{ modelValue || '#ffffff' }}</span>
    </div>

    <Teleport :to="teleport">
      <Transition name="ccp-dialog">
        <div
          v-if="show"
          class="ccp-overlay"
          :class="{ 'ccp-anchored': isAnchored }"
          :data-rizui-theme="themeAttr"
          @click.self="cancel"
        >
          <div class="ccp-dialog">
            <div class="ccp-header">
              <span>选取颜色</span>
              <button class="ccp-close" @click="cancel" type="button">
                <svg viewBox="0 0 24 24" width="12" height="12" stroke="currentColor" stroke-width="2.5" fill="none"><path d="M18 6L6 18M6 6l12 12"/></svg>
              </button>
            </div>

            <div
              class="ccp-sv"
              ref="svRef"
              :style="svBg"
              @mousedown="startSVDrag"
              @touchstart.prevent="startSVDrag"
            >
              <div class="ccp-sv-thumb" :style="{ left: svX + '%', top: svY + '%' }"></div>
            </div>

            <div
              class="ccp-hue"
              ref="hueRef"
              @mousedown="startHueDrag"
              @touchstart.prevent="startHueDrag"
            >
              <div class="ccp-hue-thumb" :style="{ left: huePct + '%' }"></div>
            </div>

            <div class="ccp-footer">
              <span class="ccp-preview" :style="{ background: tempColor }"></span>
              <rizui_text
                :model-value="tempColor"
                :color="props.color"
                placeholder="#ffffff"
                style="flex: 1; height: 28px; text-align: left; font-family: inherit"
                @update:model-value="onHexValue"
              />
              <button class="ccp-confirm" :style="confirmStyle" @click="confirm" type="button" title="确定">
                <svg viewBox="0 0 24 24" width="16" height="16" stroke="currentColor" stroke-width="3" fill="none"><path d="M20 6L9 17l-5-5"/></svg>
              </button>
            </div>
          </div>
        </div>
      </Transition>
    </Teleport>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from 'vue'
import rizui_text from '../Input/rizui_text.vue'

const props = defineProps({
  /** v-model 绑定颜色值 Color value via v-model */
  modelValue: { type: String, default: '#ffffff' },
  /** 自定义主题色，空则跟随全局主题色 Custom theme color */
  color:      { type: String, default: '' },
  /** 挂载目标（选择器），默认 body Teleport target */
  teleport:   { type: String, default: 'body' },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:      { type: String, default: 'auto' }
})

const emit = defineEmits(['update:modelValue'])

const isAnchored = computed(() => props.teleport !== 'body')

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const cssColor = computed(() => props.color || 'var(--rizui_themecolor)')

const show = ref(false)
const tempColor = ref('#ffffff')
const svRef = ref(null)
const hueRef = ref(null)
const hueVal = ref(0)
const huePct = ref(0)
const svX = ref(100)
const svY = ref(0)
let dragHue = false
let dragSV = false

const svBg = computed(() => {
  const base = hslToHex(hueVal.value, 100, 50)
  return { background: `linear-gradient(to right, #fff, ${base})` }
})

const confirmStyle = computed(() => ({
  background: cssColor.value,
  borderColor: cssColor.value
}))

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})

function hslToHex(h, s, l) {
  s /= 100; l /= 100
  const a = s * Math.min(l, 1 - l)
  const f = (n) => {
    const k = (n + h / 30) % 12
    return Math.round(255 * (l - a * Math.max(Math.min(k - 3, 9 - k, 1), -1)))
      .toString(16)
      .padStart(2, '0')
  }
  return `#${f(0)}${f(8)}${f(4)}`
}

function hexToHsl(hex) {
  let r = 0, g = 0, b = 0
  hex = hex.replace('#', '')
  if (hex.length === 3) {
    r = parseInt(hex[0] + hex[0], 16) / 255
    g = parseInt(hex[1] + hex[1], 16) / 255
    b = parseInt(hex[2] + hex[2], 16) / 255
  } else if (hex.length >= 6) {
    r = parseInt(hex.slice(0, 2), 16) / 255
    g = parseInt(hex.slice(2, 4), 16) / 255
    b = parseInt(hex.slice(4, 6), 16) / 255
  }
  const max = Math.max(r, g, b), min = Math.min(r, g, b)
  const l = (max + min) / 2
  let h2 = 0, s2 = 0
  if (max !== min) {
    const d = max - min
    s2 = l > 0.5 ? d / (2 - max - min) : d / (max + min)
    if (max === r) h2 = ((g - b) / d + (g < b ? 6 : 0)) / 6
    else if (max === g) h2 = ((b - r) / d + 2) / 6
    else h2 = ((r - g) / d + 4) / 6
  }
  h2 = Math.round(h2 * 360); if (h2 < 0) h2 += 360
  return { h: h2, s: Math.round(s2 * 100), l: Math.round(l * 100) }
}

function syncFromHex(hex) {
  const hsl = hexToHsl(hex)
  hueVal.value = hsl.h
  huePct.value = (hsl.h / 360) * 100
  svX.value = hsl.s
  const denom = 100 - hsl.s * 0.5
  svY.value = denom > 0 ? Math.max(0, Math.min(100, Math.round(100 - hsl.l * 100 / denom))) : 0
}

function updateFromHSL() {
  const s = svX.value
  const baseL = 100 - s * 0.5
  const l = Math.round(baseL * (100 - svY.value) / 100)
  tempColor.value = hslToHex(hueVal.value, s, l)
}

function open() {
  tempColor.value = props.modelValue || '#ffffff'
  syncFromHex(tempColor.value)
  show.value = true
}

function confirm() {
  emit('update:modelValue', tempColor.value)
  show.value = false
}

function cancel() {
  show.value = false
}

function onHexValue(v) {
  const val = String(v ?? '')
  tempColor.value = val
  if (/^#[0-9a-fA-F]{6}$/.test(val.trim())) syncFromHex(val.trim())
}

function hueFromX(cx) {
  const r = hueRef.value.getBoundingClientRect()
  const p = Math.max(0, Math.min(100, ((cx - r.left) / r.width) * 100))
  huePct.value = p
  hueVal.value = Math.round((p / 100) * 360)
  updateFromHSL()
}

function startHueDrag(e) {
  dragHue = true
  hueFromX(e.clientX || e.touches[0].clientX)
}

function svFromClient(cx, cy) {
  const r = svRef.value.getBoundingClientRect()
  svX.value = Math.max(0, Math.min(100, ((cx - r.left) / r.width) * 100))
  svY.value = Math.max(0, Math.min(100, ((cy - r.top) / r.height) * 100))
  updateFromHSL()
}

function startSVDrag(e) {
  dragSV = true
  svFromClient(e.clientX || e.touches[0].clientX, e.clientY || e.touches[0].clientY)
}

function onMove(e) {
  if (dragHue) hueFromX(e.clientX || e.touches[0].clientX)
  if (dragSV) svFromClient(e.clientX || e.touches[0].clientX, e.clientY || e.touches[0].clientY)
}

function onUp() { dragHue = false; dragSV = false }

onMounted(() => {
  document.addEventListener('mousemove', onMove)
  document.addEventListener('mouseup', onUp)
  document.addEventListener('touchmove', onMove, { passive: false })
  document.addEventListener('touchend', onUp)
})

onBeforeUnmount(() => {
  document.removeEventListener('mousemove', onMove)
  document.removeEventListener('mouseup', onUp)
  document.removeEventListener('touchmove', onMove)
  document.removeEventListener('touchend', onUp)
})
</script>

<style scoped>
.ccp-root {
  display: inline-flex;
  align-items: center;
}

/* 组件级强制暗色（theme="Dark"） */
.ccp-root[data-rizui-theme="dark"],
.ccp-overlay[data-rizui-theme="dark"] {
  --rz-bg: var(--rz-dark-bg);
  --rz-border: var(--rz-dark-border);
  --rz-text: var(--rz-dark-text);
  --rz-subtext: var(--rz-dark-subtext);
  --rz-shadow: var(--rz-dark-shadow);
}

/* 组件级强制亮色（theme="Light"） */
.ccp-root[data-rizui-theme="light"],
.ccp-overlay[data-rizui-theme="light"] {
  --rz-bg: #fff;
  --rz-border: #e8ecf2;
  --rz-text: #334;
  --rz-subtext: #99a;
  --rz-shadow: rgba(30, 41, 59, 0.08);
}

.ccp-swatch {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 4px 10px 4px 4px;
  border: 1.5px solid var(--rz-border);
  border-radius: 999px;
  background: var(--rz-bg);
  cursor: pointer;
  user-select: none;
}

.ccp-swatch:hover {
  border-color: v-bind('cssColor');
  box-shadow: 0 0 0 3px rgba(0, 0, 0, 0.06);
}

.ccp-swatch-dot {
  width: 24px;
  height: 24px;
  border-radius: 50%;
  border: 2px solid rgba(0, 0, 0, 0.08);
  flex-shrink: 0;
}

.ccp-swatch-hex {
  font-size: 12px;
  color: var(--rz-text);
  font-family: inherit;
}

.ccp-overlay {
  position: fixed;
  inset: 0;
  z-index: 9999;
  background: transparent;
  display: flex;
  align-items: center;
  justify-content: center;
}

.ccp-anchored {
  position: absolute;
}

.ccp-dialog-enter-active,
.ccp-dialog-leave-active {
  transition: opacity 0.25s ease, transform 0.25s ease;
}
.ccp-dialog-enter-from,
.ccp-dialog-leave-to {
  opacity: 0;
  transform: translateY(30px);
}

.ccp-dialog {
  background: var(--rz-bg);
  border-radius: 16px;
  padding: 16px 18px;
  box-shadow: 0 16px 48px var(--rz-shadow);
  width: 280px;
}

.ccp-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 14px;
  font-weight: 700;
  color: var(--rz-text);
  margin-bottom: 12px;
}

.ccp-close {
  width: 24px;
  height: 24px;
  border: none;
  border-radius: 50%;
  background: color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--rz-subtext);
  transition: all 0.2s;
}

.ccp-close:hover {
  background: #e55;
  color: #fff;
}

.ccp-sv {
  position: relative;
  width: 100%;
  height: 150px;
  border-radius: 8px;
  overflow: hidden;
  cursor: crosshair;
  margin-bottom: 10px;
}

.ccp-sv::after {
  content: '';
  position: absolute;
  inset: 0;
  background: linear-gradient(to top, #000, transparent);
}

.ccp-sv-thumb {
  position: absolute;
  width: 14px;
  height: 14px;
  border: 2px solid #fff;
  border-radius: 50%;
  transform: translate(-50%, -50%);
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.3);
  pointer-events: none;
  z-index: 2;
}

.ccp-hue {
  position: relative;
  width: 100%;
  height: 14px;
  border-radius: 7px;
  background: linear-gradient(to right, #f00, #ff0, #0f0, #0ff, #00f, #f0f, #f00);
  cursor: pointer;
  margin-bottom: 14px;
}

.ccp-hue-thumb {
  position: absolute;
  top: -2px;
  width: 14px;
  height: 18px;
  border: 2px solid #fff;
  border-radius: 4px;
  transform: translateX(-50%);
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.3);
  pointer-events: none;
  background: transparent;
}

.ccp-footer {
  display: flex;
  align-items: center;
  gap: 10px;
}

.ccp-preview {
  width: 28px;
  height: 28px;
  border-radius: 6px;
  border: 1px solid var(--rz-border);
  flex-shrink: 0;
}

.ccp-confirm {
  width: 28px;
  height: 28px;
  border: none;
  border-radius: 50%;
  color: #fff;
  cursor: pointer;
  font-family: inherit;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: opacity 0.2s;
}

.ccp-confirm:hover {
  opacity: 0.85;
}
</style>
