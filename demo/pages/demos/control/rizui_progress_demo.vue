<script setup>
import { ref, inject } from 'vue'
import {
  rizui_progress,
  rizui_card,
  rizui_cfgrow,
  rizui_button,
  rizui_infobox,
  rizui_code,
  rizui_switch
} from 'riz-ui'

const themeColor = inject('themeColor')

const progressVal = ref(45)
const isIndeterminate = ref(true)

function addProgress(delta) {
  progressVal.value = Math.max(0, Math.min(100, progressVal.value + delta))
}

const props = [
  { name: 'modelValue',    type: 'Number',          default: '0',       desc: '当前进度值（支持 v-model）' },
  { name: 'value',         type: 'Number',          default: 'undefined', desc: '进度值（modelValue 别名，二者等价）' },
  { name: 'min',           type: 'Number',          default: '0',       desc: '最小值' },
  { name: 'max',           type: 'Number',          default: '100',     desc: '最大值' },
  { name: 'indeterminate', type: 'Boolean',         default: 'false',   desc: '是否为不定进度模式（未知进度/加载缓冲）' },
  { name: 'color',         type: 'String',          default: '""',      desc: '自定义颜色或 CSS 渐变，为空跟随 --rizui_themecolor' },
  { name: 'track-color',   type: 'String',          default: '""',      desc: '自定义轨道背景底色' },
  { name: 'status',        type: 'String',          default: '""',      desc: '预设状态色："info" | "success" | "warn" | "error"' },
  { name: 'height',        type: 'Number | String', default: '10',      desc: '进度条高度（数字默认为 px）' },
  { name: 'rounded',       type: 'Boolean',         default: 'true',    desc: '是否为胶囊大圆角' },
  { name: 'show-text',     type: 'Boolean',         default: 'false',   desc: '是否显示进度文本' },
  { name: 'text-inside',   type: 'Boolean',         default: 'false',   desc: '进度文本是否内嵌在进度条内部（建议较大高度）' },
  { name: 'text',          type: 'String',          default: '""',      desc: '自定义显示文本覆盖' },
  { name: 'text-style',    type: 'Object',          default: '{}',      desc: '外部文本内联样式覆盖' },
  { name: 'format',        type: 'Function',        default: 'null',    desc: '文本格式化函数：(value, percentage) => string' },
  { name: 'theme',         type: 'String',          default: "'auto'",  desc: '主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]

const slots = [
  { name: 'default', desc: '自定义进度文本内容，作用域参数：{ value, percentage }' }
]

const usage = `<!-- 1. 基础长条进度条 -->
<rizui_progress v-model="progressVal" show-text />

<!-- 2. 不定进度模式（加载中/未知总进度） -->
<rizui_progress indeterminate />
<rizui_progress indeterminate show-text text="资源解包中..." />

<!-- 3. 状态色与自定义渐变 -->
<rizui_progress :value="80" status="success" show-text />
<rizui_progress :value="60" color="linear-gradient(90deg, #ff7675, #e84393)" />

<!-- 4. 内嵌文字与自定义高度 -->
<rizui_progress :value="70" :height="20" text-inside show-text />

<!-- 5. 直角与自定义轨道底色 -->
<rizui_progress :value="50" :rounded="false" track-color="rgba(0,0,0,0.1)" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>长条进度条</h2>
        <p class="sec-tag">rizui_progress</p>
      </div>
      <p class="sec-desc">
        精致轻量的胶囊长条进度条，支持精确进度、不定进度（Indeterminate）加载模式、状态色彩与文本内外嵌。
      </p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 1. 基础进度条与交互调节 -->
      <rizui_card title="基础进度" desc="支持平滑宽度过渡与外部百分比显示">
        <rizui_cfgrow label="当前进度" :hint="`数值: ${progressVal}%`">
          <div style="display: flex; gap: 8px;">
            <rizui_button text="-10%" @click="addProgress(-10)" />
            <rizui_button text="+10%" primary @click="addProgress(10)" />
          </div>
        </rizui_cfgrow>

        <div style="padding: 12px 0 6px;">
          <rizui_progress v-model="progressVal" show-text />
        </div>
      </rizui_card>

      <!-- 2. 不定进度模式（未知进度） -->
      <rizui_card title="不定进度模式 (Indeterminate)" desc="用于加载缓冲、后台计算或未知总时长的任务">
        <rizui_cfgrow label="切换模式" hint="控制不定进度模式激活状态">
          <rizui_switch v-model="isIndeterminate" />
        </rizui_cfgrow>

        <div style="display: flex; flex-direction: column; gap: 14px; padding: 12px 0 6px;">
          <div>
            <div style="font-size: 12px; color: var(--rz-subtext); margin-bottom: 6px;">基础不定流动条</div>
            <rizui_progress :indeterminate="isIndeterminate" />
          </div>

          <div>
            <div style="font-size: 12px; color: var(--rz-subtext); margin-bottom: 6px;">带外部加载文案提示</div>
            <rizui_progress
              :indeterminate="isIndeterminate"
              show-text
              text="同步中..."
              color="#39c5bb"
            />
          </div>
        </div>
      </rizui_card>

      <!-- 3. 状态色与自定义配色 -->
      <rizui_card title="状态色与渐变风格" desc="提供 success、warn、error 语义状态，亦可传入任意 CSS 渐变色">
        <div style="display: flex; flex-direction: column; gap: 14px; padding: 6px 0;">
          <rizui_cfgrow label="Info（默认主题色）" hint="跟随全局 --rizui_themecolor">
            <rizui_progress :value="60" status="info" show-text style="width: 200px;" />
          </rizui_cfgrow>

          <rizui_cfgrow label="Success 成功状态" hint="预设高明度绿">
            <rizui_progress :value="100" status="success" show-text style="width: 200px;" />
          </rizui_cfgrow>

          <rizui_cfgrow label="Warn 警告状态" hint="预设柔和橙黄">
            <rizui_progress :value="80" status="warn" show-text style="width: 200px;" />
          </rizui_cfgrow>

          <rizui_cfgrow label="Error 异常状态" hint="预设红色">
            <rizui_progress :value="35" status="error" show-text style="width: 200px;" />
          </rizui_cfgrow>

          <rizui_cfgrow label="自定义炫彩渐变" hint="支持 linear-gradient 等 CSS 语法">
            <rizui_progress
              :value="75"
              color="linear-gradient(90deg, #36d1dc 0%, #5b86e5 100%)"
              show-text
              style="width: 200px;"
            />
          </rizui_cfgrow>
        </div>
      </rizui_card>

      <!-- 4. 尺寸与内嵌文字 -->
      <rizui_card title="高度与内嵌文字" desc="加粗高度可在进度条内展示清晰文字">
        <div style="display: flex; flex-direction: column; gap: 14px; padding: 6px 0;">
          <rizui_cfgrow label="纤细模式 (height=6)" hint="极简低干扰">
            <rizui_progress :value="50" :height="6" style="width: 200px;" />
          </rizui_cfgrow>

          <rizui_cfgrow label="加粗内嵌模式 (height=20)" hint="文字内嵌在进度内部">
            <rizui_progress
              :value="72"
              :height="20"
              show-text
              text-inside
              style="width: 200px;"
            />
          </rizui_cfgrow>

          <rizui_cfgrow label="直角与格式化函数" hint="rounded=false 与自定义格式">
            <rizui_progress
              :value="128"
              :max="256"
              :rounded="false"
              show-text
              :format="(val, pct) => `${val} / 256 MB`"
              style="width: 200px;"
            />
          </rizui_cfgrow>
        </div>
      </rizui_card>
    </section>

    <!-- Props 表格 -->
    <section class="sec-props">
      <h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table">
        <thead>
          <tr>
            <th>属性</th>
            <th>类型</th>
            <th>默认值</th>
            <th>说明</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="p in props" :key="p.name">
            <td><code>{{ p.name }}</code></td>
            <td><code>{{ p.type }}</code></td>
            <td><code>{{ p.default || '—' }}</code></td>
            <td>{{ p.desc }}</td>
          </tr>
        </tbody>
      </table>
    </section>

    <!-- Slots 表格 -->
    <section class="sec-props">
      <h3><i class="fa-solid fa-puzzle-piece"></i> Slots</h3>
      <table class="props-table">
        <thead>
          <tr>
            <th>插槽名</th>
            <th>说明</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="s in slots" :key="s.name">
            <td><code>{{ s.name }}</code></td>
            <td>{{ s.desc }}</td>
          </tr>
        </tbody>
      </table>
    </section>

    <!-- 主题模式提示 -->
    <rizui_infobox type="info">
      <code>theme</code> 支持 <code>Dark</code> / <code>Light</code> / <code>auto</code>（默认）。<code>auto</code> 跟随顶栏主题开关（全局 <code>data-rizui-theme</code>）或系统 <code>prefers-color-scheme</code>；<code>Dark</code> / <code>Light</code> 可对本组件局部强制明暗。
    </rizui_infobox>

    <!-- 主题色提示 -->
    <rizui_infobox type="info">
      未指定 <code>color</code> 时，控件自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。可以在顶部取色器实时换色进行验证。
    </rizui_infobox>

    <section class="sec-usage">
      <h3><i class="fa-solid fa-code"></i> 用法</h3>
      <rizui_code block lang="html" :code="usage" />
    </section>
  </main>
</template>
