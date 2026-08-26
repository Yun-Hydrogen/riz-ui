<script setup>
import { openURL, rizui_button, rizui_card, rizui_cfgrow } from 'riz-ui'

const usage = `import { openURL } from 'riz-ui'

<!-- 模板里直接绑定 -->
<rizui_button text="打开文档" @click="openURL('https://github.com/YunHydrogen/riz-ui')" />

<!-- 或 JS 里调用 -->
openURL('https://example.com', '_blank')`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>打开链接</h2><p class="sec-tag">openURL</p></div>
      <p class="sec-desc">一个工具函数，用「合成 &lt;a&gt; 点击」打开链接，避免 <code>window.open</code> 被弹窗拦截，也适合在 Teleport（如 Toast）里用。</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>
      <rizui_card title="跳转示例" desc="点按钮在新标签页打开">
        <rizui_cfgrow label="GitHub 仓库">
          <rizui_button text="打开仓库" primary @click="openURL('https://github.com/YunHydrogen/riz-ui')" r-icon="fa-solid fa-arrow-up-right-from-square"/>
        </rizui_cfgrow>
        <rizui_cfgrow label="哔哩哔哩">
          <rizui_button text="打开 B 站" @click="openURL('https://space.bilibili.com/560229787')" r-icon="fa-solid fa-arrow-up-right-from-square"/>
        </rizui_cfgrow>
      </rizui_card>
    </section>

    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>
