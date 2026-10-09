<template>
  <div
    class="cpr-root"
    :data-rizui-theme="themeAttr"
    :class="{
      'cpr-with-text': showText && !textInside,
      'cpr-indeterminate-mode': indeterminate
    }"
  >
    <!-- 进度条轨道 -->
    <div
      class="cpr-track"
      :style="trackStyle"
      role="progressbar"
      :aria-valuenow="indeterminate ? undefined : currentValue"
      :aria-valuemin="min"
      :aria-valuemax="max"
    >
      <!-- 进度填充条 -->
      <div
        class="cpr-bar"
        :class="{
          'cpr-bar-indeterminate': indeterminate
        }"
        :style="barStyle"
      >
        <!-- 内嵌文案 -->
        <span
          v-if="showText && textInside && !indeterminate"
          class="cpr-text-inside"
        >
          <slot :value="currentValue" :percentage="percentage">
            {{ displayText }}
          </slot>
        </span>
      </div>
    </div>

    <!-- 外部文案（右侧） -->
    <div
      v-if="showText && !textInside"
      class="cpr-text-outside"
      :style="textStyle"
    >
      <slot :value="currentValue" :percentage="percentage">
        <span v-if="indeterminate" class="cpr-indeterminate-text">
          <i class="fa-solid fa-spinner fa-spin" style="margin-right: 4px;"></i>{{ text || '加载中...' }}
        </span>
        <span v-else>{{ displayText }}</span>
      </slot>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

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
  /** 自定义颜色，支持 CSS 颜色值或渐变；为空跟随全局主题色 Custom color */
  color:         { type: String, default: '' },
  /** 自定义轨道底色 Custom track background color */
  trackColor:    { type: String, default: '' },
  /** 预设状态配色：'info' | 'success' | 'warn' | 'error' Preset status color */
  status:        { type: String, default: '' },
  /** 进度条高度（数字转为 px，或传入带单位字符串如 '14px'）Height */
  height:        { type: [Number, String], default: 10 },
  /** 是否为胶囊圆角 Rounded capsule border radius */
  rounded:       { type: Boolean, default: true },
  /** 是否显示进度文本文字 Show text */
  showText:      { type: Boolean, default: false },
  /** 文本是否内嵌在进度条内部（建议 height >= 14px）Display text inside bar */
  textInside:    { type: Boolean, default: false },
  /** 自定义文本内容覆盖 Custom text override */
  text:          { type: String, default: '' },
  /** 外部文本内联样式覆盖 Text style override */
  textStyle:     { type: Object, default: () => ({}) },
  /** 文本格式化函数：(value, percentage) => string */
  format:        { type: Function, default: null },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:         { type: String, default: 'auto' }
})

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

/* 进度条前景填充色 */
const cssBarColor = computed(() => {
  if (props.color) return props.color
  if (props.status && STATUS_COLORS[props.status]) {
    return STATUS_COLORS[props.status]
  }
  return 'var(--rizui_themecolor)'
})

/* 轨道高度格式化 */
const formattedHeight = computed(() => {
  return typeof props.height === 'number' ? `${props.height}px` : props.height
})

/* 轨道样式 */
const trackStyle = computed(() => {
  const styles = {
    height: formattedHeight.value,
    borderRadius: props.rounded ? '999px' : '4px'
  }
  if (props.trackColor) {
    styles.background = props.trackColor
  }
  return styles
})

/* 填充条样式 */
const barStyle = computed(() => {
  const styles = {
    background: cssBarColor.value,
    borderRadius: props.rounded ? '999px' : '4px'
  }
  if (!props.indeterminate) {
    styles.width = `${percentage.value}%`
  }
  return styles
})

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})
</script>

<style scoped>
.cpr-root {
  display: flex;
  align-items: center;
  width: 100%;
  box-sizing: border-box;

  /* 亮色模式变量 */
  --cpr-track-bg: var(--rz-bg-soft, rgba(248, 250, 252, 0.6));
  --cpr-border: var(--rz-border, #e8ecf2);
  --cpr-text: var(--rz-text, #334);
  --cpr-subtext: var(--rz-subtext, #99a);
}

/* 带有右侧文案时的间距 */
.cpr-with-text {
  gap: 12px;
}

/* 进度条轨道 */
.cpr-track {
  position: relative;
  flex: 1;
  width: 100%;
  background: var(--cpr-track-bg);
  border: 1px solid var(--cpr-border);
  box-sizing: border-box;
  overflow: hidden;
  box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.05);
}

/* 填充条基础 */
.cpr-bar {
  position: relative;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: flex-end;
  box-sizing: border-box;
  transition: width 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  min-width: 0;
}

/* 不定进度模式（Indeterminate） */
.cpr-bar-indeterminate {
  position: absolute;
  top: 0;
  bottom: 0;
  width: 35%;
  border-radius: inherit;
  animation: cpr-indeterminate-anim 1.8s cubic-bezier(0.65, 0, 0.35, 1) infinite;
  box-shadow: 0 0 10px color-mix(in srgb, currentColor 30%, transparent);
}

@keyframes cpr-indeterminate-anim {
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

/* 内嵌文字 */
.cpr-text-inside {
  font-size: 10px;
  font-weight: 700;
  line-height: 1;
  color: #fff;
  padding: 0 6px;
  white-space: nowrap;
  user-select: none;
  text-shadow: 0 1px 2px rgba(0, 0, 0, 0.3);
}

/* 外部（右侧）文字 */
.cpr-text-outside {
  font-size: 13px;
  font-weight: 600;
  color: var(--cpr-text);
  font-variant-numeric: tabular-nums;
  white-space: nowrap;
  user-select: none;
  min-width: 44px;
  text-align: right;
}

.cpr-indeterminate-text {
  font-size: 12px;
  color: var(--cpr-subtext);
}

/* ── 组件级强制暗色（theme="Dark"） ── */
.cpr-root[data-rizui-theme="dark"] {
  --cpr-track-bg: var(--rz-dark-bg-soft, rgba(30, 38, 48, 0.9));
  --cpr-border: var(--rz-dark-border, #323d4b);
  --cpr-text: var(--rz-dark-text, #e6ebf2);
  --cpr-subtext: var(--rz-dark-subtext, #93a1b3);
}

/* ── 组件级强制亮色（theme="Light"） ── */
.cpr-root[data-rizui-theme="light"] {
  --cpr-track-bg: rgb(248, 250, 252, 0.6);
  --cpr-border: #e8ecf2;
  --cpr-text: #334;
  --cpr-subtext: #99a;
}
</style>

