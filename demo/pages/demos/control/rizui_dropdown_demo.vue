<script setup>
import { ref, inject } from 'vue'
import { rizui_dropdown, rizui_card, rizui_cfgrow, rizui_infobox } from 'riz-ui'
const themeColor = inject('themeColor')
const dropdownVal = ref('option2')
const dropdownOptions = [
  { value: 'option1', label: 'Vulkan' },
  { value: 'option2', label: 'OpenGL' },
  { value: 'option3', label: 'off' }
]

const props = [
  { name:'modelValue', type:'String | Number', default:'""', desc:'v-model 绑定值' },
  { name:'options',    type:'Array',           default:'[]', desc:'选项 [{ value, label }]' },
  { name:'color',      type:'String',          default:'""', desc:'主题色，空时跟随 --rizui_themecolor' }
]
const events = ['update:modelValue']
const usage = 
`
const dropdownOptions = [
  { value: 'option1', label: 'Vulkan' },
  { value: 'option2', label: 'OpenGL' },
  { value: 'option3', label: 'DirectX' }]

<!-- 一个下拉选项 -->
<rizui_cfgrow label="选择后端" hint="点击展开，点外部自动关闭">
  <rizui_dropdown v-model="dropdownVal" :options="dropdownOptions"/>
</rizui_cfgrow>
`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>下拉选择</h2><p class="sec-tag">rizui_dropdown</p></div>
      <p class="sec-desc">下拉选择器</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <rizui_card title="图形选项" desc="显示异常时可以修改此处设置">
        <rizui_cfgrow label="选择图形加速选项" hint="游戏画面异常时，可尝试更改此处设置。" icon="fa-solid fa-server" :icon-style="{ color: themeColor }">
          <rizui_dropdown v-model="dropdownVal" :options="dropdownOptions"/>
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
