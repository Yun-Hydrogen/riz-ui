<script setup>
import { ref, inject } from 'vue'
import { Ctrl_Button, Cntr_Card, Cntr_Cfgrow, Cntr_Infobox } from 'riz-ui'
const themeColor = inject('themeColor')

const clickCount = ref(0)

const props = [
  { name:'text',         type:'String',  default:'""',     desc:'按钮文本，可为空' },
  { name:'textInstyle',  type:'Object',  default:'{}',     desc:'文本内联样式覆盖' },
  { name:'l-icon',       type:'String',  default:'""',     desc:'左侧图标（FA class全部类型均可）' },
  { name:'l-icon-instyle', type:'Object', default:'{}',    desc:'左侧图标内联样式覆盖' },
  { name:'r-icon',       type:'String',  default:'""',     desc:'右侧图标（FA class全部类型均可）' },
  { name:'r-icon-instyle', type:'Object', default:'{}',    desc:'右侧图标内联样式覆盖' },
  { name:'primary',      type:'Boolean', default:'false',  desc:'是否以主题色填充背景' },
  { name:'danger',       type:'Boolean', default:'false',  desc:'是否危险/警告按钮（红色）' },
  { name:'long',         type:'Boolean', default:'false',  desc:'是否宽度撑满父容器' },
  { name:'color',        type:'String',  default:'""',     desc:'主题色，为空时跟随 --rizui_themecolor' },
  { name:'disabled',     type:'Boolean', default:'false',  desc:'是否禁用' },
  { name:'type',         type:'String',  default:'"button"', desc:'原生按钮 type' }
]
const events = ['click']
const usage = `<!-- 按钮放在配置行里 -->
<Cntr_Cfgrow label="操作" hint="主操作 + 危险操作">
  <Ctrl_Button text="保存" l-icon="fa-solid fa-floppy-disk" primary />
  <Ctrl_Button text="删除" danger />
</Cntr_Cfgrow>

<!-- 通栏按钮 -->
<Ctrl_Button text="确认提交" primary long />

<!-- color 为空时自动跟随 --rizui_themecolor，在 :root 覆盖即可换色 -->
<Ctrl_Button text="跟随主题色" primary />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>按钮</h2><p class="sec-tag">Ctrl_Button</p></div>
      <p class="sec-desc">胶囊形按钮组件，支持主题色填充、通栏、左右图标与禁用态。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <Cntr_Card title="基础用法" desc="普通 / primary / long 三种状态">
        <div class="btn-row">
          <Ctrl_Button text="普通按钮" />
          <Ctrl_Button text="主按钮" primary />
          <Ctrl_Button text="危险按钮" danger l-icon="fa-solid fa-triangle-exclamation" />
          <Ctrl_Button text="通栏按钮" long />
          <Ctrl_Button text="主通栏按钮" primary long l-icon="fa-solid fa-house" />
        </div>
      </Cntr_Card>

      <Cntr_Card title="图标按钮" desc="左右图标支持完整 FA class 组合与内联样式覆盖">
        <div class="btn-row">
          <Ctrl_Button text='保存(l-icon="fa-solid fa-floppy-disk")' l-icon="fa-solid fa-floppy-disk" primary />
          <Ctrl_Button text='下一步(r-icon="fa-solid fa-arrow-right")' r-icon="fa-solid fa-arrow-right" />
          <Ctrl_Button l-icon="fa-solid fa-magnifying-glass" r-icon="fa-solid fa-chevron-down fa-spin-snap-8" text='搜索(l-icon="fa-solid fa-magnifying-glass" r-icon="fa-solid fa-chevron-down fa-spin-snap-8")' />
        </div>
        <div class="btn-row">
          <Ctrl_Button text="自定义样式" :text-instyle="{ fontSize:'14px', fontWeight:700 }"
                       l-icon="fa-solid fa-star" :l-icon-instyle="{ color:'#ffb300' }" primary style="margin-top: 8px;"/>
        </div>
      </Cntr_Card>

      <Cntr_Card title="空文本 / 禁用" desc="text 为空时仅渲染图标；disabled 禁用交互">
        <div class="btn-row">
          <Ctrl_Button l-icon="fa-solid fa-plus" primary />
          <Ctrl_Button l-icon="fa-solid fa-rotate-right" />
          <Ctrl_Button text="禁用按钮" primary disabled />
          <Ctrl_Button text="点击计数" primary @click="clickCount++" />
          <span class="demo-echo">点击次数：<code>{{ clickCount }}</code></span>
        </div>
      </Cntr_Card>
    </section>

    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>

    <!-- 主题色提示 -->
    <Cntr_Infobox type="info">
      未指定 <code>color</code> 时，按钮（及所有控件）自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </Cntr_Infobox>

    <section class="sec-events"><h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul><li v-for="e in events" :key="e"><code>{{ e }}</code></li></ul>
    </section>

    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>

<style scoped>
.btn-row {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: 12px;
}

.btn-row + .btn-row {
  margin-top: 12px;
}
</style>
