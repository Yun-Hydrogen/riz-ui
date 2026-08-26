<script setup>
import { ref, inject } from 'vue'
import { rizui_checkbox, rizui_card, rizui_cfgrow, rizui_infobox } from 'riz-ui'
const themeColor = inject('themeColor')

const check1 = ref(true)
const check2 = ref(false)
const check3 = ref(true)

const props = [
  { name:'modelValue',   type:'Boolean', default:'false', desc:'v-model 绑定值' },
  { name:'color',        type:'String',  default:'""', desc:'主题色，空时跟随 --rizui_themecolor' },
  { name:'disabled',     type:'Boolean', default:'false', desc:'是否禁用' },
  { name:'r-label',      type:'String',  default:'""', desc:'右侧文字（默认位置）' },
  { name:'r-label-style',type:'Object',  default:'{}', desc:'右侧文字内联样式覆盖' },
  { name:'l-label',      type:'String',  default:'""', desc:'左侧文字' },
  { name:'l-label-style',type:'Object',  default:'{}', desc:'左侧文字内联样式覆盖' }
]
const events = ['update:modelValue']
const usage = `<!-- 右侧文字（默认） -->
<rizui_cfgrow label="订阅更新" hint="点击勾选">
  <rizui_checkbox v-model="subscribed" r-label="邮件通知"/>
</rizui_cfgrow>

<!-- 左侧文字 -->
<rizui_checkbox v-model="a" l-label="左侧文字"/>

<!-- 自定义颜色 / 禁用 -->
<rizui_checkbox v-model="b" color="#8b5cf6" r-label="紫色"/>
<rizui_checkbox :model-value="true" disabled r-label="不可修改"/>`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>复选框</h2><p class="sec-tag">rizui_checkbox</p></div>
      <p class="sec-desc">纯 CSS 复选框</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <rizui_card title="基础用法" desc="r-label 右侧（默认）/ l-label 左侧 / color 指定颜色">
        <rizui_cfgrow label="隐私协议" hint="我已阅读并同意隐私协议">
          <rizui_checkbox v-model="check1" r-label="隐私协议"/>
        </rizui_cfgrow>
        <rizui_cfgrow label="公平竞技公约" hint="我已阅读并承诺公平竞技公约">
          <rizui_checkbox v-model="check2" l-label="我小桃不会再总力战开挂了呜呜呜" :l-label-style="{ color: themeColor }"/>
        </rizui_cfgrow>
        <rizui_cfgrow label="固定绿色" hint="color 显式指定">
          <rizui_checkbox v-model="check3" color="#10f918" r-label="小绿"/>
        </rizui_cfgrow>        

      </rizui_card>

      <rizui_card title="自定义颜色" desc="color prop 单独覆盖主题色">
        <rizui_cfgrow label="主题色" hint="跟随 --rizui_themecolor">
          <rizui_checkbox v-model="check1" :color="themeColor" r-label="跟着顶栏变"/>
        </rizui_cfgrow>

      </rizui_card>

      <rizui_card title="禁用状态" desc="设置 disabled 属性后不可交互">
        <rizui_cfgrow label="已勾选但禁用">
          <rizui_checkbox :model-value="true" disabled r-label="不能改"/>
        </rizui_cfgrow>
        <rizui_cfgrow label="未勾选且禁用">
          <rizui_checkbox :model-value="false" disabled r-label="也不能改"/>
        </rizui_cfgrow>
      </rizui_card>
    </section>

    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>

    <rizui_infobox type="info">
      未指定 <code>color</code> 时，控件自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色。
    </rizui_infobox>

    <section class="sec-events"><h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul><li v-for="e in events" :key="e"><code>{{ e }}</code></li></ul>
    </section>
    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>
