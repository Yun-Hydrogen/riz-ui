<script setup>
import { ref, inject } from 'vue'
import { Ctrl_Slider, Cntr_Card, Cntr_Cfgrow } from 'riz-ui'
const themeColor = inject('themeColor')
const sliderVal = ref(50)

const props = [
  { name:'modelValue',   type:'Number', default:'0',   desc:'v-model 绑定值' },
  { name:'min',          type:'Number', default:'0',   desc:'最小值' },
  { name:'max',          type:'Number', default:'100', desc:'最大值' },
  { name:'step',         type:'Number', default:'1',   desc:'步长' },
  { name:'color',        type:'String', default:'"#39c5bb"', desc:'主题色' },
  { name:'display',      type:'String', default:'""',  desc:'显示后缀' },
  { name:'displayValue', type:'String', default:'""',  desc:'自定义显示文本' }
]
const events = ['update:modelValue']
const usage = `<Ctrl_Slider v-model="vol" :min="0" :max="100" :color="c" display="%" />
<Ctrl_Slider v-model="size" :min="50" :max="200" :step="5" display="px" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>步进滑条</h2><p class="sec-tag">Ctrl_Slider</p></div>
      <p class="sec-desc">步进滑条组件。支持拖拽 / 点击 ± 按钮 / 长按快速增减</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 实时预览</h3>

      <!-- 基础用法 -->
      <Cntr_Card title="基础用法" desc="此卡片介绍了 Ctrl_Slider 的基本用法">
        <Cntr_Cfgrow label="这是一个滑条" hint="范围0~100 步长1 拥有%作为单位">
          <Ctrl_Slider v-model="sliderVal" :min="0" :max="100" :color="themeColor" display="%"/>
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="像素值" hint="50~200 步长 5">
          <Ctrl_Slider v-model="sliderVal" :min="50" :max="200" :step="5" :color="themeColor" display="px"/>
        </Cntr_Cfgrow>
        <div class="demo-echo">sliderVal = <code>{{ sliderVal }}</code></div>
      </Cntr_Card>
    </section>
    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>
    <section class="sec-events"><h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul><li v-for="e in events" :key="e"><code>{{ e }}</code></li></ul>
    </section>
    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>
