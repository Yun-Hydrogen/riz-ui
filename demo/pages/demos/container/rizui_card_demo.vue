<script setup>
import { inject } from 'vue'
import { rizui_card, rizui_cfgrow, rizui_infobox, rizui_code } from 'riz-ui'
const themeColor = inject('themeColor')

const props = [
  { name: 'title',        type: 'String',  default: '""',    desc: '标题文字' },
  { name: 'title-style', type: 'Object',  default: '{}',    desc: '标题内联样式覆盖' },
  { name: 'desc',         type: 'String',  default: '""',    desc: '描述文字' },
  { name: 'desc-style',  type: 'Object',  default: '{}',    desc: '描述内联样式覆盖' },
  { name: 'icon',         type: 'String',  default: '""',    desc: 'FA 图标类名（支持全部 FA class 组合）' },
  { name: 'icon-style',  type: 'Object',  default: '{}',    desc: '图标内联样式覆盖' },
  { name: 'theme',        type: 'String',  default: "'auto'", desc: '主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]
const slots = ['default — 卡片内容区']

const usage = `<!-- 1. 基础卡片：标题 + 描述 + 图标 -->
<rizui_card
  title="MomoTalk 信箱"
  desc="老师，您有 3 条未读消息 · 最近来自游戏开发部"
  icon="fa-solid fa-envelope-open-text"
  :icon-style="{ color: '#f0a030', fontSize: '28px' }">
  <rizui_cfgrow label="天童爱丽丝" hint="邦邦卡邦！老师，周末来游戏开发部一起打游戏吧！" />
</rizui_card>

<!-- 2. 空卡片：不设置任何 prop，仅作背景容器 -->
<rizui_card>
  <rizui_cfgrow label="干净的背景板" hint="想放什么就放什么" />
</rizui_card>

<!-- 3. 图标动效：完整支持 FA class -->
<rizui_card
  title="总力战 · 备战中"
  desc="图标加个 fa-spin 就能一直转"
  icon="fa-solid fa-gear fa-spin"
  :icon-style="{ color: themeColor, fontSize: '22px' }" />

<!-- 4. 样式定制：title-style / desc-style / icon-style -->
<rizui_card
  title="沙勒公告"
  desc="来自什亭之箱的系统通知"
  icon="fa-solid fa-bullhorn"
  :title-style="{ color: '#e278b3', fontSize: '17px' }"
  :desc-style="{ color: '#c084fc' }"
  :icon-style="{ color: '#e278b3', fontSize: '24px' }" />

<!-- 5. 强制明暗：theme="Dark" / theme="Light" -->
<rizui_card title="强制暗色" desc="theme='Dark'" theme="Dark" />
<rizui_card title="强制亮色" desc="theme='Light'" theme="Light" />`

const card1_usage =
`<rizui_card
  title="MomoTalk 信箱"
  desc="老师，您有 3 条未读消息 · 最近来自游戏开发部"
  icon="fa-solid fa-envelope-open-text"
  :icon-style="{ color: '#f0a030', fontSize: '28px' }">
  <rizui_cfgrow label="天童爱丽丝" hint="邦邦卡邦！老师，周末来打游戏吧！" />
</rizui_card>`

const card2_usage = `不设置任何 prop，它就是一个干净的背景容器`

const card3_usage =
`<rizui_card
  title="总力战 · 备战中"
  desc="图标支持全部 FA class 组合"
  icon="fa-solid fa-gear fa-spin"
  :icon-style="{ color: themeColor, fontSize: '22px' }" />`

const card4_usage =
`<rizui_card
  title="沙勒公告"
  desc="来自什亭之箱的系统通知 · 今日基沃托斯天气：晴"
  icon="fa-solid fa-bullhorn"
  :title-style="{ color: '#e278b3', fontSize: '17px' }"
  :desc-style="{ color: '#c084fc' }"
  :icon-style="{ color: '#e278b3', fontSize: '24px' }">
  <rizui_cfgrow label="阿洛娜" hint="老师，今天也要元气满满哦～" />
</rizui_card>`

const card5_usage =
`<rizui_card title="强制暗色" desc="theme='Dark'" theme="Dark" icon="fa-solid fa-moon" />
<rizui_card title="强制亮色" desc="theme='Light'" theme="Light" icon="fa-solid fa-sun" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>卡片容器</h2>
        <p class="sec-tag">rizui_card</p>
      </div>
      <p class="sec-desc">高度自定义的卡片容器。</p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- MomoTalk 信箱 · 标准卡片 -->
      <rizui_card
        title="MomoTalk 信箱"
        desc="老师，您有 3 条未读消息 · 最近来自游戏开发部"
        icon="fa-solid fa-envelope-open-text"
        :icon-style="{ color: '#f0a030', fontSize: '28px' }">
        <rizui_cfgrow
          label="天童爱丽丝"
          hint="邦邦卡邦！老师，周末来游戏开发部一起打游戏吧！" />
        <rizui_cfgrow
          label="小鸟游星野"
          hint="老师要不要再来阿拜多斯体验大叔的枕头呀~" />
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card1_usage" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 纯背景容器 · 空卡片 -->
      <rizui_card>
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card2_usage" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 旋转图标 · 图标动效 -->
      <rizui_card
        title="总力战 · 备战中"
        desc="icon 支持全部 FA class，加个 fa-spin 就能一直转"
        icon="fa-solid fa-gear fa-spin"
        :icon-style="{ color: themeColor, fontSize: '22px' }" style="margin-top: 20px">
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card3_usage" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 样式定制 · Custom Style -->
      <rizui_card
        title="沙勒公告"
        desc="来自什亭之箱的系统通知 · 今日基沃托斯天气：晴"
        icon="fa-solid fa-bullhorn"
        :icon-style="{ color: '#e278b3', fontSize: '24px' }"
        :title-style="{ fontSize: '17px', letterSpacing: '1px', color: '#e278b3' }"
        :desc-style="{ color: '#c084fc' }">
        <rizui_cfgrow
          label="阿洛娜"
          hint="老师，今天也要元气满满地处理联邦学生会的事务哦～" />
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card4_usage" />
        </rizui_cfgrow>
      </rizui_card>
      
      <table class="props-table">
        <thead>
          <tr>
            <th>属性</th>
            <th>类型</th>
            <th>必填</th>
            <th>默认值</th>
            <th>说明</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="p in props" :key="p.name">
            <td><code>{{ p.name }}</code></td>
            <td><code>{{ p.type }}</code></td>
            <td>{{ p.required || '—' }}</td>
            <td><code>{{ p.default || '—' }}</code></td>
            <td>{{ p.desc }}</td>
          </tr>
        </tbody>
      </table>
    </section>

    <!-- 主题模式提示 -->
    <rizui_infobox type="info">
      <code>theme</code> 支持 <code>Dark</code> / <code>Light</code> / <code>auto</code>（默认）。<code>auto</code> 跟随顶栏主题开关（全局 <code>data-rizui-theme</code>）或系统 <code>prefers-color-scheme</code>；<code>Dark</code> / <code>Light</code> 可对本组件局部强制明暗。试试右上角主题按钮实时切换，或在你的项目中于 <code>&lt;html&gt;</code> 上设置 <code>data-rizui-theme</code>。
    </rizui_infobox>

    <section class="sec-slots">
      <h3><i class="fa-solid fa-puzzle-piece"></i> Slots</h3>
      <ul><li v-for="s in slots" :key="s"><code>{{ s }}</code></li></ul>
    </section>
    <section class="sec-usage">
      <h3><i class="fa-solid fa-code"></i> 用法</h3>
      <rizui_code block lang="html" :code="usage" />
    </section>
  </main>
</template>
