<script setup>
import { ref, inject } from 'vue'
import { Ctrl_Timeline, Cntr_Card, Cntr_Cfgrow, Cntr_Infobox } from 'riz-ui'
const themeColor = inject('themeColor')
const timelineVal = ref(45)

const props = [
  { name:'modelValue', type:'Number', default:'0',   desc:'v-model 绑定的秒数' },
  { name:'min',        type:'Number', default:'0',   desc:'最小值（秒）' },
  { name:'max',        type:'Number', default:'120', desc:'最大值（秒）' },
  { name:'step',       type:'Number', default:'1',   desc:'步长（秒）' },
  { name:'color',      type:'String', default:'""', desc:'主题色，空时跟随 --rizui_themecolor' }
]
const events = ['update:modelValue']
const usage = `<Cntr_Cfgrow label="播放进度" hint="点击或拖拽设定时间点">
  <Ctrl_Timeline v-model="timelineVal" :min="0" :max="120"/>
</Cntr_Cfgrow>

<!-- 短范围，刻度更密集 -->
<Ctrl_Timeline v-model="t" :min="0" :max="60" :step="1" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>时间轴</h2><p class="sec-tag">Ctrl_Timeline</p></div>
      <p class="sec-desc">一条带刻度的进度轴，填满颜色的轨道 + 悬浮时间气泡，点一下或拖手柄就能定位。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <Cntr_Card title="视频播放进度" desc="0~120 秒，点轨道或拖倒三角跳转">
        <Cntr_Cfgrow label="这是一个播放进度条" hint="点一下轨道，或拖住倒三角滑到想看的时刻">
          <div style="flex:1;min-width:0"><Ctrl_Timeline v-model="timelineVal" :min="0" :max="120"/></div>
        </Cntr_Cfgrow>
        <div class="demo-echo">当前时间：<code>{{ Math.floor(timelineVal/60) }}:{{ String(Math.floor(timelineVal%60)).padStart(2,'0') }}</code></div>
      </Cntr_Card>

      <Cntr_Card title="短一点的倒计时" desc="0~60 秒，刻度更细">
        <Cntr_Cfgrow label="这是一个倒计时条" hint="范围短了刻度自动变密，更好对齐">
          <div style="flex:1;min-width:0"><Ctrl_Timeline v-model="timelineVal" :min="0" :max="60"/></div>
        </Cntr_Cfgrow>
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