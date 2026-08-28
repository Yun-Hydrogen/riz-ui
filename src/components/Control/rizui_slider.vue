<template>
  <div
    ref="rootRef"
    class="csl-root"
    :class="{ 'csl-dragging': isDragging }"
    :data-rizui-theme="themeAttr"
    :style="rootStyle"
    @mousedown.left="startDrag"
    @touchstart.prevent="startDrag"
  >
    <button class="csl-btn csl-minus">−</button>
    <span class="csl-value">{{ displayText }}</span>
    <button class="csl-btn csl-plus">+</button>
  </div>
</template>

<script setup>
import { computed, ref, onMounted, onBeforeUnmount } from 'vue'

const props = defineProps({
  /** v-model 绑定值 Slider value via v-model */
  modelValue:   { type: Number, default: 0 },
  /** 最小值 Min */
  min:          { type: Number, default: 0 },
  /** 最大值 Max */
  max:          { type: Number, default: 100 },
  /** 步进 Step */
  step:         { type: Number, default: 1 },
  /** 自定义颜色，空则跟随主题色 Custom color (defaults to theme) */
  color:        { type: String, default: '' },
  /** 显示单位文案 Display unit text */
  display:      { type: String, default: '' },
  /** 显示值覆盖 Display value override */
  displayValue: { type: String, default: '' },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:        { type: String, default: 'auto' }
})

const emit = defineEmits(['update:modelValue'])

const rootRef = ref(null)

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const cssColor = computed(() => props.color || 'var(--rizui_themecolor)')

const fillPct = computed(() => {
  const range = props.max - props.min
  if (range <= 0) return 0
  return Math.max(0, Math.min(100, ((props.modelValue - props.min) / range) * 100))
})

const rootStyle = computed(() => ({
  background: `linear-gradient(to right, color-mix(in srgb, ${cssColor.value} 20%, transparent) ${fillPct.value}%, transparent ${fillPct.value}%)`,
  borderColor: cssColor.value
}))

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})

function parseNum(v) {
  return props.step < 1 ? parseFloat(v) : parseInt(v)
}

function change(delta) {
  const raw = props.modelValue + delta * props.step
  const clamped = Math.max(props.min, Math.min(props.max, raw))
  const rounded = parseNum(clamped.toFixed(10))
  if (rounded !== props.modelValue) {
    emit('update:modelValue', rounded)
  }
}

const isDragging = ref(false)

function clientXToValue(cx) {
  if (!rootRef.value) return props.modelValue
  const r = rootRef.value.getBoundingClientRect()
  const pct = Math.max(0, Math.min(1, (cx - r.left) / r.width))
  const raw = props.min + pct * (props.max - props.min)
  const stepped = Math.round(raw / props.step) * props.step
  return parseNum(Math.max(props.min, Math.min(props.max, stepped)).toFixed(10))
}

function startDrag(e) {
  isDragging.value = true
  const target = e.target
  const isMinus = target.closest('.csl-minus')
  const isPlus  = target.closest('.csl-plus')

  if (isMinus) { change(-1); startRepeat(-1) }
  else if (isPlus) { change(1); startRepeat(1) }
  else {
    const val = clientXToValue(e.clientX || e.touches?.[0]?.clientX || 0)
    if (val !== props.modelValue) emit('update:modelValue', val)
  }
}

function onDragMove(e) {
  if (!isDragging.value) return
  stopRepeat()
  const val = clientXToValue(e.clientX || e.touches?.[0]?.clientX || 0)
  if (val !== props.modelValue) emit('update:modelValue', val)
}

function onDragEnd() { isDragging.value = false; stopRepeat() }

const repeatTimer = ref(null)
const REPEAT_DELAY = 400, REPEAT_RATE = 60

function startRepeat(dir) {
  stopRepeat()
  repeatTimer.value = setTimeout(() => {
    repeatTimer.value = setInterval(() => change(dir), REPEAT_RATE)
  }, REPEAT_DELAY)
}

function stopRepeat() {
  if (repeatTimer.value) {
    clearTimeout(repeatTimer.value)
    clearInterval(repeatTimer.value)
    repeatTimer.value = null
  }
}

onMounted(() => {
  document.addEventListener('mousemove', onDragMove)
  document.addEventListener('mouseup', onDragEnd)
  document.addEventListener('touchmove', onDragMove, { passive: false })
  document.addEventListener('touchend', onDragEnd)
})

onBeforeUnmount(() => {
  document.removeEventListener('mousemove', onDragMove)
  document.removeEventListener('mouseup', onDragEnd)
  document.removeEventListener('touchmove', onDragMove)
  document.removeEventListener('touchend', onDragEnd)
  stopRepeat()
})

const displayText = computed(() => {
  if (props.displayValue) return props.displayValue
  const v = props.step < 1 ? props.modelValue.toFixed(1) : props.modelValue
  return props.display ? `${v}${props.display}` : String(v)
})
</script>

<style scoped>
.csl-root {
  display: inline-flex;
  align-items: center;
  height: 26px;
  min-width: 160px;
  border-radius: 999px;
  border: 1.5px solid var(--rz-border);
  background: var(--rz-bg);
  overflow: hidden;
  user-select: none;
  cursor: col-resize;
}

.csl-btn {
  width: 28px;
  height: 100%;
  border: none;
  background: transparent;
  font-size: 14px;
  font-weight: 600;
  color: var(--rz-subtext);
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  font-family: inherit;
  line-height: 1;
  padding: 0;
  outline: none;
}

.csl-btn:hover {
  background: color-mix(in srgb, var(--rz-text) 6%, var(--rz-bg));
  color: var(--rz-text);
}

.csl-value {
  flex: 1;
  text-align: center;
  font-size: 13px;
  font-weight: 600;
  color: var(--rz-text);
  font-variant-numeric: tabular-nums;
  pointer-events: none;
}
</style>
