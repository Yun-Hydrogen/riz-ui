<script setup>
import { ref, inject } from 'vue'
import { Ctrl_Timeline, Cntr_Card, Cntr_Cfgrow } from 'riz-ui'
const themeColor = inject('themeColor')
const timelineVal = ref(45)

const props = [
  { name:'modelValue', type:'Number', default:'0',   desc:'v-model 绑定的秒数' },
  { name:'min',        type:'Number', default:'0',   desc:'最小值（秒）' },
  { name:'max',        type:'Number', default:'120', desc:'最大值（秒）' },
  { name:'step',       type:'Number', default:'1',   desc:'步长（秒）' },
  { name:'color',      type:'String', default:'"#39c5bb"', desc:'主题色' }
]
const events = ['update:modelValue']
const usage = `
<Ctrl_Timeline v-model="time" :min="0" :max="120" :color="theme" />

<!-- 自定义范围（短时间轴，刻度更密集） -->
<Ctrl_Timeline v-model="t" :min="0" :max="60" :step="1" :color="theme" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>时间轴</h2><p class="sec-tag">Ctrl_Timeline</p></div>
      <p class="sec-desc">带刻度线的时间轴控件。渐变填充轨道 + 气泡时间标签 + 圆点拖拽手柄 + 底部时间刻度，支持点击 / 拖拽定位。</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 实时预览</h3>
      <Cntr_Card title="基础用法" desc="0~120 秒，点击或拖拽轨道设定时间">
        <Cntr_Cfgrow label="播放进度" hint="点击或拖拽设定时间点">
          <div style="flex:1;min-width:0"><Ctrl_Timeline v-model="timelineVal" :min="0" :max="120" :color="themeColor"/></div>
        </Cntr_Cfgrow>
        <div class="demo-echo">当前时间：<code>{{ Math.floor(timelineVal/60) }}:{{ String(Math.floor(timelineVal%60)).padStart(2,'0') }}</code></div>
      </Cntr_Card>

      <Cntr_Card title="短范围" desc="0~60 秒，刻度更密集">
        <Cntr_Cfgrow label="倒计时" hint="短范围自动使用更细的刻度间隔">
          <div style="flex:1;min-width:0"><Ctrl_Timeline v-model="timelineVal" :min="0" :max="60" :color="themeColor"/></div>
        </Cntr_Cfgrow>
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