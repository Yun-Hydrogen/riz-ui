<script setup>
import { ref, inject } from 'vue'
import { rizui_slider, rizui_card, rizui_cfgrow, rizui_infobox, rizui_code } from 'riz-ui'
const themeColor = inject('themeColor')
const sliderVal = ref(50)
const battlespeed = ref(1)

const props = [
  { name:'modelValue',   type:'Number', default:'0',   desc:'v-model 绑定值' },
  { name:'min',          type:'Number', default:'0',   desc:'最小值' },
  { name:'max',          type:'Number', default:'100', desc:'最大值' },
  { name:'step',         type:'Number', default:'1',   desc:'步长' },
  { name:'color',        type:'String', default:'""', desc:'主题色，空时跟随 --rizui_themecolor' },
  { name:'display',      type:'String', default:'""',  desc:'显示后缀' },
  { name:'displayValue', type:'String', default:'""',  desc:'自定义显示文本' },
  { name:'theme',       type:'String',  default:"'auto'", desc:'主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]
const events = ['update:modelValue']
const usage = `<!-- 百分比滑条 -->
<rizui_cfgrow label="音量" hint="0~100，单位 %">
  <rizui_slider v-model="sliderVal" :min="0" :max="100" display="%"/>
</rizui_cfgrow>

<!-- 像素值滑条 -->
<rizui_cfgrow label="边距" hint="50~200，步长 5">
  <rizui_slider v-model="sliderVal" :min="50" :max="200" :step="5" display="px"/>
</rizui_cfgrow>`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>步进滑条</h2><p class="sec-tag">rizui_slider</p></div>
      <p class="sec-desc">步进滑条，支持拖拽、点击 ±、长按连续增减。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 基础用法 -->
      <rizui_card title="基础用法" desc="Tip: 拖一下或点 ±，长按会连续增减">
        <rizui_cfgrow label="作战倍速" :hint='`1-3倍速 当前倍数:${battlespeed}`'>
          <rizui_slider v-model="battlespeed" :min="1" :max="3" display="倍速"/>
        </rizui_cfgrow>
        <rizui_cfgrow label="全局音量" :hint="`调整全局音量大小 当前音量:${sliderVal}%`">
          <rizui_slider v-model="sliderVal" :min="0" :max="100" :step="1" display="%"/>
        </rizui_cfgrow>
      </rizui_card>
    </section>
    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>
    <!-- 主题模式提示 -->
    <rizui_infobox type="info">
      <code>theme</code> 支持 <code>Dark</code> / <code>Light</code> / <code>auto</code>（默认）。<code>auto</code> 跟随顶栏主题开关（全局 <code>data-rizui-theme</code>）或系统 <code>prefers-color-scheme</code>；<code>Dark</code> / <code>Light</code> 可对本组件局部强制明暗。试试右上角主题按钮实时切换，或在你的项目中于 <code>&lt;html&gt;</code> 上设置 <code>data-rizui-theme</code>。
    </rizui_infobox>
    <!-- 主题色提示 -->
    <rizui_infobox type="info">
      未指定 <code>color</code> 时，控件自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </rizui_infobox>

    <section class="sec-events"><h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul><li v-for="e in events" :key="e"><code>{{ e }}</code></li></ul>
    </section>
    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <rizui_code block lang="html" :code="usage" />
    </section>
  </main>
</template>
