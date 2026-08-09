<script setup>
import { ref, inject } from 'vue'
import { Ctrl_ColorPicker, Cntr_Card, Cntr_Cfgrow } from 'riz-ui'
const themeColor = inject('themeColor')
const colorVal = ref('#66ccff')

const props = [
  { name:'modelValue', type:'String', default:'"#ffffff"', desc:'v-model 绑定的 hex 颜色' },
  { name:'color',      type:'String', default:'"#39c5bb"', desc:'主题色' },
  { name:'teleport',   type:'String', default:'"body"',    desc:'Dialog 挂载目标选择器' }
]
const events = ['update:modelValue']
const usage = `<Ctrl_ColorPicker v-model="color" :color="themeColor" />

<!-- 限定遮罩范围 -->
<Ctrl_ColorPicker v-model="color" teleport=".config-left" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>颜色选择器</h2><p class="sec-tag">Ctrl_ColorPicker</p></div>
      <p class="sec-desc">内置触发 swatch + HSL 调色盘。支持 SV 面板拖拽、色相条、hex 直接输入。</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 实时预览</h3>
      <Cntr_Card title="基础用法" desc="">
        <Cntr_Cfgrow label="主题色选择" hint="点击色块打开 HSL 调色盘">
          <Ctrl_ColorPicker v-model="colorVal" :color="themeColor"/>
        </Cntr_Cfgrow>
        <div class="cp-preview" :style="{ background: colorVal }">{{ colorVal }}</div>
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
