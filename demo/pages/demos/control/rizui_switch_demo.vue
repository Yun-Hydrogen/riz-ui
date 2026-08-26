<script setup>
import { ref, inject } from 'vue'
import { Ctrl_Switch, Cntr_Card, Cntr_Cfgrow, Cntr_Infobox } from 'riz-ui'
const themeColor = inject('themeColor')
const switchOn = ref(true)

const props = [
  { name:'modelValue', type:'Boolean', default:'false', desc:'v-model 绑定值' },
  { name:'color',      type:'String',  default:'""', desc:'主题色，空时跟随 --rizui_themecolor' },
  { name:'disabled',   type:'Boolean', default:'false', desc:'是否禁用' }
]
const events = ['update:modelValue']
const usage = `<!-- 一个功能开关 -->
<Cntr_Cfgrow label="启用功能" hint="点击切换">
  <Ctrl_Switch v-model="switchOn"/>
</Cntr_Cfgrow>

<!-- 禁用状态 -->
<Cntr_Cfgrow label="始终开启" hint="设置 disabled 属性">
  <Ctrl_Switch :model-value="true" disabled/>
</Cntr_Cfgrow>`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>开关</h2><p class="sec-tag">Ctrl_Switch</p></div>
      <p class="sec-desc">纯 CSS 开关，替代原生 checkbox。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <Cntr_Card title="控制一个功能的开关" desc="v-model 双向绑定，开合立即可见">
        <Cntr_Cfgrow label="这是一个功能开关" hint="点一下，下面的值跟着变">
          <Ctrl_Switch v-model="switchOn"/>
        </Cntr_Cfgrow>
        <div class="demo-echo">当前值：<code>{{ switchOn }}</code></div>
      </Cntr_Card>
      <Cntr_Card title="禁用时什么样" desc="设置 disabled 属性后不可交互">
        <Cntr_Cfgrow label="这是一个禁用的开关（开着）">
          <Ctrl_Switch :model-value="true" disabled/>
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="这是一个禁用的开关（关着）">
          <Ctrl_Switch :model-value="false" disabled/>
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
