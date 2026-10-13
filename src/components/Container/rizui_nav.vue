<template>
  <div
    class="rnav-container"
    :class="{
      'rnav-is-floating': floating,
      'rnav-is-fixed': fixed,
      'rnav-is-collapsed': isCollapsed
    }"
    :style="containerStyle"
  >
    <aside
      class="rnav-root"
      :data-rizui-theme="themeAttr"
      :class="{
        'rnav-collapsed': isCollapsed,
        'rnav-bordered': bordered,
        'rnav-fixed': fixed,
        'rnav-floating-overlay': floating
      }"
      :style="rootStyle"
    >
    <!-- 顶部标题 / 品牌区 / 折叠切换按钮 -->
    <div v-if="$slots.header || title || collapsible" class="rnav-header">
      <slot
        name="header"
        :collapsed="isCollapsed"
        :toggle-collapse="toggleCollapse"
      >
        <div
          v-if="!isCollapsed && (icon || title)"
          class="rnav-brand"
          @click="handleBrandClick"
        >
          <span v-if="icon && !isCollapsed" class="rnav-brand-icon">
            <i :class="icon"></i>
          </span>
          <span v-if="title && !isCollapsed" class="rnav-brand-title">
            {{ title }}
          </span>
        </div>

        <button
          v-if="collapsible"
          class="rnav-collapse-btn"
          :class="{ 'rnav-collapsed-btn': isCollapsed }"
          type="button"
          :title="isCollapsed ? '展开导航栏' : '折叠导航栏'"
          @click="toggleCollapse"
        >
          <svg
            viewBox="0 0 24 24"
            width="15"
            height="15"
            stroke="currentColor"
            stroke-width="2.4"
            stroke-linecap="round"
            stroke-linejoin="round"
            fill="none"
            class="rnav-chevron-icon"
          >
            <polyline points="15 18 9 12 15 6"></polyline>
          </svg>
        </button>
      </slot>
    </div>

    <!-- 菜单项滚动区域 -->
    <div class="rnav-body">
      <slot
        :items="normalizedItems"
        :active-id="currentActiveId"
        :collapsed="isCollapsed"
      >
        <rizui_nav_item
          v-for="(item, idx) in normalizedItems"
          :key="item.id || item.route || item.label || idx"
          :item="item"
          :depth="0"
          :collapsed="isCollapsed"
          :active-id="currentActiveId"
          :expanded-ids="expandedSet"
          @select="handleSelect"
          @toggle="handleToggle"
        >
          <template #icon="slotProps"><slot name="icon" v-bind="slotProps" /></template>
          <template #label="slotProps"><slot name="label" v-bind="slotProps" /></template>
        </rizui_nav_item>
      </slot>
    </div>

    <!-- 底部区域 (可选) -->
    <div v-if="$slots.footer" class="rnav-footer">
      <slot
        name="footer"
        :collapsed="isCollapsed"
        :toggle-collapse="toggleCollapse"
      />
    </div>
  </aside>
  </div>
</template>

<script setup>
import { computed, ref, reactive, watch } from 'vue'
import rizui_nav_item from './rizui_nav_item.vue'

defineOptions({
  name: 'rizui_nav'
})

const props = defineProps({
  /** 菜单项列表（树形结构，支持嵌套 children） */
  items:               { type: Array,   default: () => [] },
  /** 当前选中的菜单项 id 或 route（支持 v-model） */
  modelValue:          { type: [String, Number], default: '' },
  /** 是否折叠为 Rail 图标模式（支持 v-model:collapsed） */
  collapsed:           { type: Boolean, default: false },
  /** 是否允许折叠（显示折叠汉堡按钮） */
  collapsible:         { type: Boolean, default: false },
  /** 导航栏标题/品牌名 */
  title:               { type: String,  default: '' },
  /** 导航栏左上角品牌图标（完整 FA class） */
  icon:                { type: String,  default: '' },
  /** 展开时的宽度（数字转为 px） */
  width:               { type: [Number, String], default: 240 },
  /** 折叠时的紧凑宽度（数字转为 px） */
  collapsedWidth:      { type: [Number, String], default: 64 },
  /** 导航栏高度，默认为页面高度 '100vh' */
  height:              { type: [Number, String], default: '100vh' },
  /** 是否固定定位在页面左侧（position: fixed） */
  fixed:               { type: Boolean, default: false },
  /**
   * 是否浮动在页面上（Overlay 悬浮模式）：
   * - true: 浮动在页面上，不占用页面排版空间（覆盖在内容之上）
   * - false: 不浮动在页面上，在页面布局中占用固定空间（主内容不被遮挡）；视觉上仍呈现精致悬浮岛美学
   */
  floating:            { type: Boolean, default: false },
  /**
   * 悬浮岛边距（与视口四周边缘的间距，如 12 表示留出 12px 悬浮间距，传 0 则紧贴边缘）
   */
  inset:               { type: [Number, String], default: 12 },
  /** 固定定位时的顶部偏移量（如 0 或 '60px'） */
  top:                 { type: [Number, String], default: 0 },
  /** 固定定位时的底部偏移量（默认 0） */
  bottom:              { type: [Number, String], default: 0 },
  /** 主题色，为空跟随全局 --rizui_themecolor */
  color:               { type: String,  default: '' },
  /** 默认展开的菜单项 id 数组 */
  defaultExpandedKeys: { type: Array,   default: () => [] },
  /** 手风琴折叠模式（同级子目录只允许展开一个） */
  accordion:           { type: Boolean, default: false },
  /** 是否显示外边框与卡片阴影 */
  bordered:            { type: Boolean, default: true },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 */
  theme:               { type: String,  default: 'auto' }
})

const emit = defineEmits([
  'update:modelValue',
  'update:collapsed',
  'select',
  'toggle',
  'brand-click'
])

function handleBrandClick() {
  emit('brand-click')
}

/* 折叠状态同步 */
const internalCollapsed = ref(props.collapsed)
watch(() => props.collapsed, (val) => {
  internalCollapsed.value = val
})

const isCollapsed = computed(() => internalCollapsed.value)

function toggleCollapse() {
  internalCollapsed.value = !internalCollapsed.value
  emit('update:collapsed', internalCollapsed.value)
}

/* 激活状态（同时兼容受控与非受控） */
const internalActiveId = ref(props.modelValue)
watch(() => props.modelValue, (val) => {
  internalActiveId.value = val
})

const currentActiveId = computed(() => {
  return props.modelValue !== undefined && props.modelValue !== ''
    ? props.modelValue
    : internalActiveId.value
})

/* 展开状态集合 */
const expandedSet = reactive(new Set(props.defaultExpandedKeys))

/* 数据源格式归一化（兼容 label / name / zhName，以及 children / comps） */
function normalizeItem(raw) {
  const norm = {
    ...raw,
    id: raw.id ?? raw.route ?? raw.label ?? raw.name ?? raw.zhName ?? '',
    label: raw.label ?? raw.name ?? raw.zhName ?? '',
    icon: raw.icon ?? '',
    children: (raw.children || raw.comps || []).map(normalizeItem)
  }
  return norm
}

const normalizedItems = computed(() => {
  return props.items.map(normalizeItem)
})

/* 查找包含某个激活路径的父级，并默认展开 */
function findAncestors(list, targetId, path = []) {
  for (const it of list) {
    const itId = it.id ?? it.route
    if (itId === targetId || it.route === targetId) {
      return path
    }
    if (it.children && it.children.length > 0) {
      const res = findAncestors(it.children, targetId, [...path, itId])
      if (res) return res
    }
  }
  return null
}

watch(
  () => [props.modelValue, normalizedItems.value],
  ([val]) => {
    if (val) {
      const ancestors = findAncestors(normalizedItems.value, val)
      if (ancestors) {
        for (const aId of ancestors) {
          expandedSet.add(aId)
        }
      }
    }
  },
  { immediate: true }
)

function handleSelect(item) {
  const id = item.id ?? item.route ?? item.label
  internalActiveId.value = id
  emit('update:modelValue', id)
  emit('select', item)
}

function handleToggle(item) {
  const id = item.id ?? item.route ?? item.label
  const willOpen = !expandedSet.has(id)

  if (props.accordion && willOpen) {
    expandedSet.clear()
  }

  if (willOpen) {
    expandedSet.add(id)
  } else {
    expandedSet.delete(id)
  }

  emit('toggle', { item, expanded: willOpen })
}

/* 尺寸格式化与数值提取 */
const toPx = (v) => {
  if (typeof v === 'number') return v
  if (typeof v === 'string') {
    const num = parseFloat(v)
    return isNaN(num) ? 0 : num
  }
  return 0
}

const formatSize = (v) => (typeof v === 'number' ? `${v}px` : v)

/* 悬浮边缘边距（呼吸间隙） */
const currentInset = computed(() => {
  if (props.inset === undefined || props.inset === null) return 12
  return toPx(props.inset)
})

/* 卡片本体宽度（展开 vs 折叠） */
const currentCardWidth = computed(() => {
  const w = isCollapsed.value ? props.collapsedWidth : props.width
  return toPx(w)
})

/* 外层占位容器样式：控制是否占用页面空间 */
const containerStyle = computed(() => {
  if (props.floating) {
    return {
      width: '0px',
      minWidth: '0px',
      flexBasis: '0px',
      flexShrink: 0
    }
  }

  // 不浮动时：占用固定空间（卡片宽 + 左右悬浮边距）
  const ins = currentInset.value
  const totalW = currentCardWidth.value + ins * 2
  return {
    width: `${totalW}px`,
    minWidth: `${totalW}px`,
    flexBasis: `${totalW}px`,
    flexShrink: 0
  }
})

/* 侧边栏卡片本体样式：视觉上始终保持悬浮岛质感 */
const rootStyle = computed(() => {
  const ins = currentInset.value
  const topVal = props.top ? formatSize(props.top) : `${ins}px`
  const bottomVal = props.bottom ? formatSize(props.bottom) : `${ins}px`
  const leftVal = `${ins}px`

  // 始终为页面高度（扣减上下悬浮边距）
  const heightVal = props.height && props.height !== '100vh'
    ? formatSize(props.height)
    : `calc(100vh - ${topVal} - ${bottomVal})`

  return {
    '--rnav-color': props.color || 'var(--rizui_themecolor)',
    '--rnav-top': topVal,
    '--rnav-bottom': bottomVal,
    '--rnav-left': leftVal,
    width: `${currentCardWidth.value}px`,
    height: heightVal,
    maxHeight: heightVal
  }
})

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})
</script>

<style scoped>
/* ── 外层占位/排版容器 ── */
.rnav-container {
  position: relative;
  box-sizing: border-box;
  flex-shrink: 0;
  transition:
    width 0.3s cubic-bezier(0.2, 0.8, 0.2, 1),
    min-width 0.3s cubic-bezier(0.2, 0.8, 0.2, 1),
    flex-basis 0.3s cubic-bezier(0.2, 0.8, 0.2, 1);
}

/* 浮动在页面上：宽度为 0，不占用页面排版空间 */
.rnav-container.rnav-is-floating {
  width: 0 !important;
  min-width: 0 !important;
  flex-basis: 0 !important;
  pointer-events: none;
}

.rnav-container.rnav-is-floating .rnav-root {
  pointer-events: auto;
}

/* 非 fixed 状态下如果浮动，作为绝对定位浮在父容器上 */
.rnav-container.rnav-is-floating:not(.rnav-is-fixed) .rnav-root {
  position: absolute;
  top: var(--rnav-top, 12px);
  bottom: var(--rnav-bottom, 12px);
  left: var(--rnav-left, 12px);
  z-index: 990;
}

/* ── 侧边栏本体卡片（视觉上始终呈现精致悬浮岛美学） ── */
.rnav-root {
  position: relative;
  display: flex;
  flex-direction: column;
  box-sizing: border-box;
  background: var(--rz-bg, #fff);
  color: var(--rz-text, #334);
  padding: 10px 8px;
  overflow: hidden;
  user-select: none;
  border-radius: 18px;
  backdrop-filter: blur(16px);
  -webkit-backdrop-filter: blur(16px);
  transition:
    width 0.3s cubic-bezier(0.2, 0.8, 0.2, 1),
    padding 0.3s ease,
    background-color 0.2s ease,
    border-color 0.2s ease,
    box-shadow 0.2s ease;
}

/* 固定在视口左侧（悬浮岛位置） */
.rnav-root.rnav-fixed {
  position: fixed;
  top: var(--rnav-top, 12px);
  bottom: var(--rnav-bottom, 12px);
  left: var(--rnav-left, 12px);
  z-index: 100;
}

/* 浮动在页面上方时提升层级并增强深度阴影 */
.rnav-root.rnav-floating-overlay {
  z-index: 990;
  box-shadow: 0 16px 48px var(--rz-shadow, rgba(30, 41, 59, 0.16));
}

/* 视觉悬浮卡片边框与胶囊阴影（无论固定占用空间还是覆盖浮动，视觉上皆为悬浮岛） */
.rnav-bordered {
  border: 1px solid var(--rz-border, #e8ecf2);
  box-shadow: 0 10px 32px var(--rz-shadow, rgba(30, 41, 59, 0.08));
}

/* 顶部区域 (Header) */
.rnav-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 4px 6px 10px;
  margin-bottom: 4px;
  border-bottom: 1px solid color-mix(in srgb, var(--rz-border) 60%, transparent);
  flex-shrink: 0;
  gap: 8px;
  min-height: 38px;
}

.rnav-brand {
  display: flex;
  align-items: center;
  gap: 8px;
  min-width: 0;
  flex: 1;
}

.rnav-brand-center {
  justify-content: center;
}

.rnav-brand-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-size: 16px;
  color: var(--rnav-color);
  flex-shrink: 0;
}

.rnav-brand-title {
  font-size: 15px;
  font-weight: 700;
  color: var(--rz-text);
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

/* 折叠/展开汉堡按钮 */
.rnav-collapse-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 28px;
  height: 28px;
  border: none;
  background: transparent;
  border-radius: 6px;
  color: var(--rz-subtext);
  cursor: pointer;
  flex-shrink: 0;
  outline: none;
  transition: background 0.16s ease, color 0.16s ease, transform 0.1s ease;
}

.rnav-collapse-btn:hover {
  background: color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  color: var(--rz-text);
}

.rnav-collapse-btn:active {
  transform: scale(0.92);
}

.rnav-chevron-icon {
  display: block;
  transform-origin: center;
  transition: transform 0.3s cubic-bezier(0.2, 0.8, 0.2, 1);
}

.rnav-collapsed-btn .rnav-chevron-icon {
  transform: rotate(180deg);
}

/* 菜单项主体滚动区：滚动必须严格限制在内部，禁止穿透 */
.rnav-body {
  flex: 1;
  min-height: 0;
  overflow-y: auto;
  overflow-x: hidden;
  overscroll-behavior: contain;
  padding-right: 2px;
}

/* 自定义纤细精致滚动条 */
.rnav-body::-webkit-scrollbar {
  width: 4px;
}
.rnav-body::-webkit-scrollbar-thumb {
  background: color-mix(in srgb, var(--rz-text) 14%, transparent);
  border-radius: 999px;
}
.rnav-body::-webkit-scrollbar-thumb:hover {
  background: color-mix(in srgb, var(--rz-text) 24%, transparent);
}

/* 底部区域 (Footer) */
.rnav-footer {
  padding: 10px 6px 4px;
  border-top: 1px solid color-mix(in srgb, var(--rz-border) 60%, transparent);
  flex-shrink: 0;
}

/* ── 折叠模式 (Rail 极简图标状态) ── */
.rnav-collapsed {
  padding: 10px 6px;
}

.rnav-collapsed .rnav-header {
  justify-content: center;
  padding: 4px 0 8px;
}

.rnav-collapsed .rnav-body {
  padding-right: 0;
}

/* ── 组件级强制暗色（theme="Dark"） ── */
.rnav-root[data-rizui-theme="dark"] {
  --rz-bg: var(--rz-dark-bg, #1e2630);
  --rz-border: var(--rz-dark-border, #323d4b);
  --rz-text: var(--rz-dark-text, #e6ebf2);
  --rz-subtext: var(--rz-dark-subtext, #93a1b3);
  --rz-shadow: var(--rz-dark-shadow, rgba(0, 0, 0, 0.4));
}

/* ── 组件级强制亮色（theme="Light"） ── */
.rnav-root[data-rizui-theme="light"] {
  --rz-bg: #fff;
  --rz-border: #e8ecf2;
  --rz-text: #334;
  --rz-subtext: #99a;
  --rz-shadow: rgba(30, 41, 59, 0.08);
}
</style>
