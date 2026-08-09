<script setup>
import { ref, inject } from 'vue'
import { UiCard, UiConfigRow, UiSwitch, NotReadyOverlay } from 'riz-ui'
const themeColor = inject('themeColor')
const switchOn = ref(true)
const deprecated = true
const props = [
  { name:'label',        type:'String',  required:'是',  desc:'左侧标签文字' },
  { name:'hint',         type:'String',  default:'""', desc:'标签下方提示小字' },
  { name:'stack',        type:'Boolean', default:'false', desc:'竖向布局' },
  { name:'notReady',     type:'Boolean', default:'false', desc:'显示未就绪遮罩' },
  { name:'notReadyText', type:'String',  default:'"等待实现"', desc:'遮罩提示文字' }
]
const slots = ['default — 控件区']
const usage = `<UiConfigRow label="持续置顶" hint="窗口将始终位于最前">
  <UiSwitch v-model="on" :color="theme" />
</UiConfigRow>

<UiConfigRow label="程序路径" hint="..." stack>
  <input class="my-input" />
</UiConfigRow>

<UiConfigRow label="联动播报" notReady notReadyText="开发中" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>配置行</h2><p class="sec-tag">UiConfigRow</p><p class="sec-deprecated">已弃用</p></div>
      <p class="sec-desc">标准配置行组件，左侧标签 + 提示 + 右侧控件区。支持 stack 竖向布局和 notReady 遮罩。</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 实时预览</h3>
      <UiCard title="横向模式（默认）" desc="">
        <UiConfigRow label="开启功能" hint="使用 v-model 双向绑定"><UiSwitch v-model="switchOn" :color="themeColor"/></UiConfigRow>
        <UiConfigRow label="禁用示例" hint="设置 disabled 属性"><UiSwitch :model-value="true" :color="themeColor" disabled/></UiConfigRow>
      </UiCard>
      <UiCard title="Stack 竖向模式" desc="">
        <UiConfigRow label="自定义输入" hint="stack 模式下控件在标签下方" stack>
          <input type="text" placeholder="请输入内容..." style="width:100%;padding:8px 12px;border:1.5px solid #d0d8e0;border-radius:8px;font-size:13px;outline:none;font-family:inherit"/>
        </UiConfigRow>
      </UiCard>
      <UiCard title="NotReady 遮罩模式" desc="">
        <div class="nro-demo">
          <UiConfigRow label="联动播报" hint="此功能正在开发"><UiSwitch :model-value="false" disabled/></UiConfigRow>
          <NotReadyOverlay text="开发中"/>
        </div>
      </UiCard>
    </section>
    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>
    <section class="sec-slots"><h3><i class="fa-solid fa-puzzle-piece"></i> Slots</h3>
      <ul><li v-for="s in slots" :key="s"><code>{{ s }}</code></li></ul>
    </section>
    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>
