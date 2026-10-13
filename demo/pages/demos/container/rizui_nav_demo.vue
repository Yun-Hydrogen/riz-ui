<script setup>
import { ref, inject } from 'vue'
import {
  rizui_nav,
  rizui_card,
  rizui_cfgrow,
  rizui_switch,
  rizui_infobox,
  rizui_code
} from 'riz-ui'

const themeColor = inject('themeColor')

const currentActive = ref('dashboard')
const isCollapsed = ref(false)
const isAccordion = ref(false)
const isCollapsible = ref(true)
const isFloating = ref(false)

const demoNavItems = [
  {
    id: 'dashboard',
    label: '控制面板',
    icon: 'fa-solid fa-house',
    badge: '9+'
  },
  {
    id: 'projects',
    label: '项目工程',
    icon: 'fa-solid fa-folder-tree',
    children: [
      { id: 'proj-active', label: '进行中任务', icon: 'fa-solid fa-play', tag: '进行' },
      { id: 'proj-archived', label: '历史归档', icon: 'fa-solid fa-box-archive' },
      {
        id: 'proj-deep',
        label: '多层目录示例',
        icon: 'fa-solid fa-folder',
        children: [
          { id: 'deep-sub1', label: '三级子目录 A', icon: 'fa-solid fa-file-code' },
          { id: 'deep-sub2', label: '三级子目录 B', icon: 'fa-solid fa-file-lines' }
        ]
      }
    ]
  },
  {
    id: 'analytics',
    label: '数据分析',
    icon: 'fa-solid fa-chart-pie',
    children: [
      { id: 'ana-realtime', label: '实时监控', icon: 'fa-solid fa-bolt' },
      { id: 'ana-reports', label: '财务报表', icon: 'fa-solid fa-file-invoice-dollar' }
    ]
  },
  {
    id: 'settings',
    label: '系统配置',
    icon: 'fa-solid fa-gear'
  },
  {
    id: 'legacy',
    label: '废弃项示例',
    icon: 'fa-solid fa-clock-rotate-left',
    deprecated: true
  }
]

const props = [
  { name: 'items',                 type: 'Array',            default: '[]',       desc: '树形导航数据源，支持 children 多级嵌套' },
  { name: 'modelValue',            type: 'String | Number',  default: '""',       desc: '当前激活项的 id 或 route（支持 v-model）' },
  { name: 'collapsed',             type: 'Boolean',          default: 'false',    desc: '是否折叠为极简图标 Rail 模式（支持 v-model:collapsed）' },
  { name: 'collapsible',           type: 'Boolean',          default: 'false',    desc: '是否显示顶部展开/折叠汉堡切换按钮' },
  { name: 'title',                 type: 'String',           default: '""',       desc: '导航栏顶部品牌/标题文本' },
  { name: 'icon',                  type: 'String',           default: '""',       desc: '导航栏顶部品牌图标（完整 FA class）' },
  { name: 'width',                 type: 'Number | String',  default: '240',      desc: '展开时的宽度（默认 240px）' },
  { name: 'collapsed-width',       type: 'Number | String',  default: '64',       desc: '折叠为 Rail 模式时的紧凑宽度（默认 64px）' },
  { name: 'height',                type: 'Number | String',  default: "'100vh'",  desc: '导航栏高度，默认为页面高度 100vh，超出在内部独立滚动' },
  { name: 'fixed',                 type: 'Boolean',          default: 'false',    desc: '是否固定定位在屏幕左侧（position: fixed）' },
  { name: 'floating',              type: 'Boolean',          default: 'false',    desc: '是否浮动在页面上（Overlay 模式）：true 不占空间直接覆盖；false 占用固定空间；视觉上皆呈现精致悬浮岛' },
  { name: 'inset',                 type: 'Number | String',  default: '12',       desc: '悬浮岛边缘呼吸间距（像素，传 0 则无缝贴合边缘）' },
  { name: 'top',                   type: 'Number | String',  default: '0',        desc: '固定定位时距离顶部的偏移量（如避开特定顶栏）' },
  { name: 'bottom',                type: 'Number | String',  default: '0',        desc: '固定定位时距离底部的偏移量' },
  { name: 'color',                 type: 'String',           default: '""',       desc: '激活高亮主题色，为空跟随 --rizui_themecolor' },
  { name: 'default-expanded-keys', type: 'Array',            default: '[]',       desc: '默认展开的节点 id 数组' },
  { name: 'accordion',             type: 'Boolean',          default: 'false',    desc: '手风琴模式（同层级子目录只允许展开一个）' },
  { name: 'bordered',              type: 'Boolean',          default: 'true',     desc: '是否显示外边框与卡片阴影' },
  { name: 'theme',                 type: 'String',           default: "'auto'",   desc: '主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]

const events = [
  'update:modelValue (val: string | number)',
  'update:collapsed (collapsed: boolean)',
  'select (item: NavItem)',
  'toggle (payload: { item: NavItem, expanded: boolean })',
  'brand-click ()'
]

const slots = [
  { name: 'header', desc: '自定义导航栏顶部区域（作用域参数：{ collapsed, toggleCollapse }）' },
  { name: 'footer', desc: '自定义导航栏底部区域（作用域参数：{ collapsed, toggleCollapse }）' },
  { name: 'icon',   desc: '自定义菜单项图标（作用域参数：{ item, active }）' },
  { name: 'label',  desc: '自定义菜单项文字（作用域参数：{ item, active }）' }
]

const usage = `<!-- 1. 基础用法与树形子目录 -->
<rizui_nav
  v-model="currentActive"
  title="应用导航"
  icon="fa-solid fa-compass"
  :items="navItems"
  collapsible
/>

<!-- 2. 手风琴互斥展开模式 -->
<rizui_nav
  v-model="currentActive"
  :items="navItems"
  accordion
/>

<!-- 3. 浮动在页面上（Overlay 悬浮模式，不占用页面排版空间） -->
<rizui_nav
  v-model="currentActive"
  :items="navItems"
  fixed
  floating
/>

<!-- 4. 自定义底部快捷栏 -->
<rizui_nav v-model="currentActive" :items="navItems">
  <template #footer="{ collapsed }">
    <div v-if="!collapsed" style="font-size: 12px; color: var(--rz-subtext); text-align: center;">
      v26.9 · 沙勒工作区
    </div>
  </template>
</rizui_nav>`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>导航栏 / 侧边栏</h2>
        <p class="sec-tag">rizui_nav</p>
      </div>
      <p class="sec-desc">
        现代容器导航组件，支持无限级树状子目录展开收起、极简 Rail 折叠、文字旁图标、胶囊指示动效及平滑高度过渡。
      </p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <rizui_card title="交互控制台" desc="实时切换导航栏状态与模式">
        <rizui_cfgrow label="折叠为 Rail 模式" hint="收起侧栏仅展示图标">
          <rizui_switch v-model="isCollapsed" />
        </rizui_cfgrow>
        <rizui_cfgrow label="开启折叠切换按钮" hint="顶部提供切换汉堡按钮">
          <rizui_switch v-model="isCollapsible" />
        </rizui_cfgrow>
        <rizui_cfgrow label="手风琴折叠互斥模式" hint="展开子目录时自动收起同级其他目录">
          <rizui_switch v-model="isAccordion" />
        </rizui_cfgrow>
        <rizui_cfgrow label="浮动在页面上 (floating)" hint="开启则悬浮覆盖不占排版空间，关闭则占用固定空间；视觉上皆为精致悬浮岛">
          <rizui_switch v-model="isFloating" />
        </rizui_cfgrow>
        <rizui_cfgrow label="当前激活项" :hint="`当前选择：${currentActive}`">
          <code style="padding: 2px 8px; border-radius: 4px; background: var(--rz-bg); border: 1px solid var(--rz-border);">
            {{ currentActive }}
          </code>
        </rizui_cfgrow>

        <!-- 导航栏实时预览容器 -->
        <div style="display: flex; justify-content: center; padding: 20px 0; background: color-mix(in srgb, var(--rz-text) 3%, transparent); border-radius: 14px; margin-top: 10px; position: relative;">
          <div style="height: 480px; position: relative;">
            <rizui_nav
              v-model="currentActive"
              v-model:collapsed="isCollapsed"
              :collapsible="isCollapsible"
              :accordion="isAccordion"
              :floating="isFloating"
              height="100%"
              title="控制中心"
              icon="fa-solid fa-shapes"
              :items="demoNavItems"
              :default-expanded-keys="['projects']"
            >
              <template #footer="{ collapsed }">
                <div v-if="!collapsed" style="font-size: 11px; color: var(--rz-subtext); text-align: center; padding: 4px 0;">
                  <i class="fa-solid fa-shield-halved" style="margin-right: 4px;"></i> 沙勒安全终端
                </div>
              </template>
            </rizui_nav>
          </div>
        </div>
      </rizui_card>
    </section>

    <!-- Props 表格 -->
    <section class="sec-props">
      <h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table">
        <thead>
          <tr>
            <th>属性</th>
            <th>类型</th>
            <th>默认值</th>
            <th>说明</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="p in props" :key="p.name">
            <td><code>{{ p.name }}</code></td>
            <td><code>{{ p.type }}</code></td>
            <td><code>{{ p.default || '—' }}</code></td>
            <td>{{ p.desc }}</td>
          </tr>
        </tbody>
      </table>
    </section>

    <!-- Events 表格 -->
    <section class="sec-events">
      <h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul>
        <li v-for="e in events" :key="e"><code>{{ e }}</code></li>
      </ul>
    </section>

    <!-- Slots 表格 -->
    <section class="sec-props">
      <h3><i class="fa-solid fa-puzzle-piece"></i> Slots</h3>
      <table class="props-table">
        <thead>
          <tr>
            <th>插槽名</th>
            <th>说明</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="s in slots" :key="s.name">
            <td><code>{{ s.name }}</code></td>
            <td>{{ s.desc }}</td>
          </tr>
        </tbody>
      </table>
    </section>

    <!-- 主题模式提示 -->
    <rizui_infobox type="info">
      <code>theme</code> 支持 <code>Dark</code> / <code>Light</code> / <code>auto</code>（默认）。<code>auto</code> 跟随顶栏主题开关（全局 <code>data-rizui-theme</code>）或系统 <code>prefers-color-scheme</code>；<code>Dark</code> / <code>Light</code> 可对本组件局部强制明暗。
    </rizui_infobox>

    <!-- 主题色提示 -->
    <rizui_infobox type="info">
      未指定 <code>color</code> 时，选中项与指示胶囊自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。可在顶部取色器实时换色进行验证。
    </rizui_infobox>

    <section class="sec-usage">
      <h3><i class="fa-solid fa-code"></i> 用法</h3>
      <rizui_code block lang="html" :code="usage" />
    </section>
  </main>
</template>
