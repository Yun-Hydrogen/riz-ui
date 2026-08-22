<script setup>
import { Cntr_Infobox, Cntr_Card } from 'riz-ui'

const props = [
  { name:'type',        type:'String',  default:'"info"',  desc:'info / warn / error' },
  { name:'text',        type:'String',  default:'""',      desc:'提示文字，可空' },
  { name:'textInstyle', type:'Object',  default:'{}',      desc:'文字样式覆盖' },
  { name:'icon',        type:'String',  default:'""',      desc:'自定义图标，空时按 type' },
  { name:'iconInstyle', type:'Object',  default:'{}',      desc:'图标样式覆盖' }
]
const usage = `<!-- 三种预设 -->
<Cntr_Infobox type="info" text="提示" />
<Cntr_Infobox type="warn" text="警告" />
<Cntr_Infobox type="error" text="报错" />

<!-- 自定义图标 -->
<Cntr_Infobox type="info" text="自己换个图标"
              icon="fa-solid fa-bell"
              :icon-instyle="{ fontSize:'20px', color:'#e6a23c' }" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>提示框</h2><p class="sec-tag">Cntr_Infobox</p></div>
      <p class="sec-desc">长方形的提示框，撑满整行、虚线边框，左边带图标。三种预设配色。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <!--三种预设-->
      <Cntr_Card title="三种配色" desc="info / warn / error">
        <div class="demo-stack">
          <Cntr_Infobox type="info" text="一切正常，只是随便提醒你一句。" />
          <Cntr_Infobox type="warn" text="注意，这个设置可能会有点影响。" />
          <Cntr_Infobox type="error" text="坏了，这一步没跑通。" />
        </div>
      </Cntr_Card>

      <!--自定义-->
      <Cntr_Card title="自己换图标" desc="icon / 文字都能改样式">
        <div class="demo-stack">
          <Cntr_Infobox type="info" text="换个响铃图标，fa-bell"
                        icon="fa-solid fa-bell"
                        :icon-instyle="{ fontSize:'20px', color:'#e6a23c' }" />
          <Cntr_Infobox type="warn" text="文字加粗一下"
                        :text-instyle="{ fontWeight:700, fontSize:'14px' }" />
        </div>
      </Cntr_Card>
    </section>

    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>

    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>

<style scoped>
.demo-stack {
  display: flex;
  flex-direction: column;
  gap: 10px;
}
</style>
