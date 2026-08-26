<script setup>
import { ref, inject } from 'vue'
import { rizui_timeline, rizui_card, rizui_cfgrow, rizui_infobox } from 'riz-ui'
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
const usage = `<rizui_cfgrow label="播放进度" hint="点击或拖拽设定时间点">
  <rizui_timeline v-model="timelineVal" :min="0" :max="120"/>
</rizui_cfgrow>

<!-- 短范围，刻度更密集 -->
<rizui_timeline v-model="t" :min="0" :max="60" :step="1" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>时间轴</h2><p class="sec-tag">rizui_timeline</p></div>
      <p class="sec-desc">一条带刻度的进度轴/时间轴(专门用于时间选择相关的滑条？)</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <rizui_card title="播放相关" desc="调整开始抽取时音乐的相关设置">
        <rizui_cfgrow label="背景音乐起始位置" hint="音乐将从选定位置开始播放" stack>
          <rizui_timeline v-model="timelineVal" :min="0" :max="120"/>
        </rizui_cfgrow>
      </rizui_card>

    </section>
    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>
    <!-- 主题色提示 -->
    <rizui_infobox type="info">
      未指定 <code>color</code> 时，控件自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </rizui_infobox>

    <section class="sec-events"><h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul><li v-for="e in events" :key="e"><code>{{ e }}</code></li></ul>
    </section>
    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>