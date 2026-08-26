<script setup>
import { ref, inject } from 'vue'
import { Ctrl_Slider, Cntr_Card, Cntr_Cfgrow, Cntr_Infobox } from 'riz-ui'
const themeColor = inject('themeColor')
const sliderVal = ref(50)

const props = [
  { name:'modelValue',   type:'Number', default:'0',   desc:'v-model 绑定值' },
  { name:'min',          type:'Number', default:'0',   desc:'最小值' },
  { name:'max',          type:'Number', default:'100', desc:'最大值' },
  { name:'step',         type:'Number', default:'1',   desc:'步长' },
  { name:'color',        type:'String', default:'""', desc:'主题色，空时跟随 --rizui_themecolor' },
  { name:'display',      type:'String', default:'""',  desc:'显示后缀' },
  { name:'displayValue', type:'String', default:'""',  desc:'自定义显示文本' }
]
const events = ['update:modelValue']
const usage = `<!-- 百分比滑条 -->
<Cntr_Cfgrow label="音量" hint="0~100，单位 %">
  <Ctrl_Slider v-model="sliderVal" :min="0" :max="100" display="%"/>
</Cntr_Cfgrow>

<!-- 像素值滑条 -->
<Cntr_Cfgrow label="边距" hint="50~200，步长 5">
  <Ctrl_Slider v-model="sliderVal" :min="50" :max="200" :step="5" display="px"/>
</Cntr_Cfgrow>`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>步进滑条</h2><p class="sec-tag">Ctrl_Slider</p></div>
      <p class="sec-desc">步进滑条，支持拖拽、点击 ±、长按连续增减。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 基础用法 -->
      <Cntr_Card title="调个音量试试" desc="拖一下或点 ±，长按会连续增减">
        <Cntr_Cfgrow label="这是一个音量滑条" hint="范围 0~100，单位 %">
          <Ctrl_Slider v-model="sliderVal" :min="0" :max="100" display="%"/>
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="这是一个像素值滑条" hint="50~200，步长 5">
          <Ctrl_Slider v-model="sliderVal" :min="50" :max="200" :step="5" display="px"/>
        </Cntr_Cfgrow>
        <div class="demo-echo">sliderVal = <code>{{ sliderVal }}</code></div>
      </Cntr_Card>
    </section>
    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>
    <!-- 主题色提示 -->
    <Cntr_Infobox type="info">
      未指定 <code>color</code> 时，控件自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </Cntr_Infobox>

    <section class="sec-events"><h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul><li v-for="e in events" :key="e"><code>{{ e }}</code></li></ul>
    </section>
    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>
