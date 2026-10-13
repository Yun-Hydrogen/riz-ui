<template>
  <div
    class="rcp-root"
    :data-rizui-theme="themeAttr"
    :style="rootStyle"
    :class="{
      'rcp-indeterminate-mode': indeterminate
    }"
    role="progressbar"
    :aria-valuenow="indeterminate ? undefined : currentValue"
    :aria-valuemin="min"
    :aria-valuemax="max"
  >
    <svg
      class="rcp-svg"
      :class="{
        'rcp-svg-indeterminate': indeterminate && clockwise,
        'rcp-svg-indeterminate-reverse': indeterminate && !clockwise
      }"
      viewBox="0 0 100 100"
    >
      <defs v-if="gradientColors.length > 1">
        <linearGradient :id="gradientId" x1="0%" y1="0%" x2="100%" y2="100%">
          <stop
            v-for="(stop, idx) in gradientColors"
            :key="idx"
            :offset="stop.offset"
            :stop-color="stop.color"
          />
        </linearGradient>
      </defs>

      <!-- 底层轨道圆环 -->
      <circle
        class="rcp-track"
        :cx="50"
        :cy="50"
        :r="radius"
        :stroke-width="strokeWidthCalculated"
        :style="trackStyle"
        fill="none"
      />

      <!-- 进度圆环 -->
      <circle
        class="rcp-bar"
        :class="{ 'rcp-bar-indeterminate': indeterminate }"
        :cx="50"
        :cy="50"
        :r="radius"
        :stroke="barStroke"
        :stroke-width="strokeWidthCalculated"
        :stroke-linecap="strokeLinecap"
        :stroke-dasharray="indeterminate ? undefined : perimeter"
        :stroke-dashoffset="indeterminate ? undefined : dashOffset"
        :transform="transformOrigin"
        fill="none"
      />
    </svg>

    <!-- 中心内容区（文字/图标/插槽） -->
    <div v-if="showText || $slots.default" class="rcp-content" :style="textStyle">
      <slot :value="currentValue" :percentage="percentage">
        <div v-if="indeterminate" class="rcp-indeterminate-text">
          <i class="fa-solid fa-spinner fa-spin" style="margin-bottom: 4px;"></i>
          <span>{{ text || '加载中' }}</span>
        </div>
        <div v-else class="rcp-default-text">
          {{ displayText }}
        </div>
      </slot>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

let uid = 0

const props = defineProps({
  /** 当前进度值（v-model 绑定）Progress value via v-model */
  modelValue:    { type: Number, default: 0 },
  /** 进度值（与 modelValue 兼容）Progress value alias */
  value:         { type: Number, default: undefined },
  /** 最小值 Min value */
  min:           { type: Number, default: 0 },
  /** 最大值 Max value */
  max:           { type: Number, default: 100 },
  /** 不定进度模式（用于加载中、处理中等未知进度的场景）Indeterminate mode */
  indeterminate: { type: Boolean, default: false },
  /** 环形进度条尺寸直径（数字转为 px，如 120，或传入字符串 '120px'）Diameter size */
  size:          { type: [Number, String], default: 120 },
  /** 环形线条粗细（相对单位，默认 8）Stroke width */
  strokeWidth:   { type: Number, default: 8 },
  /** 环端样式：'round' 圆角 / 'butt' 平头 / 'square' 方头 Linecap style */
  strokeLinecap: { type: String, default: 'round' },
  /** 自定义颜色，支持普通色、RGB、Hex 或渐变字符串/数组 Custom color or gradient */
  color:         { type: [String, Array], default: '' },
  /** 自定义轨道底色 Custom track color */
  trackColor:    { type: String, default: '' },
  /** 预设状态配色：'info' | 'success' | 'warn' | 'error' Preset status color */
  status:        { type: String, default: '' },
  /** 是否顺时针 Clockwise */
  clockwise:     { type: Boolean, default: true },
  /** 是否显示中心文案 Show center text */
  showText:      { type: Boolean, default: true },
  /** 自定义文本覆盖 Custom text override */
  text:          { type: String, default: '' },
  /** 中心文字内联样式覆盖 Text style override */
  textStyle:     { type: Object, default: () => ({}) },
  /** 文本格式化函数：(value, percentage) => string */
  format:        { type: Function, default: null },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:         { type: String, default: 'auto' }
})

const currentUid = ++uid
const gradientId = `rcp-gradient-${currentUid}`

/* 当前有效数值 */
const currentValue = computed(() => {
  return props.value !== undefined ? props.value : props.modelValue
})

/* 计算百分比 0 ~ 100 */
const percentage = computed(() => {
  if (props.indeterminate) return 0
  const range = props.max - props.min
  if (range <= 0) return 0
  const pct = ((currentValue.value - props.min) / range) * 100
  return Math.max(0, Math.min(100, Math.round(pct * 10) / 10))
})

/* 格式化展示文本 */
const displayText = computed(() => {
  if (props.text) return props.text
  if (typeof props.format === 'function') {
    return props.format(currentValue.value, percentage.value)
  }
  return `${percentage.value}%`
})

/* 预设状态色映射 */
const STATUS_COLORS = {
  info:    'var(--rizui_themecolor)',
  success: '#34c759',
  warn:    '#e6a23c',
  error:   '#e05555'
}

/* 解析可能存在的渐变色配置 */
const gradientColors = computed(() => {
  if (Array.isArray(props.color)) {
    const len = props.color.length
    if (len === 0) return []
    return props.color.map((c, i) => ({
      color: c,
      offset: `${(i / (len - 1 || 1)) * 100}%`
    }))
  }
  if (typeof props.color === 'string' && props.color.includes('gradient')) {
    // 粗略解析 gradient 字符串中的颜色，例如 linear-gradient(90deg, #36d1dc, #5b86e5)
    const matches = props.color.match(/(#[0-9a-fA-F]{3,8}|rgba?\([^)]+\)|hsl\([^)]+\)|[a-zA-Z]+)/g) || []
    const validColors = matches.filter(m => !['linear', 'radial', 'gradient', 'deg', 'to', 'right', 'left', 'top', 'bottom'].includes(m.toLowerCase()))
    if (validColors.length > 1) {
      return validColors.map((c, i) => ({
        color: c,
        offset: `${(i / (validColors.length - 1)) * 100}%`
      }))
    }
  }
  return []
})

/* 主题与状态主色 */
const effectiveColor = computed(() => {
  if (gradientColors.value.length > 1) {
    return `url(#${gradientId})`
  }
  if (typeof props.color === 'string' && props.color) {
    return props.color
  }
  if (props.status && STATUS_COLORS[props.status]) {
    return STATUS_COLORS[props.status]
  }
  return 'var(--rizui_themecolor)'
})

const barStroke = computed(() => effectiveColor.value)

/* 几何尺寸计算 */
const strokeWidthCalculated = computed(() => Math.max(1, Math.min(40, props.strokeWidth)))
const radius = computed(() => (100 - strokeWidthCalculated.value) / 2)
const perimeter = computed(() => 2 * Math.PI * radius.value)

const dashOffset = computed(() => {
  if (props.indeterminate) {
    return perimeter.value * 0.75
  }
  return perimeter.value * (1 - percentage.value / 100)
})

const transformOrigin = computed(() => {
  if (props.clockwise) {
    return 'rotate(-90 50 50)'
  }
  // 逆时针
  return 'rotate(-90 50 50) scale(1, -1) translate(0, -100)'
})

/* 尺寸格式化 */
const formattedSize = computed(() => {
  return typeof props.size === 'number' ? `${props.size}px` : props.size
})

const rootStyle = computed(() => ({
  width: formattedSize.value,
  height: formattedSize.value
}))

const trackStyle = computed(() => {
  if (props.trackColor) {
    return { stroke: props.trackColor }
  }
  return {}
})

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})
</script>

<style scoped>
.rcp-root {
  position: relative;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  box-sizing: border-box;

  /* 亮色语义变量 */
  --rcp-track: var(--rz-border, #e8ecf2);
  --rcp-text: var(--rz-text, #334);
  --rcp-subtext: var(--rz-subtext, #99a);
}

.rcp-svg {
  width: 100%;
  height: 100%;
  overflow: visible;
}

/* 底层轨道 */
.rcp-track {
  stroke: var(--rcp-track);
  transition: stroke 0.3s ease;
}

/* 进度填充圆环 */
.rcp-bar {
  transition: stroke-dashoffset 0.3s cubic-bezier(0.4, 0, 0.2, 1), stroke 0.3s ease;
}

/* 不定进度旋转动效 */
.rcp-svg-indeterminate {
  animation: rcp-spin 1.8s linear infinite;
  transform-origin: center;
}

.rcp-svg-indeterminate-reverse {
  animation: rcp-spin-reverse 1.8s linear infinite;
  transform-origin: center;
}

.rcp-bar-indeterminate {
  animation: rcp-dash 1.8s ease-in-out infinite;
  transition: none;
}

@keyframes rcp-spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

@keyframes rcp-spin-reverse {
  0% {
    transform: rotate(360deg);
  }
  100% {
    transform: rotate(0deg);
  }
}

/* 450 的 gap 远大于圆环周长 (~289)，确保在任何时刻圆环上只存在单个连贯的弧线，彻底解决滑条分离分段问题 */
@keyframes rcp-dash {
  0% {
    stroke-dasharray: 1, 450;
    stroke-dashoffset: 0;
  }
  50% {
    stroke-dasharray: 180, 450;
    stroke-dashoffset: -40;
  }
  100% {
    stroke-dasharray: 1, 450;
    stroke-dashoffset: -289;
  }
}

/* 中心内容区 */
.rcp-content {
  position: absolute;
  inset: 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
  padding: 6px;
  user-select: none;
  pointer-events: none;
}

.rcp-default-text {
  font-size: 18px;
  font-weight: 700;
  color: var(--rcp-text);
  font-variant-numeric: tabular-nums;
  line-height: 1.2;
}

.rcp-indeterminate-text {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  font-size: 12px;
  color: var(--rcp-subtext);
}

/* ── 组件级强制暗色（theme="Dark"） ── */
.rcp-root[data-rizui-theme="dark"] {
  --rcp-track: var(--rz-dark-border, #323d4b);
  --rcp-text: var(--rz-dark-text, #e6ebf2);
  --rcp-subtext: var(--rz-dark-subtext, #93a1b3);
}

/* ── 组件级强制亮色（theme="Light"） ── */
.rcp-root[data-rizui-theme="light"] {
  --rcp-track: #e8ecf2;
  --rcp-text: #334;
  --rcp-subtext: #99a;
}
</style>
