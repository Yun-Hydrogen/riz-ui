<template>
  <div ref="rootRef" class="cdd-root" :class="{ 'cdd-open': isOpen }" :data-rizui-theme="themeAttr">
    <button class="cdd-trigger" @click="toggle" type="button">
      <span>{{ selectedLabel }}</span>
      <svg class="cdd-arrow" viewBox="0 0 12 7" width="10" height="6">
        <path
          d="M1 1l5 5 5-5"
          stroke="currentColor"
          stroke-width="1.5"
          fill="none"
          stroke-linecap="round"
        />
      </svg>
    </button>
    <Transition name="cdd-pop">
      <div v-if="isOpen" class="cdd-drop">
        <button
          v-for="opt in options"
          :key="opt.value"
          class="cdd-opt"
          :class="{ 'cdd-active': opt.value === modelValue }"
          :style="opt.value === modelValue ? activeStyle : {}"
          @click="select(opt.value)"
          type="button"
        >{{ opt.label }}</button>
      </div>
    </Transition>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from 'vue'

const props = defineProps({
  /** v-model 绑定值 Selected value via v-model */
  modelValue: { type: [String, Number], default: '' },
  /** 选项列表 Options */
  options:    { type: Array, default: () => [] },
  /** 自定义颜色，空则跟随主题色 Custom color (defaults to theme) */
  color:      { type: String, default: '' },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:      { type: String, default: 'auto' }
})

const emit = defineEmits(['update:modelValue'])

const rootRef = ref(null)
const isOpen = ref(false)

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const cssColor = computed(() => props.color || 'var(--rizui_themecolor)')

const selectedLabel = computed(() => {
  const opt = props.options.find(o => o.value === props.modelValue)
  return opt ? opt.label : String(props.modelValue || '')
})

const activeStyle = computed(() => ({
  color: cssColor.value,
  background: `color-mix(in srgb, ${cssColor.value} 8%, transparent)`
}))

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})

function toggle() { isOpen.value = !isOpen.value }

function select(value) {
  emit('update:modelValue', value)
  isOpen.value = false
}

function onClickOutside(e) {
  if (rootRef.value && !rootRef.value.contains(e.target)) {
    isOpen.value = false
  }
}

onMounted(() => {
  document.addEventListener('mousedown', onClickOutside, true)
  document.addEventListener('touchstart', onClickOutside, true)
})

onBeforeUnmount(() => {
  document.removeEventListener('mousedown', onClickOutside, true)
  document.removeEventListener('touchstart', onClickOutside, true)
})
</script>

<style scoped>
.cdd-root {
  position: relative;
  display: inline-block;
  user-select: none;
}

/* 组件级强制暗色（theme="Dark"） */
.cdd-root[data-rizui-theme="dark"] {
  --rz-bg: var(--rz-dark-bg);
  --rz-border: var(--rz-dark-border);
  --rz-text: var(--rz-dark-text);
  --rz-subtext: var(--rz-dark-subtext);
  --rz-shadow: var(--rz-dark-shadow);
}

/* 组件级强制亮色（theme="Light"） */
.cdd-root[data-rizui-theme="light"] {
  --rz-bg: #fff;
  --rz-border: #e8ecf2;
  --rz-text: #334;
  --rz-subtext: #99a;
  --rz-shadow: rgba(30, 41, 59, 0.08);
}

.cdd-trigger {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 6px 14px;
  border: 1.5px solid var(--rz-border);
  border-radius: 999px;
  background: var(--rz-bg);
  font-size: 12px;
  font-family: inherit;
  color: var(--rz-text);
  cursor: pointer;
  outline: none;
  white-space: nowrap;
}

.cdd-trigger:hover {
  border-color: color-mix(in srgb, var(--rz-text) 40%, var(--rz-bg));
}

.cdd-open .cdd-trigger {
  border-color: v-bind('cssColor');
  box-shadow: 0 0 0 3px color-mix(in srgb, var(--rz-text) 6%, transparent);
}

.cdd-arrow {
  flex-shrink: 0;
  transition: transform 0.2s;
  color: var(--rz-subtext);
}

.cdd-open .cdd-arrow {
  transform: rotate(180deg);
}

.cdd-drop {
  position: absolute;
  top: calc(100% + 6px);
  left: 0;
  min-width: 100%;
  background: var(--rz-bg);
  border: 1px solid var(--rz-border);
  border-radius: 10px;
  box-shadow: 0 8px 24px var(--rz-shadow);
  padding: 4px;
  z-index: 100;
}

.cdd-opt {
  display: block;
  width: 100%;
  padding: 7px 14px;
  border: none;
  border-radius: 7px;
  background: transparent;
  font-size: 12px;
  font-family: inherit;
  color: var(--rz-text);
  cursor: pointer;
  text-align: left;
  outline: none;
  transition: background 0.12s;
  white-space: nowrap;
}

.cdd-opt:hover {
  background: color-mix(in srgb, var(--rz-text) 6%, var(--rz-bg));
}

.cdd-active {
  font-weight: 600;
}

.cdd-pop-enter-active,
.cdd-pop-leave-active {
  transition: opacity 0.15s ease, transform 0.15s ease;
}
.cdd-pop-enter-from,
.cdd-pop-leave-to {
  opacity: 0;
  transform: translateY(-6px);
}
</style>
