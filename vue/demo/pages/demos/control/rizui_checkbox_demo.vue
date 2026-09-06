<script setup>
import { ref, inject } from 'vue'
import { rizui_checkbox, rizui_card, rizui_cfgrow, rizui_infobox, rizui_code } from 'riz-ui'
const themeColor = inject('themeColor')

const agreement = ref(true)
const fairPlay = ref(false)
const midoriCheck = ref(true)
const themeCheck = ref(true)
const pyroxeneCheck = ref(true)
const creditCheck = ref(true)
const momoiCheck = ref(false)

const props = [
  { name: 'modelValue',    type: 'Boolean', default: 'false',  desc: 'v-model 绑定布尔值' },
  { name: 'color',         type: 'String',  default: '""',     desc: '主题色，为空时跟随 --rizui_themecolor' },
  { name: 'disabled',      type: 'Boolean', default: 'false',  desc: '是否禁用（不可交互）' },
  { name: 'r-label',       type: 'String',  default: '""',     desc: '右侧标签文字（默认位置）' },
  { name: 'r-label-style', type: 'Object',  default: '{}',     desc: '右侧文字内联样式覆盖' },
  { name: 'l-label',       type: 'String',  default: '""',     desc: '左侧标签文字' },
  { name: 'l-label-style', type: 'Object',  default: '{}',     desc: '左侧文字内联样式覆盖' },
  { name: 'theme',         type: 'String',  default: "'auto'", desc: '主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]
const events = ['update:modelValue']

const usage = `<!-- 1. 右侧文字（默认形态） -->
<rizui_cfgrow label="沙勒安全协议" hint="勾选确认知晓沙勒各项保密条款">
  <rizui_checkbox v-model="agreement" r-label="已阅读并同意安全协议" />
</rizui_cfgrow>

<!-- 2. 左侧文字与样式定制 -->
<rizui_checkbox v-model="fairPlay" l-label="公平竞技公约" :l-label-style="{ color: themeColor }" />

<!-- 3. 自定义专属颜色与禁用态 -->
<rizui_checkbox v-model="midoriCheck" color="#10f918" r-label="才羽绿（小绿）" />
<rizui_checkbox :model-value="true" disabled r-label="不可修改" />`

const card1_usage =
` <rizui_card title="沙勒协议签署 | 基础用法" 
desc="r-label 右侧标签（默认）/ l-label 左侧标签 / 自定义文字样式">
  <rizui_cfgrow label="沙勒安全协议" hint="r-label 设置右侧说明文本（默认位置）">
  <rizui_checkbox v-model="agreement" r-label="已阅读并同意安全协议" />
  </rizui_cfgrow>
  <rizui_cfgrow label="公平对局公约" hint="l-label 设置左侧文本，结合 l-label-style 自定义高亮色">
  <rizui_checkbox v-model="fairPlay" l-label="我游戏王小桃再也不在总力战开挂了呜呜呜~" 
  :l-label-style="{ color: themeColor }" />
  </rizui_cfgrow>`

const card2_usage =
`<rizui_checkbox v-model="themeCheck" :color="themeColor" r-label="跟随主题色" />
<rizui_checkbox v-model="pyroxeneCheck" color="#66ccff" r-label="青辉石蓝 (#66ccff)" />
<rizui_checkbox v-model="creditCheck" color="#f0a030" r-label="信用点金 (#f0a030)" />
`

const card3_usage =
`<rizui_checkbox :model-value="false" disabled r-label="连接状态" />
`
  
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>复选框</h2>
        <p class="sec-tag">rizui_checkbox</p>
      </div>
      <p class="sec-desc">轻量纯 CSS 勾选控件。支持 v-model 响应式绑定、左右侧标签排版、标签样式覆盖、自定义主题色与明暗主题切换。</p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 协议签署 · 基础形态 -->
      <rizui_card title="基础用法" desc="r-label 右侧标签（默认）/ l-label 左侧标签 / 自定义文字样式">
        <rizui_cfgrow label="沙勒安全协议" hint="r-label 设置右侧说明文本（默认位置）">
          <rizui_checkbox v-model="agreement" r-label="已阅读并同意安全协议" />
        </rizui_cfgrow>
        <rizui_cfgrow label="公平对局公约" hint="l-label 设置左侧文本，结合 l-label-style 自定义高亮色">
          <rizui_checkbox v-model="fairPlay" l-label="我游戏王小桃再也不在总力战开挂了呜呜呜~" :l-label-style="{ color: themeColor }" />
        </rizui_cfgrow>
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card1_usage" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 物资与角色配色 -->
      <rizui_card title="自定义色彩" desc="color 属性单独覆盖勾选框高亮背景色">
        <rizui_cfgrow label="沙勒全局主题色" hint="未指定或绑定 themeColor 时跟随全局 CSS 变量">
          <rizui_checkbox v-model="themeCheck" :color="themeColor" r-label="跟随主题色" />
        </rizui_cfgrow>
        <rizui_cfgrow label="青辉石配色" hint="color 设为青辉石蓝 #66ccff">
          <rizui_checkbox v-model="pyroxeneCheck" color="#66ccff" r-label="青辉石自动补给" />
        </rizui_cfgrow>
        <rizui_cfgrow label="信用点配色" hint="color 设为信用点金 #f0a030">
          <rizui_checkbox v-model="creditCheck" color="#f0a030" r-label="信用点收益加倍" />
        </rizui_cfgrow>

        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card2_usage" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 权限锁定 · 禁用状态 -->
      <rizui_card title="禁用状态" desc="disabled 属性阻止用户交互，常用于展现状态">
        <rizui_cfgrow label="启用什亭之箱保护" hint="disabled 且 modelValue=true，已勾选但禁止修改">
          <rizui_checkbox :model-value="true" disabled r-label="什亭之箱保护状态" />
        </rizui_cfgrow>
        <rizui_cfgrow label="连接到圣所之塔" hint="disabled 且 modelValue=false，未勾选且禁止激活">
          <rizui_checkbox :model-value="false" disabled r-label="连接状态" />
        </rizui_cfgrow>
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card3_usage" />
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
      未指定 <code>color</code> 时，复选框（及所有控件）自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
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
