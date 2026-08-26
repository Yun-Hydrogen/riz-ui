<script setup>
import { rizui_infobox, rizui_card } from 'riz-ui'
import rizui_cfgrow from '../../../../src/components/Container/rizui_cfgrow.vue';

const props = [
  { name:'type',        type:'String',  default:'"info"',  desc:'info / warn / error' },
  { name:'text',        type:'String',  default:'""',      desc:'提示文字，可空' },
  { name:'text-style', type:'Object',  default:'{}',      desc:'文字样式覆盖' },
  { name:'icon',        type:'String',  default:'""',      desc:'自定义图标，空时按 type' },
  { name:'icon-style', type:'Object',  default:'{}',      desc:'图标样式覆盖' },
  { name:'color',       type:'String',  default:'""',      desc:'仅 info 生效，空则跟随主题色' }
]
const usage = `<!-- 三种预设 -->
<rizui_infobox type="info" text="提示" />
<rizui_infobox type="warn" text="警告" />
<rizui_infobox type="error" text="报错" />

<!-- info：默认跟随主题色，color prop 可自定义 -->
<rizui_infobox type="info" text="跟随主题色" />
<rizui_infobox type="info" text="自定义颜色" color="#8b5cf6" />

<!-- 自定义图标 -->
<rizui_infobox type="info" text="自己换个图标"
              icon="fa-solid fa-bell"
              :icon-style="{ fontSize:'20px', color:'#e6a23c' }" />

<!-- text + 插槽：有插槽就自动排到文字下方 -->
<rizui_infobox type="info" text="标题文字">
  <div>插槽内容，排在文字下方。</div>
</rizui_infobox>`

const gacha = "📘📘📘📘📘📘📘📘📘📙"
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>提示框</h2><p class="sec-tag">rizui_infobox</p></div>
      <p class="sec-desc">长方形的提示框，撑满整行、虚线边框，左边带图标。三种预设配色。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <!--三种预设-->
      <rizui_card title="三种预设样式" desc="info / warn / error">
        <div class="demo-stack">
          <rizui_infobox type="info" text="emm...今天真是个好天气！" />
          <rizui_infobox type="warn" text="看起来，好像要下雨......" />
          <rizui_infobox type="error" text="老师，请立即下达撤退命令！" />
        </div>
      </rizui_card>

      <!--自定义-->
      <rizui_card title="自定义样式" desc="icon / 文字都能改样式">
        <div class="demo-stack">
          <rizui_infobox type="info" :text="'抽取结果: ' + gacha"
                        icon="fa-solid fa-bell"
                        :icon-style="{ fontSize:'20px', color:'#e6a23c' }" />
          <rizui_infobox type="warn" text="青辉石 -1200"
                        :text-style="{ fontWeight:700, fontSize:'14px' }" />
          <rizui_infobox type="info" text="Point x 199" color="#8b5cf6" />
        </div>
      </rizui_card>

      <rizui_card title="文字 + 插槽" desc="可以放置更多控件！">
        <rizui_infobox type="info" text="这是标题文字">
          <rizui_cfgrow label="这是一个配置行" hint="可以放置更多控件" />
        </rizui_infobox>
      </rizui_card>

    </section>

    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>

    <!-- 主题色提示 -->
    <rizui_infobox type="info">
      <code>info</code> 默认跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>），<code>color</code> 可自定义（仅 info 生效）；warn / error 用预设色。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </rizui_infobox>

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
