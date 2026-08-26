<script setup>
import { ref, inject } from 'vue'
import { rizui_button, rizui_card, rizui_cfgrow, rizui_infobox } from 'riz-ui'
const themeColor = inject('themeColor')

const clickCount = ref(0)

const props = [
  { name:'text',         type:'String',  default:'""',     desc:'按钮文本，可为空' },
  { name:'text-style',  type:'Object',  default:'{}',     desc:'文本内联样式覆盖' },
  { name:'l-icon',       type:'String',  default:'""',     desc:'左侧图标（FA class全部类型均可）' },
  { name:'l-icon-style', type:'Object', default:'{}',    desc:'左侧图标内联样式覆盖' },
  { name:'r-icon',       type:'String',  default:'""',     desc:'右侧图标（FA class全部类型均可）' },
  { name:'r-icon-style', type:'Object', default:'{}',    desc:'右侧图标内联样式覆盖' },
  { name:'primary',      type:'Boolean', default:'false',  desc:'是否以主题色填充背景' },
  { name:'danger',       type:'Boolean', default:'false',  desc:'是否危险/警告按钮（红色）' },
  { name:'long',         type:'Boolean', default:'false',  desc:'是否宽度撑满父容器' },
  { name:'color',        type:'String',  default:'""',     desc:'主题色，为空时跟随 --rizui_themecolor' },
  { name:'disabled',     type:'Boolean', default:'false',  desc:'是否禁用' },
  { name:'type',         type:'String',  default:'"button"', desc:'原生按钮 type' }
]
const events = ['click']
const usage = `<!-- 按钮放在配置行里 -->
<rizui_cfgrow label="操作" hint="主操作 + 危险操作">
  <rizui_button text="保存" l-icon="fa-solid fa-floppy-disk" primary />
  <rizui_button text="删除" danger />
</rizui_cfgrow>

<!-- 通栏按钮 -->
<rizui_button text="确认提交" primary long />

<!-- color 为空时自动跟随 --rizui_themecolor，在 :root 覆盖即可换色 -->
<rizui_button text="跟随主题色" primary />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>按钮</h2><p class="sec-tag">rizui_button</p></div>
      <p class="sec-desc">胶囊形按钮组件，支持主题色填充、通栏、左右图标与禁用态。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <rizui_card title="基础用法" desc="普通 / primary / long 三种状态">
        <div class="btn-row">
          <rizui_button text="普通按钮" />
          <rizui_button text="主按钮" primary />
          <rizui_button text="危险按钮" danger l-icon="fa-solid fa-triangle-exclamation" />
          <rizui_button text="通栏按钮" long />
          <rizui_button text="主通栏按钮" primary long l-icon="fa-solid fa-house" />
        </div>
      </rizui_card>

      <rizui_card title="图标按钮" desc="左右图标支持完整 FA class 组合与内联样式覆盖">
        <div class="btn-row">
          <rizui_button text='保存(l-icon="fa-solid fa-floppy-disk")' l-icon="fa-solid fa-floppy-disk" primary />
          <rizui_button text='下一步(r-icon="fa-solid fa-arrow-right")' r-icon="fa-solid fa-arrow-right" />
          <rizui_button l-icon="fa-solid fa-magnifying-glass" r-icon="fa-solid fa-chevron-down fa-spin-snap-8" text='搜索(l-icon="fa-solid fa-magnifying-glass" r-icon="fa-solid fa-chevron-down fa-spin-snap-8")' />
        </div>
        <div class="btn-row">
          <rizui_button text="自定义样式" :text-style="{ fontSize:'14px', fontWeight:700 }"
                       l-icon="fa-solid fa-star" :l-icon-style="{ color:'#ffb300' }" primary style="margin-top: 8px;"/>
        </div>
      </rizui_card>

      <rizui_card title="空文本 / 禁用" desc="text 为空时仅渲染图标；disabled 禁用交互">
        <div class="btn-row">
          <rizui_button l-icon="fa-solid fa-plus" primary />
          <rizui_button l-icon="fa-solid fa-rotate-right" />
          <rizui_button text="禁用按钮" primary disabled />
          <rizui_button text="点击计数" primary @click="clickCount++" />
          <span class="demo-echo">点击次数：<code>{{ clickCount }}</code></span>
        </div>
      </rizui_card>
    </section>

    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>

    <!-- 主题色提示 -->
    <rizui_infobox type="info">
      未指定 <code>color</code> 时，按钮（及所有控件）自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </rizui_infobox>

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
