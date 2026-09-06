<script setup>
import { inject } from 'vue'
import { UiCard, UiConfigRow, UiSwitch, UiDropdown, NotReadyOverlay, rizui_code } from 'riz-ui'
const themeColor = inject('themeColor')
const dropdownOptions = [
  { value: 'option1', label: '选项 Alpha' },
  { value: 'option2', label: '选项 Beta' },
  { value: 'option3', label: '选项 Gamma' }
]
const deprecated = true
const props = [
  { name:'text', type:'String', default:'"等待实现"', desc:'遮罩提示文字' }
]
const usage = `<div style="position: relative">
  ...正常内容...
  <NotReadyOverlay text="功能开发中" />
</div>`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>未就绪遮罩</h2><p class="sec-tag">NotReadyOverlay</p><p class="sec-deprecated">已弃用</p></div>
      <p class="sec-desc">半透明毛玻璃遮罩 + 居中徽章，用于标记"功能暂不可用"的 UI 区域。父容器需 position: relative。</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <UiCard title="基础用法" desc="">
        <div class="nro-demo">
          <UiConfigRow label="联动播报" hint="功能尚未实现"><UiSwitch :model-value="false" disabled/></UiConfigRow>
          <UiConfigRow label="高级筛选" hint="敬请期待"><UiDropdown :model-value="''" :options="dropdownOptions"/></UiConfigRow>
          <NotReadyOverlay text="功能开发中"/>
        </div>
      </UiCard>
      <UiCard title="自定义文字" desc="">
        <div class="nro-demo">
          <UiConfigRow label="数据导出" hint="即将支持"><UiSwitch :model-value="false" disabled/></UiConfigRow>
          <NotReadyOverlay text="即将上线"/>
        </div>
      </UiCard>
    </section>
    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>
    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <rizui_code block lang="html" :code="usage" />
    </section>
  </main>
</template>
