<template>
  <div ref="rootRef" class="cdd-root" :class="{ 'cdd-open': isOpen }">
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
  modelValue: { type: [String, Number], default: '' },
  options:    { type: Array, default: () => [] },
  color:      { type: String, default: '' }
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

.cdd-trigger {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 6px 14px;
  border: 1.5px solid #d0d8e0;
  border-radius: 999px;
  background: #fff;
  font-size: 12px;
  font-family: inherit;
  color: #445;
  cursor: pointer;
  outline: none;
  transition: border-color 0.2s, box-shadow 0.2s;
  white-space: nowrap;
}

.cdd-trigger:hover {
  border-color: #aab4c0;
}

.cdd-open .cdd-trigger {
  border-color: v-bind('cssColor');
  box-shadow: 0 0 0 3px rgba(0, 0, 0, 0.04);
}

.cdd-arrow {
  flex-shrink: 0;
  transition: transform 0.2s;
  color: #99a;
}

.cdd-open .cdd-arrow {
  transform: rotate(180deg);
}

.cdd-drop {
  position: absolute;
  top: calc(100% + 6px);
  left: 0;
  min-width: 100%;
  background: #fff;
  border: 1px solid #e8ecf2;
  border-radius: 10px;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.1);
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
  color: #445;
  cursor: pointer;
  text-align: left;
  outline: none;
  transition: background 0.12s;
  white-space: nowrap;
}

.cdd-opt:hover {
  background: #f5f5f8;
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
