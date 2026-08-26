<script setup>
import { ref, inject } from 'vue'
import { rizui_dialog, rizui_card, rizui_cfgrow, rizui_infobox, rizui_button } from 'riz-ui'
const themeColor = inject('themeColor')

const showBasic = ref(false)
const showNoClose = ref(false)
const showNoMask = ref(false)
const showNoCloseandMask = ref(false)
const CannotClose = ref(false)

const props = [
  { name:'show',         type:'Boolean', default:'false',    desc:'v-model:show 控制显隐' },
  { name:'title',        type:'String',  default:'""',       desc:'标题文字' },
  { name:'title-style', type:'Object',  default:'{}',       desc:'标题内联样式覆盖' },
  { name:'icon',         type:'String',  default:'""',       desc:'标题左侧图标（FA class）' },
  { name:'icon-style',  type:'Object',  default:'{}',       desc:'图标内联样式覆盖' },
  { name:'color',        type:'String',  default:'""',       desc:'主题色，空时跟随 --rizui_themecolor' },
  { name:'teleport',     type:'String',  default:'"body"',   desc:'挂载目标选择器' },
  { name:'width',        type:'String',  default:'"auto"',   desc:'对话框宽度' },
  { name:'closable',     type:'Boolean', default:'true',     desc:'是否显示关闭按钮' },
  { name:'maskClose',    type:'Boolean', default:'true',     desc:'点击遮罩是否关闭' }
]
const slots = ['default — 主体内容', 'footer — 底部按钮区（可选）']
const events = ['update:show']
const usage = `const show = ref(false)

<rizui_dialog v-model:show="show" title="确认" icon="fa-solid fa-circle-question" width="320px">
  <p>确定要执行此操作吗？</p>
  <template #footer>
    <rizui_button text="取消" @click="show = false" />
    <rizui_button text="确认" primary @click="doIt" />
  </template>
</rizui_dialog>`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>对话框</h2><p class="sec-tag">rizui_dialog</p></div>
      <p class="sec-desc">居中弹出对话框，支持 Teleport 挂载、Transition 动画、遮罩关闭、图标与主题色。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <rizui_card title="基础用法" desc="标题 + 内容 + footer 按钮">
        <rizui_cfgrow label="打开对话框" hint="点击打开基础对话框">
          <rizui_button text="打开" primary @click="showBasic = true" r-icon="fa-solid fa-square-arrow-up-right"/>
        </rizui_cfgrow>
      </rizui_card>

      <rizui_card title="变体" desc="禁用关闭按钮 / 禁用遮罩关闭">
        <rizui_cfgrow label="无关闭按钮" hint="closable=false，仅能通过按钮或遮罩关闭">
          <rizui_button text="打开" @click="showNoClose = true" />
        </rizui_cfgrow>
        <rizui_cfgrow label="禁用遮罩关闭" hint="maskClose=false，仅能通过按钮或者关闭按钮关闭">
          <rizui_button text="打开" @click="showNoMask = true" />
        </rizui_cfgrow>
        <rizui_cfgrow label="禁用关闭按钮和遮罩关闭" 
        hint="closable=false 且 maskClose=false，仅能通过按钮关闭">
          <rizui_button text="打开" @click="showNoCloseandMask = true" />
        </rizui_cfgrow>
        <rizui_cfgrow label="不要试图点开这个dialog" hint="closable=false 且 maskClose=false，且没有按钮关闭">
          <rizui_button text="打开" @click="CannotClose = true" danger/>
        </rizui_cfgrow>
      </rizui_card>
    </section>

    <!-- 对话框实例 -->
    <rizui_dialog v-model:show="showBasic" title="对话框示例" icon="fa-solid fa-circle-info" width="360px">
      <p style="margin-bottom:8px">这是 <strong>rizui_dialog</strong> 组件。</p>
      <p style="margin-bottom:8px">支持 Teleport 挂载、Transition 动画、遮罩关闭。</p>
      <p>当前主题色：<code>{{ themeColor }}</code></p>
      <template #footer>
        <rizui_button text="取消" @click="showBasic = false" />
        <rizui_button text="确认" primary @click="showBasic = false" />
      </template>
    </rizui_dialog>

    <rizui_dialog v-model:show="showNoClose" title="无关闭按钮" :closable="false">
      <p>该对话框没有右上角关闭按钮，只能通过下方按钮或遮罩关闭。</p>
      <template #footer>
        <rizui_button text="知道了" primary long @click="showNoClose = false" />
      </template>
    </rizui_dialog>

    <rizui_dialog v-model:show="showNoMask" title="禁用遮罩关闭" :maskClose="false">
      <p>点击遮罩不会关闭，请使用下方按钮或关闭按钮。</p>
      <template #footer>
        <rizui_button text="关闭" primary long @click="showNoMask = false" />
      </template>
    </rizui_dialog>

    <rizui_dialog v-model:show="showNoCloseandMask" title="禁用关闭按钮和遮罩关闭" :closable="false" :maskClose="false">
      <p>该对话框没有右上角关闭按钮，点击遮罩也不会关闭，只能通过下方按钮关闭。</p>
      <template #footer>
        <rizui_button text="关闭" primary long @click="showNoCloseandMask = false" />
      </template>
    </rizui_dialog>

    <rizui_dialog v-model:show="CannotClose" title="无法关闭的对话框" :closable="false" :maskClose="false">
      <p>该对话框没有右上角关闭按钮，点击遮罩也不会关闭，且没有下方按钮关闭。</p>
      <p>你无法关闭它，除非刷新页面。</p>
    </rizui_dialog>

    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>

    <!-- 主题色提示 -->
    <rizui_infobox type="info">
      未指定 <code>color</code> 时，对话框（及所有控件）自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </rizui_infobox>

    <section class="sec-slots"><h3><i class="fa-solid fa-puzzle-piece"></i> Slots</h3>
      <ul><li v-for="s in slots" :key="s"><code>{{ s }}</code></li></ul>
    </section>

    <section class="sec-events"><h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul><li v-for="e in events" :key="e"><code>{{ e }}</code></li></ul>
    </section>

    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>
