<script setup>
import {ref, inject} from 'vue'
import {rizui_text, rizui_card, rizui_cfgrow, rizui_infobox} from 'riz-ui'

const themeColor = inject('themeColor')
const textVal = ref('')
const numVal = ref(null)
const plainVal = ref('')
const searchVal = ref('')
const searched = ref(false)

function onSearch() {
  searched.value = true
}

const props = [
  {name: 'modelValue', type: 'String | Number', default: '""', desc: 'v-model 绑定值'},
  {name: 'type', type: 'String', default: '"text"', desc: '输入类型 text/number'},
  {name: 'placeholder', type: 'String', default: '""', desc: '占位提示文字'},
  {name: 'color', type: 'String', default: '""', desc: '主题色，空时跟随 --rizui_themecolor'},
  {name: 'plaintext', type: 'Boolean', default: 'false', desc: '多行纯文本模式'},
  {name: 'disabled', type: 'Boolean', default: 'false', desc: '禁用'},
  {name: 'readonly', type: 'Boolean', default: 'false', desc: '只读'}
]
const events = ['update:modelValue', 'enter(value) — 按下回车触发，默认释放聚焦']
const usage = `<rizui_cfgrow label="昵称" hint="文本输入，用原生 style 控制宽度">
  <rizui_text v-model="textVal" placeholder="请输入昵称" style="width: 160px"/>
</rizui_cfgrow>

<rizui_cfgrow label="坐标 X" hint="数字输入">
  <rizui_text v-model="numVal" type="number" placeholder="X"/>
</rizui_cfgrow>

<rizui_cfgrow label="备注" hint="plaintext 多行模式" stack>
  <rizui_text v-model="plainVal" plaintext placeholder="请输入多行内容..."/>
</rizui_cfgrow>

<rizui_cfgrow label="搜索" hint="按回车触发 enter 事件，默认释放聚焦">
  <rizui_text v-model="searchVal" placeholder="按回车试试" style="width: 160px" @enter="onSearch"/>
</rizui_cfgrow>`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>文本输入</h2>
        <p class="sec-tag">rizui_text</p></div>
      <p class="sec-desc">文本 / 数字输入框，聚焦高亮主题色。plaintext 切换为多行文本框。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!--类型卡片-->
      <rizui_card title="文本和数字" desc="type 决定你输入的是字还是数">
        <rizui_cfgrow label="昵称" hint="type=text，输什么就是什么">
          <rizui_text v-model="textVal" placeholder="请输入昵称" style="width: 160px"/>
        </rizui_cfgrow>

        <rizui_cfgrow label="坐标 X" hint="type=number，只能输数字">
          <rizui_text v-model="numVal" type="number" placeholder="X"/>
        </rizui_cfgrow>
      </rizui_card>

      <rizui_card title="Plaintext 多行" desc="存在 plaintext 时切换为 textarea 大文本框，宽度 100%">
        <rizui_cfgrow label="备注" hint="多行纯文本输入" stack>
          <rizui_text v-model="plainVal" plaintext placeholder="请输入多行内容..."/>
        </rizui_cfgrow>
      </rizui_card>

      <rizui_card title="禁用 & 只读" desc="">
        <rizui_cfgrow label="禁用状态">
          <rizui_text :model-value="'不可编辑'" disabled/>
        </rizui_cfgrow>
        <rizui_cfgrow label="只读状态">
          <rizui_text :model-value="'只读内容'" readonly/>
        </rizui_cfgrow>
      </rizui_card>

      <rizui_card title="回车事件" desc="按 Enter 触发 @enter 并释放聚焦">
        <rizui_cfgrow label="搜索" hint="按回车会 blur 并触发 @enter">
          <rizui_text v-model="searchVal" placeholder="按回车试试" style="width: 160px" @enter="onSearch"/>
        </rizui_cfgrow>
        <p v-if="searched" class="demo-echo">搜索了：<code>{{ searchVal }}</code></p>
      </rizui_card>
    </section>
    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
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
    <!-- 主题色提示 -->
    <rizui_infobox type="info">
      未指定 <code>color</code> 时，控件自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </rizui_infobox>

    <section class="sec-events"><h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul>
        <li v-for="e in events" :key="e"><code>{{ e }}</code></li>
      </ul>
    </section>
    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>
