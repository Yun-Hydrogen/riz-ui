<script setup>
import { ref, inject } from 'vue'
import { UiCard, UiConfigRow, UiDropdown, rizui_code } from 'riz-ui'
const themeColor = inject('themeColor')
const dropdownVal = ref('option2')
const dropdownOptions = [
  { value: 'option1', label: '选项 Alpha' },
  { value: 'option2', label: '选项 Beta' },
  { value: 'option3', label: '选项 Gamma' }
]
const deprecated = true
const props = [
  { name:'modelValue', type:'String | Number', default:'""', desc:'v-model 绑定值' },
  { name:'options',    type:'Array',           default:'[]', desc:'选项 [{ value, label }]' },
  { name:'color',      type:'String',          default:'""', desc:'主题色，空时跟随 --rizui_themecolor' }
]
const events = ['update:modelValue']
const usage = `const opts = [{ value: 'a', label: 'Alpha' }, { value: 'b', label: 'Beta' }]

<UiDropdown v-model="sel" :options="opts" :color="theme" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>下拉选择</h2><p class="sec-tag">UiDropdown</p><p class="sec-deprecated">已弃用</p></div>
      <p class="sec-desc">自定义下拉选择器，替代原生 select。支持 v-model、主题色高亮、点击外部关闭。</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <UiCard title="基础用法" desc="">
        <UiConfigRow label="选择后端" hint="点击展开下拉面板"><UiDropdown v-model="dropdownVal" :options="dropdownOptions"/></UiConfigRow>
        <div class="demo-echo">当前选中：<code>{{ dropdownVal }}</code></div>
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
