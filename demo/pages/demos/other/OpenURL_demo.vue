<script setup>
import { openURL, rizui_button, rizui_card, rizui_cfgrow, rizui_code, rizui_infobox } from 'riz-ui'

const card1Usage = `import { openURL } from 'riz-ui'

<!-- 模板事件中直接调用 -->
 <rizui_button
    text="打开仓库"
    primary
    r-icon="fa-solid fa-arrow-up-right-from-square"
    @click="openURL('https://github.com/Yun-Hydrogen/riz-ui')"
  />

<!-- JavaScript 脚本中直接执行 -->
openURL('https://github.com/Yun-Hydrogen/riz-ui', '_blank')`

const params = [
  { name: 'url',    type: 'String', default: '必填',    desc: '要跳转的目标外部链接地址（需包含 http:// 或 https://）' },
  { name: 'target', type: 'String', default: '"_blank"', desc: '窗口打开模式，默认在新标签页打开（如 _blank / _self）' }
]
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>外部连接跳转</h2>
        <p class="sec-tag">openURL</p>
      </div>
      <p class="sec-desc">通过动态合成 <rizui_code style="font-size:10px">&lt;a&gt;</rizui_code> 标签并触发真实用户点击，彻底规避原生 <code>window.open</code> 在异步事件、Teleport 悬浮窗或通知弹窗中被浏览器安全策略拦截的问题。</p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 链接跳转卡片 -->
      <rizui_card title="在新窗口打开链接" desc="在新标签页安全打开外部站点，自动注入 noopener noreferrer">
        <rizui_cfgrow label="沙勒中央数据库 (GitHub)" hint="点击在新标签页访问 RizUI 开源项目主页">
          <rizui_button
            text="打开仓库"
            primary
            r-icon="fa-solid fa-arrow-up-right-from-square"
            @click="openURL('https://github.com/Yun-Hydrogen/riz-ui')"
          />
        </rizui_cfgrow>

        <rizui_cfgrow label="哔哩哔哩" hint="点击访问">
          <rizui_button
            text="前往频道"
            r-icon="fa-solid fa-arrow-up-right-from-square"
            @click="openURL('https://space.bilibili.com/560229787')"
          />
        </rizui_cfgrow>

        <rizui_code block lang="html" :code="card1Usage" style="margin-top: 12px" />
      </rizui_card>
    </section>

    <section class="sec-props">
      <h3><i class="fa-solid fa-list-check"></i> 参数说明</h3>
      <table class="props-table">
        <thead>
          <tr>
            <th>参数名</th>
            <th>类型</th>
            <th>默认值</th>
            <th>说明</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="p in params" :key="p.name">
            <td><code>{{ p.name }}</code></td>
            <td><code>{{ p.type }}</code></td>
            <td><code>{{ p.default }}</code></td>
            <td>{{ p.desc }}</td>
          </tr>
        </tbody>
      </table>
    </section>

    <!-- 说明提示 -->
    <rizui_infobox type="info">
      <code>openURL</code> 在每次调用时会在 <code>document.body</code> 动态挂载一个隐藏的锚点链接并立即触发 <code>click()</code>，随后自动销毁 DOM 节点。非常适合在 <code>rizui_toast</code>、<code>rizui_dialog</code> 等异构或异步回调中使用。
    </rizui_infobox>
  </main>
</template>
