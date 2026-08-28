<template>
  <div class="inpt-wrap" :class="{ 'inpt-wrap-plain': plaintext }" :data-rizui-theme="themeAttr" :style="rootStyle">
    <textarea
      v-if="plaintext"
      class="inpt-root inpt-plain"
      :value="modelValue"
      :placeholder="placeholder"
      :disabled="disabled"
      :readonly="readonly"
      rows="4"
      @input="$emit('update:modelValue', $event.target.value)"
    ></textarea>
    <input
      v-else
      class="inpt-root"
      :type="type"
      :value="modelValue"
      :placeholder="placeholder"
      :disabled="disabled"
      :readonly="readonly"
      @input="$emit('update:modelValue', type === 'number' ? ($event.target.value === '' ? null : Number($event.target.value)) : $event.target.value)"
      @keydown.enter="onEnter"
    />
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  /** v-model 绑定值 Input value via v-model */
  modelValue:   { type: [String, Number], default: '' },
  /** 输入类型 text / number Input type */
  type:         { type: String,  default: 'text' },
  /** 占位提示文字 Placeholder */
  placeholder:  { type: String,  default: '' },
  /** 自定义颜色，空则跟随主题色 Custom color (defaults to theme) */
  color:        { type: String,  default: '' },
  /** 多行纯文本模式（textarea）Multiline plaintext mode */
  plaintext:    { type: Boolean, default: false },
  /** 禁用 Disabled */
  disabled:     { type: Boolean, default: false },
  /** 只读 Readonly */
  readonly:     { type: Boolean, default: false },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:        { type: String,  default: 'auto' }
})

const emit = defineEmits(['update:modelValue', 'enter'])

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const rootStyle = computed(() => ({
  '--inpt-color': props.color || 'var(--rizui_themecolor)'
}))

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})

/* Enter：触发 enter 事件，默认释放聚焦（blur） */
function onEnter(e) {
  emit('enter', props.modelValue)
  e.target.blur()
}
</script>

<style scoped>
/* ── 包装层：承载聚焦时主题色边框「中轴向两侧展开」动画 ── */
.inpt-wrap {
  position: relative;
  display: inline-block;
  width: 100%;
  border-radius: 999px;
  text-align: center;
}

.inpt-wrap-plain {
  display: block;
  border-radius: 10px;
}

.inpt-root {
  width: 100%;
  height: 100%;
  padding: 5px 12px;
  border: 1.5px solid var(--rz-border);
  border-radius: 999px;
  font-size: 12px;
  text-align: inherit;
  outline: none;
  font-family: inherit;
  color: var(--rz-text);
  background: var(--rz-bg);
  box-sizing: border-box;
}

/* 组件级强制暗色（theme="Dark"） */
.inpt-wrap[data-rizui-theme="dark"] {
  --rz-bg: var(--rz-dark-bg);
  --rz-border: var(--rz-dark-border);
  --rz-text: var(--rz-dark-text);
  --rz-subtext: var(--rz-dark-subtext);
}

/* 组件级强制亮色（theme="Light"） */
.inpt-wrap[data-rizui-theme="light"] {
  --rz-bg: #fff;
  --rz-border: #e8ecf2;
  --rz-text: #334;
  --rz-subtext: #99a;
}

/* ── 聚焦动画：完整圆角主题色边框从中轴往两侧展开并蔓延发光；失焦反向收回 ── */
.inpt-wrap::before {
  content: '';
  position: absolute;
  inset: 0;
  border-radius: inherit;
  border: 2px solid var(--inpt-color);
  pointer-events: none;
  opacity: 0;
  filter: drop-shadow(0 0 6px color-mix(in srgb, var(--inpt-color) 55%, transparent));
  mask-image: linear-gradient(#000, #000);
  -webkit-mask-image: linear-gradient(#000, #000);
  mask-repeat: no-repeat;
  -webkit-mask-repeat: no-repeat;
  mask-size: 0% 100%;
  -webkit-mask-size: 0% 100%;
  mask-position: center;
  -webkit-mask-position: center;
  transition: mask-size 0.35s ease, -webkit-mask-size 0.35s ease, opacity 0.2s ease;
}

.inpt-wrap:focus-within::before {
  mask-size: 100% 100%;
  -webkit-mask-size: 100% 100%;
  opacity: 1;
}

.inpt-root::placeholder {
  color: color-mix(in srgb, var(--rz-text) 55%, var(--rz-bg));
}

.inpt-root:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.inpt-root:read-only {
  background: color-mix(in srgb, var(--rz-text) 4%, var(--rz-bg));
  color: var(--rz-subtext);
}

/* 隐藏 number 输入框的上下箭头 */
.inpt-root::-webkit-inner-spin-button,
.inpt-root::-webkit-outer-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

.inpt-root {
  -moz-appearance: textfield;
  appearance: textfield;
}

/* ── plaintext 多行模式 ── */
.inpt-plain {
  display: block;
  width: 100%;
  border-radius: 10px;
  text-align: left;
  line-height: 1.6;
  resize: vertical;
  min-height: 80px;
  padding: 8px 12px;
}

/* ── 滚动条：主题色半透明 ── */
.inpt-root::-webkit-scrollbar {
  width: 8px;
  height: 8px;
}

.inpt-root::-webkit-scrollbar-track {
  background: transparent;
}

.inpt-root::-webkit-scrollbar-thumb {
  background: color-mix(in srgb, var(--inpt-color) 28%, transparent);
  border-radius: 999px;
}

.inpt-root::-webkit-scrollbar-thumb:hover {
  background: color-mix(in srgb, var(--inpt-color) 48%, transparent);
}

.inpt-root {
  scrollbar-width: thin;
  scrollbar-color: color-mix(in srgb, var(--inpt-color) 28%, transparent) transparent;
}
</style>