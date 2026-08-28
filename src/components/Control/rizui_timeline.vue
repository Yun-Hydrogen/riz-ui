<template>
  <div class="ctl-root" :data-rizui-theme="themeAttr" :style="rootStyle">
    <!-- 轨道（点击 / 拖拽定位） -->
    <div
      ref="trackRef"
      class="ctl-track"
      :class="{ 'ctl-dragging': isDragging }"
      @mousedown.left="startDrag"
      @touchstart.prevent="startDrag"
    >
      <div class="ctl-fill" :style="fillStyle"></div>

      <!-- 主刻度 -->
      <div
        v-for="t in ticks"
        :key="'t' + t"
        class="ctl-tick"
        :class="{ 'ctl-tick-minor': t % minorEvery !== 0 }"
        :style="{ left: tickPct(t) + '%' }"
      ></div>

      <!-- 指示器（倒三角 + 时间标签） -->
      <div class="ctl-indicator" :style="indicatorStyle">
        <span class="ctl-bubble">{{ displayTime }}</span>
        <svg class="ctl-arrow" viewBox="0 0 12 6" width="12" height="6">
          <path d="M0 0l6 6 6-6z" :fill="cssColor" />
        </svg>
      </div>
    </div>

    <!-- 底部时间刻度标签 -->
    <div class="ctl-labels">
      <span
        v-for="t in labelTicks"
        :key="'l' + t"
        class="ctl-label-tick"
        :style="{ left: tickPct(t) + '%' }"
      >{{ formatTick(t) }}</span>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, onMounted, onBeforeUnmount } from 'vue'

const props = defineProps({
  /** v-model 绑定时间值（秒）Time value in seconds via v-model */
  modelValue: { type: Number, default: 0 },
  /** 最小值 Min */
  min:        { type: Number, default: 0 },
  /** 最大值 Max */
  max:        { type: Number, default: 120 },
  /** 步进 Step */
  step:       { type: Number, default: 1 },
  /** 自定义颜色，空则跟随主题色 Custom color (defaults to theme) */
  color:      { type: String, default: '' },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:      { type: String, default: 'auto' }
})

const emit = defineEmits(['update:modelValue'])

const trackRef = ref(null)
const isDragging = ref(false)

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const cssColor = computed(() => props.color || 'var(--rizui_themecolor)')

/* ── 刻度生成 ── */
const minorEvery = computed(() => (props.max - props.min <= 60 ? 2 : 3))

const ticks = computed(() => {
  const arr = []
  const interval = props.max - props.min <= 60 ? 5 : 10
  for (let v = props.min; v <= props.max; v += interval) arr.push(v)
  return arr
})

const labelTicks = computed(() => {
  const arr = []
  const interval = props.max - props.min <= 60 ? 15 : 30
  for (let v = props.min; v <= props.max; v += interval) arr.push(v)
  return arr
})

function tickPct(v) {
  const range = props.max - props.min
  return range <= 0 ? 0 : ((v - props.min) / range) * 100
}

function formatTick(s) {
  const m = Math.floor(s / 60)
  const rs = s % 60
  return `${m}:${String(rs).padStart(2, '0')}`
}

/* ── 填充 / 指示器 ── */
const fillPct = computed(() => {
  const range = props.max - props.min
  return range <= 0 ? 0 : Math.max(0, Math.min(100, ((props.modelValue - props.min) / range) * 100))
})

/* 颜色变量通过内联 style 注入，空 color 时回退到全局主题色 */
const rootStyle = computed(() => ({
  '--ctl-color': cssColor.value
}))

const fillStyle = computed(() => ({
  width: fillPct.value + '%',
  background: `color-mix(in srgb, ${cssColor.value} 20%, transparent)`
}))

const indicatorStyle = computed(() => ({
  left: fillPct.value + '%'
}))

const displayTime = computed(() => {
  const s = Math.max(0, Math.round(props.modelValue || 0))
  return `${Math.floor(s / 60)}:${String(s % 60).padStart(2, '0')}`
})

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})

/* ── 拖拽逻辑 ── */
function parseNum(v) {
  return props.step < 1 ? parseFloat(v) : parseInt(v)
}

function clientXToValue(cx) {
  if (!trackRef.value) return props.modelValue
  const r = trackRef.value.getBoundingClientRect()
  const pct = Math.max(0, Math.min(1, (cx - r.left) / r.width))
  const raw = props.min + pct * (props.max - props.min)
  const stepped = Math.round(raw / props.step) * props.step
  return parseNum(Math.max(props.min, Math.min(props.max, stepped)).toFixed(10))
}

function startDrag(e) {
  isDragging.value = true
  const val = clientXToValue(e.clientX || e.touches?.[0]?.clientX || 0)
  if (val !== props.modelValue) emit('update:modelValue', val)
}

function onMove(e) {
  if (!isDragging.value) return
  const val = clientXToValue(e.clientX || e.touches?.[0]?.clientX || 0)
  if (val !== props.modelValue) emit('update:modelValue', val)
}

function onUp() {
  isDragging.value = false
}

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
.ctl-root {
  width: 100%;
  padding: 26px 12px 0;
  user-select: none;
}

/* 组件级强制暗色（theme="Dark"） */
.ctl-root[data-rizui-theme="dark"] {
  --rz-bg: var(--rz-dark-bg);
  --rz-border: var(--rz-dark-border);
  --rz-text: var(--rz-dark-text);
  --rz-subtext: var(--rz-dark-subtext);
}

/* 组件级强制亮色（theme="Light"） */
.ctl-root[data-rizui-theme="light"] {
  --rz-bg: #fff;
  --rz-border: #e8ecf2;
  --rz-text: #334;
  --rz-subtext: #99a;
}

/* ── 轨道 ── */
.ctl-track {
  position: relative;
  width: 100%;
  height: 32px;
  cursor: pointer;
}

.ctl-track::before {
  content: '';
  position: absolute;
  top: 13px;
  left: 0;
  right: 0;
  height: 6px;
  border-radius: 999px;
  background: var(--rz-border);
  box-shadow: inset 0 1px 2px color-mix(in srgb, var(--rz-text) 10%, transparent);
}

.ctl-fill {
  position: absolute;
  top: 13px;
  left: 0;
  height: 6px;
  border-radius: 999px 0 0 999px;
  transition: width 0.08s ease;
}

/* ── 刻度 ── */
.ctl-tick {
  position: absolute;
  top: 8px;
  width: 2px;
  height: 12px;
  border-radius: 1px;
  background: color-mix(in srgb, var(--rz-text) 22%, var(--rz-bg));
  transform: translateX(-50%);
}

.ctl-tick-minor {
  top: 11px;
  height: 6px;
  background: color-mix(in srgb, var(--rz-text) 14%, var(--rz-bg));
}

/* ── 指示器 ── */
.ctl-indicator {
  position: absolute;
  top: -14px;
  display: flex;
  flex-direction: column;
  align-items: center;
  transform: translateX(-50%);
  pointer-events: none;
  transition: left 0.08s ease;
}

.ctl-bubble {
  font-size: 11px;
  font-weight: 700;
  color: var(--rz-text);
  font-variant-numeric: tabular-nums;
  white-space: nowrap;
  background: var(--rz-bg);
  border: 1.5px solid var(--ctl-color);
  border-radius: 999px;
  padding: 1px 8px;
  box-shadow: 0 2px 6px var(--rz-shadow);
  margin-bottom: 2px;
}

.ctl-arrow {
  display: block;
  flex-shrink: 0;
  filter: drop-shadow(0 1px 2px rgba(0, 0, 0, 0.25));
  transition: transform 0.15s ease, filter 0.15s ease;
}

/* 拖拽/悬停时放大倒三角 + 发光 */
.ctl-dragging .ctl-arrow,
.ctl-track:hover .ctl-arrow {
  transform: scale(1.3);
  filter: drop-shadow(0 0 4px var(--ctl-color));
}

/* ── 底部时间刻度标签 ── */
.ctl-labels {
  position: relative;
  width: 100%;
  height: 16px;
  margin-top: 4px;
}

.ctl-label-tick {
  position: absolute;
  font-size: 10px;
  color: var(--rz-subtext);
  transform: translateX(-50%);
  font-variant-numeric: tabular-nums;
}
</style>