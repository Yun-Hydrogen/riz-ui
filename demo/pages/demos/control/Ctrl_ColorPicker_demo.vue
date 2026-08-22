<script setup>
import { ref, inject } from 'vue'
import { Ctrl_ColorPicker, Cntr_Card, Cntr_Cfgrow, Cntr_Infobox } from 'riz-ui'
const themeColor = inject('themeColor')
const colorVal = ref('#66ccff')

const props = [
  { name:'modelValue', type:'String', default:'"#ffffff"', desc:'v-model 绑定的 hex 颜色' },
  { name:'color',      type:'String', default:'""', desc:'主题色，空时跟随 --rizui_themecolor' },
  { name:'teleport',   type:'String', default:'"body"',    desc:'Dialog 挂载目标选择器' }
]
const events = ['update:modelValue']
const usage = `<Cntr_Cfgrow label="主题色" hint="点击色块打开 HSL 调色盘">
  <Ctrl_ColorPicker v-model="colorVal"/>
</Cntr_Cfgrow>

<!-- 限定遮罩范围 -->
<Ctrl_ColorPicker v-model="color" teleport=".config-left" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>颜色选择器</h2><p class="sec-tag">Ctrl_ColorPicker</p></div>
      <p class="sec-desc">点一下色块弹出色盘，拖饱和度、拉色相、直接输 hex，选完立刻生效。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <Cntr_Card title="给应用换个主题色" desc="拖色盘或直接输入 hex">
        <Cntr_Cfgrow label="这是一个主题色选择器" hint="点开色盘拖一拖，右边实时预览颜色">
          <Ctrl_ColorPicker v-model="colorVal"/>
        </Cntr_Cfgrow>
        <div class="cp-preview" :style="{ background: colorVal }">{{ colorVal }}</div>
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
