<template>
  <div class="ctl-root">
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

      <!-- 指示器（手柄 + 时间标签） -->
      <div class="ctl-indicator" :style="indicatorStyle">
        <span class="ctl-bubble">{{ displayTime }}</span>
        <span class="ctl-thumb"></span>
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
  modelValue: { type: Number, default: 0 },
  min:        { type: Number, default: 0 },
  max:        { type: Number, default: 120 },
  step:       { type: Number, default: 1 },
  color:      { type: String, default: '#39c5bb' }
})

const emit = defineEmits(['update:modelValue'])

const trackRef = ref(null)
const isDragging = ref(false)

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

const fillStyle = computed(() => ({
  width: fillPct.value + '%',
  background: `linear-gradient(90deg, ${props.color}55, ${props.color})`
}))

const indicatorStyle = computed(() => ({
  left: fillPct.value + '%'
}))

const displayTime = computed(() => {
  const s = Math.max(0, Math.round(props.modelValue || 0))
  return `${Math.floor(s / 60)}:${String(s % 60).padStart(2, '0')}`
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
  --ctl-color: v-bind('props.color');
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
  background: #e8ecf2;
  box-shadow: inset 0 1px 2px rgba(0, 0, 0, 0.05);
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
  background: #cfd6df;
  transform: translateX(-50%);
}

.ctl-tick-minor {
  top: 11px;
  height: 6px;
  background: #dde3ea;
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
  color: #334;
  font-variant-numeric: tabular-nums;
  white-space: nowrap;
  background: #fff;
  border: 1.5px solid var(--ctl-color);
  border-radius: 999px;
  padding: 1px 8px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.12);
  margin-bottom: 5px;
}

.ctl-thumb {
  width: 12px;
  height: 12px;
  border-radius: 50%;
  background: var(--ctl-color);
  border: 2.5px solid #fff;
  box-shadow: 0 0 0 1.5px var(--ctl-color), 0 2px 6px rgba(0, 0, 0, 0.2);
  transition: transform 0.15s ease, box-shadow 0.15s ease;
}

/* 拖拽时放大 + 外发光 */
.ctl-dragging .ctl-thumb,
.ctl-track:hover .ctl-thumb {
  transform: scale(1.35);
  box-shadow: 0 0 0 1.5px var(--ctl-color), 0 0 0 5px color-mix(in srgb, var(--ctl-color) 25%, transparent), 0 2px 8px rgba(0, 0, 0, 0.25);
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
  color: #aab;
  transform: translateX(-50%);
  font-variant-numeric: tabular-nums;
}
</style>