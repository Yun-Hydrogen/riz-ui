<script setup>
import { ref, inject } from 'vue'
import { rizui_switch, rizui_card, rizui_cfgrow, rizui_infobox, rizui_code } from 'riz-ui'
const themeColor = inject('themeColor')
const switchOn = ref(true)

const props = [
  { name:'modelValue', type:'Boolean', default:'false', desc:'v-model 绑定值' },
  { name:'color',      type:'String',  default:'""', desc:'主题色，空时跟随 --rizui_themecolor' },
  { name:'disabled',   type:'Boolean', default:'false', desc:'是否禁用' },
  { name:'theme',       type:'String',  default:"'auto'", desc:'主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]
const events = ['update:modelValue']
const usage = `<!-- 一个功能开关 -->
<rizui_cfgrow label="启用功能" hint="点击切换">
  <rizui_switch v-model="switchOn"/>
</rizui_cfgrow>

<!-- 禁用状态 -->
<rizui_cfgrow label="始终开启" hint="设置 disabled 属性">
  <rizui_switch :model-value="true" disabled/>
</rizui_cfgrow>`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>开关</h2><p class="sec-tag">rizui_switch</p></div>
      <p class="sec-desc">纯 CSS 的开关</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <rizui_card title="示例" desc="v-model 双向绑定">
        <rizui_cfgrow label="是否展示学生EX技能动画" hint="关闭可节省内存">
          <rizui_switch v-model="switchOn"/>
        </rizui_cfgrow>
      </rizui_card>
      <rizui_card title="禁用属性"" desc="设置 disabled 属性后不可交互，可用于展示状态">
        <rizui_cfgrow label="自动作战" hint="剧情模式下无法关闭">
          <rizui_switch :model-value="true" disabled/>
        </rizui_cfgrow>
        <rizui_cfgrow label="倍速作战" hint="特殊关卡无法开启">
          <rizui_switch :model-value="false" disabled/>
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
