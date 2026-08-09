<script setup>
import {ref, inject} from 'vue'
import {Inpt_Text, Cntr_Card, Cntr_Cfgrow} from 'riz-ui'

const themeColor = inject('themeColor')
const textVal = ref('')
const numVal = ref(null)
const plainVal = ref('')

const props = [
  {name: 'modelValue', type: 'String | Number', default: '""', desc: 'v-model 绑定值'},
  {name: 'type', type: 'String', default: '"text"', desc: '输入类型 text/number'},
  {name: 'placeholder', type: 'String', default: '""', desc: '占位提示文字'},
  {name: 'color', type: 'String', default: '"#39c5bb"', desc: '聚焦边框主题色'},
  {name: 'plaintext', type: 'Boolean', default: 'false', desc: '多行纯文本模式（textarea）'},
  {name: 'disabled', type: 'Boolean', default: 'false', desc: '禁用'},
  {name: 'readonly', type: 'Boolean', default: 'false', desc: '只读'}
]
const events = ['update:modelValue']
const usage = `
<Inpt_Text v-model="name" placeholder="请输入" :color="theme" />
<Inpt_Text v-model="x" type="number" :color="theme" />
<Inpt_Text v-model="content" :color="theme" plaintext placeholder="多行文本" />
<!-- 用原生 style 控制宽度 -->
<Inpt_Text v-model="name" style="width: 160px" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>文本输入</h2>
        <p class="sec-tag">Inpt_Text</p></div>
      <p class="sec-desc">文本/数字输入框，拥有聚焦效果。plaintext 模式切换为多行大文本框。</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!--类型卡片-->
      <Cntr_Card title="输入类型" desc="这里展示了两种不同的输入类型的文本框">
        <Cntr_Cfgrow label="昵称" hint="type=text,这是文本类型的输入框">
          <Inpt_Text v-model="textVal" placeholder="请输入昵称" :color="themeColor" style="width: 160px"/>
        </Cntr_Cfgrow>

        <Cntr_Cfgrow label="坐标 X" hint="type=number,这是数字类型的输入">
          <Inpt_Text v-model="numVal" type="number" :color="themeColor" placeholder="X"/>
        </Cntr_Cfgrow>
      </Cntr_Card>

      <Cntr_Card title="Plaintext 多行" desc="存在 plaintext 时切换为 textarea 大文本框，宽度 100%">
        <Cntr_Cfgrow label="备注" hint="多行纯文本输入" stack>
          <Inpt_Text v-model="plainVal" :color="themeColor" plaintext placeholder="请输入多行内容..."/>
        </Cntr_Cfgrow>
      </Cntr_Card>

      <Cntr_Card title="禁用 & 只读" desc="">
        <Cntr_Cfgrow label="禁用状态">
          <Inpt_Text :model-value="'不可编辑'" :color="themeColor" disabled/>
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="只读状态">
          <Inpt_Text :model-value="'只读内容'" :color="themeColor" readonly/>
        </Cntr_Cfgrow>
      </Cntr_Card>
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
