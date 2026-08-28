<script setup>
import { ref, inject } from 'vue'
import {rizui_text, rizui_card, rizui_cfgrow, rizui_infobox, rizui_toast, rizui_code} from 'riz-ui'

const themeColor = inject('themeColor')
const repStudent = ref('安守实梨')
const pyroxeneNum = ref(1200)
const workLog = ref('')
const searchStudent = ref('')
const searchResult = ref('')

function onSearch(val) {
  searchResult.value = val
}

const card1Usage = `
<rizui_text v-model="repStudent" placeholder="请输入代表学员"  />
<rizui_text v-model="pyroxeneNum" type="number" placeholder="数量" style="width: 120px" />
`

const card2Usage = `
<rizui_cfgrow label="活动报告" hint="开启 plaintext 属性切换为多行文本域" stack>
<rizui_text v-model="workLog" plaintext placeholder="在此在此输入社团工作/活动报告..." />
`

const card3Usage = 
`<rizui_text :model-value="'72小时后可重新设置'" disabled/>
<rizui_text :model-value="'A.R.O.N.A'" readonly/>
`

const card4Usage = 
` <rizui_text v-model="searchStudent" placeholder="输入学员姓名并按回车（例：爱丽丝）" @enter="onSearch" />
`

const props = [
  { name: 'modelValue',  type: 'String | Number', default: '""',     desc: 'v-model 双向绑定的输入值' },
  { name: 'type',        type: 'String',          default: '"text"', desc: '原生输入框类型：text / number' },
  { name: 'placeholder', type: 'String',          default: '""',     desc: '未输入时的占位提示文字' },
  { name: 'color',       type: 'String',          default: '""',     desc: '聚焦边框与光标主题色，为空时跟随 --rizui_themecolor' },
  { name: 'plaintext',   type: 'Boolean',         default: 'false',  desc: '是否启用多行纯文本模式（渲染为 textarea）' },
  { name: 'disabled',    type: 'Boolean',         default: 'false',  desc: '是否禁用输入（不可交互且变灰）' },
  { name: 'readonly',    type: 'Boolean',         default: 'false',  desc: '是否只读（可选中复制但不可更改）' },
  { name: 'theme',       type: 'String',          default: "'auto'", desc: '主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]
const events = [
  'update:modelValue — 输入内容发生变动时触发',
  'enter(value) — 按下回车键时触发，并自动释放输入焦点'
]
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>文本输入</h2>
        <p class="sec-tag">rizui_text</p>
      </div>
      <p class="sec-desc">基础文本输入控件。支持单行文本、数值限制、多行纯文本（plaintext）、回车监听、禁用与只读状态。</p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 基础输入 · 单行与数字 -->
      <rizui_card title="基础类型用法" desc="通过 type 切换常规文本输入与数值输入">
        <rizui_cfgrow label="值日生" hint="type='text'（默认），单行字符串输入">
          <rizui_text v-model="repStudent" placeholder="请输入代表学员" />
        </rizui_cfgrow>

        <rizui_cfgrow label="恢复体力点" hint="type='number'，限制为数值输入" ic>
          <rizui_text v-model="pyroxeneNum" type="number" placeholder="数量" style="width: 120px" />
        </rizui_cfgrow>
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card1Usage" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 多行文本域 -->
      <rizui_card title="Plaintext 多行模式" desc="设置 plaintext 属性自动切换为多行纯文本域（textarea）">
        <rizui_cfgrow label="活动报告" hint="开启 plaintext 属性切换为多行纯文本输入域" stack>
          <rizui_text v-model="workLog" plaintext placeholder="在此输入社团工作/活动报告..." />
        </rizui_cfgrow>
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card2Usage" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 状态控制 · 禁用与只读 -->
      <rizui_card title="禁用与只读" desc="disabled 阻止输入交互，readonly 仅允许查看与复制">
        <rizui_cfgrow label="重新设置名字" hint="disabled=true 阻止编辑与交互，呈现半透明锁定状态">
          <rizui_text :model-value="'72小时后可重新设置'" disabled />
        </rizui_cfgrow>
        <rizui_cfgrow label="什亭之箱系统代号" hint="readonly=true 仅可选中复制不可修改">
          <rizui_text :model-value="'A.R.O.N.A'" readonly />
        </rizui_cfgrow>
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card3Usage" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 回车事件监听 -->
      <rizui_card title="回车事件" desc="按下 Enter 键触发 @enter 事件并自动释放聚焦">
        <rizui_cfgrow label="检索学员花名册" hint="输入文字后按回车触发 @enter 事件并 blur 释放焦点">
          <rizui_text v-model="searchStudent" placeholder="输入学员姓名并按回车（例：爱丽丝）" @enter="onSearch" />
        </rizui_cfgrow>
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="card4Usage" />
        </rizui_cfgrow>
      </rizui_card>
    </section>

    <rizui_toast v-if="searchResult" type="warn" @close="searchResult=false" title="当前未连接到数据中心" hint="请稍后再试">
      检索失败：<strong>{{ searchResult }}</strong>
    </rizui_toast>

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
      未指定 <code>color</code> 时，控件自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </rizui_infobox>

    <section class="sec-events">
      <h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul>
        <li v-for="e in events" :key="e"><code>{{ e }}</code></li>
      </ul>
    </section>
  </main>
</template>

<style scoped>
.demo-echo {
  margin-top: 12px;
  font-size: 13px;
  color: var(--rz-subtext);
}
.demo-echo code {
  color: var(--rizui_themecolor);
  font-weight: 600;
}
</style>
