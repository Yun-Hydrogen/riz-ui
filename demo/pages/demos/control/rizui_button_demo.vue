<script setup>
import { ref, inject } from 'vue'
import { rizui_button, rizui_card, rizui_cfgrow, rizui_infobox, rizui_code, openURL } from 'riz-ui'
const themeColor = inject('themeColor')

const apRecoverCount = ref(0)

const props = [
  { name: 'text',         type: 'String',  default: '""',       desc: '按钮文本，可为空' },
  { name: 'text-style',  type: 'Object',  default: '{}',       desc: '文本内联样式覆盖' },
  { name: 'l-icon',       type: 'String',  default: '""',       desc: '左侧图标（完整 FA class）' },
  { name: 'l-icon-style', type: 'Object', default: '{}',      desc: '左侧图标内联样式覆盖' },
  { name: 'r-icon',       type: 'String',  default: '""',       desc: '右侧图标（完整 FA class）' },
  { name: 'r-icon-style', type: 'Object', default: '{}',      desc: '右侧图标内联样式覆盖' },
  { name: 'primary',      type: 'Boolean', default: 'false',    desc: '是否以主题色填充背景' },
  { name: 'danger',       type: 'Boolean', default: 'false',    desc: '是否危险/警告按钮（红色）' },
  { name: 'long',         type: 'Boolean', default: 'false',    desc: '是否宽度撑满父容器' },
  { name: 'color',        type: 'String',  default: '""',       desc: '主题色，为空时跟随 --rizui_themecolor' },
  { name: 'disabled',     type: 'Boolean', default: 'false',    desc: '是否禁用' },
  { name: 'type',         type: 'String',  default: '"button"', desc: '原生按钮 type' },
  { name: 'theme',        type: 'String',  default: "'auto'",   desc: '主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]
const events = ['click']

const usage = `<!-- 1. 基础指挥指令：日常作战指令与紧急撤退 -->
<rizui_cfgrow label="总力战指挥" hint="下达突击与撤退指令">
  <rizui_button text="下达出击指令" r-icon="fa-solid fa-rocket" primary />
  <rizui_button text="全员撤退" danger />
</rizui_cfgrow>
<rizui_button text="确认进入总力战" primary long />

<rizui_button text="发送MomoTalk" l-icon="fa-solid fa-paper-plane" primary />
<rizui_button l-icon="fa-solid fa-rotate-right" />

<rizui_button text="消耗青辉石招募" primary color="#5bc0be" />
<rizui_button text="兑换信用点" primary color="#f0a030" />

<!-- 5. 链接跳转：推荐用 openURL（组件库导出）配合 @click 打开外部链接 -->
<rizui_button text="沙勒档案库（B站）" l-icon="fa-brands fa-bilibili" @click="openURL('https://space.bilibili.com/560229787')" />`

const card1_usage =
`<rizui_button text="确认指令" />
<rizui_button text="动员" primary r-icon="fa-solid fa-rocket" />
<rizui_button text="撤退" danger />
<rizui_button text="确认沙勒出击部署" primary long />`

const card2_usage =
`<rizui_button text="发送MomoTalk" l-icon="fa-solid fa-paper-plane" primary />
<rizui_button text="查看学员档案" r-icon="fa-solid fa-angle-right" />
<rizui_button l-icon="fa-solid fa-plus" primary />
<rizui_button text="特别好评" l-icon="fa-solid fa-star" :l-icon-style="{ color: '#ffb300' }" primary />`

const card3_usage =
`<rizui_button text="沙勒媒体" l-icon="fa-brands fa-bilibili"
              @click="openURL('https://space.bilibili.com/560229787')" />`
  
const card4_usage =
`<rizui_button text="权限未解锁" primary disabled />
<rizui_button text="青辉石" primary color="#66ccff" />
<rizui_button text="信用点" primary color="#f0a030" />
<rizui_button text="消耗青辉石补充体力点" primary @click="apRecoverCount++" />`

</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>按钮</h2>
        <p class="sec-tag">rizui_button</p>
      </div>
      <p class="sec-desc">支持普通、主要、危险、通栏四种基础样式，以及左侧图标、右侧图标、自定义颜色和外部链接跳转等功能。</p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 沙勒指挥指令 · 基本样式 -->
      <rizui_card title="沙勒日常指令 | 基本样式" desc="普通 / primary / danger / long 四种基础形态">
        <rizui_cfgrow label="日常确认" hint="普通按钮形态，无额外填充">
          <rizui_button text="确认" />
        </rizui_cfgrow>
        <rizui_cfgrow label="主指令（Primary）" hint="primary 属性使用主题色填充背景，适合主要操作">
          <rizui_button text="动员" primary r-icon="fa-solid fa-rocket" />
        </rizui_cfgrow>
        <rizui_cfgrow label="危险指令（Danger）" hint="danger 属性呈现红色，用于删除、撤退等高危操作">
          <rizui_button text="撤退" danger />
        </rizui_cfgrow>
        <rizui_cfgrow label="通栏指令（Long）" hint="long 属性撑满父容器宽度，适合表单底部操作" stack>
          <rizui_button text="确认进入总力战" primary long />
        </rizui_cfgrow>
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card1_usage" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- MomoTalk 与交互图标按钮 -->
      <rizui_card title="MomoTalk 与图标指令 | 进阶用法" desc="l-icon / r-icon 支持完整 FA class，text 留空时为仅图标按钮">
        <rizui_cfgrow label="左侧图标" hint="l-icon 放置于文字左侧">
          <rizui_button text="发送MomoTalk" l-icon="fa-solid fa-paper-plane" primary />
        </rizui_cfgrow>
        <rizui_cfgrow label="右侧图标" hint="r-icon 放置于文字右侧">
          <rizui_button text="查看学员档案" r-icon="fa-solid fa-angle-right" />
        </rizui_cfgrow>
        <rizui_cfgrow label="纯图标快捷操作" hint="text 留空时渲染为紧凑图标按钮">
          <div style="gap: 10px ; display: flex; flex-wrap: wrap">
            <rizui_button l-icon="fa-solid fa-plus" primary />
            <rizui_button l-icon="fa-solid fa-rotate-right" />
            <rizui_button l-icon="fa-solid fa-heart" danger />
          </div>
        </rizui_cfgrow>
        <rizui_cfgrow label="图标样式覆盖" hint="l-icon-style / r-icon-style / text-style 支持覆盖内联样式">
          <rizui_button text="特别好评" l-icon="fa-solid fa-star" :l-icon-style="{ color: '#ffb300' }" primary />
        </rizui_cfgrow>
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card2_usage" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 基沃托斯外部通信 · 链接跳转 -->
      <rizui_card title="外部链接跳转" desc="推荐用 openURL() 打开链接（组件库内置，避免弹窗拦截）">
        <rizui_cfgrow label="跳转外部频道" hint="@click 绑定 openURL(url)，默认新标签页 _blank">
          <rizui_button text="沙勒媒体" l-icon="fa-brands fa-bilibili" @click="openURL('https://space.bilibili.com/560229787')" />
          <rizui_button text="代码库（GitHub）" l-icon="fa-brands fa-github" @click="openURL('https://github.com/Yun-Hydrogen/riz-ui')" />
        </rizui_cfgrow>
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card3_usage" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 权限状态与物资配色 -->
      <rizui_card title="权限状态与物资配色 | 自定义" desc="disabled 禁用态、color 自定义主题色与 click 点击事件">
        <rizui_cfgrow label="权限未解锁" hint="disabled=true 禁用按钮交互，整体半透明">
          <rizui_button text="  权限锁定" primary disabled />
        </rizui_cfgrow>
        <rizui_cfgrow label="专属配色" hint="color 属性传入十六进制或 CSS 颜色，覆盖默认主题色">
          <div style="gap: 10px ; display: flex; flex-wrap: wrap">
            <rizui_button text="青辉石" primary color="#66ccff" />
            <rizui_button text="信用点" primary color="#f0a030" />
            <rizui_button text="神名精髓" primary color="#8b5cf6" />
          </div>
        </rizui_cfgrow>
        <rizui_cfgrow label="体力点恢复操作" hint="绑定 @click 监听点击事件">
          <rizui_button text="消耗青辉石补充体力点" primary @click="apRecoverCount++" r-icon="fa-solid fa-bolt" />
          <span class="demo-echo" style="font-size: 14px; color: var(--rz-subtext); margin-left: 8px; position: relative">
            已补充 <code>{{ apRecoverCount * 120 }}</code> 体力点
          </span>
        </rizui_cfgrow>
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card4_usage" />
        </rizui_cfgrow>
      </rizui_card>


    </section>

    <section class="sec-props">
      <h3><i class="fa-solid fa-list-check"></i> Props</h3>
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

    <!-- 主题色提示 -->
    <rizui_infobox type="info">
      未指定 <code>color</code> 时，按钮（及所有控件）自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </rizui_infobox>

    <section class="sec-events">
      <h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul><li v-for="e in events" :key="e"><code>{{ e }}</code></li></ul>
    </section>

    <section class="sec-usage">
      <h3><i class="fa-solid fa-code"></i> 用法</h3>
      <rizui_code block lang="html" :code="usage" />
    </section>
  </main>
</template>

<style scoped>
.demo-echo {
  font-size: 14px;
  color: var(--rz-subtext);
}
.demo-echo code {
  color: var(--rizui_themecolor);
  font-weight: 600;
}
</style>
