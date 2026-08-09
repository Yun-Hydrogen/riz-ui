<template>
  <div class="uxt-root">
    <div
      ref="trackRef"
      class="uxt-track"
      @mousedown.left="startDrag"
      @touchstart.prevent="startDrag"
    >
      <div class="uxt-fill" :style="fillStyle"></div>
      <div
        v-for="t in ticks"
        :key="t"
        class="uxt-tick"
        :style="{ left: tickPct(t) + '%' }"
      ></div>
      <div class="uxt-indicator" :style="indicatorStyle">
        <svg class="uxt-arrow" viewBox="0 0 12 6" width="10" height="5">
          <path d="M0 0l6 6 6-6z" :fill="props.color" />
        </svg>
        <span class="uxt-label">{{ displayTime }}</span>
      </div>
    </div>
    <div class="uxt-labels">
      <span
        v-for="t in labelTicks"
        :key="t"
        class="uxt-label-tick"
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

const ticks = computed(() => {
  const arr = []
  const interval = props.max - props.min <= 60 ? 10 : 30
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

const fillPct = computed(() => {
  const range = props.max - props.min
  return range <= 0 ? 0 : Math.max(0, Math.min(100, ((props.modelValue - props.min) / range) * 100))
})

const fillStyle = computed(() => ({
  width: fillPct.value + '%',
  background: props.color + '33'
}))

const indicatorStyle = computed(() => ({
  left: fillPct.value + '%'
}))

const displayTime = computed(() => {
  const s = Math.max(0, Math.round(props.modelValue || 0))
  return `${Math.floor(s / 60)}:${String(s % 60).padStart(2, '0')}`
})

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
.uxt-root {
  width: 100%;
  padding: 0 12px;
  user-select: none;
}

.uxt-track {
  position: relative;
  width: 100%;
  height: 28px;
  cursor: pointer;
}

.uxt-track::before {
  content: '';
  position: absolute;
  top: 10px;
  left: 0;
  right: 0;
  height: 4px;
  border-radius: 2px;
  background: #e8ecf2;
}

.uxt-fill {
  position: absolute;
  top: 10px;
  left: 0;
  height: 4px;
  border-radius: 2px;
  transition: width 0.08s;
}

.uxt-tick {
  position: absolute;
  top: 8px;
  width: 1px;
  height: 8px;
  background: #cfd6df;
  transform: translateX(-50%);
}

.uxt-indicator {
  position: absolute;
  top: -2px;
  display: flex;
  flex-direction: column;
  align-items: center;
  transform: translateX(-50%);
  pointer-events: none;
  transition: left 0.08s;
}

.uxt-arrow {
  flex-shrink: 0;
  margin-bottom: -1px;
}

.uxt-label {
  font-size: 11px;
  font-weight: 700;
  color: #334;
  font-variant-numeric: tabular-nums;
  white-space: nowrap;
  margin-top: 2px;
  background: #fff;
  padding: 0 3px;
  border-radius: 3px;
}

.uxt-labels {
  position: relative;
  width: 100%;
  height: 16px;
}

.uxt-label-tick {
  position: absolute;
  font-size: 10px;
  color: #aab;
  transform: translateX(-50%);
  font-variant-numeric: tabular-nums;
}
</style>
