<template>
  <Teleport :to="stackEl">
    <Transition name="tst-pop" @after-leave="onLeave">
      <div
        v-if="visible"
        class="tst-root"
        :class="`tst-${type}`"
        :style="{ '--tst-color': cssColor, '--tst-countdown': countdown + 's', '--tst-width': width }"
        @click="onClick"
      >
        <!-- 左侧图标 -->
        <div class="tst-icon" :style="iconStyle">
          <i :class="icon || defaultIcon"></i>
        </div>

        <!-- 内容：title / hint 与默认插槽可同时使用，或只用一个（自闭合/独立闭合均可）
             有 title/hint 且带 slot 时，slot 排在文字下方 -->
        <div class="tst-content">
          <div v-if="title" class="tst-title" :style="titleStyle">{{ title }}</div>
          <div v-if="hint" class="tst-hint" :style="hintStyle">{{ hint }}</div>
          <div v-if="hasSlot" class="tst-slot" :class="{ 'tst-slot-stack': isStack }">
            <slot />
          </div>
        </div>

        <!-- 关闭按钮 -->
        <button v-if="closable" class="tst-close" @click.stop="requestClose" type="button">
          <svg viewBox="0 0 24 24" width="10" height="10" stroke="currentColor" stroke-width="3" fill="none"><path d="M18 6L6 18M6 6l12 12"/></svg>
        </button>

        <!-- 底部倒计时进度条：countdown>0 时从 100% 线性缩到 0%，countdown=0 时保持全满 -->
        <div v-if="showBar" class="tst-bar">
          <div class="tst-bar-inner" :class="{ 'tst-bar-full': !(countdown > 0) }"></div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup>
import { ref, computed, useSlots, onMounted, onBeforeUnmount } from 'vue'

const props = defineProps({
  type:         { type: String,  default: 'info' },
  title:        { type: String,  default: '' },
  hint:         { type: String,  default: '' },
  titleStyle: { type: Object,  default: () => ({}) },
  hintStyle:  { type: Object,  default: () => ({}) },
  icon:         { type: String,  default: '' },
  iconStyle:  { type: Object,  default: () => ({}) },
  countdown:    { type: Number,  default: 3 },
  closable:     { type: Boolean, default: true },
  showBar:      { type: Boolean, default: true },
  color:        { type: String,  default: '' },
  teleport:     { type: String,  default: 'body' },
  width:        { type: String,  default: '' }
})

const emit = defineEmits(['close', 'click'])

const slots = useSlots()
const hasSlot = computed(() => !!slots.default)
/* 有 title/hint 且带插槽时，插槽排到文字下方（类似 Cfgrow stack） */
const isStack = computed(() => (!!props.title || !!props.hint) && hasSlot.value)

/* 内部显隐：进场即显示，关闭时先播退场动画再 emit close */
const visible = ref(true)
/* Teleport 目标：共享栈容器，所有 toast 在里面纵向排队。
   setup 阶段同步创建，保证首次渲染时目标非空 */
const stackEl = ref(typeof document !== 'undefined' ? ensureStack() : null)

const DEFAULT_ICONS = {
  info:  'fa-solid fa-circle-info',
  warn:  'fa-solid fa-triangle-exclamation',
  error: 'fa-solid fa-circle-exclamation'
}
const defaultIcon = computed(() => DEFAULT_ICONS[props.type] || DEFAULT_ICONS.info)

/* 颜色：info 跟随主题色，color prop 仅对 info 生效（自定义）；warn/error 用预设色 */
const TYPE_COLORS = { warn: '#e6a23c', error: '#e05555' }
const cssColor = computed(() => {
  if (props.type === 'info') return props.color || 'var(--rizui_themecolor)'
  return TYPE_COLORS[props.type] || 'var(--rizui_themecolor)'
})

/* 共享栈容器：挂到 teleport 指定的目标元素下（默认 body）。
   子元素绝对定位，由布局函数按顺序排 top，移除时剩余 toast 平滑上移 */
function ensureStack() {
  const host = document.querySelector(props.teleport) || document.body
  let el = document.getElementById('rz-toast-stack')
  if (!el) {
    el = document.createElement('div')
    el.id = 'rz-toast-stack'
  }
  if (el.parentElement !== host) host.appendChild(el)
  /* 只绑定一次布局观察 */
  if (!el.__rzBound) {
    el.__rzBound = true
    /* FLIP 布局：先无过渡定位，再以 transform 补偿旧位置，过渡到新位置，实现平滑上移 */
    const layout = (instant) => {
      const children = [...el.children].filter((c) => c.classList.contains('tst-root'))
      const prev = new Map()
      for (const c of children) prev.set(c, c.getBoundingClientRect().top)
      let y = 0
      let maxW = 0
      for (const c of children) {
        c.style.transition = 'none'
        c.style.top = y + 'px'
        y += c.offsetHeight + 10
        maxW = Math.max(maxW, c.offsetWidth)
      }
      /* 容器宽度跟随最宽 toast，保证右缘对齐 */
      if (maxW > 0) el.style.width = maxW + 'px'
      if (!instant) {
        void el.offsetHeight /* 强制回流，应用新位置 */
        for (const c of children) {
          const dy = prev.get(c) - c.getBoundingClientRect().top
          if (Math.abs(dy) > 0.5) {
            /* FLIP：先位移回旧位置，再过渡回新位置 */
            c.style.transform = `translateY(${dy}px)`
            void c.offsetHeight
            c.style.transition = 'transform 0.3s ease'
            c.style.transform = ''
            c.addEventListener('transitionend', () => {
              c.style.transition = ''
            }, { once: true })
          } else {
            c.style.transition = ''
          }
        }
      } else {
        /* 新增：定位后恢复过渡，避免残留 transition:none 覆盖退场动画 */
        for (const c of children) c.style.transition = ''
      }
    }
    const mo = new MutationObserver((muts) => {
      const added = muts.some((m) => m.addedNodes.length)
      if (added) {
        /* 新 toast：无过渡直接定位 */
        layout(true)
      } else {
        /* 有 toast 被移除：剩余 toast 平滑上移 */
        layout(false)
      }
    })
    mo.observe(el, { childList: true })
  }
  return el
}

/* 底部倒计时进度条：CSS animation 线性缩到 0，颜色跟随 cssColor */
const shownAt = Date.now()
let timer = null

onMounted(() => {
  if (props.countdown > 0) timer = setTimeout(requestClose, props.countdown * 1000)
})

onBeforeUnmount(() => {
  if (timer) clearTimeout(timer)
})

/* 关闭：先播退场动画，动画结束后 emit('close', 停留秒数) */
function requestClose() {
  if (!visible.value) return
  visible.value = false
}

function onLeave() {
  const duration = Math.round((Date.now() - shownAt) / 100) / 10
  emit('close', duration)
}

/* 点击：作为独立事件发出，不自动关闭，可绑定到其他行为 */
function onClick() {
  emit('click')
}

/* 暴露 close：供外部绑定在 click 等事件上触发（带退场动画） */
defineExpose({ close: requestClose })
</script>

<style scoped>
/* ── 进场：CSS animation 从右侧滑入淡入 ── */
.tst-root {
  animation: tst-enter 0.25s ease;
}
@keyframes tst-enter {
  from { opacity: 0; transform: translateX(40px); }
  to   { opacity: 1; transform: translateX(0); }
}

/* ── 退场：Transition 向右滑出淡出 ── */
.tst-pop-leave-active {
  transition: opacity 0.25s ease, transform 0.25s ease;
}
.tst-pop-leave-to {
  opacity: 0;
  transform: translateX(40px);
}

/* ── 白色卡片：无边框、大圆角、投影；绝对定位由栈容器布局，top 变化平滑上移 ── */
.tst-root {
  position: absolute;
  top: 0;
  right: 0;
  display: flex;
  align-items: flex-start;
  gap: 12px;
  width: var(--tst-width, 320px);
  max-width: 86vw;
  padding: 14px 16px;
  padding-right: 40px;
  border-radius: 14px;
  border: none;
  background: #fff;
  box-shadow: 0 8px 28px rgba(0, 0, 0, 0.14);
  box-sizing: border-box;
  font-size: 13px;
  color: #445;
  line-height: 1.6;
  cursor: pointer;
  overflow: hidden;
  pointer-events: auto;
}


.tst-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  font-size: 18px;
  color: var(--tst-color);
  line-height: 1;
  margin-top: 2px;
}

.tst-content {
  flex: 1;
  min-width: 0;
}

.tst-title {
  font-size: 14px;
  font-weight: 600;
  color: #333;
  line-height: 1.5;
}

.tst-hint {
  font-size: 12.5px;
  color: #889;
  line-height: 1.6;
  margin-top: 2px;
}

/* ── stack 模式：slot 在文字下方独立一行 ── */
.tst-slot-stack {
  margin-top: 8px;
  padding-top: 8px;
  border-top: 1px dashed color-mix(in srgb, var(--tst-color) 35%, transparent);
}

.tst-close {
  position: absolute;
  top: 10px;
  right: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 22px;
  height: 22px;
  border: none;
  border-radius: 50%;
  background: #f2f3f5;
  color: #99a;
  cursor: pointer;
  transition: background 0.15s, color 0.15s;
}

.tst-close:hover { 
  background: color-mix(in srgb, var(--tst-color) 30%, #fff);
  color: var(--tst-color);
}

/* ── 底部倒计时进度条 ── */
/* 槽（进度走完后剩下的部分）：淡色主题色 / 预设色 */
.tst-bar {
  position: absolute;
  left: 0;
  bottom: 0;
  width: 100%;
  height: 3px;
  background: color-mix(in srgb, var(--tst-color) 40%, transparent);
}

/* 进度条本身：保持原色 */
.tst-bar-inner {
  height: 100%;
  width: 100%;
  border-radius: 0 3px 3px 0;
  background: var(--tst-color);
}

/* 有倒计时才播放收缩动画；countdown=0 时保持全满 */
.tst-bar-inner:not(.tst-bar-full) {
  animation: tst-shrink var(--tst-countdown) linear forwards;
}

@keyframes tst-shrink {
  from { width: 100%; }
  to   { width: 0%; }
}
</style>

<!-- 共享栈容器：fixed 右上角，子元素绝对定位，JS 布局纵向排队并平滑上移 -->
<style>
#rz-toast-stack {
  position: fixed;
  top: 20px;
  right: 20px;
  width: 320px;
  max-width: 86vw;
  z-index: 9999;
  pointer-events: none;
}
</style>
