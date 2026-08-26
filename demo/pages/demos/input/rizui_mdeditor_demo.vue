<script setup>
import { ref, inject } from 'vue'
import { rizui_mdeditor, rizui_card, rizui_infobox } from 'riz-ui'
const themeColor = inject('themeColor')

const md = ref(`# 你好，RizUI 👋

这是一个 **Markdown 编辑器**，支持*顶栏工具栏*和 ~~实时预览~~。

## 功能一览

- 加粗 / 斜体 / 删除线
- 标题、引用、列表
- \`行内代码\` 与代码块
- [链接](https://github.com/YunHydrogen/riz-ui)
- 表格

> 提示：点击右上角 👁 可切换预览。

| 组件 | 类型 |
| --- | --- |
| rizui_mdeditor | Markdown 编辑器 |
| rizui_text | 文本输入 |`)

const props = [
  { name:'modelValue',  type:'String',  default:'""',      desc:'v-model 绑定的 Markdown 文本' },
  { name:'placeholder', type:'String',  default:'"在此输入…"', desc:'占位提示文字' },
  { name:'color',       type:'String',  default:'""',      desc:'主题色，空时跟随 --rizui_themecolor' },
  { name:'disabled',    type:'Boolean', default:'false',   desc:'禁用编辑' },
  { name:'readonly',    type:'Boolean', default:'false',   desc:'只读模式，仅显示预览，不可编辑' },
  { name:'height',      type:'String',  default:'"320px"', desc:'编辑 / 预览区高度' },
  { name:'default',     type:'String',  default:'"edit"',  desc:'初始板块："edit" 编辑 / "preview" 预览' }
]
const events = ['update:modelValue']
const usage = `const md = ref('# 标题')

<rizui_mdeditor v-model="md" height="360px" />

<!-- 默认加载编辑板块（default="edit"），可切换预览 -->
<rizui_mdeditor v-model="md" default="preview" />

<!-- 禁用编辑，但仍可切换 编辑 / 预览 -->
<rizui_mdeditor v-model="md" disabled />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>Markdown 编辑器</h2><p class="sec-tag">rizui_mdeditor</p></div>
      <p class="sec-desc">带顶栏工具栏的 Markdown 编辑器，支持光标处插入语法与实时预览。工具栏按钮可对选中文本加粗 / 斜体 / 插入代码 / 链接等。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <!--标准用法-->  
      <rizui_card title="基础用法" desc="工具栏 + 编辑 / 预览选项卡">
        <rizui_mdeditor v-model="md" :height="'340px'" />
      </rizui_card>

      <!--只读模式-->
      <rizui_card title="只读模式" desc="readonly=true，仅显示预览区，不可编辑">
        <rizui_mdeditor v-model="md" readonly height="auto" />
      </rizui_card>

      <!-- 禁用编辑 -->
      <rizui_card title="禁用编辑" desc="disabled=true，编辑区不可输入，但可切换预览">
        <rizui_mdeditor v-model="md" disabled />
      </rizui_card>
    </section>

    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>

    <!-- 主题色提示 -->
    <rizui_infobox type="info">
      未指定 <code>color</code> 时，编辑器（及所有控件）自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </rizui_infobox>

    <section class="sec-events"><h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul><li v-for="e in events" :key="e"><code>{{ e }}</code></li></ul>
    </section>

    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>
