<script setup>
import { ref, inject } from 'vue'
import { UiCard, UiConfigRow, UiSwitch } from 'riz-ui'
const themeColor = inject('themeColor')
const switchOn = ref(true)
const deprecated = true
const props = [
  { name:'title', type:'String', required:'是', desc:'卡片标题' },
  { name:'desc',  type:'String', default:'""', desc:'描述文字，空串则不显示' }
]
const slots = ['default — 卡片内容区']
const usage = `<UiCard title="浮窗外观" desc="调整抽取结果浮窗的视觉样式">
  <UiConfigRow label="持续置顶">...</UiConfigRow>
</UiCard>`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>卡片容器</h2><p class="sec-tag">UiCard</p><p class="sec-deprecated">已弃用</p></div>
      <p class="sec-desc">统一的配置面板卡片容器，提供标题 + 描述 + 内容插槽的标准布局。</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <UiCard title="示例卡片" desc="这是 desc 描述文字，可留空">
        <UiConfigRow label="示例配置项"><UiSwitch v-model="switchOn"/></UiConfigRow>
      </UiCard>
      <UiCard title="无描述卡片" desc=""><div style="font-size:13px;color:#99a">desc="" 时不显示描述区</div></UiCard>
    </section>
    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>
    <section class="sec-slots"><h3><i class="fa-solid fa-puzzle-piece"></i> Slots</h3>
      <ul><li v-for="s in slots" :key="s"><code>{{ s }}</code></li></ul>
    </section>
    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>
