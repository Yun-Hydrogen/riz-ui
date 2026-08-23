<script setup>
import { ref, inject } from 'vue'
import { Ctrl_Button, Ctrl_Switch, Ctrl_Slider, Ctrl_Timeline, Ctrl_Dropdown, Cntr_Card, Cntr_Cfgrow } from 'riz-ui'

const themeColor = inject('themeColor')

const switchOn = ref(true)
const sliderVal = ref(60)
const timelineVal = ref(30)
const dropdownVal = ref('')

const usage = `<!-- 1. CSS 变量方式：任何元素都能直接拿（推荐，不用 JS） -->
<div :style="{ color: 'var(--rizui_themecolor)' }">这行字是主题色</div>
<div style="background: var(--rizui_themecolor)">背景是主题色</div>

<!-- 2. 一处定义，处处使用：全局只改一个值 -->
\u003cstyle>
:root { --rizui_themecolor: #8b5cf6; }  /* 全库控件 + 所有 var() 一起变 */
<\/style>

<!-- 3. 只有 JS 要读色值才用 inject（一般用不到） -->
<script setup>
import { inject } from 'vue'
const themeColor = inject('themeColor')  // 返回 ref，模板自动解包
<\/script>
<Ctrl_Button :text="themeColor" primary />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>主题色</h2><p class="sec-tag">--rizui_themecolor</p></div>
      <p class="sec-desc">全库共用同一个 CSS 变量 <code>--rizui_themecolor</code>，默认 <code>#66CCFF</code>。顶栏的取色器改了它，所有控件一起变。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <Cntr_Card title="CSS 变量直接用" desc="不用 JS，直接在 style 里 var() 就行">
        <div class="thm-row">
          <span class="thm-chip" :style="{ color: 'var(--rizui_themecolor)' }">文字色</span>
          <span class="thm-chip thm-chip-bg" :style="{ background: 'var(--rizui_themecolor)' }">背景色</span>
          <span class="thm-chip thm-chip-line" :style="{ borderColor: 'var(--rizui_themecolor)', color: 'var(--rizui_themecolor)' }">边框色</span>
        </div>
      </Cntr_Card>

      <Cntr_Card title="JS 拿当前值" desc="一般用不到：只有 JS 需要读色值（拼进样式/传 prop）时才 inject">
        <Cntr_Cfgrow label="当前主题色" hint="顶栏取色器改的就是它">
          <span class="thm-value" :style="{ color: 'var(--rizui_themecolor)' }">{{ themeColor }}</span>
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="给按钮上主题色" hint="color prop 传给它">
          <Ctrl_Button :text="'当前色 ' + themeColor" primary :color="themeColor" />
        </Cntr_Cfgrow>
      </Cntr_Card>

      <Cntr_Card title="控件空 color 自动跟随" desc="什么都不传，天然就是主题色">
        <Cntr_Cfgrow label="按钮" hint="primary 空 color">
          <Ctrl_Button text="跟着主题色走" primary />
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="开关" hint="空 color">
          <Ctrl_Switch v-model="switchOn" />
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="滑条" hint="空 color">
          <Ctrl_Slider v-model="sliderVal" />
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="时间轴" hint="空 color">
          <Ctrl_Timeline v-model="timelineVal" />
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="下拉" hint="空 color">
          <Ctrl_Dropdown v-model="dropdownVal" :options="['主题色方案', '跟随变色', '其他预设']" />
        </Cntr_Cfgrow>
      </Cntr_Card>
    </section>

    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>

<style scoped>
.thm-row { display: flex; gap: 12px; flex-wrap: wrap; }
.thm-chip {
  display: inline-block;
  padding: 6px 14px;
  border-radius: 8px;
  font-size: 13px;
  background: #f6f7f9;
}
.thm-chip-bg { color: #fff; }
.thm-chip-line { border: 1.5px solid; background: transparent; }
.thm-value {
  font-weight: 600;
  font-family: monospace;
}
</style>
