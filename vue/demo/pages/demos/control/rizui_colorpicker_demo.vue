<script setup>
import { ref, inject } from 'vue'
import { rizui_colorpicker, rizui_card, rizui_cfgrow, rizui_infobox, rizui_code } from 'riz-ui'
const themeColor = inject('themeColor')

const schaleColor = ref('#66CCFF')
const haloColor = ref('#5BC0BE')

const codeUsage1 = `<!-- 点击色块打开取色弹窗并双向绑定 HEX 颜色值 -->
<rizui_cfgrow label="什亭之箱色调" hint="点击色块打开调色盘">
  <rizui_colorpicker v-model="schaleColor" />
</rizui_cfgrow>

<div class="cp-preview" :style="{ background: schaleColor }">
  什亭之箱色调：{{ schaleColor }}
</div>`


const props = [
  { name: 'modelValue', type: 'String', default: '"#ffffff"', desc: 'v-model 绑定的十六进制 HEX 颜色值' },
  { name: 'color',      type: 'String', default: '""',        desc: '主题色，为空时跟随 --rizui_themecolor' },
  { name: 'teleport',   type: 'String', default: '"body"',    desc: '内置 Dialog 挂载目标选择器' },
  { name: 'theme',      type: 'String', default: "'auto'",    desc: '主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]
const events = ['update:modelValue']
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>颜色选择器</h2>
        <p class="sec-tag">rizui_colorpicker</p>
      </div>
      <p class="sec-desc">基于内置弹窗的取色器控件。点击色块即可弹出调色盘与色相滑块，支持实时拾取、HEX 值双向绑定与手动输入。</p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 沙勒终端主题定制 -->
      <rizui_card
        title="什亭之箱个性化 | 基础调色"
        desc="点击色块唤出 HSL 调色弹窗，支持色盘拖拽与 HEX 输入">
        <rizui_cfgrow
          label="什亭之箱色调"
          hint="点击色块打开调色盘，实时修改当前绑定的颜色变量">
          <rizui_colorpicker v-model="schaleColor" />
        </rizui_cfgrow stack>
        <div class="cp-preview" :style="{ background: schaleColor, width: '100%', textAlign: 'center' }">
          {{ schaleColor }}
        </div>
        <rizui_cfgrow stack>
          <rizui_code block lang="html" :code="codeUsage1" />
        </rizui_cfgrow>
      </rizui_card>

    </section>

    <section class="sec-props">
      <h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table">
        <thead>
          <tr>
            <th>属性</th>
            <th>类型</th>
            <th>必填</th>
            <th>默认值</th>
            <th>说明</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="p in props" :key="p.name">
            <td><code>{{ p.name }}</code></td>
            <td><code>{{ p.type }}</code></td>
            <td>{{ p.required || '—' }}</td>
            <td><code>{{ p.default || '—' }}</code></td>
            <td>{{ p.desc }}</td>
          </tr>
        </tbody>
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

    <section class="sec-events">
      <h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul><li v-for="e in events" :key="e"><code>{{ e }}</code></li></ul>
    </section>
  </main>
</template>

<style scoped>
.cp-preview {
  font-family: 'UI';
  src: url('./fonts/UI.ttf') format('truetype');
  font-weight: normal;
  font-style: bold;
  font-display: block;
  margin: 12px 0 0 0;
  padding: 12px 16px;
  border-radius: 8px;
  color: #fff;
  text-shadow: 0 1px 2px rgba(0, 0, 0, 0.4);
  display: inline-block;
  letter-spacing: 0.5px;
}
</style>
