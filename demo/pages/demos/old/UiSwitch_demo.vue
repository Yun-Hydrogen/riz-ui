<script setup>
import { ref, inject } from 'vue'
import { UiCard, UiConfigRow, UiSwitch } from 'riz-ui'
const themeColor = inject('themeColor')
const switchOn = ref(true)
const deprecated = true
const props = [
  { name:'modelValue', type:'Boolean', default:'false', desc:'v-model 绑定的开关状态' },
  { name:'color',      type:'String',  default:'"#39c5bb"', desc:'选中态轨道颜色' },
  { name:'disabled',   type:'Boolean', default:'false', desc:'是否禁用' }
]
const events = ['update:modelValue']
const usage = `<UiSwitch v-model="enabled" :color="themeColor" />
<UiSwitch v-model="enabled" :color="themeColor" disabled />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>开关</h2><p class="sec-tag">UiSwitch</p><p class="sec-deprecated">已弃用</p></div>
      <p class="sec-desc">纯 CSS 开关组件，替代原生 checkbox。支持 v-model 绑定、主题色、禁用态。</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 实时预览</h3>
      <UiCard title="基础用法" desc="">
        <UiConfigRow label="开关状态" hint="点击切换"><UiSwitch v-model="switchOn" :color="themeColor"/></UiConfigRow>
        <div class="demo-echo">当前值：<code>{{ switchOn }}</code></div>
      </UiCard>
      <UiCard title="禁用状态" desc="">
        <UiConfigRow label="始终开启（禁用）"><UiSwitch :model-value="true" :color="themeColor" disabled/></UiConfigRow>
        <UiConfigRow label="始终关闭（禁用）"><UiSwitch :model-value="false" :color="themeColor" disabled/></UiConfigRow>
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
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>
