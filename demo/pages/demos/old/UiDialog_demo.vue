<script setup>
import { ref, inject, computed } from 'vue'
import { UiCard, UiConfigRow, UiDialog } from 'riz-ui'
const themeColor = inject('themeColor')
const showDialog = ref(false)
const deprecated = true
const demoBtnStyle = computed(() => ({ background: themeColor.value }))
const props = [
  { name:'show',     type:'Boolean', default:'false', desc:'v-model:show 控制显隐' },
  { name:'title',    type:'String',  default:'""',    desc:'标题文字' },
  { name:'color',    type:'String',  default:'""',    desc:'主题色，空时跟随 --rizui_themecolor' },
  { name:'teleport', type:'String',  default:'"body"', desc:'挂载目标选择器' },
  { name:'width',    type:'String',  default:'"auto"', desc:'对话框宽度' }
]
const slots = ['default — 主体内容', 'footer — 底部按钮区（可选）']
const events = ['update:show']
const usage = `const show = ref(false)

<UiDialog v-model:show="show" title="确认" width="320px">
  <p>确定要执行此操作吗？</p>
  <template #footer>
    <button @click="show = false">取消</button>
    <button @click="doIt">确认</button>
  </template>
</UiDialog>`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>对话框</h2><p class="sec-tag">UiDialog</p><p class="sec-deprecated">已弃用</p></div>
      <p class="sec-desc">居中弹出对话框，支持 Teleport 指定挂载容器、Transition 动画、遮罩层点击关闭。</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <UiCard title="基础用法" desc="">
        <UiConfigRow label="示例对话框"><button class="demo-btn" :style="demoBtnStyle" @click="showDialog=true">打开对话框</button></UiConfigRow>
      </UiCard>
      <UiDialog v-model:show="showDialog" title="对话框示例" width="360px">
        <p style="margin-bottom:8px">这是 <strong>UiDialog</strong> 组件。</p>
        <p style="margin-bottom:8px">支持 Teleport 挂载、Transition 动画、遮罩关闭。</p>
        <p>当前主题色：<code>{{ themeColor }}</code></p>
        <template #footer>
          <button class="demo-btn demo-btn-ghost" @click="showDialog=false">取消</button>
          <button class="demo-check" :style="demoBtnStyle" @click="showDialog=false" title="确认">
            <svg viewBox="0 0 24 24" width="16" height="16" stroke="currentColor" stroke-width="3" fill="none"><path d="M20 6L9 17l-5-5"/></svg>
          </button>
        </template>
      </UiDialog>
    </section>
    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>
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
