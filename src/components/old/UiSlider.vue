<template>
  <div
    ref="rootRef"
    class="uist-root"
    :class="{ 'uist-dragging': isDragging }"
    :style="rootStyle"
    @mousedown.left="startDrag"
    @touchstart.prevent="startDrag"
  >
    <button class="uist-btn uist-minus">−</button>
    <span class="uist-value">{{ displayText }}</span>
    <button class="uist-btn uist-plus">+</button>
  </div>
</template>

<script setup>
import { computed, ref, onMounted, onBeforeUnmount } from 'vue'

const props = defineProps({
  modelValue:   { type: Number, default: 0 },
  min:          { type: Number, default: 0 },
  max:          { type: Number, default: 100 },
  step:         { type: Number, default: 1 },
  color:        { type: String, default: '#39c5bb' },
  display:      { type: String, default: '' },
  displayValue: { type: String, default: '' }
})

const emit = defineEmits(['update:modelValue'])

const rootRef = ref(null)

const fillPct = computed(() => {
  const range = props.max - props.min
  if (range <= 0) return 0
  return Math.max(0, Math.min(100, ((props.modelValue - props.min) / range) * 100))
})

const rootStyle = computed(() => ({
  background: `linear-gradient(to right, ${props.color}33 ${fillPct.value}%, transparent ${fillPct.value}%)`,
  borderColor: props.color
}))

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
  const isMinus = target.closest('.uist-minus')
  const isPlus  = target.closest('.uist-plus')

  if (isMinus) {
    change(-1)
    startRepeat(-1)
  } else if (isPlus) {
    change(1)
    startRepeat(1)
  } else {
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

function onDragEnd() {
  isDragging.value = false
  stopRepeat()
}

const repeatTimer = ref(null)
const REPEAT_DELAY = 400
const REPEAT_RATE = 60

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
.uist-root {
  display: inline-flex;
  align-items: center;
  height: 26px;
  min-width: 160px;
  border-radius: 999px;
  border: 1.5px solid #d0d8e0;
  background: #fff;
  overflow: hidden;
  user-select: none;
  cursor: col-resize;
  transition: border-color 0.2s;
}

.uist-btn {
  width: 28px;
  height: 100%;
  border: none;
  background: transparent;
  font-size: 14px;
  font-weight: 600;
  color: #667;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: background 0.15s, color 0.15s;
  font-family: inherit;
  line-height: 1;
  padding: 0;
  outline: none;
}

.uist-btn:hover {
  background: rgba(0, 0, 0, 0.04);
  color: #334;
}

.uist-value {
  flex: 1;
  text-align: center;
  font-size: 13px;
  font-weight: 600;
  color: #445;
  font-variant-numeric: tabular-nums;
  pointer-events: none;
}
</style>
