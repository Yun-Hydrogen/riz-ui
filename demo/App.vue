<script setup>
import {ref, computed, reactive, provide, watch} from 'vue'
import {useRoute, useRouter} from 'vue-router'
import {rizui_colorpicker, rizui_nav} from 'riz-ui'

const route = useRoute()
const router = useRouter()

const themeColor = ref('#66CCFF')
provide('themeColor', themeColor)

/* ── Demo 暗色主题开关（auto / dark / light 循环） ── */
const THEME_STORAGE = 'rizui-demo-theme'
const demoTheme = ref(localStorage.getItem(THEME_STORAGE) || 'auto')

const themeLabel = computed(() => (
  { auto: '自动', dark: '暗色', light: '亮色' }[demoTheme.value]
))

function applyDemoTheme(mode) {
  if (mode === 'auto') {
    delete document.documentElement.dataset.rizuiTheme
  } else {
    document.documentElement.dataset.rizuiTheme = mode
  }
  document.documentElement.style.setProperty('--rizui_theme', mode)
}
applyDemoTheme(demoTheme.value)

function cycleDemoTheme() {
  const order = ['auto', 'dark', 'light']
  const next = order[(order.indexOf(demoTheme.value) + 1) % order.length]
  demoTheme.value = next
  localStorage.setItem(THEME_STORAGE, next)
  applyDemoTheme(next)
}

/* ── 分类 & 组件（仅用于导航） ── */
const categories = [
  {
    id: 'container', name: '容器样式', icon: 'fa-solid fa-box-archive', comps: [
      {route: '/demo/rizui-card', zhName: '卡片', icon: 'fa-solid fa-square', deprecated: false},
      {route: '/demo/rizui-cfgrow', zhName: '配置行', icon: 'fa-solid fa-list', deprecated: false},
      {route: '/demo/rizui-infobox', zhName: '提示框', icon: 'fa-solid fa-circle-info', deprecated: false},
      {route: '/demo/rizui-dialog', zhName: '对话框', icon: 'fa-solid fa-window-maximize', deprecated: false},
      {route: '/demo/rizui-toast', zhName: 'Toast', icon: 'fa-solid fa-bell', deprecated: false},
      {route: '/demo/rizui-code', zhName: '代码块', icon: 'fa-solid fa-code', deprecated: false},
      {route: '/demo/rizui-nav', zhName: '导航栏', icon: 'fa-solid fa-bars-staggered', deprecated: false}
    ]
  },
  {
    id: 'ioput', name: '开关控制', icon: 'fa-solid fa-sliders', comps: [
      {route: '/demo/rizui-switch', zhName: '开关', icon: 'fa-solid fa-toggle-on', deprecated: false},
      {route: '/demo/rizui-slider', zhName: '滑条', icon: 'fa-solid fa-sliders', deprecated: false},
      {route: '/demo/rizui-dropdown', zhName: '下拉', icon: 'fa-solid fa-square-caret-down', deprecated: false},
      {route: '/demo/rizui-colorpicker', zhName: '取色器', icon: 'fa-solid fa-palette', deprecated: false},
      {route: '/demo/rizui-timeline', zhName: '时间轴', icon: 'fa-solid fa-timeline', deprecated: false},
      {route: '/demo/rizui-button', zhName: '按钮', icon: 'fa-solid fa-hand-pointer', deprecated: false},
      {route: '/demo/rizui-checkbox', zhName: '复选框', icon: 'fa-solid fa-square-check', deprecated: false},
      {route: '/demo/rizui-progress', zhName: '进度条', icon: 'fa-solid fa-bars-progress', deprecated: false},
      {route: '/demo/rizui-circle-progress', zhName: '环形进度条', icon: 'fa-solid fa-circle-notch', deprecated: false}
    ]
  },
  {
    id: 'input', name: '输入组件', icon: 'fa-solid fa-keyboard', comps: [
      {route: '/demo/rizui-text', zhName: '文本输入', icon: 'fa-solid fa-font', deprecated: false},
      {route: '/demo/rizui-mdeditor', zhName: 'MD 编辑器', icon: 'fa-solid fa-pen-nib', deprecated: false}
    ]
  },
  {
    id: 'other', name: '其他控件', icon: 'fa-solid fa-cubes', comps: [
      {route: '/demo/theme-color', zhName: '主题色', icon: 'fa-solid fa-droplet', deprecated: false},
      {route: '/demo/open-url', zhName: '打开链接', icon: 'fa-solid fa-arrow-up-right-from-square', deprecated: false}
    ]
  },
  {
    id: 'old', name: '旧组件', icon: 'fa-solid fa-clock-rotate-left', comps: [
      {route: '/demo/ui-card', zhName: 'UiCard', icon: 'fa-solid fa-cube', deprecated: true},
      {route: '/demo/ui-config-row', zhName: 'UiConfigRow', icon: 'fa-solid fa-cube', deprecated: true},
      {route: '/demo/ui-switch', zhName: 'UiSwitch', icon: 'fa-solid fa-cube', deprecated: true},
      {route: '/demo/ui-slider', zhName: 'UiSlider', icon: 'fa-solid fa-cube', deprecated: true},
      {route: '/demo/ui-dropdown', zhName: 'UiDropdown', icon: 'fa-solid fa-cube', deprecated: true},
      {route: '/demo/ui-color-picker', zhName: 'UiColorPicker', icon: 'fa-solid fa-cube', deprecated: true},
      {route: '/demo/ui-timeline', zhName: 'UiTimeline', icon: 'fa-solid fa-cube', deprecated: true},
      {route: '/demo/ui-dialog', zhName: 'UiDialog', icon: 'fa-solid fa-cube', deprecated: true},
      {route: '/demo/not-ready-overlay', zhName: 'NotReadyOverlay', icon: 'fa-solid fa-cube', deprecated: true}
    ]
  }
]

const isDemoPage = computed(() => route.path.startsWith('/demo/'))
const isSidebarCollapsed = ref(false)
const isSidebarFloating = ref(false)

function findCatId(path) {
  for (const cat of categories) {
    if (cat.comps.some(c => path === c.route)) return cat.id
  }
  return null
}

/* 展开的栏目集合（可同时展开多个） */
const expandedCats = reactive(new Set())
expandedCats.add(findCatId(route.path) || 'container')

/* 路由变化时确保当前所在分类展开 */
watch(() => route.path, (path) => {
  const id = findCatId(path)
  if (id && !expandedCats.has(id)) expandedCats.add(id)
})

function toggleCat(catId) {
  if (expandedCats.has(catId)) expandedCats.delete(catId)
  else expandedCats.add(catId)
}

function goHome() {
  expandedCats.clear()
  expandedCats.add('container')
  router.push('/')
}

function goPricing() {
  router.push('/pricing')
}
</script>

<template>
  <div class="app" :style="{ '--rizui_themecolor': themeColor }">
    <!-- 背景浮动泡泡（仅 demo 装饰） -->
    <div class="demo-bubbles" aria-hidden="true">
      <span class="demo-bub b1"></span><span class="demo-bub b2"></span>
      <span class="demo-bub b3"></span><span class="demo-bub b4"></span>
      <span class="demo-bub b5"></span><span class="demo-bub b6"></span>
      <span class="demo-bub b7"></span><span class="demo-bub b8"></span>
      <span class="demo-bub b9"></span><span class="demo-bub b10"></span>
    </div>

    <!-- ═══ 顶部全局导航栏（全宽贯穿，位于最上方） ═══ -->
    <header class="topbar">
      <div class="topbar-inner">
        <div class="topbar-left">
          <span class="topbar-home" @click="goHome()">RizUI</span>
          <rizui_colorpicker v-model="themeColor"/>
        </div>
        <div class="topbar-right">
          <!-- 浮动 / 固定模式切换开关（仅 Demo 页展示） -->
          <button
            v-if="isDemoPage"
            class="theme-switch"
            :title="`侧栏排版：${isSidebarFloating ? '浮动覆盖（不占空间）' : '固定留空（占用空间）'}`"
            @click="isSidebarFloating = !isSidebarFloating"
          >
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <rect x="3" y="3" width="18" height="18" rx="2"/>
              <line x1="9" y1="3" x2="9" y2="21"/>
            </svg>
            <span>{{ isSidebarFloating ? '浮动' : '固定' }}</span>
          </button>
          <button class="theme-switch" :title="`主题模式：${themeLabel}（点击切换）`" @click="cycleDemoTheme()">
            <!-- auto：显示器图标 -->
            <svg v-if="demoTheme === 'auto'" width="14" height="14" viewBox="0 0 24 24" fill="none"
                 stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <rect x="2" y="3" width="20" height="14" rx="2"/>
              <line x1="8" y1="21" x2="16" y2="21"/>
              <line x1="12" y1="17" x2="12" y2="21"/>
            </svg>
            <!-- dark：月亮图标 -->
            <svg v-else-if="demoTheme === 'dark'" width="14" height="14" viewBox="0 0 24 24" fill="none"
                 stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"/>
            </svg>
            <!-- light：太阳图标 -->
            <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none"
                 stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <circle cx="12" cy="12" r="4"/>
              <path d="M12 2v2M12 20v2M4.93 4.93l1.41 1.41M17.66 17.66l1.41 1.41M2 12h2M20 12h2M6.34 17.66l-1.41 1.41M19.07 4.93l-1.41 1.41"/>
            </svg>
            <span>{{ themeLabel }}</span>
          </button>
          <span class="topbar-price" @click="goPricing()">AGPLv3 · 许可</span>
          <a class="topbar-gh" href="https://github.com/Yun-Hydrogen/riz-ui" target="_blank">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor">
              <path
                  d="M12 0C5.37 0 0 5.37 0 12c0 5.3 3.438 9.8 8.205 11.387.6.113.82-.258.82-.577 0-.285-.01-1.04-.015-2.04-3.338.724-4.042-1.61-4.042-1.61-.546-1.387-1.333-1.756-1.333-1.756-1.089-.745.083-.73.083-.73 1.205.085 1.838 1.236 1.838 1.236 1.07 1.835 2.809 1.305 3.495.998.108-.776.418-1.305.762-1.604-2.665-.3-5.466-1.332-5.466-5.93 0-1.31.465-2.38 1.235-3.22-.135-.303-.54-1.523.105-3.176 0 0 1.005-.322 3.3 1.23.96-.267 1.98-.399 3-.405 1.02.006 2.04.138 3 .405 2.28-1.552 3.285-1.23 3.285-1.23.645 1.653.24 2.873.12 3.176.765.84 1.23 1.91 1.23 3.22 0 4.61-2.805 5.625-5.475 5.92.42.36.81 1.096.81 2.22 0 1.606-.015 2.896-.015 3.286 0 .315.21.69.825.57C20.565 21.795 24 17.295 24 12 24 5.37 18.63 0 12 0z"/>
            </svg>
            GitHub
          </a>
        </div>
      </div>
    </header>

    <!-- ═══ Demo 模式：侧边栏在顶栏下方 ═══ -->
    <div v-if="isDemoPage" class="demo-workbench">
      <rizui_nav
        class="demo-sidebar-nav"
        fixed
        :top="76"
        :floating="isSidebarFloating"
        :items="categories"
        :model-value="route.path"
        :width="236"
        :collapsed-width="64"
        v-model:collapsed="isSidebarCollapsed"
        collapsible
        title="RizUI"
        icon="fa-solid fa-shapes"
        @brand-click="goHome()"
      />
      <div class="content">
        <router-view/>
      </div>
    </div>

    <!-- ═══ 首页 / 独立页面：无侧边栏 ═══ -->
    <router-view v-else />
  </div>
</template>

<style>
/* Demo 使用本地 UI 字体（版权字体，仅用于演示；组件库不内置以避免版权纠纷） */
@font-face {
  font-family: 'UI';
  src: url('./fonts/UI.ttf') format('truetype');
  font-weight: normal;
  font-style: normal;
  font-display: block;
}

*,
*::before,
*::after {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

body {
  font-family: 'UI', -apple-system, BlinkMacSystemFont, 'Segoe UI', 'PingFang SC',
  'Microsoft YaHei', sans-serif;
  background: #f3f5f8;
  color: var(--rz-text);
  isolation: isolate;
}

code {
  font-family: 'Cascadia Code', 'Fira Code', Consolas, monospace;
  font-size: 12px;
}

.app {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

/* ── 上顶栏（全宽） ── */
.topbar {
  width: 100%;
  background: var(--rz-bg);
  border-bottom: 1px solid var(--rz-border);
  position: sticky;
  top: 0;
  z-index: 1000;
}

.topbar-inner {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px 24px;
  gap: 14px;
}

.topbar-left {
  display: flex;
  align-items: center;
  flex-shrink: 0;
}

.topbar-home {
  font-size: 28px;
  font-weight: 800;
  color: var(--rz-text);
  cursor: pointer;
  user-select: none;
  white-space: nowrap;
  transition: color 0.15s;
  margin-right: 12px;
}

.topbar-home:hover {
  color: v-bind(themeColor);
}

.topbar-right {
  display: flex;
  align-items: center;
  gap: 14px;
  flex-shrink: 0;
}

.topbar-price {
  font-size: 12px;
  color: var(--rz-subtext);
  white-space: nowrap;
  cursor: pointer;
  transition: color 0.15s;
}

.topbar-price:hover {
  color: var(--rz-text);
}

.topbar-gh {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-size: 12px;
  color: var(--rz-subtext);
  text-decoration: none;
  white-space: nowrap;
  transition: color 0.15s;
}

.topbar-gh:hover {
  color: var(--rz-text);
}

/* 主题模式切换按钮 */
.theme-switch {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 5px 12px;
  border: 1px solid var(--rz-border);
  border-radius: 999px;
  background: color-mix(in srgb, var(--rz-text) 5%, var(--rz-bg));
  color: var(--rz-subtext);
  font-size: 12px;
  font-weight: 600;
  font-family: inherit;
  cursor: pointer;
  white-space: nowrap;
  transition: background 0.15s, color 0.15s, border-color 0.15s;
}

.theme-switch:hover {
  background: color-mix(in srgb, var(--rz-text) 10%, var(--rz-bg));
  color: var(--rz-text);
}

.theme-switch svg {
  flex-shrink: 0;
}

/* ── Demo 模式工作区布局 ── */
.demo-workbench {
  display: flex;
  flex: 1;
  position: relative;
}

.sbar-cat {
  margin-bottom: 2px;
}

.sbar-head {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  padding: 8px 10px;
  border: none;
  background: transparent;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  font-family: inherit;
  color: var(--rz-text);
  cursor: pointer;
  transition: background 0.15s;
}

.sbar-head:hover {
  background: color-mix(in srgb, var(--rz-text) 6%, var(--rz-bg));
}

.sbar-caret {
  font-size: 14px;
  color: var(--rz-subtext);
  transition: transform 0.2s;
}

.sbar-caret.open {
  transform: rotate(90deg);
}

/* 展开 / 收起动画：内层单行 grid，行高 0fr↔1fr（元素常驻 DOM，收起高度平滑收缩） */
.sbar-items {
  display: grid;
  grid-template-rows: 0fr;
  opacity: 0;
  padding: 2px 0 6px;
  transition: grid-template-rows 0.28s ease, opacity 0.18s ease;
}
.sbar-items.open {
  grid-template-rows: 1fr;
  opacity: 1;
}
.sbar-items-inner {
  overflow: hidden;
  min-height: 0;
}

.sbar-item {
  display: block;
  padding: 6px 10px 6px 22px;
  border-radius: 7px;
  font-size: 13px;
  color: var(--rz-subtext);
  text-decoration: none;
  transition: background 0.15s, color 0.15s;
}

.sbar-item:hover {
  background: color-mix(in srgb, var(--rz-text) 6%, var(--rz-bg));
  color: var(--rz-text);
}

.sbar-item.active {
  background: color-mix(in srgb, var(--rizui_themecolor) 10%, var(--rz-bg));
  font-weight: 600;
}

.sbar-item.deprecated {
  opacity: 0.6;
}

.content {
  flex: 1;
  min-width: 0;
}.depr-tag {
  display: inline-block;
  margin-left: 5px;
  padding: 0 5px;
  border-radius: 4px;
  background: #fff3cd;
  color: #856404;
  font-size: 10px;
  font-weight: 600;
  line-height: 1.4;
  vertical-align: middle;
}

/* ── 主内容区 ── */
.main {
  flex: 1;
  max-width: 860px;
  margin: 0 auto;
  width: 100%;
  padding: 24px 20px 60px;
}

/* ── 首页 ── */
.home-hero {
  text-align: center;
  padding: 60px 0 40px;
}

.home-logo {
  font-size: 40px;
  font-weight: 800;
  color: var(--rz-text);
  margin-bottom: 12px;
}

.home-desc {
  font-size: 16px;
  color: var(--rz-subtext);
  margin-bottom: 28px;
}

.home-actions {
  display: flex;
  justify-content: center;
  gap: 12px;
}

.home-btn {
  display: inline-block;
  padding: 10px 24px;
  border-radius: 10px;
  font-size: 14px;
  font-weight: 600;
  text-decoration: none;
  cursor: pointer;
  font-family: inherit;
  border: none;
  transition: opacity 0.2s;
}

.home-btn:hover {
  opacity: 0.85;
}

.home-btn-primary {
  color: #fff;
}

.home-btn-ghost {
  background: var(--rz-bg);
  color: var(--rz-subtext);
  border: 1.5px solid var(--rz-border);
}

.home-btn-ghost:hover {
  background: color-mix(in srgb, var(--rz-text) 6%, var(--rz-bg));
  opacity: 1;
}

.home-features {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: 16px;
  margin-bottom: 36px;
}

.home-feat {
  background: var(--rz-bg);
  border-radius: 14px;
  padding: 20px;
  border: 1px solid var(--rz-border);
  text-align: center;
}

.home-feat-icon {
  font-size: 28px;
  margin-bottom: 8px;
  color: v-bind(themeColor);
}

.home-feat h3 {
  font-size: 14px;
  font-weight: 700;
  color: var(--rz-text);
  margin-bottom: 6px;
}

.home-feat p,
.home-feat code {
  font-size: 12px;
  color: var(--rz-subtext);
  line-height: 1.6;
}

.home-quick h2 {
  font-size: 18px;
  font-weight: 700;
  margin-bottom: 12px;
  color: var(--rz-text);
}

/* ── 许可页 ── */
.pricing-page .sec-header {
  margin-bottom: 24px;
}

.pricing-list {
  margin-top: 8px;
}

.pricing-row {
  font-size: 13px;
  color: color-mix(in srgb, var(--rz-text) 80%, transparent);
  padding: 6px 0;
  display: flex;
  align-items: center;
  gap: 8px;
}

.pricing-row span {
  flex-shrink: 0;
}

/* ── 组件页 Sections ── */
.sec-header {
  margin-bottom: 24px;
}

.sec-title-row {
  display: flex;
  align-items: center;
  gap: 10px;
}

.sec-header h2 {
  font-size: 22px;
  font-weight: 800;
  color: var(--rz-text);
}

.sec-tag {
  display: inline-block;
  padding: 1px 10px;
  border-radius: 999px;
  background: color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  font-size: 11px;
  color: var(--rz-subtext);
}

.sec-deprecated {
  display: inline-block;
  padding: 1px 8px;
  border-radius: 999px;
  background: #fff3cd;
  color: #856404;
  font-size: 11px;
  font-weight: 600;
}

.sec-desc {
  margin-top: 10px;
  font-size: 13px;
  color: var(--rz-subtext);
  line-height: 1.6;
}

.sec-demo,
.sec-props,
.sec-slots,
.sec-events,
.sec-usage {
  margin-bottom: 28px;
}

.sec-demo h3,
.sec-props h3,
.sec-slots h3,
.sec-events h3,
.sec-usage h3 {
  font-size: 15px;
  font-weight: 700;
  margin-bottom: 12px;
  color: var(--rz-text);
}

.sec-demo h3 .fa-solid,
.sec-props h3 .fa-solid,
.sec-slots h3 .fa-solid,
.sec-events h3 .fa-solid,
.sec-usage h3 .fa-solid {
  margin-right: 6px;
  color: v-bind(themeColor);
}

/* ── Props 表格 ── */
.props-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 12px;
  background: var(--rz-bg);
  border-radius: 10px;
  overflow: hidden;
  border: 1px solid var(--rz-border);
}

.props-table th {
  text-align: left;
  padding: 8px 12px;
  background: color-mix(in srgb, var(--rz-text) 4%, var(--rz-bg));
  font-weight: 600;
  color: var(--rz-subtext);
  border-bottom: 1px solid var(--rz-border);
}

.props-table td {
  padding: 8px 12px;
  border-bottom: 1px solid color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  color: color-mix(in srgb, var(--rz-text) 88%, transparent);
}

.props-table tr:last-child td {
  border-bottom: none;
}

.props-table code {
  background: color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  padding: 1px 6px;
  border-radius: 4px;
}

.sec-slots ul,
.sec-events ul {
  list-style: none;
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.sec-slots li,
.sec-events li {
  background: color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  padding: 4px 12px;
  border-radius: 6px;
  font-size: 12px;
}

.code-block {
  background: #1e1e2e;
  color: #cdd6f4;
  padding: 16px 18px;
  border-radius: 10px;
  font-size: 12px;
  line-height: 1.7;
  overflow-x: auto;
  white-space: pre;
}

.demo-echo {
  font-size: 12px;
  color: var(--rz-subtext);
  padding-top: 4px;
}

.demo-echo code {
  background: color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  padding: 1px 6px;
  border-radius: 4px;
  color: color-mix(in srgb, var(--rz-text) 85%, transparent);
}

.demo-btn {
  padding: 7px 18px;
  border: none;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  color: #fff;
  cursor: pointer;
  font-family: inherit;
  transition: opacity 0.2s;
}

.demo-btn:hover {
  opacity: 0.85;
}

.demo-btn-ghost {
  background: color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  color: var(--rz-subtext);
}

.demo-btn-ghost:hover {
  background: color-mix(in srgb, var(--rz-text) 12%, var(--rz-bg));
  opacity: 1;
}

.demo-check {
  width: 32px;
  height: 32px;
  border: none;
  border-radius: 50%;
  color: #fff;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: opacity 0.2s;
}

.demo-check:hover {
  opacity: 0.85;
}

.cp-preview {
  margin-top: 8px;
  height: 36px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 12px;
  font-weight: 700;
  color: #fff;
  text-shadow: 0 1px 3px rgba(0, 0, 0, 0.3);
  font-family: 'Cascadia Code', 'Fira Code', Consolas, monospace;
}

.nro-demo {
  position: relative;
  overflow: hidden;
  border-radius: 10px;
  padding: 0 12px;
  background: color-mix(in srgb, var(--rz-text) 3%, var(--rz-bg));
}

/* ── 背景泡泡（仅 demo 装饰） ── */
.demo-bubbles {
  position: fixed;
  inset: 0;
  pointer-events: none;
  overflow: hidden;
  z-index: -1;
  filter: blur(10px);
  background: var(--rz-bg);
}

.demo-bub {
  position: absolute;
  border-radius: 50%;
  background: v-bind(themeColor);
  opacity: 0;
  animation: db-float var(--dur, 12s) ease-in-out infinite;
  animation-delay: var(--delay, 0s);
}

.b1 {
  width: 190px;
  height: 190px;
  top: 5%;
  left: 5%;
  --dur: 38s;
  --delay: 0s;
  opacity: 0.10;
}

.b2 {
  width: 180px;
  height: 180px;
  top: 55%;
  left: 75%;
  --dur: 35s;
  --delay: -5s;
  opacity: 0.16;
}

.b3 {
  width: 200px;
  height: 200px;
  top: 72%;
  left: 8%;
  --dur: 40s;
  --delay: -10s;
  opacity: 0.15;
}

.b4 {
  width: 185px;
  height: 185px;
  top: 20%;
  left: 60%;
  --dur: 36s;
  --delay: -15s;
  opacity: 0.16;
}

.b5 {
  width: 195px;
  height: 195px;
  top: 38%;
  left: 35%;
  --dur: 42s;
  --delay: -7s;
  opacity: 0.15;
}

.b6 {
  width: 190px;
  height: 190px;
  top: 82%;
  left: 48%;
  --dur: 34s;
  --delay: -12s;
  opacity: 0.16;
}

.b7 {
  width: 180px;
  height: 180px;
  top: 15%;
  left: 85%;
  --dur: 38s;
  --delay: -3s;
  opacity: 0.16;
}

.b8 {
  width: 200px;
  height: 200px;
  top: 48%;
  left: 15%;
  --dur: 41s;
  --delay: -9s;
  opacity: 0.15;
}

.b9 {
  width: 185px;
  height: 185px;
  top: 60%;
  left: 52%;
  --dur: 37s;
  --delay: -14s;
  opacity: 0.16;
}

.b10 {
  width: 195px;
  height: 195px;
  top: 30%;
  left: 78%;
  --dur: 39s;
  --delay: -6s;
  opacity: 0.16;
}

@keyframes db-float {
  0% {
    transform: translate(0, 0) scale(1);
  }
  20% {
    transform: translate(100px, -80px) scale(1.1);
  }
  40% {
    transform: translate(-70px, 90px) scale(0.9);
  }
  60% {
    transform: translate(-110px, -40px) scale(1.08);
  }
  80% {
    transform: translate(50px, 60px) scale(0.95);
  }
  100% {
    transform: translate(0, 0) scale(1);
  }
}

/* ── 暗色模式：demo 外壳适配 ── */
[data-rizui-theme="dark"] body {
  background: #161d27;
}

[data-rizui-theme="dark"] .depr-tag,
[data-rizui-theme="dark"] .sec-deprecated {
  background: #4a3b10;
  color: #ffd54f;
}
</style>


