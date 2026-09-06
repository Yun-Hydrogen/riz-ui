<script setup>
import { ref, inject } from 'vue'
import { UiCard, UiConfigRow, UiSlider, rizui_code } from 'riz-ui'
const themeColor = inject('themeColor')
const sliderVal = ref(50)
const deprecated = true
const props = [
  { name:'modelValue',   type:'Number', default:'0',   desc:'v-model 绑定值' },
  { name:'min',          type:'Number', default:'0',   desc:'最小值' },
  { name:'max',          type:'Number', default:'100', desc:'最大值' },
  { name:'step',         type:'Number', default:'1',   desc:'步长' },
  { name:'color',        type:'String', default:'""', desc:'主题色，空时跟随 --rizui_themecolor' },
  { name:'display',      type:'String', default:'""',  desc:'显示后缀，如 "%" "px"' },
  { name:'displayValue', type:'String', default:'""',  desc:'完全自定义显示文本' }
]
const events = ['update:modelValue']
const usage = `<UiSlider v-model="vol" :min="0" :max="100" :color="c" display="%" />
<UiSlider v-model="size" :min="50" :max="200" :step="5" display="px" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>步进滑条</h2><p class="sec-tag">UiSlider</p><p class="sec-deprecated">已弃用</p></div>
      <p class="sec-desc">圆角矩形步进器，支持拖拽设定值 / 点击 ± 按钮 / 长按快速连续增减。背景色随数值比例填充。</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <UiCard title="基础用法" desc="">
        <UiConfigRow label="百分比" hint="0~100 步长 1"><UiSlider v-model="sliderVal" :min="0" :max="100" display="%"/></UiConfigRow>
        <UiConfigRow label="像素值" hint="50~200 步长 5"><UiSlider v-model="sliderVal" :min="50" :max="200" :step="5" display="px"/></UiConfigRow>
        <div class="demo-echo">sliderVal = <code>{{ sliderVal }}</code></div>
      </UiCard>
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
      <rizui_code block lang="html" :code="usage" />
    </section>
  </main>
</template>
