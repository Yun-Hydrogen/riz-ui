<script setup>
import { ref, inject } from 'vue'
import {
  rizui_circle_progress,
  rizui_card,
  rizui_cfgrow,
  rizui_button,
  rizui_switch,
  rizui_infobox,
  rizui_code
} from 'riz-ui'

const themeColor = inject('themeColor')

const progressVal = ref(68)
const isIndeterminate = ref(true)
const isClockwise = ref(true)

function addProgress(delta) {
  progressVal.value = Math.max(0, Math.min(100, progressVal.value + delta))
}

const props = [
  { name: 'modelValue',    type: 'Number',                 default: '0',       desc: '当前进度值（支持 v-model）' },
  { name: 'value',         type: 'Number',                 default: 'undefined', desc: '进度值（modelValue 别名，二者等价）' },
  { name: 'min',           type: 'Number',                 default: '0',       desc: '最小值' },
  { name: 'max',           type: 'Number',                 default: '100',     desc: '最大值' },
  { name: 'indeterminate', type: 'Boolean',                default: 'false',   desc: '是否为不定进度模式（未知进度/加载中）' },
  { name: 'size',          type: 'Number | String',        default: '120',     desc: '环形进度条直径尺寸（数字转为 px）' },
  { name: 'stroke-width',  type: 'Number',                 default: '8',       desc: '线条宽度粗细（相对单位）' },
  { name: 'stroke-linecap',type: 'String',                 default: "'round'", desc: "环端样式：'round' | 'butt' | 'square'" },
  { name: 'color',         type: 'String | Array',         default: '""',      desc: '自定义颜色、渐变或颜色数组，为空跟随 --rizui_themecolor' },
  { name: 'track-color',   type: 'String',                 default: '""',      desc: '自定义底层轨道圆环颜色' },
  { name: 'status',        type: 'String',                 default: '""',      desc: '预设状态色："info" | "success" | "warn" | "error"' },
  { name: 'clockwise',     type: 'Boolean',                default: 'true',    desc: '是否顺时针方向' },
  { name: 'show-text',     type: 'Boolean',                default: 'true',    desc: '是否显示中心文案' },
  { name: 'text',          type: 'String',                 default: '""',      desc: '自定义中心文本内容覆盖' },
  { name: 'text-style',    type: 'Object',                 default: '{}',      desc: '中心文字内联样式覆盖' },
  { name: 'format',        type: 'Function',               default: 'null',    desc: '文本格式化函数：(value, percentage) => string' },
  { name: 'theme',         type: 'String',                 default: "'auto'",  desc: '主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]

const slots = [
  { name: 'default', desc: '自定义环形中心内容，作用域参数：{ value, percentage }' }
]

const usage = `<!-- 1. 基础环形进度条 -->
<rizui_circle_progress v-model="progressVal" />

<!-- 2. 不定进度模式（加载中/未知总进度） -->
<rizui_circle_progress indeterminate />
<rizui_circle_progress indeterminate text="同步中" />

<!-- 3. 状态色与渐变风格 -->
<rizui_circle_progress :value="100" status="success" />
<rizui_circle_progress :value="75" :color="['#36d1dc', '#5b86e5']" />

<!-- 4. 自定义尺寸与中心插槽 -->
<rizui_circle_progress :value="80" :size="90" :stroke-width="6">
  <template #default="{ percentage }">
    <span style="font-size: 14px; color: #34c759;">
      <i class="fa-solid fa-check"></i> {{ percentage }}%
    </span>
  </template>
</rizui_circle_progress>`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>环形进度条</h2>
        <p class="sec-tag">rizui_circle_progress</p>
      </div>
      <p class="sec-desc">
        SVG 矢量高清环形进度条，支持精确百分比过渡、不定进度（Indeterminate）旋转流动、渐变色渲染与中心自定义插槽。
      </p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 1. 基础用法与交互 -->
      <rizui_card title="基础用法" desc="支持平滑百分比动画过渡与顺/逆时针切换">
        <rizui_cfgrow label="当前进度" :hint="`数值: ${progressVal}%`">
          <div style="display: flex; gap: 8px;">
            <rizui_button text="-10%" @click="addProgress(-10)" />
            <rizui_button text="+10%" primary @click="addProgress(10)" />
          </div>
        </rizui_cfgrow>
        <rizui_cfgrow label="顺时针方向" hint="切换环形进度绘制方向">
          <rizui_switch v-model="isClockwise" />
        </rizui_cfgrow>

        <div style="display: flex; justify-content: center; padding: 16px 0;">
          <rizui_circle_progress
            v-model="progressVal"
            :clockwise="isClockwise"
          />
        </div>
      </rizui_card>

      <!-- 2. 不定进度模式 -->
      <rizui_card title="不定进度模式 (Indeterminate)" desc="用于加载等待、异步任务处理或未知总时长">
        <rizui_cfgrow label="切换模式" hint="控制不定进度模式激活状态">
          <rizui_switch v-model="isIndeterminate" />
        </rizui_cfgrow>

        <div style="display: flex; justify-content: space-around; align-items: center; padding: 20px 0; flex-wrap: wrap; gap: 20px;">
          <div style="text-align: center;">
            <rizui_circle_progress
              :indeterminate="isIndeterminate"
              :size="100"
              color="#39c5bb"
            />
            <div style="font-size: 12px; color: var(--rz-subtext); margin-top: 8px;">默认加载提示</div>
          </div>

          <div style="text-align: center;">
            <rizui_circle_progress
              :indeterminate="isIndeterminate"
              :size="100"
              text="校验中..."
              color="#e6a23c"
            />
            <div style="font-size: 12px; color: var(--rz-subtext); margin-top: 8px;">自定义文字</div>
          </div>

          <div style="text-align: center;">
            <rizui_circle_progress
              :indeterminate="isIndeterminate"
              :size="100"
              :show-text="false"
              color="linear-gradient(90deg, #ff7675, #e84393)"
            />
            <div style="font-size: 12px; color: var(--rz-subtext); margin-top: 8px;">极简纯环模式</div>
          </div>
        </div>
      </rizui_card>

      <!-- 3. 状态配色与渐变色彩 -->
      <rizui_card title="状态色与渐变风格" desc="支持 success / warn / error 语义色，或传入颜色数组实现平滑圆环渐变">
        <div style="display: flex; justify-content: space-around; align-items: center; padding: 20px 0; flex-wrap: wrap; gap: 20px;">
          <div style="text-align: center;">
            <rizui_circle_progress :value="100" status="success" :size="90" />
            <div style="font-size: 12px; color: var(--rz-subtext); margin-top: 8px;">Success (100%)</div>
          </div>

          <div style="text-align: center;">
            <rizui_circle_progress :value="75" status="warn" :size="90" />
            <div style="font-size: 12px; color: var(--rz-subtext); margin-top: 8px;">Warn (75%)</div>
          </div>

          <div style="text-align: center;">
            <rizui_circle_progress :value="30" status="error" :size="90" />
            <div style="font-size: 12px; color: var(--rz-subtext); margin-top: 8px;">Error (30%)</div>
          </div>

          <div style="text-align: center;">
            <rizui_circle_progress
              :value="85"
              :color="['#36d1dc', '#5b86e5']"
              :size="90"
            />
            <div style="font-size: 12px; color: var(--rz-subtext); margin-top: 8px;">双色渐变</div>
          </div>
        </div>
      </rizui_card>

      <!-- 4. 尺寸、线条粗细与中心插槽 -->
      <rizui_card title="尺寸微调与中心插槽" desc="支持自定义直径、线条粗细以及使用 slot 完全接管中心内容">
        <div style="display: flex; justify-content: space-around; align-items: center; padding: 20px 0; flex-wrap: wrap; gap: 20px;">
          <div style="text-align: center;">
            <rizui_circle_progress :value="60" :size="70" :stroke-width="5" />
            <div style="font-size: 12px; color: var(--rz-subtext); margin-top: 8px;">迷你纤细 (70px / 5px)</div>
          </div>

          <div style="text-align: center;">
            <rizui_circle_progress :value="50" :size="130" :stroke-width="12" />
            <div style="font-size: 12px; color: var(--rz-subtext); margin-top: 8px;">加粗厚重 (130px / 12px)</div>
          </div>

          <div style="text-align: center;">
            <rizui_circle_progress :value="92" :size="110" color="#34c759">
              <template #default="{ percentage }">
                <div style="display: flex; flex-direction: column; align-items: center;">
                  <i class="fa-solid fa-cloud-arrow-up" style="font-size: 16px; color: #34c759; margin-bottom: 2px;"></i>
                  <span style="font-size: 15px; font-weight: 700;">{{ percentage }}%</span>
                  <span style="font-size: 10px; color: var(--rz-subtext);">已备份</span>
                </div>
              </template>
            </rizui_circle_progress>
            <div style="font-size: 12px; color: var(--rz-subtext); margin-top: 8px;">插槽多行布局</div>
          </div>
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
      未指定 <code>color</code> 时，圆环填充色自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。可在顶部取色器实时换色进行验证。
    </rizui_infobox>

    <section class="sec-usage">
      <h3><i class="fa-solid fa-code"></i> 用法</h3>
      <rizui_code block lang="html" :code="usage" />
    </section>
  </main>
</template>
