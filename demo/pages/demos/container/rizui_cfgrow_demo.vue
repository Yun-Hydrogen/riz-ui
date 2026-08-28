<script setup>
import { ref, inject } from 'vue'
import { rizui_cfgrow, rizui_card, rizui_switch, rizui_slider, rizui_text, rizui_button, rizui_infobox, rizui_code } from 'riz-ui'

const themeColor = inject('themeColor')
const systemSync = ref(true)
const overclock = ref(false)
const shieldPower = ref(80)
const memoText = ref('')
const apLimit = ref(360)
const rainbowMode = ref(true)

const props = [
  { name: 'label',       type: 'String',  default: '""',     desc: '左侧标题文字' },
  { name: 'label-style', type: 'Object',  default: '{}',     desc: '标题文本内联样式覆盖' },
  { name: 'hint',        type: 'String',  default: '""',     desc: '说明/提示小字' },
  { name: 'hint-style',  type: 'Object',  default: '{}',     desc: '提示小字内联样式覆盖' },
  { name: 'icon',        type: 'String',  default: '""',     desc: '左侧 FA 图标类名' },
  { name: 'icon-style',  type: 'Object',  default: '{}',     desc: '图标内联样式覆盖' },
  { name: 'stack',       type: 'Boolean', default: 'false',  desc: '是否启用竖向堆叠布局（控件放于标题下方）' },
  { name: 'disabled',    type: 'Boolean', default: 'false',  desc: '是否禁用（半透明且阻止子控件交互）' },
  { name: 'theme',       type: 'String',  default: "'auto'", desc: '主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]

const usage = `      <!-- 什亭之箱 · 基础终端设置（横向模式） -->
      <rizui_card
        title="什亭之箱设置 | 横向模式"
        desc="左侧标题与提示文字，右侧收纳各类轻量操作控件">
        <rizui_cfgrow
          label="主控系统接入"
          hint="连接沙勒的中央控制系统"
          icon="fa-solid fa-satellite-dish"
          :icon-style="{ color: themeColor }">
          <rizui_switch v-model="systemSync" />
        </rizui_cfgrow>

        <rizui_cfgrow
          label="无限制决战"
          hint="普拉娜&阿罗娜合作下的更大指挥规模和更大蓄能上限"
          icon="fa-solid fa-microchip fa-spin">
          <rizui_switch v-model="overclock" />
        </rizui_cfgrow>

        <rizui_cfgrow
          label="屏幕亮度"
          hint="调节什亭之箱屏幕亮度"
          icon="fa-regular fa-sun">
          <rizui_slider v-model="shieldPower" :min="0" :max="100" :step="5" display="%" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 任务行动日志（Stack 竖向模式） -->
      <rizui_card
        title="任务行动日志 | Stack 竖向模式"
        desc="stack 模式下控件换行置于下方，适合较长的输入框、复合控件或完整宽度的滑块">
        <rizui_cfgrow
          label="工作备忘录"
          hint="老师可以在这里记录需要注意的事项哦"
          icon="fa-solid fa-clipboard-list"
          :icon-style="{ color: themeColor }"
          stack>
          <rizui_text v-model="memoText" placeholder="点击输入文本" :plaintext="true" />
        </rizui_cfgrow>

        <rizui_cfgrow 
          label="恢复体力点"
          :hint="\`拖动滑块使用青辉石恢复体力点（当前设定：${apLimit} 体力点）\`"
          icon="fa-solid fa-bolt"
          :icon-style="{ color: '#5ae76b' }"
          stack>
          <rizui_slider
            v-model="apLimit"
            :min="100"
            :max="999"
            :step="10"
            display=" 体力点"
            style="margin: 8px 0 0 0; width: 100%" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 安全协议与限制（Disabled 禁用态） -->
      <rizui_card
        title="安全协议与限制|Disabled 禁用态"
        desc="设置 disabled 后其内部所有支持 disabled 的子控件均不可交互">
        <rizui_cfgrow
          label="关闭什亭之箱的保护场域"
          hint="老师...请不要关闭保护场域，否则将无法抵御外部攻击！ "
          icon="fa-solid fa-lock"
          :icon-style="{ color: '#f56c6c' }"
          :disabled="true">
          <rizui_switch :model-value="true" />
        </rizui_cfgrow>

        <rizui_cfgrow
          label="进入总力战"
          hint="基沃托斯正处于和平状态"
          icon="fa-solid fa-triangle-exclamation"
          :disabled="true">
          <rizui_button text="进入" primary disabled r-icon="fa-solid fa-angle-right" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 样式定制 -->
      <rizui_card
        title="样式定制 | Custom Style"
        desc="支持传入 labelStyle、hintStyle 与 iconStyle 精准定制各区域样式">
        <rizui_cfgrow
          label="由普拉娜担任助手"
          hint="我猜阿罗娜前辈又在睡懒觉了..."
          icon="fa-solid fa-wand-magic-sparkles"
          :icon-style="{ color: '#a855f7' }"
          :label-style="{ fontWeight: 'bold', color: '#a855f7' }"
          :hint-style="{ color: '#c084fc' }">
          <rizui_switch v-model="rainbowMode" />
        </rizui_cfgrow>
      </rizui_card>
      `
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>配置行</h2>
        <p class="sec-tag">rizui_cfgrow</p>
      </div>
      <p class="sec-desc">用于设置面板、表单选项与属性配置的标准行容器。支持左右横向流式排版、上下堆叠布局（stack）、自定义图标和整行禁用态。</p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 什亭之箱 · 基础终端设置（横向模式） -->
      <rizui_card
        title="什亭之箱设置 | 横向模式"
        desc="左侧标题与提示文字，右侧收纳各类轻量操作控件">
        <rizui_cfgrow
          label="主控系统接入"
          hint="连接沙勒的中央控制系统"
          icon="fa-solid fa-satellite-dish"
          :icon-style="{ color: themeColor }">
          <rizui_switch v-model="systemSync" />
        </rizui_cfgrow>

        <rizui_cfgrow
          label="无限制决战"
          hint="普拉娜&阿罗娜合作下的更大指挥规模和更大蓄能上限"
          icon="fa-solid fa-microchip fa-spin">
          <rizui_switch v-model="overclock" />
        </rizui_cfgrow>

        <rizui_cfgrow
          label="屏幕亮度"
          hint="调节什亭之箱屏幕亮度"
          icon="fa-regular fa-sun">
          <rizui_slider v-model="shieldPower" :min="0" :max="100" :step="5" display="%" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 任务行动日志（Stack 竖向模式） -->
      <rizui_card
        title="任务行动日志 | Stack 竖向模式"
        desc="stack 模式下控件换行置于下方，适合较长的输入框、复合控件或完整宽度的滑块">
        <rizui_cfgrow
          label="工作备忘录"
          hint="老师可以在这里记录需要注意的事项哦"
          icon="fa-solid fa-clipboard-list"
          :icon-style="{ color: themeColor }"
          stack>
          <rizui_text v-model="memoText" placeholder="点击输入文本" :plaintext="true" />
        </rizui_cfgrow>

        <rizui_cfgrow 
          label="恢复体力点"
          :hint="`拖动滑块使用青辉石恢复体力点（当前设定：${apLimit} 体力点）`"
          icon="fa-solid fa-bolt"
          :icon-style="{ color: '#5ae76b' }"
          stack>
          <rizui_slider
            v-model="apLimit"
            :min="100"
            :max="999"
            :step="10"
            display=" 体力点"
            style="margin: 8px 0 0 0; width: 100%" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 安全协议与限制（Disabled 禁用态） -->
      <rizui_card
        title="安全协议与限制|Disabled 禁用态"
        desc="设置 disabled 后其内部所有支持 disabled 的子控件均不可交互">
        <rizui_cfgrow
          label="关闭什亭之箱的保护场域"
          hint="老师...请不要关闭保护场域，否则将无法抵御外部攻击！ "
          icon="fa-solid fa-lock"
          :icon-style="{ color: '#f56c6c' }"
          :disabled="true">
          <rizui_switch :model-value="true" />
        </rizui_cfgrow>

        <rizui_cfgrow
          label="进入总力战"
          hint="基沃托斯正处于和平状态"
          icon="fa-solid fa-triangle-exclamation"
          :disabled="true">
          <rizui_button text="进入" primary disabled r-icon="fa-solid fa-angle-right" />
        </rizui_cfgrow>
      </rizui_card>

      <!-- 样式定制 -->
      <rizui_card
        title="样式定制 | Custom Style"
        desc="支持传入 labelStyle、hintStyle 与 iconStyle 精准定制各区域样式">
        <rizui_cfgrow
          label="由普拉娜担任助手"
          hint="我猜阿罗娜前辈又在睡懒觉了..."
          icon="fa-solid fa-wand-magic-sparkles"
          :icon-style="{ color: '#a855f7' }"
          :label-style="{ fontWeight: 'bold', color: '#a855f7' }"
          :hint-style="{ color: '#c084fc' }">
          <rizui_switch v-model="rainbowMode" />
        </rizui_cfgrow>
      </rizui_card>

    </section>

    <section class="sec-props">
      <h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table">
        <thead>
          <tr>
            <th>属性</th>
            <th>类型</th>
            <th>必填</th>
            <th>默认值</th>
            <th>说明</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="p in props" :key="p.name">
            <td><code>{{ p.name }}</code></td>
            <td><code>{{ p.type }}</code></td>
            <td>{{ p.required || '—' }}</td>
            <td><code>{{ p.default || '—' }}</code></td>
            <td>{{ p.desc }}</td>
          </tr>
        </tbody>
      </table>
    </section>

    <!-- 主题模式提示 -->
    <rizui_infobox type="info">
      <code>theme</code> 支持 <code>Dark</code> / <code>Light</code> / <code>auto</code>（默认）。<code>auto</code> 跟随顶栏主题开关（全局 <code>data-rizui-theme</code>）或系统 <code>prefers-color-scheme</code>；<code>Dark</code> / <code>Light</code> 可对本组件局部强制明暗。试试右上角主题按钮实时切换，或在你的项目中于 <code>&lt;html&gt;</code> 上设置 <code>data-rizui-theme</code>。
    </rizui_infobox>

    <section class="sec-usage">
      <h3><i class="fa-solid fa-code"></i> 用法</h3>
      <rizui_code block lang="html" :code="usage" />
    </section>
  </main>
</template>