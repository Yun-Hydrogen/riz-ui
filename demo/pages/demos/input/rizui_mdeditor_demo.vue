<script setup>
import { ref, inject } from 'vue'
import { rizui_mdeditor, rizui_card, rizui_infobox, rizui_code } from 'riz-ui'

const themeColor = inject('themeColor')

const schaleReport = ref(`# 沙勒工作备忘录 📋

尊敬的**老师**，欢迎回到*沙勒*（SCHALE）办公室。

## 今日重点事项

- 协助阿洛娜与普拉娜整理什亭之箱数据
- 与游戏开发部（小桃小绿、爱丽丝和柚子）测试新游戏
- 准备阿拜多斯对策委员会物资申请审批

> 提示：点击右上角工具栏「预览」选项卡即可实时查看渲染效果。

| 区域 / 组织 | 负责人 | 状态 |
| --- | --- | --- |
| 什亭之箱 | 阿洛娜/普拉娜 | 待机中 |
| 游戏开发部 | 柚子 | 正常 |
| 沙勒 | 老师 | 正常 |

\`\`\`ts
// 什亭之箱系统初始化
const schaleOS = initSchaleTerminal({
  admin: '老师',
  aiAssistant: ['Arona', 'Plana']
})
\`\`\``)

const card1Usage = `<rizui_mdeditor v-model="schaleReport" height="340px" />`

const card2Usage = `<!-- readonly 只读模式，隐藏顶栏工具栏，仅展示渲染内容 -->
<rizui_mdeditor v-model="schaleReport" readonly height="auto" />`

const card3Usage = `<!-- disabled 禁用模式，锁定编辑区不可输入，但保留预览切换 -->
<rizui_mdeditor v-model="schaleReport" disabled height="260px" />`

const props = [
  { name: 'modelValue',  type: 'String',  default: '""',      desc: 'v-model 双向绑定的 Markdown 文本内容' },
  { name: 'placeholder', type: 'String',  default: '"在此输入…"', desc: '未输入时的占位提示文字' },
  { name: 'color',       type: 'String',  default: '""',      desc: '主题色，空时跟随 --rizui_themecolor' },
  { name: 'disabled',    type: 'Boolean', default: 'false',   desc: '是否禁用编辑（编辑区置灰不可输入，但仍可切换预览）' },
  { name: 'readonly',    type: 'Boolean', default: 'false',   desc: '是否只读（隐藏顶栏工具栏，仅展示渲染后的 Markdown 页面）' },
  { name: 'height',      type: 'String',  default: '"320px"', desc: '编辑 / 预览画布区域高度（支持 auto 或固定高度）' },
  { name: 'default',     type: 'String',  default: '"edit"',  desc: '初始默认激活的选项卡："edit" 编辑 / "preview" 预览' },
  { name: 'theme',       type: 'String',  default: "'auto'",  desc: '主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]

const events = [
  'update:modelValue — 编辑内容发生变动时触发'
]
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>Markdown 编辑器</h2>
        <p class="sec-tag">rizui_mdeditor</p>
      </div>
      <p class="sec-desc">集成了轻量悬浮工具栏、快捷格式插入、实时渲染预览、代码高亮、只读排版与禁用状态控制的Markdown编辑器/预览工具。</p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 基础用法 -->
      <rizui_card title="基础编辑与预览" desc="支持加粗、斜体、列表、代码块、表格等工具栏快捷插入与编辑/预览选项卡切换">
        <rizui_mdeditor v-model="schaleReport" height="340px" />
        <rizui_code block lang="html" :code="card1Usage" style="margin-top: 12px" />
      </rizui_card>

      <!-- 只读模式 -->
      <rizui_card title="Readonly 只读模式" desc="设置 readonly=true 隐藏顶栏与工具栏，直接作为排版预览文档展示">
        <rizui_mdeditor v-model="schaleReport" readonly height="auto" />
        <rizui_code block lang="html" :code="card2Usage" style="margin-top: 12px" />
      </rizui_card>

      <!-- 禁用编辑 -->
      <rizui_card title="Disabled 锁定状态" desc="设置 disabled=true 锁定编辑区域阻止键盘输入，仍可自由切换至预览视窗">
        <rizui_mdeditor v-model="schaleReport" disabled height="260px" />
        <rizui_code block lang="html" :code="card3Usage" style="margin-top: 12px" />
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
      未指定 <code>color</code> 时，编辑器（及所有控件）自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </rizui_infobox>

    <section class="sec-events">
      <h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul>
        <li v-for="e in events" :key="e"><code>{{ e }}</code></li>
      </ul>
    </section>
  </main>
</template>
