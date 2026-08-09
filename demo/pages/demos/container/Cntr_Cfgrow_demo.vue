<script setup>
import {ref, inject} from 'vue'
import {Cntr_Cfgrow, Cntr_Card, Ctrl_Switch, Ctrl_Slider, Inpt_Text} from 'riz-ui'

const themeColor = inject('themeColor')
const switchOn = ref(true)
const SlideValue = ref(50)

const props = [
  {name: 'label', type: 'String', default: '""', desc: '标签文字（可选）'},
  {name: 'labelInstyle', type: 'Object', default: '{}', desc: '标签内联样式覆盖'},
  {name: 'hint', type: 'String', default: '""', desc: '提示小字（可选）'},
  {name: 'hintInstyle', type: 'Object', default: '{}', desc: '提示内联样式覆盖'},
  {name: 'icon', type: 'String', default: '""', desc: 'FA 图标类名'},
  {name: 'iconInstyle', type: 'Object', default: '{}', desc: '图标内联样式覆盖'},
  {name: 'stack', type: 'Boolean', default: 'false', desc: '竖向布局'},
  {name: 'disabled', type: 'Boolean', default: 'false', desc: '禁用子控件'}
]
const usage =
    `
      <Cntr_Card
          title="横向模式（默认）"
          desc="默认情况下，控件在右侧">
        <Cntr_Cfgrow label="这是一个带图标横向排版的配置行" hint="控件内容集中在右侧" icon="fa-solid fa-toggle-on"
                     :iconInstyle="{ color: themeColor }">
          <Ctrl_Switch v-model="switchOn" :color="themeColor"/>
        </Cntr_Cfgrow>

        <Cntr_Cfgrow
            label="这是一个图标拥有属性的配置行"
            hint="配置行的图标同样支持FA图标类名和内联样式覆盖"
            icon="fa-solid fa-sync fa-spin">
          <Ctrl_Switch v-model="switchOn" :color="themeColor"/>
        </Cntr_Cfgrow>

        <Cntr_Cfgrow label="这是一个不带图标横向排版的配置行" hint="控件内容集中在右侧">
          <Ctrl_Slider v-model="SlideValue" :min="1" :max="255" :step="1" display="px"/>
        </Cntr_Cfgrow>
      </Cntr_Card>

      <!-- Stack 竖向模式 -->
      <Cntr_Card
          title="Stack 竖向模式"
          desc="stack 模式下控件在标签下方，用于较长的控件或多行控件">
        <Cntr_Cfgrow label="自定义输入" hint="stack 模式下控件在标签下方" icon="fa-solid fa-keyboard"
                     :iconInstyle="{ color: themeColor }" stack>
          <Inpt_Text type="text" placeholder="请输入文本" :color="themeColor" :plaintext="true"/>
          <Ctrl_Slider v-model="SlideValue" :min="1" :max="255" :step="1" display="px"
                       style="margin: 8px 0 0 0; width: 100%"/>
        </Cntr_Cfgrow>
      </Cntr_Card>

      <!-- Disabled 禁用态 -->
      <Cntr_Card
          title="Disabled 禁用态"
          desc="disabled 时，其所有支持disabled属性的子控件不可交互">
        <Cntr_Cfgrow label="开关状态" hint="disabled 时子控件不可交互" icon="fa-solid fa-ban"
                     :iconInstyle="{ color: '#e55' }" :disabled="true">
          <Ctrl_Switch :model-value="true" :color="themeColor"/>
        </Cntr_Cfgrow>
`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>配置行</h2>
        <p class="sec-tag">Cntr_Cfgrow</p></div>
      <p class="sec-desc">标准配置行组件。左侧图标跨行纵贯 label 与 hint，支持 stack 竖向布局和 disabled 禁用态。</p>
    </section>
    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 横向模式（默认） -->
      <Cntr_Card
          title="横向模式（默认）"
          desc="默认情况下，控件在右侧">
        <Cntr_Cfgrow label="这是一个带图标横向排版的配置行" hint="控件内容集中在右侧" icon="fa-solid fa-toggle-on"
                     :iconInstyle="{ color: themeColor }">
          <Ctrl_Switch v-model="switchOn" :color="themeColor"/>
        </Cntr_Cfgrow>

        <Cntr_Cfgrow
            label="这是一个图标拥有属性的配置行"
            hint="配置行的图标同样支持FA图标类名和内联样式覆盖"
            icon="fa-solid fa-sync fa-spin">
          <Ctrl_Switch v-model="switchOn" :color="themeColor"/>
        </Cntr_Cfgrow>

        <Cntr_Cfgrow label="这是一个不带图标横向排版的配置行" hint="控件内容集中在右侧">
          <Ctrl_Slider v-model="SlideValue" :min="1" :max="255" :step="1" display="px"/>
        </Cntr_Cfgrow>
      </Cntr_Card>

      <!-- Stack 竖向模式 -->
      <Cntr_Card
          title="Stack 竖向模式"
          desc="stack 模式下控件在标签下方，用于较长的控件或多行控件">
        <Cntr_Cfgrow label="自定义输入" hint="stack 模式下控件在标签下方" icon="fa-solid fa-keyboard"
                     :iconInstyle="{ color: themeColor }" stack>
          <Inpt_Text type="text" placeholder="请输入文本" :color="themeColor" :plaintext="true"/>
          <Ctrl_Slider v-model="SlideValue" :min="1" :max="255" :step="1" display="px"
                       style="margin: 8px 0 0 0; width: 100%"/>
        </Cntr_Cfgrow>
      </Cntr_Card>

      <!-- Disabled 禁用态 -->
      <Cntr_Card
          title="Disabled 禁用态"
          desc="disabled 时，其所有支持disabled属性的子控件不可交互">
        <Cntr_Cfgrow label="开关状态" hint="disabled 时子控件不可交互" icon="fa-solid fa-ban"
                     :iconInstyle="{ color: '#e55' }" :disabled="true">
          <Ctrl_Switch :model-value="true" :color="themeColor"/>
        </Cntr_Cfgrow>

      </Cntr_Card>

    </section>
    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
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
    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>