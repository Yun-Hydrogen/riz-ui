<script setup>
import { ref, inject } from 'vue'
import { rizui_card, rizui_cfgrow, rizui_switch } from 'riz-ui'
const themeColor = inject('themeColor')
const switchOn = ref(true)

const props = [
  { name:'title',        type:'String', default:'""', desc:'标题文字' },
  { name:'title-style', type:'Object', default:'{}', desc:'标题内联样式覆盖' },
  { name:'desc',         type:'String', default:'""', desc:'描述文字' },
  { name:'desc-style',  type:'Object', default:'{}', desc:'描述内联样式覆盖' },
  { name:'icon',         type:'String', default:'""', desc:'FA 图标类名' },
  { name:'icon-style',  type:'Object', default:'{}', desc:'图标内联样式覆盖' }
]
const slots = ['default — 卡片内容区']

const usage = 
`<rizui_card title="项目文件夹" desc="包含 12 个文件"
  icon="fa-regular fa-folder"
  :icon-style="{ color: '#f0a030', fontSize: '24px' }"
  :title-style="{ fontSize: '16px' }"
>
  <rizui_cfgrow label="子项">...</rizui_cfgrow>
</rizui_card>`

const card1_usage = 
`<rizui_card title="项目文件夹" 
  desc="包含 12 个文件 · 最后修改 2 小时前" 
  icon="fa-regular fa-folder" 
  :icon-style="{ color: '#f0a030', fontSize: '28px' }">"`

const card2_usage =
`卡片也可以不设置任何prop，单纯作为一个背景容器`

const card3_usage =
`<rizui_card title="旋转图标示例" 
  icon="fa-solid fa-sync fa-spin" 
  :icon-style="{ color: themeColor, fontSize: '22px' }" 
  desc="图标样式支持全部 FA class 组合">`

</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>卡片容器</h2><p class="sec-tag">rizui_card</p></div>
      <p class="sec-desc">新一代卡片容器，卡片图标支持所有 FA 图标参数，每个 prop 均可通过 xxx-style 内联覆写样式。</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i>示例</h3>
      
      <!-- 标准卡片  -->
      <rizui_card title="MomoTalk 信箱" desc="包含 120 位联系人 · 最近信息 2 小时前" icon="fa-solid fa-envelope-open-text" :icon-style="{ color: '#f0a030', fontSize: '28px' }">
        <rizui_cfgrow label="这是一个标准的卡片" hint="可以在下方看到示例"></rizui_cfgrow>
        <rizui_cfgrow stack>
          <pre class="code-block"><code>{{ card1_usage }}</code></pre>
        </rizui_cfgrow> 
      </rizui_card>

      <!-- 空卡片  -->
      <rizui_card>
          <pre class="code-block"><code>{{ card2_usage }}</code></pre>
      </rizui_card>
      
      <!-- 旋转图标卡片  -->
      <rizui_card title="旋转图标示例" icon="fa-solid fa-sync fa-spin" :icon-style="{ color: themeColor, fontSize: '22px' }" desc="图标样式支持全部 FA class 组合">
        <rizui_cfgrow stack>
          <pre class="code-block"><code>{{ card3_usage }}</code></pre>
        </rizui_cfgrow>
      </rizui_card>

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
