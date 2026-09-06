<script setup>
import { ref, inject } from 'vue'
import {
  rizui_button,
  rizui_switch,
  rizui_slider,
  rizui_timeline,
  rizui_dropdown,
  rizui_card,
  rizui_cfgrow,
  rizui_infobox,
  rizui_code,
  rizui_text
} from 'riz-ui'

const themeColor = inject('themeColor')

const aronaOnline = ref(true)
const pyroxeneSlider = ref(75)
const missionProgress = ref(45)
const schaleDept = ref('沙勒特别事务局')

const card1Usage = `<!-- 1. CSS 变量方式：无需 JS，直接在任何元素/样式中引用 -->
<div class="thm-row">
  <span class="thm-chip" :style="{ color: 'var(--rizui_themecolor)' }">什亭之箱</span>
  <span class="thm-chip thm-chip-bg" :style="{ background: 'var(--rizui_themecolor)' }">沙勒</span>
  <span class="thm-chip thm-chip-line" :style="{ borderColor: 'var(--rizui_themecolor)', color: 'var(--rizui_themecolor)' }">无限制决战</span>
</div>

<!-- 2. 全局覆写：在 :root 中修改，全库所有控件与 CSS 变量实时生效 -->
<style>
:root {
  --rizui_themecolor: #66CCFF; 
}
</style>`

const card2Usage = `<!-- 3. JS 中读取（仅在需要将主题色作为变量传递或计算时使用） -->
<script setup>
import { inject } from 'vue'
const themeColor = inject('themeColor')
<\/script>

<rizui_button :text="'当前主题色 ' + themeColor" primary :color="themeColor" />`

</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>全局主题色</h2>
        <p class="sec-tag">--rizui_themecolor</p>
      </div>
      <p class="sec-desc">全库统一共用 CSS 变量 <rizui_code style="font-size: 10px;">--rizui_themecolor</rizui_code> 天依蓝 <rizui_code style="font-size: 10px;">#66CCFF</rizui_code>。通过顶栏取色器或覆写变量即可驱动所有组件、光标、滑条与高亮边框同步联动。</p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- CSS 变量引用 -->
      <rizui_card title="CSS 变量直接引用 | 零 JS 依赖" desc="在原生 style 或 class 中通过 var(--rizui_themecolor) 任意取用主题色">
        <div class="thm-row">
          <span class="thm-chip" :style="{ color: 'var(--rizui_themecolor)' }">什亭之箱</span>
          <span class="thm-chip thm-chip-bg" :style="{ background: 'var(--rizui_themecolor)' }">沙勒</span>
          <span class="thm-chip thm-chip-line" :style="{ borderColor: 'var(--rizui_themecolor)', color: 'var(--rizui_themecolor)' }">无限制决战</span>
        </div>
        <rizui_code block lang="html" :code="card1Usage" style="margin-top: 12px" />
      </rizui_card>

      <!-- JS 注入读取 -->
      <rizui_card title="JS 动态读取 | Provide / Inject" desc="顶栏取色器通过 provide 提供响应式 ref，可在业务代码中 inject 获取">
        <rizui_cfgrow label="当前沙勒主题色值" hint="响应式 ref 变量，随顶栏取色器实时刷新">
          <rizui_text v-model="themeColor"/>
        </rizui_cfgrow>
        <rizui_cfgrow label="显式绑定组件" hint="将色值变量作为 prop 传给组件">
          <rizui_button :text="'当前色 ' + themeColor" primary :color="themeColor" />
        </rizui_cfgrow>
        <rizui_code block lang="html" :code="card2Usage" style="margin-top: 12px" />
      </rizui_card>
    </section>

    <!-- 说明提示 -->
    <rizui_infobox type="info" text="可以尝试点击页面右上角的取色器选择不同颜色！"/>

  </main>
</template>

<style scoped>
.thm-row {
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
}

.thm-chip {
  display: inline-block;
  padding: 6px 14px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 500;
  background: color-mix(in srgb, var(--rz-text) 5%, var(--rz-bg));
  border: 1px solid var(--rz-border);
}

.thm-chip-bg {
  color: #fff;
  border: none;
}

.thm-chip-line {
  border: 1.5px solid;
  background: transparent;
}

.thm-value {
  font-weight: 700;
  font-family: monospace;
  font-size: 15px;
}
</style>

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
