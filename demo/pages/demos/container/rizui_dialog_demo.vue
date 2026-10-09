<script setup>
import { ref, inject } from 'vue'
import { rizui_dialog, rizui_card, rizui_cfgrow, rizui_infobox, rizui_button, rizui_code } from 'riz-ui'
const themeColor = inject('themeColor')

const showBasic = ref(false)
const showNoClose = ref(false)
const showNoMask = ref(false)
const showNoCloseandMask = ref(false)
const CannotClose = ref(false)
const showCountdownBar = ref(false)
const showIndeterminateBar = ref(false)

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
  { name:'maskClose',    type:'Boolean', default:'true',     desc:'点击遮罩是否关闭' },
  { name:'show-bar',     type:'Boolean', default:'false',    desc:'是否显示底部倒计时/进度条（默认不启用）' },
  { name:'countdown',    type:'Number',  default:'0',        desc:'自动关闭秒数，0 为不自动关；>0 时从 100% 线性收缩到 0% 并自动关闭' },
  { name:'progress',     type:'Number',  default:'undefined',desc:'自定义进度百分比 (0~100)' },
  { name:'indeterminate',type:'Boolean', default:'false',    desc:'不定进度模式（未知进度/加载中）' },
  { name:'theme',       type:'String',  default:"'auto'",   desc:'主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]
const slots = ['default — 主体内容', 'footer — 底部按钮区（可选）']
const events = ['update:show', 'close', 'timeout']
const usage = `const show = ref(false)

<!-- 1. 基础用法 -->
<rizui_dialog v-model:show="show" title="确认" icon="fa-solid fa-circle-question" width="320px">
  <p>确定要执行此操作吗？</p>
  <template #footer>
    <rizui_button text="取消" @click="show = false" />
    <rizui_button text="确认" primary @click="doIt" />
  </template>
</rizui_dialog>

<!-- 2. 倒计时进度条（类似于 Toast，5 秒自动关闭） -->
<rizui_dialog v-model:show="show" title="限时确认" show-bar :countdown="5">
  <p>此确认窗口将在 5 秒后自动关闭...</p>
</rizui_dialog>

<!-- 3. 不定进度条（加载中） -->
<rizui_dialog v-model:show="show" title="处理中" show-bar indeterminate>
  <p>数据正在同步，请稍候...</p>
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

      <!-- 进度条与倒计时（类似 Toast） -->
      <rizui_card title="底部进度条（类似 Toast）" desc="默认不启用（show-bar=false）。开启后可作为倒计时自动关闭或加载状态指示">
        <rizui_cfgrow label="5 秒倒计时自动关闭" hint="show-bar 且 countdown=5，进度条线性收缩至 0 并自动关闭">
          <rizui_button text="打开 (5s)" primary @click="showCountdownBar = true" />
        </rizui_cfgrow>
        <rizui_cfgrow label="不定进度模式" hint="show-bar 且 indeterminate，底部流光循环流动">
          <rizui_button text="打开加载对话框" @click="showIndeterminateBar = true" />
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

    <!-- 进度条对话框实例 -->
    <rizui_dialog
      v-model:show="showCountdownBar"
      title="限时任务确认"
      icon="fa-solid fa-hourglass-half"
      width="360px"
      show-bar
      :countdown="5"
    >
      <p style="margin-bottom:8px">该对话框启用了底部倒计时进度条（<code>show-bar</code> + <code>countdown="5"</code>）。</p>
      <p>底部进度条在 5 秒内平滑缩短，倒计时归零时将自动关闭对话框。</p>
      <template #footer>
        <rizui_button text="立即确认" primary long @click="showCountdownBar = false" />
      </template>
    </rizui_dialog>

    <rizui_dialog
      v-model:show="showIndeterminateBar"
      title="资源正在加载中"
      icon="fa-solid fa-cloud-arrow-down"
      width="360px"
      show-bar
      indeterminate
    >
      <p style="margin-bottom:8px">该对话框启用了不定进度模式（<code>show-bar</code> + <code>indeterminate</code>）。</p>
      <p>底部流光往复循环流动，适用于异步请求处理、资源加载中等场景。</p>
      <template #footer>
        <rizui_button text="完成 / 关闭" primary long @click="showIndeterminateBar = false" />
      </template>
    </rizui_dialog>

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
      未指定 <code>color</code> 时，对话框（及所有控件）自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </rizui_infobox>

    <section class="sec-slots"><h3><i class="fa-solid fa-puzzle-piece"></i> Slots</h3>
      <ul><li v-for="s in slots" :key="s"><code>{{ s }}</code></li></ul>
    </section>

    <section class="sec-events"><h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul><li v-for="e in events" :key="e"><code>{{ e }}</code></li></ul>
    </section>

    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <rizui_code block lang="html" :code="usage" />
    </section>
  </main>
</template>
